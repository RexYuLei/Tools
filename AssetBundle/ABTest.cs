using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public class ABTest : MonoBehaviour
{
    public string name;

    private void Awake()
    {
        ABLoad();
    }


    public void ABLoad()
    {

    }

    [MenuItem("Custom Editor/Create AssetBundles Main")]
    static void CreateAssetBundlesMain()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //遍历所有的游戏对象
        foreach (var obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
            //StreamingAssets是只读路径，不能写入
            //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
            string targetPath = Application.dataPath + "/StreamingAssets" + obj.name + ".assetbundle";
            if (BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows))
            {
                Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                Debug.Log(obj.name + "资源打包失败");
            }
        }

        //刷新编辑器
        AssetDatabase.Refresh();
    }

    [MenuItem("Custom Editor/Create AssetBundles All")]
    private static void CreateAseetBundlesAll()
    {
        Caching.ClearCache();

        string Path = Application.dataPath + "/StreamingAssets/ALL.assetbundle";
        Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
        
        if (BuildPipeline.BuildAssetBundles(Path, BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows))
        {
            
        } 
        else
        {
 
        }
        
        AssetDatabase.Refresh();
        }

    
    
    private IEnumerator LoadMainGameObject(string path)
    {
        //从服务器加载
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return uwr.SendWebRequest();
        //获得资源并实例化
        AssetBundle bundleWeb = DownloadHandlerAssetBundle.GetContent(uwr);
        var loadAsset = bundleWeb.LoadAsset<GameObject>("Assets/Players/MainPlayer.prefab");
        var loadAssetAsync = bundleWeb.LoadAssetAsync<GameObject>("Assets/Players/MainPlayer.prefab");
        yield return loadAsset;
        Instantiate(loadAsset);
        Instantiate(loadAssetAsync.asset);

        //从本地文件加载
        AssetBundle myLoadedAssetBundleFromFile =
            AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myAssetBundle"));
        if (myLoadedAssetBundleFromFile == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield return null;
        }
        var prefab = myLoadedAssetBundleFromFile.LoadAsset<GameObject>("MyObject");
        Instantiate(prefab);
        myLoadedAssetBundleFromFile.Unload(false);
        
        //从内存中加载
        var uwr2 = UnityWebRequest.Get(path);
        yield return uwr2.SendWebRequest();
        byte[] bytes = uwr2.downloadHandler.data;
        AssetBundle myLoadedAssetBundleFromMemory = AssetBundle.LoadFromMemory(bytes);//实例化同上
        
        //从托管流中加载
        var fileStream = new FileStream(Application.streamingAssetsPath, FileMode.Open, FileAccess.Read);
        var myLoadedAssetBundleFromStream = AssetBundle.LoadFromStream(fileStream);//实例化同上
        

        
        
        
        AssetBundle ab3 = AssetBundle.LoadFromFile(path);

    }
}

public class UnityResLoader
{


    public static void LoadAssetAsync<T>(string resName) where T : Object
    {
        if (string.IsNullOrEmpty(resName))
        {
            return;
        }
        
        //普通加载
        Resources.Load<T>("");

        //AB包资源加载
        

    }
}



