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


        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);

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

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
