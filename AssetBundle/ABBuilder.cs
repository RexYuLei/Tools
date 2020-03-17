using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ABBuilder : Singleton<ABBuilder>
{
    public enum BuildPlatform
    {
        Windows     = BuildTarget.StandaloneWindows,      //Windows
        Windows64   = BuildTarget.StandaloneWindows64,    //Windows64
        OSX         = BuildTarget.StandaloneOSX,          //OSX
        IOS         = BuildTarget.iOS,                    //IOS
        Android     = BuildTarget.Android,                //Android
    }

    public class TempBundleEntry
    {
        public string Path;
        public int CircleRefCount;
    }

    /// <summary>
    /// 输出的AssetBundle的目录
    /// </summary>
    public static string AssetBundlePath = "Assets/../Assetbundles";
    
    /// <summary>
    /// 资源包配置文件路径
    /// </summary>
    public static string ABEntryConfigPath = "Assets/Plugins/Common/Assetbundle/Editor/Assetbundle_Settings.json";

    /// <summary>
    /// 当前工程平台
    /// </summary>
    public BuildPlatform CurBuildPlatform = BuildPlatform.Windows;

    /// <summary>
    /// 资源项的配置缓存
    /// </summary>
    public List<ABEntry> ABEntries;

    protected override void Initialize()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 得到AssetBundle的路径前缀，根据不同的
    /// </summary>
    /// <returns></returns>
    public string GetPathPrefix_AssetBundle()
    {
        return Path.Combine(AssetBundlePath, GetManifestName()).Replace("\\", "/");
    }

    /// <summary>
    /// 等到Manifest名字
    /// </summary>
    /// <returns></returns>
    public string GetManifestName()
    {
        return GetCurrentBuildPlatformName() + "_Assetbundles";
    }

    public static string GetCurrentBuildPlatformName()
    {
        string rPlatformName = GetCurrentBuildPlatform().ToString();
        if (rPlatformName.Equals("Windows64"))
        {
            rPlatformName = "Windows";
        }

        return rPlatformName;
    }

    public static BuildPlatform GetCurrentBuildPlatform()
    {
        return (BuildPlatform) EditorUserBuildSettings.activeBuildTarget;
    }


    /// <summary>
    /// 打包资源
    /// </summary>
    /// <param name="rOptions"></param>
    public void BuildAssetBundles(BuildAssetBundleOptions rOptions)
    {
        List<AssetBundleBuild> rABBList = AssetBundleEntry_Building();

        string rABPath = GetPathPrefix_AssetBundle();
        DirectoryInfo rDirInfo = new DirectoryInfo(rABPath);
        if (!rDirInfo.Exists)
            rDirInfo.Create();            
        
        //开始打包
        rABBList.RemoveAll(rItem =>
        {
            return rItem.assetBundleName.EndsWith("/");
        });
        var rABMainfest =
            BuildPipeline.BuildAssetBundles(rABPath, rABBList.ToArray(), rOptions, (BuildTarget) CurBuildPlatform);
        //检查资源是否循环依赖了
        this.CheckABIsCircleDependence(rABMainfest);
        
        Debug.Log("资源打包完成");
    }

    /// <summary>
    /// 检查资源是否循环依赖
    /// </summary>
    /// <param name="rAbManifest"></param>
    public void CheckABIsCircleDependence(AssetBundleManifest rABManifest)
    {
        var rAllAssetBundles = rABManifest.GetAllAssetBundles();
        var rTempBundleEntries = new List<TempBundleEntry>();
        for (int i = 0; i < rAllAssetBundles.Length; i++)
        {
            rTempBundleEntries.Add(new TempBundleEntry() {Path = rAllAssetBundles[i], CircleRefCount = 0});
        }

        for (int i = 0; i < rTempBundleEntries.Count; i++)
        {
            for (int k = 0; k < rTempBundleEntries.Count; k++)
            {
                rTempBundleEntries[k].CircleRefCount = 0;
            }
            if (this.CheckABIsCircleDependence(rTempBundleEntries, rABManifest, rTempBundleEntries[i]))
            {
                Debug.LogError("Is a circle dependence: " + rTempBundleEntries[i].Path);
            }
        }
    }

    private bool CheckABIsCircleDependence(List<TempBundleEntry> rTempBundleEntries, AssetBundleManifest rABManifest,
        TempBundleEntry rBundleEntry)
    {
        rBundleEntry.CircleRefCount++;
        if (rBundleEntry.CircleRefCount > 1) return true;

        var rDependences = rABManifest.GetDirectDependencies(rBundleEntry.Path);
        for (int i = 0; i < rDependences.Length; i++)
        {
            var rDependenceEntry = rTempBundleEntries.Find((rItem) => { return rItem.Path.Equals(rDependences[i]); });
            return this.CheckABIsCircleDependence(rTempBundleEntries, rABManifest, rDependenceEntry);
        }
        return false;
    }

    /// <summary>
    /// 构建需要打包的资源的路径、包名以及包的后缀
    /// </summary>
    /// <returns></returns>
    public List<AssetBundleBuild> AssetBundleEntry_Building()
    {
        this.ABEntries = this.GenerateAssetBundleEntries();
        if (ABEntries == null)
        {
            ABEntries = new List<ABEntry>();
        }
        
        //资源预处理
        List<ABEntryProcesser> rABEntryProcessors = new List<ABEntryProcesser>();
        foreach (var rEntry in ABEntries)
        {
            ABEntryProcesser rProcesser = ABEntryProcesser.Create(rEntry);
            rProcesser.PreprocessAssets();
            rProcesser.ProcessAssetBundleLabel();
            rABEntryProcessors.Add(rProcesser);
        }
        //打包
        List<AssetBundleBuild> rABBList = new List<AssetBundleBuild>();
        foreach (var rProcessor in rABEntryProcessors)
        {
            rABBList.AddRange(rProcessor.ToABBuild());
        }

        return rABBList;
    }

    /// <summary>
    /// 生成AB包的配置项
    /// </summary>
    /// <returns></returns>
    public List<ABEntry> GenerateAssetBundleEntries()
    {
        var rABEntryConfig = ABBuilder.ReceiveAsset(ABEntryConfigPath);
        if (rABEntryConfig == null)
        {
            return new List<ABEntry>();
        }

        return rABEntryConfig.ABEntries;
    }

    public static ABEntryConfig ReceiveAsset(string rAssetPath)
    {
        ABEntryConfig rObj = null;
        if (File.Exists(rAssetPath))
        {
            rObj = JsonUtility.FromJson<ABEntryConfig>(File.ReadAllText(rAssetPath));
        }
        if (rObj == null)
        {
            rObj= new ABEntryConfig();
            var rJsonString = JsonUtility.ToJson(rObj);
            File.WriteAllText(rAssetPath,rJsonString);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        return rObj;
    }
}
