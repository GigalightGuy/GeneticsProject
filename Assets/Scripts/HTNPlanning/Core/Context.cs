using UnityEngine;
using AnimalBehaviour;

namespace HTN
{
    [RequireComponent(typeof(NavAgentTest))]
    public class Context : MonoBehaviour
    {
        public NavAgentTest NavAgent { get; private set; }
        public Animal Animal { get; private set; }
        public Dasher Dasher { get; private set; }

        public Transform CurrentTarget { get; set; }

        private void Start()
        {
            NavAgent = GetComponent<NavAgentTest>();
            Animal = GetComponent<Animal>();
            Dasher = GetComponent<Dasher>();
        }
    }
}
