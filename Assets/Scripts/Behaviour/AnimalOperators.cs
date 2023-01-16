using HTN;
using UnityEngine;

namespace AnimalBehaviour
{
    public class NavigateToOperator : IOperator
    {
        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            ctx.NavAgent.destination = ctx.CurrentTarget.position;

            callback();
        }
    }

    public class EatOperator : IOperator
    {
        public void Execute(Context ctx, TaskFinishedCallback callback)
        {
            ctx.CurrentTarget.GetComponent<Behaviour>().GetEaten();
            ctx.Animal.GainFood(30);
            ctx.CurrentTarget = null;

            callback();
        }
    }

}
