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
using SA.Android.App;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using Game.Data;
using System.Text.RegularExpressions;

using TapSDK.Core;
using TapSDK.Login;

namespace Game
{
    public class Panel_Other : MonoBehaviour
    {
        [LabelText("显示怪物技能特效")]
        public Toggle tog_Monster_Skill;
        public Toggle tog_Monster_Damage;
        public Toggle tog_Player;
        public Dropdown dp_InfoColor;
        //public Button btn_Query;

        public Panel_Tap pnlTap;
        public Panel_QQ pnlQQ;

        // Start is called before the first frame update
        void Start()
        {
            tog_Monster_Skill.onValueChanged.AddListener((isOn) =>
            {
                this.ShowSkill(isOn);
            });
            tog_Monster_Damage.onValueChanged.AddListener((isOn) =>
            {
                this.ShowDamage(isOn);
            });
            tog_Player.onValueChanged.AddListener((isOn) =>
            {
                this.ShowPlayerEffect(isOn);
            });

            dp_InfoColor.ClearOptions();
            dp_InfoColor.AddOptions(new List<string>() { "白色", "绿色", "蓝色", "紫色", "橙色", "红色", "金色" });

            dp_InfoColor.onValueChanged.AddListener(this.onValueChange);

            this.Init();
        }

        private void onValueChange(int value)
        {
            //Debug.Log("dropDown：" + value);

            AppHelper.SetData.InfoColor = value + 1;
            User_Data_Manager.SettingSave();
        }

        public void Init()
        {
            //Debug.Log("Other init");
            User user = User_Data_Manager.Data;

            tog_Monster_Skill.isOn = AppHelper.SetData.ShowMonsterSkill;
            tog_Monster_Damage.isOn = AppHelper.SetData.ShowMonsterDamage;
            tog_Player.isOn = AppHelper.SetData.ShowPlayerEffect;
            dp_InfoColor.value = AppHelper.SetData.InfoColor - 1;


            if (ConfigHelper.Channel == ConfigHelper.Channel_Tap)
            {
                pnlTap.gameObject.SetActive(true);
                pnlQQ.gameObject.SetActive(false);
            }
            else {
                pnlTap.gameObject.SetActive(false);
                pnlQQ.gameObject.SetActive(true);
            }
        }

        public void ShowSkill(bool show)
        {
            AppHelper.SetData.ShowMonsterSkill = show;
            User_Data_Manager.SettingSave();
        }
        public void ShowDamage(bool show)
        {
            AppHelper.SetData.ShowMonsterDamage = show;
            User_Data_Manager.SettingSave();
        }

        public void ShowPlayerEffect(bool show)
        {
            AppHelper.SetData.ShowPlayerEffect = show;
            User_Data_Manager.SettingSave();
            //保存到本地设置
        }
    }
}
