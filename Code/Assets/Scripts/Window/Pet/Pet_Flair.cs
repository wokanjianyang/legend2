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


    public void SetContent(int fid, long fv, double kc)
    {
        PetAtrConfig config = PetAtrConfigCategory.Instance.Get(fid);

        int total = (int)(kc / fv) * config.AtrVue;

        Txt_Name.text = "每点" + fv + "杀敌数，增加" + StringHelper.FormatAttrText(config.AtrId, config.AtrVue);
        Txt_Total.text = "（累计" + StringHelper.FormatAttrValueName(config.AtrId) + total + "）";
    }
}
