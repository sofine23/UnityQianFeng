using UnityEditor;
using System.IO;
using UnityEngine;
using System;

public class CreateAssetBundle
{
    /// <summary>
    /// 需要打包的资源文件夹
    /// </summary>
    private static string needBuildPath = Application.dataPath + "/Res";

    /// <summary>
    /// 打包好后输出的文件夹
    /// </summary>
    private static string outPutPath = Application.dataPath + "/AssetBundles";

    //编辑器扩展
    [MenuItem("AssetBundle/Build AssetBundles")]
    public static void BuildAllAssetBubdle()
    {
        //如果不存在AssetBundle文件夹，手动创建
        if (!Directory.Exists(outPutPath))
        {
            Directory.CreateDirectory(outPutPath);
        }
        //将资源打包到AssetBundle文件夹下
        BuildPipeline.BuildAssetBundles(outPutPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// 全自动打包
    /// </summary>
    [MenuItem("AssetBundle/AutoBuild")]
    public static void AutoBuild()
    {
        ClearAllBundleFilesName();
        RemoveFile(outPutPath);
        SetAssetBundlesName(needBuildPath);
        BuildAllAssetBubdle();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 清空所有资源的Bundle名称
    /// </summary>
    private static void ClearAllBundleFilesName()
    {
        //获取所有资源的文件名
        string[] allPathName = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < allPathName.Length; i++)
        {
            //强制移除该文件的BudleName
            AssetDatabase.RemoveAssetBundleName(allPathName[i], true);
        }
        Debug.Log("清空所有的Bundle名称【完成】");
    }

    /// <summary>
    /// 清除output文件夹下的所有文件
    /// </summary>
    /// <param name="rootPath"></param>
    private static void RemoveFile(string outputPath)
    {
        if (Directory.Exists(outputPath))
        {
            DirectoryInfo rootInfo = new DirectoryInfo(outputPath);
            rootInfo.Delete(true);
        }
        Debug.Log("输出目录清除完毕");
    }

    /// <summary>
    /// 设置资源的BundleName
    /// </summary>
    /// <param name="rootPath"></param>
    private static void SetAssetBundlesName(string rootPath)
    {
        //获取根路径的文件夹
        DirectoryInfo rootInfo = new DirectoryInfo(rootPath);
        //获取根路径下的所有文件【包括文件or文件夹】
        FileSystemInfo[] fileInfos = rootInfo.GetFileSystemInfos();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileSystemInfo tempFileInfo = fileInfos[i];
            // Debug.Log(tempFileInfo.FullName);//是 \
            //如果该文件为文件夹
            if (tempFileInfo is DirectoryInfo)
            {
                //递归遍历子文件下的所有文件
                SetAssetBundlesName(fileInfos[i].FullName);
            }
            //该文件不是以.meta结尾
            else if (tempFileInfo.Extension.ToLower() != ".meta")
            {
                SetAssetBundleName(tempFileInfo.FullName);
            }
            //Debug.Log(tempFileInfo.Name);
            //Debug.Log(tempFileInfo.FullName);
            //Debug.Log(tempFileInfo.Extension);
        }
        Debug.Log("设置所有的Bundle名称【完成】");
    }

    /// <summary>
    /// 设置文件的Bundle名  【Res\blue】 以Res开头 以文件名结尾
    /// </summary>
    /// <param name="filePath">以\作为层级符号</param>
    private static void SetAssetBundleName(string filePath)
    {
        //全路径变为相对路径，相对于Res文件夹下的 Res\blue.mat
        string relativePath = filePath.Substring(Application.dataPath.Length + 1);

        //拼凑导入需要的相对路径
        string impoterPath = @"Assets\" + relativePath;

        //[去前缀] [Res\]blue.mat
        //relativePath = relativePath.Substring(relativePath.LastIndexOf(@"\") + 1);

        //[去后缀] blue[.mat]
        relativePath = relativePath.Remove(relativePath.LastIndexOf(@"."));
        Debug.Log(relativePath);
        //Debug.Log(Application.dataPath);

        //通过相对路径获得import对象
        AssetImporter assetImporter = AssetImporter.GetAtPath(impoterPath);
        if (assetImporter != null)
        {
            //设置Bundle名称
            assetImporter.assetBundleName = relativePath;
        }
    }
}