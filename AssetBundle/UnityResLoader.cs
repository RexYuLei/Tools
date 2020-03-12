//#define EDITOR_USE_ASSETBUNDLE
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using Assets.Plugins.Common.Assetbundle;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class UnityResHandle
    {
        public string ResKey
        {
            get
            {
                return _resKey;
            }
        }
        public bool IsLoading
        {
            get
            {
                return _abRequest != null || _resReqeust != null;
            }
        }
        public Object Asset
        {
            get
            {
                return _asset;
            }
        }

        public Object SyncAsset
        {
            get
            {
                if (_asset != null)
                {
                    return _asset;
                }
                LoadSync();
                return _asset;
            }
        }

        public System.Action<UnityResHandle> Callbacks;

        private string _resKey;
        private Object _asset;
        private ABLoaderRequest _abRequest;
        private ResourceRequest _resReqeust;

        public UnityResHandle()
        {
            _resKey = string.Empty;
            _asset = null;
            _abRequest = null;
            _resReqeust = null;
        }
        public UnityResHandle(string resKey, Object asset)
        {
            _resKey = resKey;
            _asset = asset;
            _abRequest = null;
            _resReqeust = null;
        }

        public UnityResHandle(string resKey, ABLoaderRequest request)
        {
            _resKey = resKey;
            _asset = null;
            _resReqeust = null;
            _abRequest = request;
            _abRequest.LoadCompleted += OnAbLoadComplete;
        }

        public UnityResHandle(string resKey, ResourceRequest request)
        {
            _resKey = resKey;
            _asset = null;
            _abRequest = null;
            _resReqeust = request;
            _resReqeust.completed += OnResLoadComplete;
        }

        void OnAbLoadComplete(ABLoaderRequest req)
        {
            if (_abRequest == req)
            {
                _asset = req.Asset;
                _abRequest = null;
                NotifyFinish();
            }
        }

        void OnResLoadComplete(AsyncOperation req)
        {
            if (_resReqeust == req)
            {
                _asset = _resReqeust.asset;
                _resReqeust = null;
                NotifyFinish();
            }
        }

        public void Clear()
        {
            if (_abRequest != null)
            {
                _abRequest.LoadCompleted = null;
                _abRequest.Stop();
                _abRequest = null;
            }
            _asset = null;
            Callbacks = null;
        }

        public void LoadSync()
        {
            if (_abRequest != null)
            {
                _abRequest.LoadCompleted = null;
                _abRequest.Stop();
                ABLoader.Instance.LoadAssetSync(_abRequest);
                _asset = _abRequest.Asset;
                _abRequest = null;
                NotifyFinish();
            }
            else if (_resReqeust != null)
            {
                if (_resReqeust.isDone)
                {
                    _asset = _resReqeust.asset;
                }
                else
                {
                    _asset = Resources.Load(_resKey);
                }
                _resReqeust = null;
                NotifyFinish();
            }
        }

        public bool RecheckFinish()
        {
            if (_abRequest != null)
            {
                return false;
            }
            NotifyFinish();
            return true;
        }


        void NotifyFinish()
        {
            var callbacks = Callbacks;
            Callbacks = null;
            if (callbacks != null)
            {
                callbacks(this);
            }
        }
    }


    public class UnityResLoader : Singleton<UnityResLoader>
    {
        private class UnityResABPath
        {
            public string ResName;
            public string ABName;
            public string AssetName;
        }

        private static readonly Dictionary<string, UnityResHandle> ResHandleDict = new Dictionary<string, UnityResHandle>();
        public static readonly UnityResHandle ErrorResHandle = new UnityResHandle();

        private static readonly Dictionary<string, UnityResABPath> ABPathDict = new Dictionary<string, UnityResABPath>();


#if UNITY_EDITOR
        public static bool ForbidLoad = false;
#endif
        public static UnityResHandle GetResHandle(string resName)
        {
            if (string.IsNullOrEmpty(resName))
            {
                return ErrorResHandle;
            }
            UnityResHandle handle;
            ResHandleDict.TryGetValue(resName, out handle);
            return handle;
        }

        public static GameObject LoadAsset(string resName)
        {
            return LoadAsset<GameObject>(resName);
        }

        /// <summary>
        /// 路径解析
        /// 1. "UI/Prefabs/Common/UISystem"             没有冒号区别开的是加载一个资源包里面只有一个资源的情况
        /// 2. "ArtResources/Sound:UISystem"            有冒号区别开了的是加载一个资源包里面对应的一个资源的情况
        /// </summary>
        public static T LoadAsset<T>(string resName) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(resName))
            {
                return null;
            }
            UnityResHandle handle;
            if (ResHandleDict.TryGetValue(resName, out handle))
            {
                if (handle.IsLoading)
                {
                    handle.LoadSync();
                }
                if (handle.Asset != null)
                    return handle.Asset as T;
            }

            Object obj = null;

            // 冒号开头代表resource资源
            if (resName.StartsWith(":", StringComparison.CurrentCulture))
            {
                obj = Resources.Load(resName.Substring(1));
            }
            else
            {
                UnityResABPath resABPath = null;
                if (!ABPathDict.TryGetValue(resName, out resABPath))
                {
                    resABPath = GetResABPathByResName(resName);
                    ABPathDict.Add(resName, resABPath);
                }
                if (ABLoader.Instance != null)
                {
                    var loadRequest = ABLoader.Instance.LoadAssetSync(resABPath.ABName, resABPath.AssetName);
                    obj = loadRequest.Asset;
                }
            }

            handle = new UnityResHandle(resName, obj);
            ResHandleDict[resName] = handle;
            return obj as T;
        }

        public static Scene LoadScene(string resName, LoadSceneMode rLoadSceneMode)
        {
            UnityResABPath resABPath = null;
            if (!ABPathDict.TryGetValue(resName, out resABPath))
            {
                resABPath = GetResABPathByResName(resName);
                ABPathDict.Add(resName, resABPath);
            }
            if (ABLoader.Instance == null) return new Scene();
            var loadRequest = ABLoader.Instance.LoadSceneSync(resABPath.ABName, resABPath.AssetName, rLoadSceneMode);
            if (loadRequest.Asset == null) return new Scene();
            return loadRequest.Scene;
        }

        public static ABLoaderRequest LoadSceneAsync(string resName, LoadSceneMode rLoadSceneMode, Action<Scene> loadCompleted = null, bool allowActivation = true)
        {
            UnityResABPath resABPath = null;
            if (!ABPathDict.TryGetValue(resName, out resABPath))
            {
                resABPath = GetResABPathByResName(resName);
                ABPathDict.Add(resName, resABPath);
            }
            return ABLoader.Instance.LoadSceneAsync(resABPath.ABName, resABPath.AssetName, rLoadSceneMode, (rLoadRequest) =>
            {
                if (rLoadRequest.Asset != null && loadCompleted != null)
                {
                    loadCompleted(rLoadRequest.Scene);
                }
            }, allowActivation);
        }

        public static UnityResHandle LoadAssetAsync<T>(string resName) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(resName))
            {
                return ErrorResHandle;
            }

            UnityResHandle handle;
            if (ResHandleDict.TryGetValue(resName, out handle))
            {
                if (handle.Asset != null)
                    return handle;
            }
            if (resName.StartsWith(":", StringComparison.CurrentCulture))
            {
                var req = Resources.LoadAsync<T>(resName.Substring(1));
                if (req.isDone)
                {
                    handle = new UnityResHandle(resName, req.asset);
                }
                else
                {
                    handle = new UnityResHandle(resName, req);
                }
            }
            else
            {
                UnityResABPath resABPath = null;
                if (!ABPathDict.TryGetValue(resName, out resABPath))
                {
                    resABPath = GetResABPathByResName(resName);
                    ABPathDict.Add(resName, resABPath);
                }
                var req = ABLoader.Instance.CreateLoadAssetRequest(resABPath.ABName, resABPath.AssetName, false, typeof(T));
                handle = new UnityResHandle(resName, req);
                ABLoader.Instance.StartRequest(req);
            }

            ResHandleDict[resName] = handle;
            return handle;
        }

        public static void UnloadAsset(string resName)
        {
            if (string.IsNullOrEmpty(resName))
            {
                return;
            }
            UnityResHandle handle;
            if (ResHandleDict.TryGetValue(resName, out handle))
            {
                handle.Clear();
                ResHandleDict.Remove(resName);
            }
            UnityResABPath resABPath = null;
            if (!ABPathDict.TryGetValue(resName, out resABPath))
            {
                resABPath = GetResABPathByResName(resName);
                ABPathDict.Add(resName, resABPath);
            }
            if (ABLoader.Instance != null)
            {
                ABLoader.Instance.UnloadAsset(resABPath.ABName);
            }
        }

        public static void UnloadAssetForceToZero(string resName)
        {
            if (string.IsNullOrEmpty(resName))
            {
                return;
            }
            UnityResHandle handle;
            if (ResHandleDict.TryGetValue(resName, out handle))
            {
                handle.Clear();
                ResHandleDict.Remove(resName);
            }
            UnityResABPath resABPath = null;
            if (!ABPathDict.TryGetValue(resName, out resABPath))
            {
                resABPath = GetResABPathByResName(resName);
                ABPathDict.Add(resName, resABPath);
            }
            if (ABLoader.Instance != null)
            {
                ABLoader.Instance.UnloadAssetForceToZero(resABPath.ABName);
            }
        }

        public static void UnloadAssetFalse(string resName)
        {
            if (string.IsNullOrEmpty(resName))
            {
                return;
            }
            UnityResHandle handle;
            if (ResHandleDict.TryGetValue(resName, out handle))
            {
                handle.Clear();
                ResHandleDict.Remove(resName);
            }
            UnityResABPath resABPath = null;
            if (!ABPathDict.TryGetValue(resName, out resABPath))
            {
                resABPath = GetResABPathByResName(resName);
                ABPathDict.Add(resName, resABPath);
            }
            if (ABLoader.Instance != null)
            {
                ABLoader.Instance.UnloadAssetFalse(resABPath.ABName);
            }
        }


        public static void UnloadAllAsset()
        {
            foreach (var kvHandle in ResHandleDict)
            {
                kvHandle.Value.Clear();
            }
            foreach (var kvABResPath in ABPathDict)
            {
                var resABPath = GetResABPathByResName(kvABResPath.Key);
                ABLoader.Instance.UnloadAsset(resABPath.ABName);
            }
            ResHandleDict.Clear();
        }

        private static UnityResABPath GetResABPathByResName(string resName)
        {
            string[] tempPaths = resName.Split(':');
            string abName = "ResAssets/" + tempPaths[0] + ".ab";
            string assetName = Path.GetFileName(tempPaths[0]);
            if (tempPaths.Length == 2)
            {
                assetName = tempPaths[1];
                var folderIdx = assetName.LastIndexOf('/');
                if (folderIdx >= 0)
                {
                    assetName = assetName.Substring(folderIdx + 1);
                }
            }
            return new UnityResABPath() { ResName = resName, ABName = abName, AssetName = assetName };
        }

        public static void UnloadAll()
        {
            foreach (var kvHandle in ResHandleDict)
            {
                kvHandle.Value.Clear();
            }
            ResHandleDict.Clear();
        }

        public static void UnloadUnused()
        {
        }


