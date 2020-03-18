using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Awake()
    {
        Input.GetKeyDown(KeyCode.A);
        Vector3 pos = Input.mousePosition;
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("按键");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("预定义按键名字");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("鼠标按键按下,0是左键,1是右键,2是中间滚轮");
        }

        if (Input.anyKeyDown)
        {
            Debug.Log("任何键按下");
        }

        if (Input.inputString != "")
        {
            Debug.Log(Input.inputString);    
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
    }
}


