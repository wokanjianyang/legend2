using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet_Flair : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Count;
    public Text Txt_Total;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void SetContent(int fid, long rise, long count)
    {
        PetConfig config = PetConfigCategory.Instance.Get(fid);

        double r = rise / ConfigHelper.PetKillPercent;
        double total = r * count;

        Txt_Name.text = config.Name + "£º" + r;
        Txt_Count.text = "£¨É±µÐÊý" + count + "£©";
        Txt_Total.text = "£¨ÀÛ¼Æ" + StringHelper.FormatAttrValueName(config.AttrId) + total + "£©";
    }
}
