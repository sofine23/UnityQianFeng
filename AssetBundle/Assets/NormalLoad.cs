using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NormalLoad : MonoBehaviour
{
    /// <summary>
    /// 本地文件在Unity Asset下的路径
    /// </summary>
    private string localhostFileAssetHead;

    private string serverFileHead;

    [Header("本地Bundle文件的输出路径")]
    public string localPath = "AssetBundles";

    [Header("资源的右下角的Bundle名称")]
    public string loadAssetBundleName = @"res/model";

    [Header("资源的File名称")]
    public string loadAssetFileName = "Model";

    //总的资源列表说明文件
    private string manifestName;

    //全路径
    private string manifestFullPath;

    private void Awake()
    {
        localhostFileAssetHead = @"File:///" + Application.dataPath;
        serverFileHead = @"http://localhost";

        if (localPath.Contains("/"))
        {
            //拿到资源所在文件夹的名称
            manifestName = localPath.Substring(localPath.LastIndexOf("/") + 1);
        }
        else
        {
            manifestName = localPath;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //LoadByFile();
        StartCoroutine(LoadByWebRequest());
    }

    /// <summary>
    /// 通过文件加载
    /// </summary>
    private void LoadByFile()
    {
        //if (localPath.Contains("/"))
        //{
        //    //拿到资源所在文件夹的名称
        //    manifestName = localPath.Substring(localPath.LastIndexOf("/") + 1);
        //}
        //else
        //{
        //    manifestName = localPath;
        //}

        //示例 D://Project/Assets]/[Res/AssetBundles]/[AssetBundle]
        manifestFullPath = Application.dataPath + "/" + localPath + "/" + manifestName;

        //加载manifest的bundle
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(manifestFullPath);

        //加载真正的manifest问件
        AssetBundleManifest manifestFile = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //Debug.Log(manifestFile);
        //获取要加载的资源的所有依赖
        string[] bundleDependenciesStr = manifestFile.GetAllDependencies(loadAssetBundleName);

        //卸载manifestBundle的文件
        manifestBundle.Unload(true);
        AssetBundle[] dependenciesBundles = new AssetBundle[bundleDependenciesStr.Length];

        for (int i = 0; i < bundleDependenciesStr.Length; i++)
        {
            //每个依赖文件的全路径
            string depPath = Application.dataPath + "/" + localPath + "/" + bundleDependenciesStr[i];
            //Debug.Log(depPath);
            //加载依赖文件
            dependenciesBundles[i] = AssetBundle.LoadFromFile(depPath);
        }

        //真正的资源的全路径
        string realAssetPath = Application.dataPath + "/" + localPath + "/" + loadAssetBundleName;

        //加载真实Bundle
        AssetBundle realBundle = AssetBundle.LoadFromFile(realAssetPath);

        //Load预设体
        UnityEngine.Object realObj = realBundle.LoadAsset(loadAssetFileName);
        Debug.Log(realObj);
        if (realObj is GameObject)
        {
            Instantiate(realObj, Vector3.zero, Quaternion.identity);
        }
        else if (realObj is AudioClip)
        {
        }
        else if (realObj is Texture)
        {
        }
        //卸真正加载的包
        realBundle.Unload(true);
        //所有文件加载完成后再卸载 依赖
        for (int i = 0; i < dependenciesBundles.Length; i++)
        {
            dependenciesBundles[i].Unload(false);
        }
    }

    /// <summary>
    /// 通过网络请求加载
    /// </summary>
    private IEnumerator LoadByWebRequest()
    {
        //if (localPath.Contains("/"))
        //{
        //    //拿到资源所在文件夹的名称
        //    manifestName = localPath.Substring(localPath.LastIndexOf("/") + 1);
        //}
        //else
        //{
        //    manifestName = localPath;
        //}

        //示例 D://Project/Assets]/[Res/AssetBundles]/[AssetBundle]
        manifestFullPath = localhostFileAssetHead + "/" + localPath + "/" + manifestName;

        yield return GetBundleByWebRequest(manifestFullPath);
        AssetBundle manifestBundle = webBundle;

        //加载manifest的bundle

        //加载真正的manifest问件
        AssetBundleManifest manifestFile = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //Debug.Log(manifestFile);
        //获取要加载的资源的所有依赖
        string[] bundleDependenciesStr = manifestFile.GetAllDependencies(loadAssetBundleName);

        //卸载manifestBundle的文件
        manifestBundle.Unload(true);

        AssetBundle[] dependenciesBundles = new AssetBundle[bundleDependenciesStr.Length];

        for (int i = 0; i < bundleDependenciesStr.Length; i++)
        {
            //每个依赖文件的全路径
            string depPath = localhostFileAssetHead + "/" + localPath + "/" + bundleDependenciesStr[i];
            //Debug.Log(depPath);
            ////加载依赖文件
            //dependenciesBundles[i] = AssetBundle.LoadFromFile(depPath);

            yield return GetBundleByWebRequest(depPath);
            dependenciesBundles[i] = webBundle;
        }

        //真正的资源的全路径
        string realAssetPath = localhostFileAssetHead + "/" + localPath + "/" + loadAssetBundleName;

        ////加载真实Bundle
        //AssetBundle realBundle = AssetBundle.LoadFromFile(realAssetPath);

        yield return GetBundleByWebRequest(realAssetPath);
        AssetBundle realBundle = webBundle;

        //Load预设体
        UnityEngine.Object realObj = realBundle.LoadAsset(loadAssetFileName);
        //Debug.Log(realObj);
        if (realObj is GameObject)
        {
            Instantiate(realObj, Vector3.zero, Quaternion.identity);
        }
        else if (realObj is AudioClip)
        {
        }
        else if (realObj is Texture)
        {
        }
        //卸真正加载的包
        realBundle.Unload(true);
        //所有文件加载完成后再卸载 依赖
        for (int i = 0; i < dependenciesBundles.Length; i++)
        {
            dependenciesBundles[i].Unload(false);
        }
    }

    //用于存储每次调用【GetBundleByWebRequest】后 从网络下载的AssetBundle
    private AssetBundle webBundle;

    private IEnumerator GetBundleByWebRequest(string uri)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);
#endif

        yield return request.SendWebRequest();
        webBundle = DownloadHandlerAssetBundle.GetContent(request);
    }
}