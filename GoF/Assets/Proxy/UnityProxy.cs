using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class UnityProxy : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<DelayProxy>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MethodInfo method = GetType().GetMethod("ShowCurrentTime");
            DelayProxy.instance.DelayCallByReflection(3f, method, this, new object[] { Time.time });
        }
    }

    public void ShowCurrentTime(float crtTime)
    {
        Debug.Log("移动延时时间：" + crtTime);
        Debug.Log("执行时间：" + Time.time);
    }
}

public class DelayProxy : MonoBehaviour
{
    public static DelayProxy instance;

    private void Awake()
    {
        instance = this;
    }

    public void DelayCallByReflection
        (float delayTime, MethodInfo info, object obj, object[] parameters)
    {
        StartCoroutine(IEDelayCallByReflection
            (delayTime, info, obj, parameters));
    }

    private IEnumerator IEDelayCallByReflection
        (float delayTime, MethodInfo info, object obj, object[] parameters)
    {
        yield return new WaitForSeconds(delayTime);
        info.Invoke(obj, parameters);
    }
}