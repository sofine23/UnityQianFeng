using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此脚本也应挂在一个空对象上面
/// </summary>
public class UnitySingletonFactory : MonoBehaviour
{
    public static UnitySingletonFactory instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public T GetAsset<T>(string assetPath) where T : Object
    {
        return Resources.Load<T>(assetPath);
    }

    public T[] GetAssets<T>(string assetsPath) where T : Object
    {
        return Resources.LoadAll<T>(assetsPath);
    }
}