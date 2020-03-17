using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 一个资源包项
/// </summary>
public class ABEntry
{
}

public class ABEntryProcesser
{
    public ABEntry Entry;

    /// <summary>
    /// 预处理所有的资源
    /// </summary>
    public virtual void PreprocessAssets()
    {
        
    }

    public void ProcessAssetBundleLabel()
    {
        
    }
    
    public IEnumerable<AssetBundleBuild> ToABBuild()
    {
        throw new System.NotImplementedException();
    }

    public static ABEntryProcesser Create(ABEntry rABEntry)
    {
        ABEntryProcesser rEntryProcesser = null;
        
        
        rEntryProcesser = new ABEntryProcesser();
        rEntryProcesser.Entry = rABEntry;

        return rEntryProcesser;
    }

    
}
