using System.Threading.Tasks;
using HTN;
using UnityEngine;

namespace AnimalBehaviour
{
    public class NavigateToOperator : IOperator
    {
        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            ctx.NavAgent.Destination = ctx.CurrentTarget.position;

            callback();
        }
    }

    public class WanderOperator : IOperator
    {
        public WanderOperator(float maxWanderRange)
        {
            m_MaxWanderRange = maxWanderRange;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            Vector2 randomInCircle = Random.insideUnitCircle;
            ctx.NavAgent.Destination = new Vector3(
                randomInCircle.x * m_MaxWanderRange, 
                0f, 
                randomInCircle.y * m_MaxWanderRange);

            callback();
        }

        private float m_MaxWanderRange;
    }

    public class EatOperator : IOperator
    {
        public EatOperator(int delayInMilisec)
        {
            m_DelayInMilisec = delayInMilisec;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            ctx.CurrentTarget.GetComponent<Behaviour>().GetEaten();
            ctx.CurrentTarget = null;

            FinishTaskWithDelay(ctx, callback, (int)(m_DelayInMilisec / Time.timeScale));
        }

        private async void FinishTaskWithDelay(Context ctx, TaskFinishedCallback callback, int delay)
        {
            await Task.Run(() => Task.Delay(delay).Wait());

            ctx.Animal.GainFood(30);

            callback();
        }

        private int m_DelayInMilisec;
    }

    public class GiveBirthOperator : IOperator
    {
        public GiveBirthOperator(int delayInMilisec)
        {
            m_DelayInMilisec = delayInMilisec;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            FinishTaskWithDelay(ctx, callback, (int)(m_DelayInMilisec / Time.timeScale));
        }

        private async void FinishTaskWithDelay(Context ctx, TaskFinishedCallback callback, int delay)
        {
            await Task.Run(() => Task.Delay(delay).Wait());

            ctx.Animal.Breed();

            callback();
        }

        private int m_DelayInMilisec;
    }

    public class RestOperator : IOperator
    {
        public RestOperator(int delayInMilisec)
        {
            m_DelayInMilisec = delayInMilisec;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            FinishTaskWithDelay(callback, (int)(m_DelayInMilisec / Time.timeScale));
        }

        private async void FinishTaskWithDelay(TaskFinishedCallback callback, int delay)
        {
            await Task.Run(() => Task.Delay(delay).Wait());

            callback();
        }

        private int m_DelayInMilisec;
    }

    public class DashOperator : IOperator
    {
        public DashOperator(float speed)
        {
            m_Speed = speed;
        }

        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            ctx.Dasher.Dash(ctx.CurrentTarget.position, m_Speed, callback);
        }

        private float m_Speed;
    }

}
