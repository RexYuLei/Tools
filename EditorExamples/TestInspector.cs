using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestInspector : MonoBehaviour
{
    public Transform Trans;
    public Vector3 AniPos;
    public Vector3 AniEule;
    public Vector3 AniScale;
    public Color AniColor;

    public bool IsToggle;

    public Ease AniEase;
    public AnimationCurve AniCurve;
    
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

    public enum AniType
    {
        None = 0,
        Move,
        Rotate,
        Scale,
        Color,
    }

    public AniType AType;
}
