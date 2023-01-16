using UnityEngine;
using HTN;
using System.Collections.Generic;
using System.Linq;
using System;

public class TestBrain : MonoBehaviour
{
    private Context m_Context;
    private Planner m_Planner;
    private PlanRunner m_PlanRunner;

    private List<byte> m_CurrentWorldState = new List<byte>();

    public byte GetWSPropertie(WSProperties key)
    {
        return m_CurrentWorldState[(int)key];
    }

    public void SetWSPropertie(WSProperties key, byte value)
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

        // WorldState initialization
        int size = Enum.GetValues(typeof(WSProperties)).Length;
        m_CurrentWorldState.Capacity = size;
        byte defaultValue = default(byte);
        m_CurrentWorldState.AddRange(Enumerable.Repeat(defaultValue, size));

        m_PlanRunner.OnTaskFinished += TaskFinished;

        #region HTNDomain Initialization

        LogOperator doTrunkSlam = new LogOperator("Trunk Slam!", 500);
        LogOperator uprootTrunk = new LogOperator("Uprooted a trunk!", 3000);
        LogOperator navigateToTrunk = new LogOperator("Got to a near trunk!", 5000);

        LogOperator patrol = new LogOperator("Patrolling", 3000);

        PrimitiveTask doTrunkSlamTask = new PrimitiveTask(doTrunkSlam,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.TrunkHealth]--,
            "DoTrunkSlam");
        PrimitiveTask uprootTrunkTask = new PrimitiveTask(uprootTrunk,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.TrunkHealth] = 3,
            "UprootTrunk");
        PrimitiveTask navigateToTrunkTask = new PrimitiveTask(navigateToTrunk,
            (List<byte> ws) => true,
            (List<byte> ws) => ws[(int)WSProperties.Location] = (byte)LocationState.Trunk,
            "NavigateToTrunk");

        PrimitiveTask patrolTask = new PrimitiveTask(patrol,
            (List<byte> ws) => true,
            (List<byte> ws) => { },
            "Patrol");

        CompoundTask attackCompoundTask = new CompoundTask();

        Method trunkSlamMethod = new Method((List<byte> ws) => ws[(int)WSProperties.TrunkHealth] > 0, doTrunkSlamTask);
        Method findTrunkMethod = new Method((List<byte> ws) => true, navigateToTrunkTask, uprootTrunkTask, attackCompoundTask);

        attackCompoundTask.PopulateMethods(trunkSlamMethod, findTrunkMethod);

        CompoundTask beTrunkThumper = new CompoundTask();

        Method attackMethod = new Method((List<byte> ws) => ws[(int)WSProperties.CanSeeEnemy] == 1, attackCompoundTask);
        Method patrolMethod = new Method((List<byte> ws) => true, patrolTask);

        beTrunkThumper.PopulateMethods(attackMethod, patrolMethod);

        m_Planner.SetRootTask(beTrunkThumper);

        #endregion // HTNDomain Initialization

        #region WorldState Initialization

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
