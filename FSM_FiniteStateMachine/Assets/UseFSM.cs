using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class UseFSM : MonoBehaviour
{
    /// <summary>
    /// 总的状态机leader
    /// </summary>
    private StateMachine leader;

    //通过更改速度参数 实现切换，切换条件在Start的过渡效果里面
    [Header("速度参数")]
    [Range(0, 10)]
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        /*
         可以通过继承State实现同一种状态实现同一种效果
         比如 眩晕状态执行眩晕效果……

        每个状态 需要哪个事件 +=哪个事件
         */
        leader = new StateMachine("Leader");
        //创建子状态
        State idleState = new State("Idle");
        State walkState = new State("Walk");
        State runState = new State("Run");
        //创建子状态机
        StateMachine locomotionState = new StateMachine("Locomotion");

        //建立状态关系
        locomotionState.AddState(walkState);
        locomotionState.AddState(runState);
        //设置默认状态
        locomotionState.SetNewDefaultState(walkState);

        leader.AddState(idleState);
        leader.AddState(locomotionState);
        leader.SetNewDefaultState(idleState);

        #region Add State Event

        //添加状态事件，需要哪个事件 +=哪个事件
        leader.OnStateEnter += objects => { Debug.Log("Leader Enter"); };
        leader.OnStateUpdate += objects => { Debug.Log("Leader Update"); };
        leader.OnStateExit += objects => { Debug.Log("Leader Exit"); };

        locomotionState.OnStateEnter += objects => { Debug.Log("locomotionState Enter"); };
        locomotionState.OnStateUpdate += objects => { Debug.Log("locomotionState Update"); };
        locomotionState.OnStateExit += objects => { Debug.Log("locomotionState Exit"); };

        idleState.OnStateEnter += objects => { Debug.Log("idleState Enter"); };
        idleState.OnStateUpdate += objects => { Debug.Log("idleState Update"); };
        idleState.OnStateExit += objects => { Debug.Log("idleState Exit"); };

        walkState.OnStateEnter += objects => { Debug.Log("walkState Enter"); };
        walkState.OnStateUpdate += objects => { Debug.Log("walkState Update"); };
        walkState.OnStateExit += objects => { Debug.Log("walkState Exit"); };

        runState.OnStateEnter += objects => { Debug.Log("runState Enter"); };
        runState.OnStateUpdate += objects => { Debug.Log("runState Update"); };
        runState.OnStateExit += objects => { Debug.Log("runState Exit"); };

        #endregion Add State Event

        //添加状态的过渡关系，切换的一定是同级别的State对象
        idleState.RegisterTransitionState("Locomotion", () => { return speed > 1; });
        locomotionState.RegisterTransitionState("Idle", () => { return speed <= 1; });

        walkState.RegisterTransitionState("Run", () => { return speed > 5; });
        runState.RegisterTransitionState("Walk", () => { return speed <= 5; });

        //启动状态机
        leader.EnterState(null, null);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}