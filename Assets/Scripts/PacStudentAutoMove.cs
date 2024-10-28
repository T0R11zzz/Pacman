using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentAutoMovement : MonoBehaviour
{
    float speed = 2.0f;
    AudioSource moveAudio;

    Vector2[] corners = new Vector2[]
        {
            new Vector2(-12, -1), 
            new Vector2(-7, -1), 
            new Vector2(-7, -5), 
            new Vector2(-12, -5)
        };

    int currentCornerIndex = 0;

    Quaternion[] rotations = new Quaternion[]
    {
        Quaternion.Euler(0, 0, 180),  
        Quaternion.Euler(0, 0, 0),    
        Quaternion.Euler(0, 0, 270),  
        Quaternion.Euler(0, 0, 90)   
    };

    // Start is called before the first frame update
    void Start()
    {
        moveAudio = GetComponent<AudioSource>();
        if (moveAudio != null)
        {
            moveAudio.Play();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = corners[currentCornerIndex];

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentCornerIndex = (currentCornerIndex + 1) % corners.Length;
        }

        Vector2 direction = targetPosition - (Vector2)transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            /*if(moveAudio != null && !moveAudio.isPlaying)
            {
                moveAudio.Play();
            }*/
        }

        /*else 
        {
            if (moveAudio != null && moveAudio.isPlaying)
            {
                moveAudio.Stop();
            }
        }*/

    }
}
