using System.Collections.Generic;

namespace HTN
{
    public class PlanRunner
    {
        public event TaskFinishedCallback OnTaskFinished;

        public void Tick(Context context, List<byte> currentWS)
        {
            if (m_RunningTask == null)
            {
                if (m_CurrentPlan.Count > 0)
                {
                    m_RunningTask = m_CurrentPlan.Dequeue();
                    if (m_RunningTask.CheckConditionMet(currentWS))
                    {
                        m_RunningTask.Execute(context, OnTaskFinished);
                    }
                    else
                    {
                        m_NeedsNewPlan = true;
                    }
                }
                else
                {
                    m_NeedsNewPlan = true;
                }
                
            }
        }

        public void ApplyCurrentTaskEffects(List<byte> currentWS)
        {
            m_RunningTask.ApplyEffects(currentWS);
            m_RunningTask = null;
        }

        public void WSIsDirty() => m_NeedsNewPlan = true;

        public bool NeedsNewPlan() => m_NeedsNewPlan;

        public void SetNewPlan(PrimitiveTask[] plan)
        {
            m_CurrentPlan = new Queue<PrimitiveTask>(plan);
            m_NeedsNewPlan = false;
        }

        private Queue<PrimitiveTask> m_CurrentPlan;
        private PrimitiveTask m_RunningTask;

        private bool m_NeedsNewPlan = true;
    }
}
