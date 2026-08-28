using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class Dialog_System : MonoBehaviour
    {
        public Button Btn_Close;

        public Button Btn_Fashion;
        public Button Btn_SoulRing;
        public Button Btn_Ring;
        public Button Btn_Wing;
        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            Btn_Fashion.onClick.AddListener(OnClick_Fashion);
            Btn_SoulRing.onClick.AddListener(OnClick_SoulRing);
            Btn_Ring.onClick.AddListener(OnClick_Ring);
            Btn_Wing.onClick.AddListener(OnClick_Wing);
        }


        private void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
        private void OnClick_Fashion()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new OpenDialogEvent() { Type = DialogType.Fashion });
        }

        private void OnClick_SoulRing()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new OpenDialogEvent() { Type = DialogType.SoulRing });
        }

        private void OnClick_Ring()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new OpenDialogEvent() { Type = DialogType.Ring });
        }

        private void OnClick_Wing()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new OpenDialogEvent() { Type = DialogType.Wing });
        }
    }
}
