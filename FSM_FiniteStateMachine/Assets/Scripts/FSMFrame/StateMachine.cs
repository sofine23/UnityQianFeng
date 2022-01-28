using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSM
{
    public class StateMachine : State
    {
        public StateMachine(string name) : base(name)
        {
            manageredState = new Dictionary<string, State>();
            StateMachineEventBind();
        }

        /// <summary>
        /// 绑定初始事件
        /// </summary>
        private void StateMachineEventBind()
        {
            OnStateUpdate += CheckCurrentStateTransition;
        }

        #region Manager States

        /// <summary>
        /// 默认状态
        /// </summary>
        private State defaultState;

        /// <summary>
        /// 当前状态
        /// </summary>
        private State currentState;

        /// <summary>
        /// 被管理的状态 string 状态名 State 对应的状态
        /// </summary>
        private Dictionary<string, State> manageredState;

        /// <summary>
        /// 添加状态,添加的第一个状态为默认状态
        /// </summary>
        /// <param name="stateName"></param>
        public State AddState(string stateName)
        {
            if (IsRun)
            {
                Debug.LogWarning("状态已经启动，不能进行状态的Add");
                return null;
            }
            if (manageredState.ContainsKey(stateName))
            {
                Debug.LogWarning($"已经有{stateName}状态");
                return manageredState[stateName];
            }
            State crtState = new State(stateName);
            //添加状态
            manageredState.Add(stateName, crtState);

            if (manageredState.Count == 1)//当当前添加的状态为第一个状态时
            {
                //设置默认状态
                defaultState = crtState;
            }
            return crtState;
        }

        /// <summary>
        /// 添加状态,添加的第一个状态为默认状态
        /// </summary>
        /// <param name="crtState"></param>
        public void AddState(State crtState)
        {
            if (IsRun)
            {
                Debug.LogWarning("状态已经启动，不能进行状态的Add");
                return;
            }
            if (manageredState.ContainsValue(crtState))
            {
                Debug.LogWarning($"已经有{crtState.StateName}状态");
                return;
            }
            //添加状态
            manageredState.Add(crtState.StateName, crtState);

            if (manageredState.Count == 1)//当当前添加的状态为第一个状态时
            {
                //清除上一次的默认状态
                defaultState = null;
                //设置默认状态
                defaultState = crtState;
            }
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="stateName"></param>
        public void RemoveState(string stateName)
        {
            if (IsRun)
            {
                Debug.LogWarning("状态已经启动，不能进行状态的Remove");
                return;
            }
            //如果状态存在
            if (manageredState.ContainsKey(stateName))
            {
                //当前要删除的状态
                State state = manageredState[stateName];
                //移除状态
                manageredState.Remove(stateName);
                if (state == defaultState)
                {
                    SetNewDefaultState(null);
                }
            }
        }

        /// <summary>
        /// 设置状态机的默认状态
        /// </summary>
        /// <param name="defaultState">如需自动选择新状态，则参数为null</param>
        public void SetNewDefaultState(State defaultState)
        {
            if (defaultState != null && manageredState.ContainsValue(defaultState))
            {
                this.defaultState = defaultState;
                return;
            }
            //遍历到的第一个即设置为默认状态
            foreach (State item in manageredState.Values)
            {
                defaultState = item;
                return;
            }
        }

        #endregion Manager States

        #region State Machine Event

        /// <summary>
        /// 状态机的进入
        /// </summary>
        /// <param name="enterEventParameters"></param>
        /// <param name="updataEventParameters"></param>
        public override void EnterState(object[] enterEventParameters, object[] updataEventParameters)
        {
            //先执行当前状态机的Enter
            base.EnterState(enterEventParameters, updataEventParameters);
            //再执行子状态的 Enter

            if (defaultState == null)//判断是否有默认状态
                return;
            //设置当前状态就是默认状态
            currentState = defaultState;
            //当前子状态执行Enter事件(进入默认的子状态)
            currentState.EnterState(enterEventParameters, updataEventParameters);
        }

        /// <summary>
        /// 状态机的离开
        /// </summary>
        /// <param name="Parameters"></param>
        public override void ExitState(object[] parameters)
        {
            //如果当前状态不为null
            if (currentState != null)
            {
                //当前状态先离开
                currentState.ExitState(parameters);
            }
            //状态机再离开
            base.ExitState(parameters);
        }

        #endregion State Machine Event

        #region StateMachine Check State Transition

        /// <summary>
        /// 检测当前状态是否满足过渡条件，满足则过渡
        /// </summary>
        private void CheckCurrentStateTransition(object[] objs)
        {
            string targetState = currentState.CheckTransition();
            //如果触发满足过渡状态的事件
            if (targetState != null)
            {
                TransitionState(targetState);
            }
        }

        /// <summary>
        /// 过渡到目标状态
        /// </summary>
        /// <param name="targetStateName"></param>
        private void TransitionState(string targetStateName)
        {
            //要过渡的状态是否被 [当前状态机] 所管理    不能跨状态机
            if (manageredState.ContainsKey(targetStateName))
            {
                //当前状态离开
                currentState.ExitState(null);
                //切换到目标状态
                currentState = manageredState[targetStateName];
                //目标状态执行进入方法
                currentState.EnterState(null, null);
            }
        }

        #endregion StateMachine Check State Transition
    }
}