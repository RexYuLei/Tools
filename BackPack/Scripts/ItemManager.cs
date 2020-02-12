using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;

public class ItemManager
{
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ItemManager();
            }
            return instance;
        }
    }


    public JsonData ItemConfig;
    public List<BackPackItem> BackPackItemList;

    public void LoadItemConfigData()
    {
        BackPackItemList = new List<BackPackItem>();
        this.ItemConfig = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Config/BackPackItems.json", Encoding.GetEncoding("GB2312")));
        DecodeJson();
    }

    public void DecodeJson()
    {
    //    for(int i = 0; i < this.ItemConfig.Count; i++)
    //    {
    //        int itemID = (int)ItemConfig[i]["ItemID"]
    //    }
        foreach (JsonData data in ItemConfig)
        {
            var itemID = (int)data["ItemID"];
            var itemName = data["ItemName"].ToString();
            var itemDesc = data["ItemDesc"].ToString();
            var itemType = (int)data["ItemType"];
            var itemIcon = data["ItemIcon"].ToString();
            var itemBgIcon = data["ItemBgIcon"].ToString();
            var itemCount = (int)data["ItemCount"];
            var itemQuality = (int)data["ItemQuality"];
            var itemOpreation = (int)data["ItemOpreation"];

            Debug.LogError(itemID + " " + itemName + " " + itemDesc + " " + itemType + " "
                + itemIcon + " " + itemBgIcon + " " + itemCount + " " + itemQuality + " " + itemOpreation + " ");
            BackPackItem backPackItem = new BackPackItem(itemID, itemName, itemDesc, itemType, 
                itemIcon, itemBgIcon, itemCount, itemQuality, itemOpreation);
            BackPackItemList.Add(backPackItem);
        }
    }

    //IEnumerator start()
    //{
    //    yield return null;
    //}
}
