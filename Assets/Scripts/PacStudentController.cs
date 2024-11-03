using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 targetPosition;
    private bool isLerping = false;

    private Vector3 currentDirection = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private ParticleSystem dustEffect;

    private float centerThreshold = 0.01f;

    public GameObject wallCollisionEffectPrefab;
    public AudioClip wallCollisionSound;

    private Vector3 lastPosition;

    public Canvas HUD;
    public TextMeshProUGUI scoreText;           
    public int score = 0;           
    public int lives = 3;
    private Image[] lifeImages;

    public float teleportCooldown = 1.0f;  
    private bool isTeleporting = false;

    public TextMeshProUGUI ghostTimerText;       
    public AudioClip normalMusic;               
    public AudioClip scaredMusic;
    public AudioClip movingNoPelletMusic;

    private bool isGhostScared = false;          
    private float ghostScaredTimer = 0f;        
    private float scaredDuration = 10f;        
    private float recoveringDuration = 3f;

    void Start()
    {
        targetPosition = transform.position;
        currentDirection = Vector3.zero;
        lastDirection = Vector3.zero;
        lastPosition = transform.position;

        scoreText = HUD.transform.Find("Score").GetComponent<TextMeshProUGUI>();

        if (scoreText == null)
        {
            Debug.LogError("Score Text component not found!");
        }

        lifeImages = new Image[3];
        lifeImages[0] = HUD.transform.Find("Life1").GetComponent<Image>();
        lifeImages[1] = HUD.transform.Find("Life2").GetComponent<Image>();
        lifeImages[2] = HUD.transform.Find("Life3").GetComponent<Image>();

        UpdateScore(0); 
        UpdateLivesUI();
        //Debug.Log("Starting Position: " + targetPosition);
        //Debug.Log("Initial lastDirection: " + lastDirection);

        if (audioSource != null && movingNoPelletMusic != null)
        {
            audioSource.clip = movingNoPelletMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        dustEffect = transform.Find("DustEffect").GetComponent<ParticleSystem>();

        ghostTimerText.gameObject.SetActive(false);




    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();
        if (isLerping)
        {
            PerformLerp();
        }

        else 
        {
            lastPosition = transform.position; 
            CheckAndMoveAtCenter();         
        }

        // Change Ghost State
        if (isGhostScared)
        {
            ghostScaredTimer -= Time.deltaTime;
            ghostTimerText.text = Mathf.Ceil(ghostScaredTimer).ToString();

            if (ghostScaredTimer <= recoveringDuration && ghostScaredTimer > 0)
            {
                SetGhostsToRecovering();  
            }
            else if (ghostScaredTimer <= 0)
            {
                isGhostScared = false;
                ghostTimerText.gameObject.SetActive(false);
                audioSource.clip = normalMusic;
                audioSource.Play();
                SetGhostsToWalking();  
            }
        }

    }
    

    private void HandleInput()
    {
        

        if (Input.GetKeyDown(KeyCode.W))
        {
            lastDirection = Vector3.up;
            //Debug.Log("Input detected: W");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastDirection = Vector3.down;
            //Debug.Log("Input detected: S");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastDirection = Vector3.left;
            //Debug.Log("Input detected: A");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastDirection = Vector3.right;
            //Debug.Log("Input detected: D");
        }
    }

    private void CheckAndMoveAtCenter()
    {
        if (Vector3.Distance(transform.position, targetPosition) < centerThreshold)
        {
            transform.position = targetPosition;
            TryChangeDirectionAtCenter();
        }
    }

    private void TryChangeDirectionAtCenter()
    {
        if (lastDirection != Vector3.zero && CanMoveInDirection(lastDirection))
        {
            currentDirection = lastDirection; 
            targetPosition = transform.position + currentDirection; 
            isLerping = true;

            animator.SetBool("isWalking", true);
            PlayMovementAudio();
            SetAnimationDirection(currentDirection);
            PlayDustEffect();
        }
        else if (currentDirection != Vector3.zero && CanMoveInDirection(currentDirection))
        {
            targetPosition = transform.position + currentDirection;
            isLerping = true;

            animator.SetBool("isWalking", true);
            PlayMovementAudio();
            SetAnimationDirection(currentDirection);
            PlayDustEffect();
        }
    }    

    private bool CanMoveInDirection(Vector3 direction)
    {
        Vector3 checkPosition = transform.position + direction;
        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, 0.05f);

        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log($"Detected Collider Tag: {hitCollider.tag} at Position: {hitCollider.transform.position}");
            if (hitCollider.CompareTag("Wall"))
            {
                //Debug.Log($"Wall detected in direction {direction} at {checkPosition}");
                return false; 
            }
        }
        return true; 

       
    }

    private void PerformLerp()
    {
        lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < centerThreshold)
        {
            transform.position = targetPosition;
            transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
            isLerping = false;

            animator.SetBool("isWalking", false);
            StopMovementAudio();
            StopDustEffect();
        }
    }

    private void SetAnimationDirection(Vector3 direction)
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        if (direction.y > 0)
        {
            spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction.y < 0)
        {
            spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void PlayMovementAudio()
    {
        if (!isGhostScared && movingNoPelletMusic != null)
        {
            if (audioSource.clip != movingNoPelletMusic)
            {
                audioSource.clip = movingNoPelletMusic; 
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); 
            }
        }
        else if (isGhostScared && scaredMusic != null)
        {
            audioSource.clip = scaredMusic;
            audioSource.Play();
        }
    }

    private void StopMovementAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (movingNoPelletMusic != null)
        {
            audioSource.clip = movingNoPelletMusic;
            audioSource.Play();
        }
    }

    private void PlayDustEffect()
    {
        if (!dustEffect.isPlaying)
        {
            dustEffect.Play();
        }
    }

    private void StopDustEffect()
    {
        if (dustEffect.isPlaying)
        {
            dustEffect.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.tag);

        if (other.CompareTag("Wall"))
        {
            HandleWallCollision();
        }
        else if (other.CompareTag("Teleporter") && !isTeleporting)
        {
            StartCoroutine(TeleportCooldownRoutine(other));
        }
        else if (other.CompareTag("Pellet"))
        {
            HandlePelletCollision(other.gameObject);
        }
        else if (other.CompareTag("Cherry"))
        {
            HandleBonusCherryCollision(other.gameObject);
        }
        else if (other.CompareTag("PowerPellet"))
        {
            HandlePowerPelletCollision();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ghost"))
        {
            //HandleGhostCollision(other.gameObject);
        }
    }

    private void HandleWallCollision()
    {
        transform.position = lastPosition;
        PlayWallCollisionEffect();
        PlayWallCollisionSound();
    }

    private void PlayWallCollisionEffect()
    {
        if (wallCollisionEffectPrefab != null)
        {
            GameObject effect = Instantiate(wallCollisionEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();
            }
            Destroy(effect, 1.0f); 
        }
    }

    private void PlayWallCollisionSound()
    {
        if (audioSource != null && wallCollisionSound != null)
        {
            Debug.Log("Playing wall collision sound");
            audioSource.PlayOneShot(wallCollisionSound);
        }
    }

    private IEnumerator TeleportCooldownRoutine(Collider teleporter)
    {
        isTeleporting = true;  

        Vector3 newPosition = transform.position;
        string targetTeleporterName = "";

        switch (teleporter.gameObject.name)
        {
            case "Teleporter_1":
                targetTeleporterName = "Teleporter_2";
                break;
            case "Teleporter_2":
                targetTeleporterName = "Teleporter_1";
                break;
            case "Teleporter_3":
                targetTeleporterName = "Teleporter_4";
                break;
            case "Teleporter_4":
                targetTeleporterName = "Teleporter_3";
                break;
        }

        GameObject targetTeleporter = GameObject.Find(targetTeleporterName);
        if (targetTeleporter != null)
        {
            newPosition = targetTeleporter.transform.position;
            transform.position = newPosition;

            targetPosition = newPosition + currentDirection;
            isLerping = true;
        }
        else
        {
            Debug.LogError("Target teleporter not found: " + targetTeleporterName);
        }
        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }

    private void HandlePelletCollision(GameObject pellet)
    {
        Destroy(pellet);
        UpdateScore(10);
    }

    private void HandleBonusCherryCollision(GameObject cherry)
    {
        Destroy(cherry);
        UpdateScore(100);
    }

    private void HandlePowerPelletCollision()
    {
        isGhostScared = true;
        ghostScaredTimer = scaredDuration;
        ghostTimerText.gameObject.SetActive(true);

        if (audioSource != null && scaredMusic != null)
        {
            audioSource.clip = scaredMusic;
            audioSource.Play();
        }

        SetGhostsToScared();
        //StartScaredTimer(10);
    }

    /*private void HandleGhostCollision(GameObject ghost)
    {
        GhostController ghostController = ghost.GetComponent<GhostController>();
        if (ghostController != null && ghostController.isGhostScared)
        {
            ghostController.SetToDead();
            UpdateScore(300);
        }
        else
        {
            lives--;
            //RespawnPacStudent();
            UpdateLivesUI();
        }
    }*/

    public void UpdateScore(int points)
    {
        score += points;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--; 
            UpdateLivesUI(); 
        }
        if (lives <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if(lifeImages[i] != null)
            {
                lifeImages[i].gameObject.SetActive(i < lives);
            }
        }
    }

    private void SetGhostsToScared()
    {
        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        foreach (var ghost in ghosts)
        {
            ghost.SetScaredState();
        }
    }

    private void SetGhostsToRecovering()
    {
        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        foreach (var ghost in ghosts)
        {
            ghost.SetRecoveringState();
        }
    }

    private void SetGhostsToWalking()
    {
        if (audioSource != null && movingNoPelletMusic != null)
        {
            audioSource.clip = movingNoPelletMusic;
            audioSource.Play();
        }

        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        foreach (var ghost in ghosts)
        {
            ghost.SetWalkingState();
        }
    }
}



