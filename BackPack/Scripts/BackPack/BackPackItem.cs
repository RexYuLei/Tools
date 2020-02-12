using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BackPackItem
{

    public enum ItemType
    {
        UnKnown = -1,
        Equip, //装备
        Chips, //碎片
    }
    ///<summary>
    ///道具唯一ID，区别于其他道具
    ///</summary>
    public int ItemID;

    public ItemType mItemType = ItemType.UnKnown;
    public string ItemName;
    ///<summary>
    ///道具描述
    ///</summary>
    public string ItemDesc;
    public string ItemIcon;
    public string ItemBgIcon;
    public int ItemCount;
    public int ItemQuality;
    ///<summary>
    ///道具的操作，如 分解 合成 出售 装备 等等
    ///</summary>
    public int ItemOpreation;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="itemName"></param>
    /// <param name="itemDesc"></param>
    /// <param name="itemType"></param>
    /// <param name="itemIcon"></param>
    /// <param name="itemBgIcon"></param>
    /// <param name="itemCount"></param>
    /// <param name="itemQuality">0:White 1:Green 2:Blue</param>
    /// <param name="itemOpreation"></param>
    public BackPackItem(int itemID,string itemName,string itemDesc,int itemType,
        string itemIcon,string itemBgIcon, int itemCount,int itemQuality,int itemOpreation)
    {
        this.ItemID = itemID;
        this.ItemName = itemName;
        this.ItemDesc = itemDesc;
        switch (itemType)
        {
            case -1:
                this.mItemType = ItemType.UnKnown;
                break;
            case 0:
                this.mItemType = ItemType.Equip;
                break;
            case 1:
                this.mItemType = ItemType.Chips;
                break;
            default:
                break;
        }
        this.ItemIcon = itemIcon;
        this.ItemBgIcon = itemBgIcon;
        this.ItemCount = itemCount;
        this.ItemQuality = itemQuality;
        this.ItemOpreation = itemOpreation;
    }

}
