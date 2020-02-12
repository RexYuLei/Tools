using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//1 从配置文件里加载数据
//2 根据数据创建一个具体的Item（根据道具的类型加载）
//3 点击具体的Item 弹出详细的信息界面
//4 实现动画效果（背包界面的左右切入）
//5 道具类型切换功能

public class BackPackPanel : MonoBehaviour
{
    public Transform ItemTemp;
    public ScrollRect ItemScrollRect;
    public BackPackItemDetail BackPackItemDetail;

    private List<GameObject> itemObjList;

    private List<BackPackItem> itemList;

    void Awake()
    {
        ItemManager.Instance.LoadItemConfigData();
        this.itemList = ItemManager.Instance.BackPackItemList;
        this.itemObjList = new List<GameObject>();
    }
    private void OnEnable()
    {
        CreateAllItems();
    }

    /// <summary>
    /// 创建单个Item
    /// </summary>
    public GameObject CreateSingleItem()
    {
        GameObject go = GameObject.Instantiate(this.ItemTemp.gameObject, this.ItemScrollRect.content);
        return go;
    }
    public void CreateAllItems(BackPackItem.ItemType itemType = BackPackItem.ItemType.UnKnown)
    {
        int index = 0;
        for (int i = 0; i < this.itemList.Count; i++)
        {
            //每次界面打开的时候，没有实例化道具对象的时候，就去创建，关闭背包界面就去隐藏实例化对象
            //再次打开时就直接拿着背包数据对实例化对象脚本进行赋值
            if (itemList[i].mItemType != itemType) continue;
            GameObject go = null;
            if (index < this.itemObjList.Count)
            {
                go = itemObjList[index];
            }
            else
            {
                go = this.CreateSingleItem();
                this.itemObjList.Add(go);
            }
            index++;

            var info = go.GetComponent<BackPackItemShowInfo>();
            go.SetActive(info != null);
            if (info == null)
                continue;

            var item = this.itemList[i];
            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                BackPackItemDetail.SetData(item);
            });
            info.SetData(this.itemList[i]);
        }

        if (index < this.itemObjList.Count)
        {
            for (int i = index; i < this.itemObjList.Count; i++)
            {
                this.itemObjList[i].SetActive(false);
            }
        }
    }

    public void AllItemClicked(Toggle t)
    {
        Debug.Log("AllItemBtn is clicked...");
        this.CreateAllItems(BackPackItem.ItemType.UnKnown);

    }
    public void EquipClicked(Toggle t)
    {
        Debug.Log("EquipBtn is clicked...");
        this.CreateAllItems(BackPackItem.ItemType.Equip);
    }
    public void ChipClicked(Toggle t)
    {
        Debug.Log("ChipBtn is clicked...");
        this.CreateAllItems(BackPackItem.ItemType.Chips);
    }
}
