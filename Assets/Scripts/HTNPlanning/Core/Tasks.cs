using System.Collections.Generic;

namespace HTN
{
    public delegate bool Conditions(List<byte> worldState);
    public delegate void Effects(List<byte> worldState);
    public delegate void TaskFinishedCallback();

    public interface ITask
    {
        public bool IsCompound();
    }

    public interface IOperator
    {
        public void Execute(Context ctx, TaskFinishedCallback callback);
    }

    public class PrimitiveTask : ITask
    {
        public PrimitiveTask(IOperator opr, Conditions conditions, Effects effects, string debugName = "")
        {
            m_Operator = opr;
            m_Conditions = conditions;
            m_Effects = effects;

            m_DebugName = debugName;
        }

        public bool IsCompound() => false;

        public bool CheckConditionMet(List<byte> worldState) => m_Conditions(worldState);
        public void ApplyEffects(List<byte> worldState) => m_Effects(worldState);

        public void Execute(Context ctx, TaskFinishedCallback callback) => m_Operator.Execute(ctx, callback);

        public string DebugName => m_DebugName;

        private IOperator m_Operator;

        private Conditions m_Conditions;
        private Effects m_Effects;

        private string m_DebugName;
    }

    public class CompoundTask : ITask
    {
        public void PopulateMethods(params Method[] methods)
        {
            m_Methods = new List<Method>(methods);
        }

        public bool IsCompound() => true;

        public Method FindSatisfiedMethod(List<byte> worldState)
        {
            foreach (Method method in m_Methods)
            {
                if (method.CheckConditionMet(worldState))
                {
                    return method;
                }
            }

            return null;
        }

        private List<Method> m_Methods;
    }

    public class Method
    {
        public Method(Conditions conditions, params ITask[] subTasks)
        {
            m_Conditions = conditions;
            m_SubTasks = new List<ITask>(subTasks);
        }

        public bool CheckConditionMet(List<byte> worldState) => m_Conditions(worldState);

        public List<ITask> SubTasks => m_SubTasks;


        private List<ITask> m_SubTasks;

        private Conditions m_Conditions;
    }
}
