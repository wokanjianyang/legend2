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

    public Transform Tf_List_Equip;
    private List<Item_Card_Equip_Sub> equipList;

    public Transform Tf_List_Pet;
    private List<Item_Card_Pet_Sub> petList;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        this.Btn_Close.onClick.AddListener(OnClick_Close);

        equipList = Tf_List_Equip.GetComponentsInChildren<Item_Card_Equip_Sub>().ToList();

        petList = Tf_List_Pet.GetComponentsInChildren<Item_Card_Pet_Sub>().ToList();
    }


    public void show(int cardId)
    {
        this.gameObject.SetActive(true);

        CardConfig config = CardConfigCategory.Instance.Get(cardId);

        this.Txt_Title.text = config.Name;

        if (config.Stage <= 10)
        {
            Tf_List_Equip.gameObject.SetActive(true);
            Tf_List_Pet.gameObject.SetActive(false);

            List<EquipConfig> configs = EquipConfigCategory.Instance.GetCardList(cardId);

            for (int i = 0; i < equipList.Count; i++)
            {
                if (i >= configs.Count)
                {
                    equipList[i].gameObject.SetActive(false);
                }
                else
                {
                    equipList[i].SetContent(configs[i]);
                }
            }
        }
        else
        {
            Tf_List_Equip.gameObject.SetActive(false);
            Tf_List_Pet.gameObject.SetActive(true);

            List<PetConfig> configs = PetConfigCategory.Instance.GetListByCardId(cardId);

            for (int i = 0; i < petList.Count; i++)
            {
                if (i >= configs.Count)
                {
                    petList[i].gameObject.SetActive(false);
                }
                else
                {
                    petList[i].SetContent(configs[i]);
                }
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
