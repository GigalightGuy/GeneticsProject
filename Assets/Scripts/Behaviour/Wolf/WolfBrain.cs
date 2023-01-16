using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HTN;
using AnimalBehaviour;
using System.Dynamic;

public class WolfBrain : MonoBehaviour
{
    private Context m_Context;
    private Planner m_Planner;
    private PlanRunner m_PlanRunner;

    private List<byte> m_CurrentWorldState = new List<byte>();

    public byte GetWSProperty(WSProperties key)
    {
        return m_CurrentWorldState[(int)key];
    }

    public void SetWSProperty(WSProperties key, byte value)
    {
        if (m_CurrentWorldState[(int)key] == value) return;

        m_CurrentWorldState[(int)key] = value;
        m_PlanRunner.WSIsDirty();
    }

    private void Start()
    {
        m_Context = GetComponent<Context>();

        m_Planner = new Planner();
        m_PlanRunner = new PlanRunner();

        m_PlanRunner.OnTaskFinished += TaskFinished;

        #region HTNDomain Initialization

        NavigateToOperator navigateToTarget = new NavigateToOperator();
        WanderOperator wander = new WanderOperator(150f);
        EatOperator eatFood = new EatOperator(2000);
        DashOperator jump = new DashOperator(40f);
        GiveBirthOperator giveBirth = new GiveBirthOperator(5000);
        RestOperator rest = new RestOperator(10000);

        PrimitiveTask navigateToTargetTask = new PrimitiveTask(navigateToTarget,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.Navigating] = 1,
            "NavigateToTarget");
        PrimitiveTask wanderTask = new PrimitiveTask(wander,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.Navigating] = 1,
            "Wander");
        PrimitiveTask eatFoodTask = new PrimitiveTask(eatFood,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.Hunger]++,
            "EatFood");
        PrimitiveTask jumpTask = new PrimitiveTask(jump,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.TargetRange] = (byte)ProximityRange.Melee,
            "Jump");
        PrimitiveTask giveBirthTask = new PrimitiveTask(giveBirth,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.Hunger]--,
            "GiveBirth");
        PrimitiveTask restTask = new PrimitiveTask(rest,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.IsTired] = 0,
            "Rest");

        CompoundTask attack = new CompoundTask();

        Method jumpMethod = new Method((List<byte> ws) => 
            ws[(int)WSProperties.TargetRange] <= (byte)ProximityRange.Leap, 
            jumpTask, attack);
        Method eatFoodMethod = new Method((List<byte> ws) => 
            ws[(int)WSProperties.TargetRange] <= (byte)ProximityRange.Melee, 
            eatFoodTask);

        attack.PopulateMethods(eatFoodMethod, jumpMethod);

        CompoundTask beWolf = new CompoundTask();

        Method birthMethod = new Method((List<byte> ws) =>
            ws[(int)WSProperties.CanGiveBirth] == 1 &&
            ws[(int)WSProperties.Hunger] >= (byte)HungerState.Full, 
            giveBirthTask);
        Method huntMethod = new Method((List<byte> ws) =>
            ws[(int)WSProperties.HasTarget] == 1 &&
            ws[(int)WSProperties.TargetRange] <= (byte)ProximityRange.Leap,
            attack);
        Method chaseTargetMethod = new Method((List<byte> ws) =>
            ws[(int)WSProperties.HasTarget] == 1,
            navigateToTargetTask);
        Method restMethod = new Method((List<byte> ws) => 
            ws[(int)WSProperties.Hunger] >= (byte)HungerState.Full, 
            restTask);
        Method findTargetMethod = new Method((List<byte> ws) =>
            ws[(int)WSProperties.Navigating] == 0, wanderTask);

        beWolf.PopulateMethods(birthMethod, huntMethod, chaseTargetMethod, restMethod, findTargetMethod);

        m_Planner.SetRootTask(beWolf);

        #endregion // HTNDomain Initialization

        #region WorldState Initialization

        int size = Enum.GetValues(typeof(WSProperties)).Length;
        m_CurrentWorldState.Capacity = size;
        byte defaultValue = default(byte);
        m_CurrentWorldState.AddRange(Enumerable.Repeat(defaultValue, size));

        m_CurrentWorldState[(int)WSProperties.CanSeeEnemy] = 0;
        m_CurrentWorldState[(int)WSProperties.TrunkHealth] = 3;
        m_CurrentWorldState[(int)WSProperties.Location] = (byte)LocationState.None;

        #endregion // WorldState Initialization
    }

    private void Update()
    {
        if (m_PlanRunner.NeedsNewPlan())
        {
            m_PlanRunner.SetNewPlan(m_Planner.FindPlan(m_CurrentWorldState));
        }

        m_PlanRunner.Tick(m_Context, m_CurrentWorldState);
    }

    private void TaskFinished()
    {
        m_PlanRunner.ApplyCurrentTaskEffects(m_CurrentWorldState);
    }
}
