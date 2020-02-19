using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInspector : MonoBehaviour
{
    public Rect mRectValue;
    public Texture mTexture;

    [HideInInspector]
    [SerializeField]
    private Rect pRectValue;

    public Rect RectValue
    {
        get => pRectValue;
        set => pRectValue = value;
    }
    
    [HideInInspector]
    [SerializeField]
    private Texture PTexture;

    public Texture TextureValue
    {
        get => PTexture;
        set => PTexture = value;
    }
}
