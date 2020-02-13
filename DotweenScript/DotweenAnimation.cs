using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DotweenAnimation : MonoBehaviour
{
    //public float x;
    public float myValue;
    private Material material;

    private void Start()
    {
        Test();
    }


    public void Test()
    {
        //Dotween默认初始化，也可以手动初始化并设置容量
        //DOTween.Init().SetCapacity(300, 10);
        
        //通用方式
        // Tweener tweener1 = DOTween.To(() => myValue, (x) => myValue = x, 100, 1);
        // Tweener tweener2 = transform.DOMoveX(100, 1);
        

        //从自身位置移动到目标位置
        //transform.DOMoveX(10, 5);
        //从目标位置移动到自身位置
        //transform.DOMoveX(10, 5).From();
        //从（自身位置加目标位置）移动到自身位置
        //transform.DOMoveX(10, 5).From(true);
        

        // DOTween.RewindAll();
        // DOTween.Rewind(gameObject);
        // tweener1.Rewind();
        // transform.DORewind();
        
        //设置移动的缓动函数，循环和回调--两种方式
        transform.DOMove(new Vector3(1, 1, 1), 1)
            .SetEase(Ease.OutQuint)
            .SetLoops(4)
            .OnComplete((() => { }));
        Tween myTween = transform.DOMove(new Vector3(1, 1, 1), 1);
        myTween.SetEase(Ease.OutQuint);
        myTween.SetLoops(4);
        myTween.OnComplete((() => { }));
        //甚至循环类型
        //transform.DOMoveX(1, 1).SetLoops(2, LoopType.Restart);
        
        //可使用SetAs
        material.DOColor(Color.red, 2).SetAs(myTween);

        //杀死所有tween并清除所有缓存
        DOTween.Clear();

        
        //序列 Sequence
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(myTween);
        mySequence.AppendCallback(() => { });
    }

    public Transform target;
    //动画类型
    public enum AniType
    {
        None = 0,
        Move,
        Roteta,
        Scale,
    }
    public AniType AType;

    private Tweener mCurTweener;
    public Ease AniEase;
    
    //回调类型
    public enum CallbackType
    {
        None = 0,
        ActiveOtherAni,
        SendMsg,
        Both
    }

    public CallbackType CType;
    
    
    private void PlayAnimation()
    {
        if (target == null)
            target = transform;
        switch (AType)
        {
            case AniType.Move:
                {
                    mCurTweener = target.DOMove(new Vector3(0, 0, 0), 1);
                }
            break;    
        }

        mCurTweener.SetEase(AniEase);
        mCurTweener.OnComplete(() =>
        {
            switch (CType)
            {
                case CallbackType.ActiveOtherAni:
                {
                    
                }
                    break;
                case CallbackType.SendMsg:
                {
                    
                }
                    break;
                case CallbackType.Both:
                {
                    
                } 
                    break;
            }
        });

    }
}
