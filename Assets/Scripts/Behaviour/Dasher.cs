using HTN;
using UnityEngine;

namespace AnimalBehaviour
{
    public class Dasher : MonoBehaviour
    {
        public void Dash(Vector3 targetPos, float speed, TaskFinishedCallback callback)
        {
            m_TargetPos = targetPos;
            m_IsDashing = true;
            m_DashSpeed = speed;
            m_FinishedDashCallback = callback;

            m_DashDirection = (targetPos - transform.position).normalized;
        }

        private void Update()
        {
            if (m_IsDashing)
            {
                transform.position += m_DashDirection * m_DashSpeed * Time.deltaTime;

                if (Vector3.SqrMagnitude(transform.position - m_TargetPos) < 0.5f)
                {
                    OnDashFinished();
                }
            }
        }

        private void OnDashFinished()
        {
            m_IsDashing = false;
            m_FinishedDashCallback();
        }

        private Vector3 m_TargetPos;
        private Vector3 m_DashDirection;
        private bool m_IsDashing = false;
        private float m_DashSpeed;

        private TaskFinishedCallback m_FinishedDashCallback;
    }
}