using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Attr : MonoBehaviour, IBattleLife
{
    public Button btn_Close;

    public Transform Tf_Nav;
    private List<Toggle> toggleStageList = new List<Toggle>();

    private int SelectType = 0;
    private List<AttributeEnum[]> list = new List<AttributeEnum[]>();

    private Item_Attr[] items;

    public int Order => (int)ComponentOrder.Dialog;

    void Awake()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        toggleStageList = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();
        items = this.GetComponentsInChildren<Item_Attr>();

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        list.Add(array1);
        list.Add(array2);
        list.Add(array3);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowDialogUserAttrEvent>(this.Open);
    }

    private void ChangePanel(int index)
    {
        this.SelectType = index;

        this.Show();
    }

    AttributeEnum[] array1 = new AttributeEnum[] {
             AttributeEnum.Atk,AttributeEnum.IncreaAtk,AttributeEnum.RateAtk ,AttributeEnum.MulAtk
            ,AttributeEnum.PhyAtk,AttributeEnum.IncreaPhyAtk,AttributeEnum.RatePhyAtk ,AttributeEnum.MulPhyAtk
            ,AttributeEnum.MagicAtk,AttributeEnum.IncreaMagicAtk,AttributeEnum.RateMagicAtk ,AttributeEnum.MulMagicAtk
            ,AttributeEnum.SpiritAtk,AttributeEnum.IncreaSpiritAtk,AttributeEnum.RateSpiritAtk ,AttributeEnum.MulSpiritAtk
            ,AttributeEnum.HP,AttributeEnum.IncreaHp,AttributeEnum.RateHp ,AttributeEnum.MulHp
            ,AttributeEnum.Def, AttributeEnum.IncreaDef,AttributeEnum.RateDef ,AttributeEnum.MulDef
        };

    AttributeEnum[] array2 = new AttributeEnum[] {
            AttributeEnum.Lucky, AttributeEnum.Curse,
            AttributeEnum.Accuracy, AttributeEnum.Miss,
            AttributeEnum.Speed, AttributeEnum.MoveSpeed,
            AttributeEnum.Cd,AttributeEnum.RestoreIncrea,
            AttributeEnum.CritRate, AttributeEnum.CritDamage,
            AttributeEnum.CritRateResist, AttributeEnum.CritDamageResist,
            AttributeEnum.DeadlyRate, AttributeEnum.DeadlyDamage,
            AttributeEnum.ExclusiveDamage,  AttributeEnum.BabelDamage,
            AttributeEnum.CardDamage, AttributeEnum.LegacyDamage,
            AttributeEnum.FashionDamage, AttributeEnum.AchievementDamage,
        };

    AttributeEnum[] array3 = new AttributeEnum[] {
            AttributeEnum.GoldKillIncrea, AttributeEnum.ExpKillIncrea,
            AttributeEnum.GoldIncrea, AttributeEnum.ExpIncrea,
            AttributeEnum.QualityIncrea, AttributeEnum.BurstIncrea,
            //AttributeEnum.SkillSuitCount, AttributeEnum.SkillBattleNumber,
            //AttributeEnum.SkillLevelRise,
            //AttributeEnum.PetBattleLimit, AttributeEnum.PetOnLimit,
        };

    private void Open(ShowDialogUserAttrEvent e)
    {
        this.gameObject.SetActive(true);


        this.Show();
    }

    public void Show()
    {

        User user = GameProcessor.Inst.User;

        AttributeEnum[] array = list[SelectType];

        for (int i = 0; i < items.Length; i++)
        {
            Item_Attr item = items[i];
            if (i < array.Length)
            {
                item.gameObject.SetActive(true);

                AttributeEnum attrId = array[i];
                item.SetContent((int)attrId, user.AttributeBonus.CalPanelSingleAtr(attrId));
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
