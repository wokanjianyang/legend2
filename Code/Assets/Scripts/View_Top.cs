using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class View_Top : MonoBehaviour, IBattleLife
    {
        [Title("顶部导航")]
        [LabelText("名称")]
        public Text tmp_Name;

        [LabelText("等级")]
        public Text tmp_Level;

        [LabelText("战力")]
        public Text tmp_BattlePower;

        [LabelText("金币")]
        public Text tmp_Gold;

        public Button Btn_Setting;

        private User user;

        string color = ConfigHelper.Channel == ConfigHelper.Channel_Tap ? "#CBFFC2" : "#76B0FF";

        public int Order => (int)ComponentOrder.TopNav;

        void Awake()
        {
            this.Btn_Setting.onClick.AddListener(this.OnClick_Setting);
        }

        public void OnBattleStart()
        {
            this.gameObject.SetActive(true);
            this.user = User_Data_Manager.Data;

            this.tmp_Name.text = string.Format("<color={0}>{1}</color>", color, user.Name);

            this.OnHeroInfoUpdateEvent(null);

            this.tmp_Level.text = formatLevel(user.Cycle.Data, user.MagicLevel.Data);
            this.tmp_BattlePower.text = $"战力：{user.AttributeBonus.GetPowerText()}";

            GameProcessor.Inst.EventCenter.AddListener<SetPlayerLevelEvent>(this.OnSetPlayerLevelEvent);
            GameProcessor.Inst.EventCenter.AddListener<UserInfoUpdateEvent>(this.OnHeroInfoUpdateEvent);
            GameProcessor.Inst.EventCenter.AddListener<SetPlayerNameEvent>(this.OnSetPlayerNameEvent);
            GameProcessor.Inst.EventCenter.AddListener<UserAttrChangeEvent>(this.OnShowPower);
        }

        public void OnClick_Setting()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent() { IsOpen = true });
        }

        private void OnSetPlayerLevelEvent(SetPlayerLevelEvent e)
        {
            this.tmp_Level.text = formatLevel(e.Cycle, e.Level);
        }

        private string formatLevel(long cycle, long level)
        {
            string text = "";
            if (cycle > 0)
            {
                text += ConfigHelper.CycleList[cycle] + " "; // string.Format("<color=#FF0000>{0}</color>", );
            }
            text += level + "级";

            return text;
        }

        private void OnHeroInfoUpdateEvent(UserInfoUpdateEvent e)
        {
            double gold = this.user.MagicGold.Data;

            string goldText = gold > 100000000 ? StringHelper.FormatNumber(gold) : gold + "";

            this.tmp_Gold.text = $"金币:{goldText}";
            //
        }

        private void OnShowPower(UserAttrChangeEvent e)
        {
            this.tmp_BattlePower.text = $"战力：{user.AttributeBonus.GetPowerText()}";
        }

        private void OnSetPlayerNameEvent(SetPlayerNameEvent e)
        {
            this.tmp_Name.text = string.Format("<color={0}>{1}</color>", color, e.Name);
        }

    }
}
