using System.Collections.Generic;
using UnityEngine;

namespace HTN
{
    public enum WSProperties
    {
        Hunger,
        IsTired,
        IsSleepy,
        HasTarget,
        HasFood,
        TargetRange,

        // Test Properties
        CanSeeEnemy,
        TrunkHealth,
        Location,
        Navigating,
        CanGiveBirth
    }

    public enum LocationState : byte
    {
        None = 0,

        Trunk,
        Enemy
    }

    public enum HungerState : byte
    {
        Starving,
        Hungry,
        Satisfied,
        Full
    }

    public enum ProximityRange : byte
    {
        Melee,
        Leap,
        ViewRange,
        OutOfRange
    }

    public struct DecompRecord
    {
        public List<byte> WorkingWS;
        public List<ITask> TasksToProcess;
        public List<PrimitiveTask> FinalPlan;
        public Method ChosenMethod;
    }

    public class Planner
    {
        public void SetRootTask(ITask rootTask) => m_RootTask = rootTask;

        public PrimitiveTask[] FindPlan(List<byte> currentWS)
        {
            m_TasksToProcess.Clear();
            m_FinalPlan.Clear();
            m_DecompHistory.Clear();

            List<byte> workingWS = new List<byte>(currentWS);
            m_TasksToProcess.Enqueue(m_RootTask);
            while (m_TasksToProcess.Count > 0)
            {
                ITask currentTask = m_TasksToProcess.Dequeue();
                if (currentTask.IsCompound())
                {
                    Method satisfiedMethod = ((CompoundTask)currentTask).FindSatisfiedMethod(workingWS);
                    if (satisfiedMethod != null)
                    {
                        RecordDecompositionOfTask(currentTask, satisfiedMethod, in workingWS);

                        foreach (ITask subTask in satisfiedMethod.SubTasks)
                        {
                            m_TasksToProcess.Enqueue(subTask);
                        }
                    }
                    else
                    {
                        if (!RestoreToLastDecomposedTask(out workingWS))
                        {
                            Debug.LogWarning("Failed to find a new plan!");
                            return new PrimitiveTask[0];
                        }
                    }
                }
                else
                {
                    PrimitiveTask currentPrimitiveTask = (PrimitiveTask)currentTask;
                    if (currentPrimitiveTask.CheckConditionMet(workingWS))
                    {
                        currentPrimitiveTask.ApplyEffects(workingWS);
                        m_FinalPlan.Add(currentPrimitiveTask);
                    }
                    else
                    {
                        if (!RestoreToLastDecomposedTask(out workingWS))
                        {
                            Debug.LogWarning("Failed to find a new plan!");
                            return new PrimitiveTask[0];
                        }
                    }
                }
            }

            // Log Task Names in plan
            LogPlan(m_FinalPlan);

            return m_FinalPlan.ToArray();
        }

        private void RecordDecompositionOfTask(ITask currentTask, Method satisfiedMethod, in List<byte> workingWS)
        {
            DecompRecord record = new DecompRecord()
            {
                WorkingWS = new List<byte>(workingWS),
                TasksToProcess = new List<ITask>(m_TasksToProcess),
                FinalPlan = new List<PrimitiveTask>(m_FinalPlan),
                ChosenMethod = satisfiedMethod
            };
            record.TasksToProcess.Insert(0, currentTask);

            m_DecompHistory.Push(record);
        }

        private bool RestoreToLastDecomposedTask(out List<byte> workingWS)
        {
            if (m_DecompHistory.Count <= 0)
            {
                workingWS = new List<byte>();
                return false;
            }
                
            DecompRecord record = m_DecompHistory.Pop();

            workingWS = record.WorkingWS;
            m_TasksToProcess = new Queue<ITask>(record.TasksToProcess);
            m_FinalPlan = record.FinalPlan;

            return true;

            // TODO: Implement logic to skip the method that failed in the next iteration of the planning
        }

        private void LogPlan(List<PrimitiveTask> plan)
        {
            string[] taskNames = new string[plan.Count];
            for (int i = 0; i < plan.Count; i++)
            {
                taskNames[i] = plan[i].DebugName;
            }

            Debug.LogWarning("New Plan: " + string.Join(',', taskNames));
        }

        private Queue<ITask> m_TasksToProcess = new Queue<ITask>();
        private Stack<DecompRecord> m_DecompHistory = new Stack<DecompRecord>();

        private List<PrimitiveTask> m_FinalPlan = new List<PrimitiveTask>();

        private ITask m_RootTask;
    }
}
