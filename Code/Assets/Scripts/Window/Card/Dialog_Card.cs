using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Card : MonoBehaviour
{
    public Button Btn_Close;

    private int SelectStage = 0;
    public Transform Tf_Nav;
    private List<Toggle> toggleStageList = new List<Toggle>();

    public Panel_Card panel1;

    public Text Txt_Title;
    private string[] Titles = { "宠物图鉴", "装备图鉴", "60-120级装备", "130-180级装备" };
    private int[] stages = { 11, 1, 2, 3 };

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggleStageList = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

        this.Btn_Close.onClick.AddListener(OnClick_Close);

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        this.ChangePanel(0);
    }

    private void ChangePanel(int index)
    {
        this.SelectStage = index;
        this.Txt_Title.text = Titles[index];
        this.panel1.Show(this.stages[index]);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
