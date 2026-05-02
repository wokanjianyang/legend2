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

    public Toggle tg_Hide;

    private int SelectStage = 0;
    public Transform Tf_Nav;
    private List<Toggle> toggleStageList = new List<Toggle>();

    public Panel_Card_Equip panel1;
    public Panel_Card_Special panel2;

    public Dialog_Card_Equip dialogCardEquip;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggleStageList = tg_Hide.GetComponentsInChildren<Toggle>().ToList();

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

        if (index == 3)
        {
            this.panel1.gameObject.SetActive(false);
            this.panel2.Show();
        }
        else
        {
            this.panel2.gameObject.SetActive(false);
            this.panel1.Show(this.SelectStage + 1);
        }
    }

    public void SelectItem(int cardId)
    {
        dialogCardEquip.show(cardId);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
