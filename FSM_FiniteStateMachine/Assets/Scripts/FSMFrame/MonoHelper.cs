using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FSM
{
    public class MonoHelper : MonoBehaviour
    {
        public class StateUpdateModule
        {
            public StateUpdateModule(Action<object[]> stateUpdateEvent, object[] stateUpdateEventPatamters)
            {
                this.stateUpdateEvent = stateUpdateEvent;
                this.stateUpdateEventPatamters = stateUpdateEventPatamters;
            }

            /// <summary>
            /// 状态更新事件
            /// </summary>
            public Action<object[]> stateUpdateEvent;

            /// <summary>
            /// 触发的参数
            /// </summary>
            public object[] stateUpdateEventPatamters;
        }

        public static MonoHelper Instance;

        /// <summary>
        /// 状态更新模块字典
        /// </summary>
        private Dictionary<string, StateUpdateModule> stateUpdateModuleDic;

        /// <summary>
        /// 状态更新模块的数组
        /// </summary>
        private StateUpdateModule[] stateUpdateModuleArray;

        #region 解决 Dic和List不能 边遍历 变动态修改Item的问题

        /// <summary>
        /// 字典中的数据导入到数组中
        /// </summary>
        private void DicToArray()
        {
            stateUpdateModuleArray = new StateUpdateModule[stateUpdateModuleDic.Count];
            int counter = 0;
            foreach (var item in stateUpdateModuleDic.Values)
            {
                stateUpdateModuleArray[counter] = item;
                counter++;
            }
        }

        #endregion 解决 Dic和List不能 边遍历 变动态修改Item的问题

        private void Awake()
        {
            Instance = this;
            stateUpdateModuleDic = new Dictionary<string, StateUpdateModule>();
        }

        [Header("更新事件执行的时间间隔")]
        public float invokeInterval = -1;

        /// <summary>
        /// 添加更新事件
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="updateEvent"></param>
        /// <param name="updateEventParameters"></param>
        public void AddUpdateEvent(string stateName, Action<object[]> updateEvent, object[] updateEventParameters)
        {
            StateUpdateModule newState = new StateUpdateModule(updateEvent, updateEventParameters);
            //如果字典中存在 该状态 则更新
            if (stateUpdateModuleDic.ContainsKey(stateName))
            {
                stateUpdateModuleDic[stateName] = newState;
            }
            else
            {
                stateUpdateModuleDic.Add(stateName, newState);
            }
            //更改更新的事件之后执行
            DicToArray();
        }

        /// <summary>
        /// 移除更新事件
        /// </summary>
        /// <param name="stateName"></param>
        public void RemoveUpdateEvent(string stateName)
        {
            if (stateUpdateModuleDic.ContainsKey(stateName))
            {
                //移除状态
                stateUpdateModuleDic.Remove(stateName);
                //更改更新的事件之后执行
                DicToArray();
            }
        }

        private void Test(object[] objs)
        {
            Debug.Log(objs.Length);
        }

        // Start is called before the first frame update
        private IEnumerator Start()
        {
            //AddUpdateEvent("TestState", Test, new object[] { 1, 2, 3 });

            while (true)
            {
                if (invokeInterval <= 0)//每帧执行一次 为默认值
                {
                    yield return 0;
                }
                else
                {
                    yield return new WaitForSeconds(invokeInterval);
                }
                //执行update事件
                for (int i = 0; i < stateUpdateModuleArray.Length; i++)
                {
                    if (stateUpdateModuleArray[i].stateUpdateEvent != null)
                    {
                        stateUpdateModuleArray[i].stateUpdateEvent(stateUpdateModuleArray[i].stateUpdateEventPatamters);
                    }
                }
                //foreach (var item in stateUpdateModuleDic)//？？？
                //{
                //    if (item.Value.stateUpdateEvent != null)
                //    {
                //        item.Value.stateUpdateEvent(item.Value.stateUpdateEventPatamters);
                //    }
                //}
            }
        }
    }
}