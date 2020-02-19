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
    public override void OnInspectorGUI()
    {
        
        //得到TestInspector对象
        var TI = (TestInspector)target;

        //绘制一个窗口
        TI.RectValue = EditorGUILayout.RectField("窗口坐标", TI.RectValue);
        //绘制一个贴图槽
        TI.TextureValue = EditorGUILayout.ObjectField("增加一个贴图",TI.TextureValue,typeof(Texture),true) as Texture;
        
    }
}
