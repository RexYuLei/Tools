using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager
{
    private static UIPanelManager instance;
    public static UIPanelManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UIPanelManager();
            }
            return instance;
        }
    }

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if(canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<string, string> panelPathDict;
    private Dictionary<string, BasePanel> panelDict;

    private Stack<BasePanel> panelStack;

    private UIPanelManager()
    {
        AddAllPanel();
    }

    /// <summary>
    /// 获取指定Panel
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    private BasePanel GetPanel(string panelType)
    {
        if(panelDict == null)
        {
            panelDict = new Dictionary<string, BasePanel>();
        }
        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);
        if(panel == null)
        {
            string path;
            panelPathDict.TryGetValue(panelType, out path);
            GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>(path), CanvasTransform, false);
            panel = panelObj.GetComponent<BasePanel>();
            panelDict.Add(panelType, panel);
        }
        return panel;
    }


    /// <summary>
    /// 字典添加所有panel
    /// </summary>
    private void AddAllPanel()
    {
        panelPathDict = new Dictionary<string, string>();

        panelPathDict.Add("名称", "名称");
    }

    /// <summary>
    /// 界面存储栈，并进行入栈和出栈操作
    /// </summary>
    public void PushPanel(string panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panelStack.Push(panel);
        panel.OnEnter();
    }
    public void PopPanel()
    {
        if(panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if(panelStack.Count <= 0)
        {
            return;
        }

        //退出栈顶面板
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        //恢复上一个面板
        if (panelStack.Count > 0)
        {
            BasePanel panel = panelStack.Peek();
            panel.OnResume();
        }
    }
}
