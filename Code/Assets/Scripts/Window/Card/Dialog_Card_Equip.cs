using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Card_Equip : MonoBehaviour
{
    public Button Btn_Close;
    public Text Txt_Title;

    public Transform Tf_List;
    private List<Item_Card_Equip_Sub> list;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        this.Btn_Close.onClick.AddListener(OnClick_Close);

        list = Tf_List.GetComponentsInChildren<Item_Card_Equip_Sub>().ToList();
    }


    public void show(int cardId)
    {
        this.gameObject.SetActive(true);

        CardConfig config = CardConfigCategory.Instance.Get(cardId);

        this.Txt_Title.text = config.Name;

        List<EquipConfig> configs = EquipConfigCategory.Instance.GetCardList(cardId);

        for (int i = 0; i < list.Count; i++)
        {
            if (i > configs.Count)
            {
                list[i].gameObject.SetActive(false);
            }
            else
            {
                list[i].SetContent(configs[i]);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