#if UNITY_EDITOR

        static UnityEngine.Object LoadAssetAtPath(string resKey, Type t, params string[] sufix)
        {
            for (int i = 0; i < sufix.Length; i++)
            {
                var obj = AssetDatabase.LoadAssetAtPath("Assets/Resources/" + resKey + sufix[i], t);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        public static string GetResourceKey(UnityEngine.Object obj)
        {
            var assetPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }
            const string resDir = "/Resources/";
            var resIndex = assetPath.IndexOf(resDir);
            if (resIndex == -1)
            {
                return null;
            }
            var resName = assetPath.Substring(resIndex + resDir.Length);
            var subFixIndex = resName.LastIndexOf('.');
            return resName.Substring(0, subFixIndex);
        }

        public static UnityEngine.Object GetAsset(string resName, string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return Resources.Load(resName);
            }
            else
            {
                return Resources.Load(resName + "/" + assetName);
            }
        }
#endif
    }

    public static class ResourceManagerExt
    {
        public static T LoadAndInstantiate<T>(this string resKey) where T : UnityEngine.Object
        {
            T res = UnityResLoader.LoadAsset<T>(resKey);
            if (res == null)
            {
                return null;
            }

            return UnityEngine.Object.Instantiate<T>(res);
        }

        public static GameObject LoadAndInstantiate(this string resKey)
        {
            GameObject res = UnityResLoader.LoadAsset<GameObject>(resKey);
            if (res == null)
            {
                return null;
            }

            return UnityEngine.Object.Instantiate<GameObject>(res);
        }

        public static GameObject LoadAndInstantiate(this string resKey, Vector3 pos, Vector3 scale)
        {
            //Debug.LogError(resKey);

            GameObject res = UnityResLoader.LoadAsset<GameObject>(resKey);
            if (res == null)
            {
                return null;
            }

            GameObject go = UnityEngine.Object.Instantiate(res, pos, Quaternion.identity) as GameObject;
            if (go == null)
            {
                return null;
            }
            go.transform.localScale = scale;
            return go;
        }

        public static GameObject LoadAndInstantiate(this string resKey, Vector3 pos, Vector3 scale, Quaternion rot)
        {
            GameObject res = UnityResLoader.LoadAsset<GameObject>(resKey);
            if (res == null)
            {
                return null;
            }

            GameObject go = UnityEngine.Object.Instantiate(res, pos, rot) as GameObject;
            if (go == null)
            {
                return null;
            }
            go.transform.localScale = scale;
            return go;
        }

        public static GameObject LoadAndInstantiate(this string resKey, Transform parent, Vector3 pos, Vector3 scale, Quaternion rot)
        {
            GameObject res = UnityResLoader.LoadAsset<GameObject>(resKey);
            if (res == null)
            {
                return null;
            }

            GameObject go = UnityEngine.Object.Instantiate(res, pos, rot) as GameObject;
            if (go == null)
            {
                return null;
            }
            go.transform.SetParent(parent, false);
            go.transform.localScale = scale;
            return go;
        }
        public static GameObject LoadAndInstantiate(this string resKey, Transform parent)
        {
            GameObject res = UnityResLoader.LoadAsset<GameObject>(resKey);
            if (res == null)
            {
                return null;
            }

            GameObject go = UnityEngine.Object.Instantiate(res) as GameObject;
            if (go == null)
            {
                return null;
            }
            go.transform.SetParent(parent, false);
            return go;
        }
    }

}
