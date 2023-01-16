using UnityEngine;

namespace HTN.Examples
{
    public class WolfSensor : MonoBehaviour
    {
        [Header("Ranges")]
        [SerializeField] private float m_MeleeRadius = 2.0f;
        [SerializeField] private float m_LeapRadius = 20.0f;
        [SerializeField] private float m_DetectionRadius = 50.0f;

        private void Start()
        {
            m_WolfBrain = GetComponent<WolfBrain>();
            m_Context = GetComponent<Context>();
        }

        private void Update()
        {
            if (m_WolfBrain.GetWSProperty(WSProperties.Navigating) == 1 && 
                Vector3.SqrMagnitude(m_Context.NavAgent.Destination - m_Context.transform.position) < 0.01f)
            {
                m_WolfBrain.SetWSProperty(WSProperties.Navigating, 0);
            }

            if (!m_Context.CurrentTarget)
            {
                m_WolfBrain.SetWSProperty(WSProperties.HasTarget, 0);

                Collider[] possibleTargets = Physics.OverlapSphere(transform.position, m_DetectionRadius);

                foreach (var possibleTarget in possibleTargets)
                {
                    if (possibleTarget.CompareTag("Prey"))
                    {
                        m_WolfBrain.SetWSProperty(WSProperties.HasTarget, 1);
                        m_Context.CurrentTarget = possibleTarget.transform;
                        break;
                    }
                }
            }


            if (m_Context.CurrentTarget)
            {
                if (Vector3.SqrMagnitude(m_Context.NavAgent.Destination - m_Context.CurrentTarget.position) < 1.0f)
                {
                    m_Context.NavAgent.Destination = m_Context.CurrentTarget.position;
                }

                float distanceToTarget = Vector3.Magnitude(m_Context.CurrentTarget.position - m_Context.transform.position);
                if (distanceToTarget < m_MeleeRadius)
                {
                    m_WolfBrain.SetWSProperty(WSProperties.TargetRange, (byte)ProximityRange.Melee);
                }
                else if (distanceToTarget < m_LeapRadius)
                {
                    m_WolfBrain.SetWSProperty(WSProperties.TargetRange, (byte)ProximityRange.Leap);
                }
                else if (distanceToTarget < m_DetectionRadius)
                {
                    m_WolfBrain.SetWSProperty(WSProperties.TargetRange, (byte)ProximityRange.ViewRange);
                }
                else
                {
                    m_WolfBrain.SetWSProperty(WSProperties.TargetRange, (byte)ProximityRange.OutOfRange);
                }
            }


            if (m_Context.Animal._currentFood < 15f)
            {
                m_WolfBrain.SetWSProperty(WSProperties.Hunger, (byte)HungerState.Starving);
            }
            else if (m_Context.Animal._currentFood < 40f)
            {
                m_WolfBrain.SetWSProperty(WSProperties.Hunger, (byte)HungerState.Hungry);
            }
            else if (m_Context.Animal._currentFood < 70f)
            {
                m_WolfBrain.SetWSProperty(WSProperties.Hunger, (byte)HungerState.Satisfied);
            }
            else
            {
                m_WolfBrain.SetWSProperty(WSProperties.Hunger, (byte)HungerState.Full);
            }
        }

        private WolfBrain m_WolfBrain;
        private Context m_Context;
    }
}
