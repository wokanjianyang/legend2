using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class View_Nav : MonoBehaviour,IBattleLife
    {

        public Button btn_Battle;
        public Button btn_Bag;
        public Button btn_Forge;
        public Button btn_Skill;
        public Button btn_More;


        // Start is called before the first frame update
        void Start()
        {
            this.btn_Battle.onClick.AddListener(this.OnClick_Battle);
            this.btn_Bag.onClick.AddListener(this.OnClick_Bag);
            this.btn_Forge.onClick.AddListener(this.OnClick_Forge);
            this.btn_Skill.onClick.AddListener(this.OnClick_Skill);
            this.btn_More.onClick.AddListener(this.OnClick_More);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.gameObject.SetActive(true);
        }

        private void OnClick_Bag()
        {
            this.ChangePage(ViewPageType.View_Bag);
        }
        private void OnClick_Battle()
        {
            this.ChangePage(ViewPageType.View_Battle);
        }

        private void OnClick_More()
        {
            this.ChangePage(ViewPageType.View_More);
        }
        private void OnClick_Skill()
        {
            this.ChangePage(ViewPageType.View_Skill);
        }
        
        private void OnClick_Forge()
        {
            this.ChangePage(ViewPageType.View_Forge);
        }

        private void ChangePage(ViewPageType page)
        {
            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { 
                Page = page
            });
        }
    }
}
