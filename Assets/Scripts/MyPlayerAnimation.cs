using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class MyPlayerAnimation : NetworkBehaviour
{
    private Vector3 previousPosition;
    public float curSpeed;
    // Update is called once per frame

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        float xspeed = curMove.x;
        float zspeed = curSpeed;
        //GetComponentInChildren<Animator>().SetFloat("xSpeed", xspeed);
        animator.SetFloat("zSpeed", curSpeed);
        
    }
}
