using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class State
    {
        public State(string name)
        {
            StateName = name;
            TransitionStates = new Dictionary<string, Func<bool>>();
            StateBaseEventBind();
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; private set; }

        /// <summary>
        /// 标记当前状态是否正在运行
        /// </summary>
        public bool IsRun { get; set; }

        #region State Base EventBinding

        /// <summary>
        /// 绑定基础回调
        /// </summary>
        private void StateBaseEventBind()
        {
            //进入状态标记
            OnStateEnter += objects => { IsRun = true; };
            //离开状态标记
            OnStateExit += objects => { IsRun = false; };
        }

        #endregion State Base EventBinding

        #region Transition States Control

        /// <summary>
        /// 当前状态机可切换的其他状态
        /// </summary>
        private Dictionary<string, Func<bool>> TransitionStates;

        /// <summary>
        /// 注册状态切换
        /// </summary>
        /// <param name="stateName">要切换的状态名称</param>
        /// <param name="condition">切换条件 只返回一个结果</param>
        public void RegisterTransitionState(string stateName, Func<bool> condition)
        {
            //若没有要切换的状态名称 Add
            if (!TransitionStates.ContainsKey(stateName))
            {
                TransitionStates.Add(stateName, condition);
            }
            else
            {
                TransitionStates[stateName] = condition;
            }
        }

        /// <summary>
        /// 取消注册切换的状态
        /// </summary>
        /// <param name="stateName"></param>
        public void UnRegisterTransitionState(string stateName)
        {
            if (TransitionStates.ContainsKey(stateName))
            {
                //移除
                TransitionStates.Remove(stateName);
            }
        }

        #endregion Transition States Control

        #region State Machine Event

        /// <summary>
        /// 状态进入事件
        /// </summary>
        public event Action<object[]> OnStateEnter;

        /// <summary>
        /// 在状态中持续调用
        /// </summary>
        public event Action<object[]> OnStateUpdate;

        /// <summary>
        /// 状态离开事件
        /// </summary>
        public event Action<object[]> OnStateExit;

        public virtual void EnterState(object[] enterEventParameters, object[] updataEventParameters)
        {
            if (OnStateEnter != null)
            {
                //执行进入状态的事件
                OnStateEnter(enterEventParameters);
            }
            //绑定当前状态的更新事件，以便后期执行
            MonoHelper.Instance.AddUpdateEvent(StateName, OnStateUpdate, updataEventParameters);
        }

        public virtual void ExitState(object[] parameters)
        {
            //移除 当前状态的更新事件
            MonoHelper.Instance.RemoveUpdateEvent(StateName);
            if (OnStateExit != null)
            {
                //执行离开状态
                OnStateExit(parameters);
            }
        }

        #endregion State Machine Event

        #region State Transition

        /// <summary>
        /// 检测状态过渡
        /// </summary>
        /// <returns>满足过渡状态条件的状态名称</returns>
        public string CheckTransition()
        {
            foreach (var item in TransitionStates)
            {
                //判断执行的条件
                if (item.Value())
                {
                    //条件满足，过渡状态
                    return item.Key;
                }
            }
            return null;
        }

        #endregion State Transition
    }
}