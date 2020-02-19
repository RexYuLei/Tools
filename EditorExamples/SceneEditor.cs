using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//自定义SceneEditorTest脚本
[CustomEditor(typeof(SceneEditorTest))]
public class SceneEditor : Editor
{
    //在OnSceneGUI()中只能通过Handles来绘制新视图，如果你想引入GUI的元素，那么就需要使用BeginGUI()和EndGUI()组合的使用。
    private void OnSceneGUI()
    {
        //得到SceneEditorTest脚本的对象
        SceneEditorTest test = (SceneEditorTest) target;
        
        //绘制文本框
        Handles.Label(test.transform.position + Vector3.up*2,"啥玩意？？？");
        
        //开始绘制GUI
        Handles.BeginGUI();
        
        
        //规定GUI显示区域
        GUILayout.BeginArea(new Rect(100,100,100,100));
        
        //GUI绘制一个按钮
        if (GUILayout.Button("按钮"))
        {
            Debug.Log("测试！！！！！！！！！！！");
        }
        
        GUILayout.Label("有了");
        
        GUILayout.EndArea();
        
        Handles.EndGUI();
    }

}
