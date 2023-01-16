using System.Threading.Tasks;
using UnityEngine;

namespace HTN.DebugBehaviour
{
    public class LogOperator : IOperator
    {
        public LogOperator(string message, int delayInMilisec)
        {
            m_MessageToLog = message;
            m_DelayInMilisec = delayInMilisec;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            FinishTaskWithDelay(callback);
        }

        private async void FinishTaskWithDelay(TaskFinishedCallback callback)
        {
            await Task.Run(() => Task.Delay(m_DelayInMilisec).Wait());

            Debug.Log(m_MessageToLog);

            callback();
        }

        private string m_MessageToLog;
        private int m_DelayInMilisec;
    }

    public class WarnOperator : IOperator
    {
        public WarnOperator(string message)
        {
            m_WarningMessage = message;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            Debug.LogWarning(m_WarningMessage);

            callback();
        }

        private string m_WarningMessage;
    }

}
