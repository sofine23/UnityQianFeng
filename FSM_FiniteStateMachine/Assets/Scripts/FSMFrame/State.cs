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
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        private string StateName { get; set; }

        #region Transition States Control

        /// <summary>
        /// 当前状态机可切换的其他状态
        /// </summary>
        private Dictionary<string, Func<bool>> TransitionStates;

        /// <summary>
        /// 注册状态切换
        /// </summary>
        /// <param name="stateName">要切换的状态名称</param>
        /// <param name="condition">切换条件</param>
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

        public void A()
        {
        }

        #endregion State Machine Event
    }

    public class
}