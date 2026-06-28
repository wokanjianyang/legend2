using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Defend : MonoBehaviour
{
    public Transform Tf_Parent;
    public Button btn_FullScreen;

    private List<Item_Defend> ItemList;

    // Start is called before the first frame update
    void Awake()
    {
        ItemList = Tf_Parent.GetComponentsInChildren<Item_Defend>().ToList();


        btn_FullScreen.onClick.AddListener(this.OnClick_Close);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        User user = User_Data_Manager.Data;
        user.DefendData.Check();

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetContent(i);
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
