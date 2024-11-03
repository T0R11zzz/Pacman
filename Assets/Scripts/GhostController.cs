using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private Animator animator;
    //public bool IsScared { get; private set; }
    //public bool IsDead { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void SetScaredState()
    {
        animator.SetTrigger("Scared");
    }

    public void SetRecoveringState()
    {
        animator.SetTrigger("Recovering");
    }

    public void SetWalkingState()
    {
        animator.SetTrigger("Walking");
    }
}
