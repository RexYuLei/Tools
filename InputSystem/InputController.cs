using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// 监听器
    /// </summary>
    public InputActionAsset mActionAsset;
    
    private InputActionMap mPlayerActionMap;

    public Transform obj;


    private void Awake()
    {
        mPlayerActionMap = mActionAsset.FindActionMap("Key");
        RegisterInputAction();
        mPlayerActionMap.Enable();
    }

    /// <summary>
    /// 注册基本监听事件
    /// </summary>
    void RegisterInputAction()
    {
        for (int i = 0; i < this.mPlayerActionMap.actions.Count; i++)
        {
            this.mPlayerActionMap.actions[i].started += OnPlayerActionStarted;
            this.mPlayerActionMap.actions[i].performed += OnPlayerActionPerformed;
            this.mPlayerActionMap.actions[i].canceled += OnPlayerActionCanceled;
        }
    }
    void UnRegisterInputAction()
    {
        for (int i = 0; i < this.mPlayerActionMap.actions.Count; i++)
        {
            this.mPlayerActionMap.actions[i].started -= OnPlayerActionStarted;
            this.mPlayerActionMap.actions[i].performed -= OnPlayerActionPerformed;
            this.mPlayerActionMap.actions[i].canceled -= OnPlayerActionCanceled;
        }
    }

    private void OnPlayerActionStarted(InputAction.CallbackContext obj)
    {
        //Debug.Log(obj.ReadValue<Vector2>() + "start...........");
        Debug.Log(obj.action.name);
    }
    private void OnPlayerActionPerformed(InputAction.CallbackContext obj)
    {
        var dir = obj.ReadValue<Vector2>();
        this.obj.Translate(dir);
        //Debug.Log(obj.ReadValue<Vector2>() + "perform...........");
        Debug.Log("Fire Perform!!!");
    }
    private void OnPlayerActionCanceled(InputAction.CallbackContext obj)
    {
        //Debug.Log(obj.ReadValue<Vector2>() + "cancel...........");
        Debug.Log("Fire Cancel!!!");


    }



}
