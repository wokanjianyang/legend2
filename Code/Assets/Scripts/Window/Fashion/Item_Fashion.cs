using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Fashion : MonoBehaviour
{
    public Image Icon;
    public Text Txt_Name;

    public Transform Tf_Attr;
    private List<Item_Attr> AttrList;

    public Text Txt_Fee;
    public Text Txt_Attr_Active;

    public Button Btn_Active;
    public Button Btn_Up;

    public FashionConfig Config { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnActive);
        Btn_Up.onClick.AddListener(OnUp);

        AttrList = Tf_Attr.GetComponentsInChildren<Item_Attr>().ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(FashionConfig config)
    {
        this.Config = config;

        Txt_Name.text = this.Config.Name;
        Icon.sprite = PrefabHelper.Instance().GetFashion(Config.Id);

        this.Show();
    }

    public void Show()
    {
        if (this.Config == null)
        {
            return;
        }

        for (int i = 0; i < AttrList.Count; i++)
        {
            AttrList[i].SetContent(this.Config.AttrIdList[i], this.Config.AttrValueList[i]);
        }

        Txt_Attr_Active.text = "出战属性：" + StringHelper.FormatAttrText(this.Config.UpAttrId, this.Config.UpAttrValue);

        User user = User_Data_Manager.Data;

        long fashionLevel = user.GetFashionLevel(this.Config.Id);

        if (fashionLevel > 0)
        {
            Btn_Active.gameObject.SetActive(false);

            if (this.Config.Id == user.FashionUpId)
            {
                Btn_Up.gameObject.SetActive(false);
            }
            else
            {
                Btn_Up.gameObject.SetActive(true);
            }
            Txt_Fee.gameObject.SetActive(false);
        }
        else
        {
            Btn_Active.gameObject.SetActive(true);
            Btn_Up.gameObject.SetActive(false);
            Txt_Fee.gameObject.SetActive(true);

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(this.Config.ItemId);
            long materialCount = user.GetMaterialCount(this.Config.ItemId);

            int fee = this.Config.Fee;
            string color = materialCount >= fee ? "#00FF00" : "#FF0000";

            Txt_Fee.text = string.Format("{3}：<color={0}>{1}/{2}</color>", color, materialCount, fee, itemConfig.Name);
        }
    }

    private void OnActive()
    {
        this.Btn_Active.gameObject.SetActive(false);

        User user = User_Data_Manager.Data;
        long fashionLevel = user.GetFashionLevel(Config.Id);

        if (fashionLevel > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经激活了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long materialCount = user.GetMaterialCount(this.Config.ItemId);

        int fee = Config.Fee;

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的道具", ToastType = ToastTypeEnum.Failure });
            this.Btn_Active.gameObject.SetActive(true);
            return;
        }

        user.SaveFashionLevel(Config.Id);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = Config.ItemId,
            Quantity = fee
        });

        Dialog_Fashion dialog = this.gameObject.GetComponentInParent<Dialog_Fashion>();
        dialog.ReFresh();

        GameProcessor.Inst.UpdateInfo();

    }

    private void OnUp()
    {
        Debug.Log("OnUp count:");

        this.Btn_Up.gameObject.SetActive(false);

        User user = User_Data_Manager.Data;
        long fashionLevel = user.GetFashionLevel(Config.Id);

        if (fashionLevel <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "还没有激活", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.FashionUpId = Config.Id;

        this.Btn_Active.gameObject.SetActive(false);
        this.Btn_Up.gameObject.SetActive(false);

        Dialog_Fashion dialog = this.gameObject.GetComponentInParent<Dialog_Fashion>();
        dialog.ReFresh();

        GameProcessor.Inst.UpdateInfo();


    }
}

