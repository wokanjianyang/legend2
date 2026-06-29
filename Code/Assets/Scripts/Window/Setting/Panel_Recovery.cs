using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.UI.Dropdown;
using System;

namespace Game
{
    public class Panel_Recovery : MonoBehaviour
    {
        [Title("普通装备")]
        public Dropdown dp_Equip_Skill;
        public InputField if_Lucky;
        public InputField if_Curse;
        public InputField if_Speed;
        public InputField if_Cd;
        public InputField if_Gold;
        public InputField if_Exp;
        public InputField if_DropRate;
        public InputField if_DropQuality;
        public InputField if_Sk;

        public Dropdown Dp_Equip_Base;

        public Transform tran_EquipRoleList;
        private Toggle[] equipRoleToggles;

        public InputField if_EquipLevel;

        [Title("传奇装备")]
        public InputField if_Legend_Level;

        [Title("自动图鉴")]
        public InputField if_Card_EquipLevel;

        [Title("其他")]
        public Dropdown dp_Pet;
        public InputField ifSpeicalLevel;

        [Title("功能")]
        public Button btn_Done;
        public Transform Tf_Nav;
        public List<Transform> Panels;

        // Start is called before the first frame update

        private int startQuality = 4;

        private List<Toggle> Panel_Toggles;



        void Awake()
        {
            Panel_Toggles = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

            this.btn_Done.onClick.AddListener(this.OnClick_Done);

            for (int i = 0; i < Panel_Toggles.Count; i++)
            {
                int index = i;

                Panel_Toggles[i].onValueChanged.AddListener((isOn) =>
                {
                    this.ShowPanel(index);
                });

            }
        }

        void Start()
        {
            this.Init();
        }

        private void ShowPanel(int index)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                this.Panels[i].gameObject.SetActive(i == index);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {
            Debug.Log("Init Recovery");

            //初始化
            equipRoleToggles = tran_EquipRoleList.GetComponentsInChildren<Toggle>();

            //普通装备保留颜色
            dp_Equip_Skill.ClearOptions();
            dp_Equip_Skill.AddOptions(new List<string>() { "无", "紫色", "橙色" });

            Dp_Equip_Base.ClearOptions();
            Dp_Equip_Base.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色" });

            dp_Pet.ClearOptions();
            dp_Pet.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色" });


            //
            User user = User_Data_Manager.Data;
            RecoverySetting setting = user.RecoverySet;

            if (setting.EquipQualityKeep > 0)
            {
                dp_Equip_Skill.value = setting.EquipQualityKeep;
            }

            if (setting.EquipQualityRecovery > 0)
            {
                Dp_Equip_Base.value = setting.EquipQualityRecovery;
            }

            if (setting.PetQuality > 0)
            {
                dp_Pet.value = setting.PetQuality;
            }

            if_Lucky.text = setting.LuckyTotal.ToString();
            if_Curse.text = setting.CurseTotal.ToString();
            if_Speed.text = setting.SpeedTotal.ToString();
            if_Cd.text = setting.CdTotal.ToString();

            if_Exp.text = setting.ExpTotal.ToString();
            if_Gold.text = setting.GoldTotal.ToString();
            if_DropRate.text = setting.DropRate.ToString();
            if_DropQuality.text = setting.DropQuality.ToString();
            if_Sk.text = setting.SkLevel.ToString();

            if_EquipLevel.text = setting.EquipLevel.ToString();

            if_Card_EquipLevel.text = setting.CardEquipLevel.ToString();

            foreach (int key in setting.EquipRole.Keys)
            {
                equipRoleToggles[key - 1].isOn = setting.EquipRole[key];
            }

            //其他
            ifSpeicalLevel.text = setting.SpecailLevel.ToString();

            //传奇装备
            if_Legend_Level.text = setting.LegendLevel.ToString();

        }


        public void OnClick_Done()
        {
            this.SaveSetting();
        }

        private void SaveSetting()
        {
            User user = User_Data_Manager.Data;

            RecoverySetting setting = user.RecoverySet;

            setting.SetTotal++;

            //普通装备回收
            setting.EquipQualityKeep = dp_Equip_Skill.value;
            setting.EquipQualityRecovery = Dp_Equip_Base.value;

            int.TryParse(if_Exp.text, out int exp);
            setting.ExpTotal = exp;

            int.TryParse(if_Gold.text, out int gold);
            setting.GoldTotal = gold;

            int.TryParse(if_Lucky.text, out int lucky);
            setting.LuckyTotal = lucky;

            int.TryParse(if_Curse.text, out int curse);
            setting.CurseTotal = curse;

            int.TryParse(if_Speed.text, out int speed);
            setting.SpeedTotal = speed;

            int.TryParse(if_Cd.text, out int cd);
            setting.CdTotal = cd;

            int.TryParse(if_DropRate.text, out int dropRate);
            setting.DropRate = dropRate;

            int.TryParse(if_DropQuality.text, out int dropQuality);
            setting.DropQuality = dropQuality;

            int.TryParse(if_Sk.text, out int sk);
            setting.SkLevel = sk;

            int.TryParse(if_EquipLevel.text, out int equipLevel);
            setting.EquipLevel = equipLevel;

            for (var i = 0; i < equipRoleToggles.Length; i++)
            {
                setting.EquipRole[i + 1] = equipRoleToggles[i].isOn;
            }


            //图鉴提交
            int.TryParse(if_EquipLevel.text, out int cardEquipLevel);
            setting.CardEquipLevel = cardEquipLevel;


            //传奇装备
            int.TryParse(if_Legend_Level.text, out int legendLevel);
            setting.LegendLevel = legendLevel;

            //其他回收
            setting.PetQuality = dp_Pet.value;

            int.TryParse(ifSpeicalLevel.text, out int speicalLevel);
            setting.SpecailLevel = speicalLevel;

            //立即执行一次回收
            GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = RuleType.Normal });

            //TaskHelper.CheckTask(TaskType.Recovery, 1);

            GameProcessor.Inst.SaveData();

            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());

        }
    }
}
