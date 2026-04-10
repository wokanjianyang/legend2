using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Map_Group : MonoBehaviour
{
    public Button Btn_Group;

    public Text Txt_Name;
    public Text Txt_Desc;
    public Text Txt_Icon;

    public Transform Tf_Item_List;

    private bool expend = false;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Group.onClick.AddListener(OnClick_Name);
    }

    private void OnEnable()
    {
    }

    public void SetContent(MapConfig mapConfig)
    {
        Txt_Name.text = mapConfig.Name;
        Txt_Desc.text = mapConfig.Memo;
    }

    private void OnClick_Name()
    {
        expend = !expend;

        Tf_Item_List.gameObject.SetActive(expend);
    }
}