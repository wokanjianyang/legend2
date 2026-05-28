using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class AViewPage : MonoBehaviour, IBattleLife
    {

        private bool isInit = false;
        private bool isOpen = false;

        private void OnChangePageEnvent(ChangePageEvent e)
        {
            this.gameObject.SetActive(this.CheckPageType(e.Page));

            //if (e.Page == ViewPageType.View_Battle)
            //{
            //    GameProcessor.Inst.PlayerInfo?.SetShow(true);


            //    //Debug.Log("open view battle");

            //    //重新计算人物属性
            //    GameProcessor.Inst.UpdateInfo();
            //}
            //else
            //{
            //    GameProcessor.Inst.PlayerInfo?.SetShow(false);
            //}


            if (this.CheckPageType(e.Page))
            {
                if (!isInit)
                {
                    isInit = true;
                    this.OnInit();
                }
                if (!this.isOpen)
                {
                    this.isOpen = true;
                    this.OnOpen();
                }
            }
            else
            {
                this.isOpen = false;
            }
        }

        protected abstract bool CheckPageType(ViewPageType page);

        virtual public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ChangePageEvent>(this.OnChangePageEnvent);
            this.OnChangePageEnvent(new ChangePageEvent
            {
                Page = ViewPageType.View_Battle
            });
        }

        public int Order
        {
            get
            {
                return (int)ComponentOrder.ViewPage;
            }
        }

        virtual public void OnInit()
        {

        }

        virtual public void OnOpen()
        {

        }
    }
}
