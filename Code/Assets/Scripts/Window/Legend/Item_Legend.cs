using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Legend : MonoBehaviour
{
    public Text Txt_Name;

    public Transform Tf_Attr;
    private List<Item_Attr> AttrList;

    public Transform Tf_Part;
    private List<Item_Legend_Part> PartList;

    public Text Txt_Set;

    public EquipLegendSetConfig Config { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        AttrList = Tf_Attr.GetComponentsInChildren<Item_Attr>().ToList();
        PartList = Tf_Part.GetComponentsInChildren<Item_Legend_Part>().ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(EquipLegendSetConfig config)
    {
        this.Config = config;

        Txt_Name.text = this.Config.Name;

        this.Show();
    }

    public void Show()
    {
        if (this.Config == null)
        {
            return;
        }

        User user = User_Data_Manager.Data;

        List<EquipLegendConfig> configs = EquipLegendConfigCategory.Instance.GetList(this.Config.Id);

        EquipLegendSet set = new EquipLegendSet(this.Config.Id);

        int k = 0;

        for (int i = 0; i < AttrList.Count; i++)
        {
            AttrList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < configs.Count; i++)
        {
            EquipLegendConfig config = configs[i];

            int lgId = config.Id;
            int lgVue = user.GetLegend(lgId);

            PartList[i].SetContent(config.Name, lgVue);

            if (lgVue > 0)
            {
                set.Add(lgVue);

                for (int j = 0; j < config.AtrIdList.Length; j++)
                {
                    AttrList[k].gameObject.SetActive(true);
                    AttrList[k].SetContent(config.AtrIdList[j], config.AtrVueList[j]);
                    k++;
                }
            }
        }

        if (set.Count >= 3)
        {
            this.Txt_Name.text = Config.Name + "£¨×ÊÖÊ" + set.Total_Fliar + "£©";
            this.Txt_Set.text = set.FormatDesc();

        }
        else
        {
            this.Txt_Name.text = Config.Name + string.Format("<color=#C8C8B4>£¨Î´¼¤»î£©</color>");
            this.Txt_Set.text = "";
        }




    }
}

