using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Skill_Level : MonoBehaviour
{
    public Transform Tf_List;
    private List<Skill_Level_Item> list;

    public Text Txt_Title;

    public Button Btn_Close;

    public Text Txt_Level;
    public Text Txt_Fee;
    public Text Txt_Exp;
    public Button Btn_Batch;

    private SkillPanel curentSp;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Batch.onClick.AddListener(OnClick_Batch);

        list = Tf_List.GetComponentsInChildren<Skill_Level_Item>().ToList();
    }

    public void Show(SkillPanel sp)
    {
        this.gameObject.SetActive(true);
        this.curentSp = sp;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetContent(sp.Config, i, sp.Level);
        }

        User user = User_Data_Manager.Data;
        SkillData sd = user.SkillList.Where(m => m.SkillId == sp.SkillId).FirstOrDefault();

        this.Txt_Title.text = sp.Config.Name + "等级预览";

        this.Txt_Level.text = string.Format("技能基础等级：{0}级", sd.MagicLevel.Data);
        this.Txt_Exp.text = string.Format("当前经验：{0}/{1}", sd.MagicExp.Data, sd.GetLevelUpExp());

        long mc = user.GetMaterialCount(ItemHelper.Shuye1);
        this.Txt_Fee.text = string.Format("当前书页：{0}（每本经验+5）", mc);

        if (mc <= 0 || sd.MagicLevel.Data >= sp.Config.MaxLevel)
        {
            this.Btn_Batch.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Batch.gameObject.SetActive(true);
        }
    }

    public void OnClick_Close()
    {
        this.curentSp = null;
        this.gameObject.SetActive(false);
    }

    public void OnClick_Batch()
    {
        this.Btn_Batch.gameObject.SetActive(false);

        User user = User_Data_Manager.Data;
        SkillData sd = user.SkillList.Where(m => m.SkillId == curentSp.SkillId).FirstOrDefault();


        if (sd.MagicLevel.Data >= curentSp.Config.MaxLevel)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "技能已经满级了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long needExp = sd.GetLevelUpExp() - sd.MagicExp.Data;

        long uc = (int)Math.Ceiling(needExp / 5.0);
        long mc = user.GetMaterialCount(ItemHelper.Shuye1);

        uc = Math.Min(uc, mc);

        if (uc < 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "经验已满,或者没有书页了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.Shuye1,
            Quantity = uc
        });

        sd.AddExp(uc * 5);

        GameProcessor.Inst.EventCenter.Raise(new SkillShowEvent());
        this.Show(curentSp);
    }
}
