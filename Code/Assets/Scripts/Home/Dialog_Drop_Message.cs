using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class Dialog_Drop_Message : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;
        public ScrollRect sr_BattleMsg;

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
        }

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        }

        private List<Text> msgPool = new List<Text>();
        private int msgId = 0;



        private void OnBattleMsgEvent(BattleMsgEvent e)
        {
            if (e.Important <= 0)
            {
                return;
            }

            msgId++;
            Text txt_msg = null;
            if (this.sr_BattleMsg.content.childCount > 100)
            {
                txt_msg = msgPool[0];
                msgPool.RemoveAt(0);
                txt_msg.transform.SetAsLastSibling();
            }
            else
            {
                var msg = GameObject.Instantiate(PrefabHelper.Instance().DropMessagePrefab());
                msg.transform.SetParent(this.sr_BattleMsg.content);
                msg.transform.localScale = Vector3.one;

                var m = msg.GetComponent<Text>();


                txt_msg = m;
            }
            msgPool.Add(txt_msg);

            txt_msg.gameObject.name = $"msg_{msgId}";
            txt_msg.text = e.Message;
            this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
        }




        private void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
