using UnityEngine;

namespace HTN.Examples
{
    public class WolfSensor : MonoBehaviour
    {
        [SerializeField] private float m_DetectionRadius;

        private void Start()
        {
            m_TestBrain = GetComponent<TestBrain>();
            m_Context = GetComponent<Context>();
        }

        private void Update()
        {
            if (Vector3.SqrMagnitude(m_Context.NavAgent.destination - m_Context.transform.position) < 0.01f)
            {
                m_TestBrain.SetWSPropertie(WSProperties.Navigating, 1);
            }

            if (!m_Context.CurrentTarget)
            {
                m_TestBrain.SetWSPropertie(WSProperties.HasTarget, 0);

                Collider[] possibleTargets = Physics.OverlapSphere(transform.position, m_DetectionRadius);

                foreach (var possibleTarget in possibleTargets)
                {
                    if (possibleTarget.CompareTag("Prey"))
                    {
                        m_TestBrain.SetWSPropertie(WSProperties.HasTarget, 1);
                        m_Context.CurrentTarget = possibleTarget.transform;
                        break;
                    }
                }
            }
        }

        private TestBrain m_TestBrain;
        private Context m_Context;
    }
}
