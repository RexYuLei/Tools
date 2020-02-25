using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//自定义TestInspector脚本面板
[CustomEditor(typeof(TestInspector),false)]
//在编辑器模式下执行脚本
[ExecuteInEditMode]
//需要继承Editor
public class TestInspectorEditor : Editor
{
    
    //面板GUI控制
    public override void OnInspectorGUI()
    {
        //得到TestInspector对象
        TestInspector TI = (TestInspector)target;

        //添加Transform引用
        TI.Trans = (Transform)EditorGUILayout.ObjectField("目标", TI.Trans, typeof(TestInspector), true);

        //绘制一个窗口
        TI.RectValue = EditorGUILayout.RectField("窗口坐标", TI.RectValue);
        //绘制一个贴图槽
        TI.TextureValue = EditorGUILayout.ObjectField("增加一个贴图",TI.TextureValue,typeof(Texture),true) as Texture;
        //绘制类型选择
        TI.AType = (TestInspector.AniType) EditorGUILayout.EnumPopup("动画类型", TI.AType);
        switch (TI.AType)
        {
            case TestInspector.AniType.Move:
            {
                TI.AniPos = EditorGUILayout.Vector3Field("移动动画", TI.AniPos);
            }
                break;
            case TestInspector.AniType.Rotate:
            {
                TI.AniEule = EditorGUILayout.Vector3Field("旋转动画", TI.AniEule);
            }
                break;
            case TestInspector.AniType.Scale:
            {
                TI.AniScale = EditorGUILayout.Vector3Field("缩放动画", TI.AniScale);
            }
                break;
            case TestInspector.AniType.Color:
            {
                TI.AniColor = EditorGUILayout.ColorField("颜色动画", TI.AniColor);
            }
                break;
        }
        
        //绘制一个勾选按钮
        TI.IsToggle = EditorGUILayout.Toggle("是否勾选", TI.IsToggle);
        //运动曲线选择
        TI.AniEase = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("动画播放曲线", TI.AniEase);
        //自定义运动曲线选择
        TI.AniCurve = EditorGUILayout.CurveField( "动画播放曲线", TI.AniCurve, Color.red,new Rect(0.0f, 0.0f, 1.0f, 1.0f));
        

        
        //开始横向布局和结束
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Button("I'm the VerticalFirst button");
        GUILayout.Button("I'm the VerticalSecond button");
        GUILayout.EndVertical();
        
        //开始纵向布局和结束
        GUILayout.BeginHorizontal("box");
        GUILayout.Button("I'm the HorizontalFirst button");
        GUILayout.Button("I'm the HorizontalSecond button");
        GUILayout.EndHorizontal();
        
    }
    
    //GameGUI控制
    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        
    }
    
    
    //SceneGUI控制
    //在OnSceneGUI()中只能通过Handles来绘制新视图，如果你想引入GUI的元素，那么就需要使用BeginGUI()和EndGUI()组合的使用。
    public void OnSceneGUI()
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
