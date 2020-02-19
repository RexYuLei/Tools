using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuEditor : EditorWindow
{
    
    //创建窗口
    [MenuItem("TestEditor/CreateWindow")]
    static void CreateWindow()
    {
        Rect wr = new Rect(0,0,500,500);
        MenuEditor window = EditorWindow.CreateWindow<MenuEditor>("大窗口来了");
        window.Show();
    }

    private string text = "还有，是空消息！！！！";
    private Texture texture;

    

    private void OnGUI()
    {
        //输入框控件
        text = EditorGUILayout.TextField("输入文字：", text);

        if (GUILayout.Button("关闭通知",GUILayout.Width(200)))
        {
            //关闭通知栏
            this.RemoveNotification();
        }
        
        //文本框显示鼠标在窗口的位置
        EditorGUILayout.LabelField("鼠标在窗口的位置",Event.current.mousePosition.ToString());
        
        //选择贴图
        texture = EditorGUILayout.ObjectField("添加贴图",texture,typeof(Texture),true) as Texture;

        if (GUILayout.Button("关闭窗口",GUILayout.Width(300)))
        {
            this.Close();
        }
    }
    
    //窗口的Awake
    public void Awake()
    {
        Debug.Log("Awake........");
    }

    //窗口的Update
    private void Update()
    {
        Debug.Log("窗口竟然也有Update????????");
    }

    private void OnFocus()
    {
        Debug.Log("当窗口获得焦点时调用一次");
    }

    private void OnLostFocus()
    {
        Debug.Log("当窗口失去焦点时调用一次");
    }

    private void OnHierarchyChange()
    {
        Debug.Log("当Hierachy视图中的任何对象发生改变时调用一次");
    }

    private void OnProjectChange()
    {
        Debug.Log("当Project视图中的资源发生改变时调用一次");
    }

    private void OnInspectorUpdate()
    {
        //窗口面板更新，不知道更新频率,但频率比Update低
        Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口的信息不会刷新
        this.Repaint();
    }

    private void OnDestroy()
    {
        Debug.Log("当窗口关闭时调用");
    }
}
