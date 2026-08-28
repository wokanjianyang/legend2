using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Weapon : MonoBehaviour
{
    public Text Txt_Name;
    public Image Img_Logo;


    public Text Txt_Exp;

    public Transform Tf_Active;
    public Text Txt_Active;

    public Transform Tf_Level;
    private Forge_Atr_Item[] LevelList;

    public Transform Tf_Layer;
    private Forge_Atr_Item[] LayerList;

    public Text Txt_Fee;

    public Text Txt_Status;
    public Button Btn_Status;
    public Button Btn_Next;

    public Toggle toggle_Auto;

    public Button Btn_Ok;
    public Button Btn_Active;

    private int WeapIndex = 1;
    private int WeaponId = 1;
    private int MaxWeapon = 1;

    private List<WeaponConfig> list;

    // Start is called before the first frame update
    void Awake()
    {
        LevelList = Tf_Level.GetComponentsInChildren<Forge_Atr_Item>();
        LayerList = Tf_Layer.GetComponentsInChildren<Forge_Atr_Item>();

        Btn_Next.onClick.AddListener(OnClick_Next);
        Btn_Status.onClick.AddListener(OnClick_Status);

        Btn_Ok.onClick.AddListener(OnClick_OK);
        Btn_Active.onClick.AddListener(OnClick_Active);

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.WeaponAuto = isOn;
        });
    }


    // Update is called once per frame
    void Start()
    {
        this.list = WeaponConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Type <= ConfigHelper.Channel).ToList();
        MaxWeapon = list.Count();
        this.Show();
    }

    void OnEnable()
    {
        toggle_Auto.isOn = AppHelper.WeaponAuto;

        if (MaxWeapon > 1)
        {
            this.Show();
        }
    }

    private void OnClick_Next()
    {
        WeapIndex++;
        WeapIndex = (WeapIndex - 1) % MaxWeapon + 1;
        WeaponId = list[WeapIndex - 1].Id;

        Show();
    }

    public void OnClick_Status()
    {
        this.Btn_Status.gameObject.SetActive(false);

        User user = User_Data_Manager.Data;

        foreach (var sp in user.WeaponData)
        {
            if (sp.Key == WeaponId)
            {
                sp.Value.Status = 1;
            }
            else
            {
                sp.Value.Status = 0;
            }
        }


        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "上阵成功", ToastType = ToastTypeEnum.Success });
        this.Show();

    }


    private void Show()
    {
        User user = User_Data_Manager.Data;

        Weapon_Data data = user.GetWeaponData(WeaponId);

        WeaponConfig config = WeaponConfigCategory.Instance.Get(WeaponId);

        //LegacyGradeConfig gradeConfig = LegacyGradeConfigCategory.Instance.GetConfig(keyId, nextLevel);

        this.Txt_Name.text = config.Name;
        this.Img_Logo.sprite = PrefabHelper.Instance().GetItemLogo(config.Logo);

        int currentLevel = (int)data.Level.Data;
        int currentLayer = currentLevel / 10;

        if (currentLevel > 0)
        {
            this.Txt_Exp.text = string.Format("<color=#FF6600>{0}级</color>（Exp:{1}/{2}）", currentLevel, data.Exp.Data, data.GetNeedExp());

            this.Tf_Active.gameObject.SetActive(true);
            this.Txt_Active.gameObject.SetActive(false);
            this.Btn_Active.gameObject.SetActive(false);
        }
        else
        {
            this.Txt_Exp.text = "未激活";

            this.Tf_Active.gameObject.SetActive(false);
            this.Txt_Active.gameObject.SetActive(true);
            this.Btn_Active.gameObject.SetActive(true);

            this.Txt_Active.text = string.Format(config.Des, config.Condtion);
        }

        if (data.Status == 1)
        {
            this.Txt_Status.gameObject.SetActive(true);
            this.Btn_Status.gameObject.SetActive(false);
        }
        else
        {
            this.Txt_Status.gameObject.SetActive(false);

            if (currentLevel > 0)
            {
                this.Btn_Status.gameObject.SetActive(true);
            }
            else
            {
                this.Btn_Status.gameObject.SetActive(false);
            }
        }


        if (data.isMaxLevel())
        {
            Txt_Fee.text = "神兵已经满级";
            Btn_Ok.gameObject.SetActive(false);
        }
        else if (!data.isExpFull())
        {  //经验未满
            Txt_Fee.text = "神兵经验不足";
            Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            int feeId = data.GetFeeId();
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(feeId);

            long fee = data.GetFee();

            long mc = user.GetMaterialCount(feeId);
            string color = mc >= fee ? "#11FF11" : "#FF1111";

            Txt_Fee.text = string.Format("{3}：<color={0}>{1}</color>/{2}", color, mc, fee, itemConfig.Name);

            if (mc >= fee)
            {
                Btn_Ok.gameObject.SetActive(true);
            }
            else
            {
                Btn_Ok.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < LevelList.Length; i++)
        {
            if (i < config.AtrIdList.Length)
            {
                int attrId = config.AtrIdList[i];
                long atrRise = config.AtrVueList[i];
                long attrCurrent = config.AtrVueList[i] * currentLevel;

                LevelList[i].SetContent(attrId, attrCurrent, atrRise);
                LevelList[i].gameObject.SetActive(true);

            }
            else
            {
                LevelList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < LayerList.Length; i++)
        {
            if (i < config.GradeAtrIdList.Length)
            {
                int attrId = config.GradeAtrIdList[i];
                long atrRise = config.GradeAtrVueList[i];
                long attrCurrent = config.GradeAtrVueList[i] * currentLayer;

                LayerList[i].SetContent(attrId, attrCurrent, atrRise);
                LayerList[i].gameObject.SetActive(true);

            }
            else
            {
                LayerList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClick_Active()
    {
        User user = User_Data_Manager.Data;

        long progress = 0;

        switch (WeaponId)
        {


            case 1:
                progress = user.MagicLevel.Data;  //等级40级
                break;
            case 2:
                progress = user.GetAchievementProgeress(AchievementProType.DayCount);  //登录7天
                break;
            case 3:
                progress = user.GetAchievementProgeress(AchievementProType.Advert);  //广告100次
                break;
            case 4:
                progress = User_Data_Manager.GetStoreNumber(5002);
                break;
            case 5:
                progress = User_Data_Manager.GetStoreNumber(5004);
                break;
            case 6:
                progress = user.GetExclusiveLevel(1201);
                break;
            case 7:
                progress = user.GetExclusiveLevel(2201);
                break;
            case 8:
                progress = user.GetExclusiveLevel(3201);
                break;
            default:
                break;
        }

        WeaponConfig config = WeaponConfigCategory.Instance.Get(WeaponId);

        if (progress < config.Condtion)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "激活条件不足", ToastType = ToastTypeEnum.Failure });
            return;
        }


        Weapon_Data data = user.GetWeaponData(WeaponId);

        if (data.Level.Data > 0)
        {

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "神兵已经激活了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        data.Level.Data = 1;  //激活就是把等级设为1级
        data.Exp.Data = 0;

        GameProcessor.Inst.UpdateInfo();

        Show();
    }

    private void OnClick_OK()
    {
        User user = User_Data_Manager.Data;

        Weapon_Data data = user.GetWeaponData(WeaponId);

        if (!data.isExpFull())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "经验不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        int feeId = data.GetFeeId();
        long fee = data.GetFee();
        long mc = user.GetMaterialCount(feeId);

        if (mc < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = feeId,
            Quantity = fee
        });

        data.Grade();


        GameProcessor.Inst.UpdateInfo();

        Show();
    }
}

