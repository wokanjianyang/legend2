using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Skill_Detail : MonoBehaviour
{
    public Button Btn_Close;

    public Text Txt_Title;

    public Transform Tf_Talent;
    public Transform Tf_Rune;
    public Transform Tf_Suit;

    List<Item_Skill_Rune> tList = new List<Item_Skill_Rune>();
    List<Item_Skill_Rune> rList = new List<Item_Skill_Rune>();
    List<Item_Skill_Rune> sList = new List<Item_Skill_Rune>();

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        tList = Tf_Talent.GetComponentsInChildren<Item_Skill_Rune>().ToList();
        rList = Tf_Rune.GetComponentsInChildren<Item_Skill_Rune>().ToList();
        sList = Tf_Suit.GetComponentsInChildren<Item_Skill_Rune>().ToList();
    }

    public void Show(SkillPanel skillPanel)
    {
        this.gameObject.SetActive(true);

        this.Txt_Title.text = skillPanel.Config.Name + "-ººƒ‹œÍ«È";

        for (int i = 0; i < tList.Count; i++)
        {
            if (i < skillPanel.TalentTextList.Count)
            {
                tList[i].gameObject.SetActive(true);
                tList[i].SetTalent(skillPanel.TalentTextList[i].Key, skillPanel.TalentTextList[i].Value);
            }
            else
            {
                tList[i].gameObject.SetActive(false);
            }
        }


        for (int i = 0; i < rList.Count; i++)
        {
            if (i < skillPanel.RuneTextList.Count)
            {
                rList[i].gameObject.SetActive(true);
                rList[i].SetRune(skillPanel.RuneTextList[i].Key, skillPanel.RuneTextList[i].Value);
            }
            else
            {
                rList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < sList.Count; i++)
        {
            if (i < skillPanel.SuitTextList.Count)
            {
                sList[i].gameObject.SetActive(true);
                sList[i].SetSuit(skillPanel.SuitTextList[i].Key, skillPanel.SuitTextList[i].Value);
            }
            else
            {
                sList[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
