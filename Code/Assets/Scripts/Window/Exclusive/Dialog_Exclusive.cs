using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Exclusive : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;

        public Transform Tf_Cycle;
        private List<Toggle> Toggle_Cycle_List = new List<Toggle>();

        public Panel_Exclusive_Material Panel1;
        public Panel_Exclusive Panel2;

        private int CycleIndex = 0;

        public Button Btn_Convert;

        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            Btn_Convert.onClick.AddListener(OnClick_Convert);

            Toggle_Cycle_List = Tf_Cycle.GetComponentsInChildren<Toggle>().ToList();


            for (int i = 0; i < Toggle_Cycle_List.Count; i++)
            {
                int index = i;
                Toggle_Cycle_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangeCycle(index);
                    }
                });
            }
        }

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<OpenDialogEvent>(this.Open);
        }

        private void Open(OpenDialogEvent e)
        {
            if (e.Type == DialogType.Exclusive)
            {
                this.gameObject.SetActive(true);
            }
        }


        void Start()
        {
            this.ChangeCycle(0);
        }

        private void ChangeCycle(int i)
        {
            this.CycleIndex = i;

            if (CycleIndex == 0)
            {
                Panel1.gameObject.SetActive(true);
                Panel2.gameObject.SetActive(false);

                Panel1.Show(0);
            }
            else
            {
                Panel1.gameObject.SetActive(false);
                Panel2.gameObject.SetActive(true);

                int[] roleList = { -1, 1, 2, 3, 0 };

                Panel2.Show(roleList[CycleIndex]);
            }
        }

        public void OnClick_Convert()
        {
            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("把所有紫色材料100:1兑换成同地图的红色材料，橙色材料10:1兑换成同地图的红色材料，是否确认？", true,
            () =>
            {
                User user = User_Data_Manager.Data;

                for (int i = 0; i < 12; i++)
                {
                    Convert(40000001 + i * 3, 100, 40000003 + i * 3);
                    Convert(40000002 + i * 3, 10, 40000003 + i * 3);
                }

                //刷新
                Panel1.Show(0);
            }, () =>
            {

            });
        }

        private void Convert(int fromId, int convetRate, int targetId)
        {
            User user = User_Data_Manager.Data;

            long count = user.GetHideMaterialCount(fromId);

            long vc = count / convetRate;
            if (vc > 0)
            {
                user.UseHideMaterialCount(fromId, vc * convetRate);
                user.SaveHideMaterialCount(targetId, vc);
            }
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
