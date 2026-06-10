using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Level_Info : MonoBehaviour
{
    public Transform Tf_List;
    private List<Skill_Level_Item> list;

    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        list = Tf_List.GetComponentsInChildren<Skill_Level_Item>().ToList();
    }

    public void Show(SkillConfig config, int currentLevel)
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetContent(config, i, currentLevel);
        }

    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
