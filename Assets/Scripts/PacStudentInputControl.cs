using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentControl : MonoBehaviour
{
    public float moveSpeed = 1f; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PacStudentIsDead() && isDead ==false)
        {
            animator.SetTrigger("DieTrigger"); 
            isDead = true; 
        }

        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");
        //Debug.Log("moveInputX: " + moveInputX + " moveInputY: " + moveInputY);

        Vector2 movement = new Vector2(moveInputX, moveInputY).normalized;
        

        if (movement.magnitude > 0)
        {
            
            animator.SetFloat("Horizontal", moveInputX);
            animator.SetFloat("Vertical", moveInputY);
            animator.SetBool("isWalking", true);

            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            if (moveInputY > 0) 
            {
                spriteRenderer.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (moveInputY < 0) 
            {
                spriteRenderer.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (moveInputX < 0) 
            {
                spriteRenderer.flipX = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (moveInputX > 0) 
            {
                spriteRenderer.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //Debug.Log("Current Rotation: " + transform.rotation.eulerAngles);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        bool  PacStudentIsDead()
        {
            return false;
        }

        
    }
}
