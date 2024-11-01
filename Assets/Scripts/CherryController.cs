using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    
    public float moveSpeed = 2f;
    private Vector3 endPosition;
    private Camera mainCamera;    

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SetMovementDirection();
    }

    private void SetMovementDirection()
    {
        Vector3 center = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        center.z = 0;

        Vector3 startPosition = transform.position;
        endPosition = 2 * center - startPosition;

        StartCoroutine(MoveTowardsCenter());
    }

    private IEnumerator MoveTowardsCenter()
    {
        while (Vector3.Distance(transform.position, endPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PacStudent"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
