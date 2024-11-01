using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    void Start()
    {
        targetPosition = transform.position;
        currentDirection = Vector3.zero;
        lastDirection = Vector3.zero;
        //Debug.Log("Starting Position: " + targetPosition);
        //Debug.Log("Initial lastDirection: " + lastDirection);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        dustEffect = transform.Find("DustEffect").GetComponent<ParticleSystem>();
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
            CheckAndMoveAtCenter();
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
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopMovementAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
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
}
