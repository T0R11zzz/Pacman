using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isLerping = false;

    private Vector3 currentDirection = Vector3.zero;
    private Vector3 lastDirection = Vector3.right;

    void Start()
    {
        targetPosition = transform.position;
        Debug.Log("Starting Position: " + targetPosition);
        Debug.Log("Initial lastDirection: " + lastDirection);
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (!isLerping)
        {
            if (CanMoveInDirection(lastDirection)) 
            {
                currentDirection = lastDirection;  
                targetPosition = transform.position + currentDirection; 
                isLerping = true;  
            }
            else if (CanMoveInDirection(currentDirection)) 
            {
                targetPosition = transform.position + currentDirection; 
                isLerping = true;
            }
        }

        if (isLerping)
        {
            PerformLerp();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastDirection = Vector3.up;
        if (Input.GetKeyDown(KeyCode.S)) lastDirection = Vector3.down;
        if (Input.GetKeyDown(KeyCode.A)) lastDirection = Vector3.left;
        if (Input.GetKeyDown(KeyCode.D)) lastDirection = Vector3.right;
    }

    private bool CanMoveInDirection(Vector3 direction)
    {
        Vector3 checkPosition = transform.position + direction;
        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, 0.2f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Wall"))
            {
                return false; 
            }
        }

        return true; 
    }

    private void PerformLerp()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isLerping = false;
        }
    }
}
