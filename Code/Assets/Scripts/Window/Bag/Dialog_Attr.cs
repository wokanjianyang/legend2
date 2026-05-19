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

    public int Order => (int)ComponentOrder.Dialog;

    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowDialogUserAttrEvent>(this.Show);
    }

    private void Show(ShowDialogUserAttrEvent e)
    {
        this.gameObject.SetActive(true);

        Item_Attr[] items = this.GetComponentsInChildren<Item_Attr>();

        User user = GameProcessor.Inst.User;

        AttributeEnum[] list = new AttributeEnum[] {
             AttributeEnum.Atk,AttributeEnum.IncreaAtk,AttributeEnum.RateAtk //,AttributeEnum.MulAtk
            ,AttributeEnum.PhyAtk,AttributeEnum.IncreaPhyAtk,AttributeEnum.RatePhyAtk //,AttributeEnum.MulPhyAtk
            ,AttributeEnum.MagicAtk,AttributeEnum.IncreaMagicAtk,AttributeEnum.RateMagicAtk //,AttributeEnum.MulMagicAtk
            ,AttributeEnum.SpiritAtk,AttributeEnum.IncreaSpiritAtk,AttributeEnum.RateSpiritAtk //,AttributeEnum.MulSpiritAtk

            ,AttributeEnum.HP,AttributeEnum.IncreaHp,AttributeEnum.RateHp //,AttributeEnum.MulAtk
            ,AttributeEnum.Def, AttributeEnum.IncreaDef,AttributeEnum.RateDef //,AttributeEnum.MulDef

            ,AttributeEnum.Speed, AttributeEnum.MoveSpeed,

            AttributeEnum.CardDamage, AttributeEnum.FashionDamage,AttributeEnum.AchievementDamage, AttributeEnum.LegacyDamage,
            //AttributeEnum.PhyDamage,  AttributeEnum.MulPhyDamageRise,
            //AttributeEnum.MagicDamage,AttributeEnum.MulMagicDamageRise,
            //AttributeEnum.SpiritDamage,AttributeEnum.MulSpiritDamageRise,

            //AttributeEnum.MulAttr, AttributeEnum.MulHp, AttributeEnum.MulDef,
            //AttributeEnum.MulAttrPhy, AttributeEnum.MulAttrMagic, AttributeEnum.MulAttrSpirit,

            //AttributeEnum.MulDamageIncrea, AttributeEnum.MulDamageResist,

            //AttributeEnum.BurstMul, AttributeEnum.RateExp, AttributeEnum.RateGold,
            //AttributeEnum.RateBurst, AttributeEnum.RateQuality,

            //AttributeEnum.Strong,AttributeEnum.StrongMul,
            //AttributeEnum.SecondExp,AttributeEnum.SecondExp,
            //AttributeEnum.CritDamage,AttributeEnum.CritRateResist,
            //AttributeEnum.Shatter,  AttributeEnum.SpiritAll,
        };

        for (int i = 0; i < items.Length; i++)
        {
            Item_Attr item = items[i];
            if (i < list.Length)
            {
                item.gameObject.SetActive(true);

                AttributeEnum attrId = list[i];
                item.SetContent((int)attrId, user.AttributeBonus.CalPanelSingleAttr(attrId));
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
