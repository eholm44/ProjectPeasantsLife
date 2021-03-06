using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class MyPlayerAnimation : NetworkBehaviour
{
    private Vector3 previousPosition;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float xSpeed = localVelocity.x;
        float zSpeed = localVelocity.z;

        animator.SetFloat("zSpeed", zSpeed);
        animator.SetFloat("xSpeed", xSpeed);
        
    }
}
