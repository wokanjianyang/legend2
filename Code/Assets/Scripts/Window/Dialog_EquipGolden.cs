using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_EquipGolden : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;

        public List<Toggle> Toggle_Plan_List = new List<Toggle>();

        public Toggle toggle;

        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);

            toggle.onValueChanged.AddListener((isOn) =>
            {
                User_Data_Manager.Data.EquipGoldenSetting = isOn;
            });
        }

        public void Init()
        {
            SlotBox[] items = this.GetComponentsInChildren<SlotBox>();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Init(21+i);
            }

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                int index = i;
                Toggle_Plan_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangePlan(index);
                    }
                });
            }
        }

        public void OnBattleStart()
        {
            this.Init();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);

            toggle.isOn = User_Data_Manager.Data.EquipGoldenSetting;
            this.InitPlanName();
        }

        private void InitPlanName()
        {
            User user = User_Data_Manager.Data;

            int EquipGoldenIndex = user.EquipGoldenIndex;
            Toggle_Plan_List[EquipGoldenIndex].isOn = true;

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                user.PlanNameList.TryGetValue(i, out string name);
                if (name != null)
                {
                    Text tt = Toggle_Plan_List[i].GetComponentInChildren<Text>();
                    tt.text = name;
                }
            }
        }

        private void ChangePlan(int i)
        {
            //GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 3, Index = i });

            GameProcessor.Inst.UpdateInfo();
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
