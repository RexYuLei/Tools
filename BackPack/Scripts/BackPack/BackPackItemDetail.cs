using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackItemDetail : MonoBehaviour
{
    public Text Name;
    public Text Desc;
    public Button SellBtn;
    public Button EquipBtn;
    public Button ConpoundBtn;
    public Button SynthesisBtn;

    void Awake() 
    {
        this.SellBtn.onClick.AddListener(this.OnSellBtnClicked);
        this.EquipBtn.onClick.AddListener(this.OnEquipBtnClicked);
        this.ConpoundBtn.onClick.AddListener(this.OnCompoundBtnClicked);
        this.SynthesisBtn.onClick.AddListener(this.OnSynthesisBtnClicked);
    }

    public void SetData(BackPackItem item)
    {
        this.Name.text = item.ItemName;
        this.Desc.text = item.ItemDesc;

        //装备显示出售和装备按钮，碎片显示分解和合成按钮
        this.SellBtn.gameObject.SetActive(item.mItemType == BackPackItem.ItemType.Equip);
        this.EquipBtn.gameObject.SetActive(item.mItemType == BackPackItem.ItemType.Equip);
        this.ConpoundBtn.gameObject.SetActive(item.mItemType == BackPackItem.ItemType.Chips);
        this.SynthesisBtn.gameObject.SetActive(item.mItemType == BackPackItem.ItemType.Chips);
    }

    /// <summary>
    /// 出售按钮被点击
    /// </summary>
    public void OnSellBtnClicked()
    {

    }

    public void OnEquipBtnClicked()
    {

    }

    public void OnCompoundBtnClicked()
    {

    }

    public void OnSynthesisBtnClicked()
    {

    }
}
