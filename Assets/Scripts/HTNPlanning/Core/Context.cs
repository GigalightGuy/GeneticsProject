using UnityEngine;
using AnimalBehaviour;

namespace HTN
{
    [RequireComponent(typeof(NavAgent))]
    public class Context : MonoBehaviour
    {
        public NavAgent NavAgent { get; private set; }
        public Animal Animal { get; private set; }
        public Dasher Dasher { get; private set; }

        public Transform CurrentTarget { get; set; }

        private void Start()
        {
            NavAgent = GetComponent<NavAgent>();
            Animal = GetComponent<Animal>();
            Dasher = GetComponent<Dasher>();
        }
    }
}
