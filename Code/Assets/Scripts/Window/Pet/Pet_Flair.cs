using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet_Flair : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Total;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void SetContent(int fid, long rise, double count)
    {
        PetAtrConfig config = PetAtrConfigCategory.Instance.Get(fid);

        double r = rise / ConfigHelper.PetKillPercent;
        double total = r * count;

        Txt_Name.text = "每点" + config.Percent + "杀敌数，增加" + StringHelper.FormatAttrValueName(config.AttrId) + r + "";
        Txt_Total.text = "（累计" + StringHelper.FormatAttrValueName(config.AttrId) + total + "）";
    }
}
