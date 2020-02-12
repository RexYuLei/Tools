using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    [SerializeField]
    private GameObject mPrefab;
    
    private Queue<GameObject> mPoolInstanceQueue = new Queue<GameObject>();

    private GameObject mCachePanel;
    private Dictionary<string, Queue<GameObject>> mPool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<GameObject,string> mGoTag = new Dictionary<GameObject, string>();

    public GameObject GetInstance()
    {
        if (mPoolInstanceQueue.Count > 0)
        {
            GameObject instanceToReuse = mPoolInstanceQueue.Dequeue();
            instanceToReuse.SetActive(true);
            return instanceToReuse;
        }

        return Instantiate(mPrefab);
    }

    public void ReturnInstance(GameObject gameObjectToPool)
    {
        mPoolInstanceQueue.Enqueue(gameObjectToPool);
        gameObjectToPool.SetActive(false);
        gameObjectToPool.transform.SetParent(gameObject.transform);
    }

    /// <summary>
    /// 清空缓存池，释放所有引用
    /// </summary>
    public void ClearCachePool()
    {
        mPool.Clear();
        mGoTag.Clear();
    }

    /// <summary>
    /// 回收GameObject
    /// </summary>
    /// <param name="go"></param>
    public void ReturnCacheGameObject(GameObject go)
    {
        if (mCachePanel == null)
        {
            mCachePanel = new GameObject();
            mCachePanel.name = "CachePanel";
            GameObject.DontDestroyOnLoad(mCachePanel);
        }
        
        if(go ==null)
            return;

        go.transform.parent = mCachePanel.transform;
        go.SetActive(false);

        if (mGoTag.ContainsKey(go))
        {
            string tag = mGoTag[go];
            RemoveMark(go);

            if (!mPool.ContainsKey(tag))
            {
                mPool[tag] = new Queue<GameObject>();
            }
            
            mPool[tag].Enqueue(go);
        }
    }

    public GameObject RequestCacheGameObject(GameObject prefab)
    {
        string tag = prefab.GetInstanceID().ToString();
        GameObject go = GetFromPool(tag);
        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(prefab);
            go.name = prefab.name + Time.time;
        }
        
        MarkWhenOut(go,tag);
        return go;
    }

    private GameObject GetFromPool(string tag)
    {
        if (mPool.ContainsKey(tag) && mPool[tag].Count > 0)
        {
            GameObject obj = mPool[tag].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return null;
    }

    private void MarkWhenOut(GameObject go, string tag)
    {
        mGoTag.Add(go,tag);
    }

    private void RemoveMark(GameObject go)
    {
        if (mGoTag.ContainsKey(go))
        {
            mGoTag.Remove(go);
        }
        else
        {
            Debug.LogError("RemoveMarkError,GameObject has not been marked");
        }
    } 
}
