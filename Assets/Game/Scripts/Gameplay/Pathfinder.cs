using UnityEngine;
using UnityEngine.AI;

namespace Game2D
{
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private NavMeshAgent agent;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            agent.SetDestination(target.position);
        }
    }
}