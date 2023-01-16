using UnityEngine;

namespace HTN.Examples
{
    public class TestVisionSensor : MonoBehaviour
    {
        private void Start()
        {
            m_TestBrain = GetComponent<TestBrain>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchWarnToggle();
            }
        }
        private void SwitchWarnToggle()
        {
            if (m_TestBrain.GetWSPropertie(WSProperties.CanSeeEnemy) == 0)
            {
                m_TestBrain.SetWSPropertie(WSProperties.CanSeeEnemy, 1);
            }
            else
            {
                m_TestBrain.SetWSPropertie(WSProperties.CanSeeEnemy, 0);
            }
        }

        private TestBrain m_TestBrain;
    }
}
