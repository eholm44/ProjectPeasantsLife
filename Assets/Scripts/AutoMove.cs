using UnityEngine;
using UnityEngine.AI;

public class AutoMove : MonoBehaviour
{
        public GameObject target = null;
    [SerializeField] private NavMeshAgent agent = null;
    void Update()
    {
        //if (!NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        //{
        //    return;
        //}
        agent.SetDestination(target.transform.position);
    }
}
