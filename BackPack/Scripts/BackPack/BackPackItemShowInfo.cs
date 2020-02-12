using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackItemShowInfo : MonoBehaviour
{
    public Image BgIcon;
    public Image Icon;
    public Text Count;
    public GameObject IsChip;

    private BackPackItem mItem;

    /// <summary>
    /// 1 数据从哪来
    /// 2 数据信息
    /// </summary>
    public void SetData(BackPackItem item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null,Please check it");
            return;
        }
        this.mItem = item;

        this.BgIcon.sprite = Resources.Load<Sprite>("Textures/Item/" + this.mItem.ItemBgIcon);
        this.BgIcon.sprite = Resources.Load<Sprite>("Textures/Item/" + this.mItem.ItemIcon);
        this.Count.text = mItem.ItemCount.ToString();
        this.IsChip.SetActive(mItem.mItemType == BackPackItem.ItemType.Chips);

        switch (this.mItem.ItemQuality)
        {
            case 1:
                this.BgIcon.color = Color.white;
                break;
            case 2:
                this.BgIcon.color = Color.blue;
                break;
            case 3:
                this.BgIcon.color = Color.red;
                break;
            default:
                break;
        }
    }
}
