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
using Game.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Game
{
    public class Panel_Code : MonoBehaviour
    {
        public Transform tf_Name;
        [LabelText("名字输入框")]
        public InputField if_Name;
        [LabelText("修改")]
        public Button btn_ChangeName;

        [LabelText("兑换码输入框")]
        public InputField if_Code;
        [LabelText("兑换")]
        public Button btn_Code;

        private long ticket = 0;
        private long GoldUnit = 1000000;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_ChangeName.onClick.AddListener(this.OnClick_ChangeName);
            this.btn_Code.onClick.AddListener(this.OnClick_Code);

            if (ConfigHelper.Channel == ConfigHelper.Channel_Tap)
            {
                tf_Name.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick_ChangeName()
        {
            string name = if_Name.text.Trim();

            if (!IsValid(name))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "名字不能超过6个字，并且只能是汉字、字母、数字", ToastType = ToastTypeEnum.Failure });
                return;
            }

            User_Data_Manager.Data.Name = name;
            GameProcessor.Inst.SaveData();
            //设置名称
            GameProcessor.Inst.EventCenter.Raise(new SetPlayerNameEvent
            {
                Name = name
            });
            GameProcessor.Inst.PlayerManager.GetHero().EventCenter.Raise(new SetPlayerNameEvent
            {
                Name = name
            });
        }

        public void OnClick_Code()
        {
            string code = if_Code.text;
            if (code != null)
            {
                code = code.Trim();

                if (code.Length == 46)
                {
                    //GameProcessor.Inst.Yundang = true;

                    long ct = TimeHelper.ClientNowSeconds();
                    if (ct - ticket <= 10)
                    {
                        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请稍后再试", ToastType = ToastTypeEnum.Failure });
                    }

                    ticket = ct;

                    User user = User_Data_Manager.Data;
                    string str_json = JsonConvert.SerializeObject(user, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    str_json = EncryptionHelper.AesEncrypt(str_json);

                    string md5 = EncryptionHelper.Md5(str_json);
                    byte[] bytes = Encoding.UTF8.GetBytes(str_json);

                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("md5", md5);


                    StartCoroutine(NetworkHelper.CreateAccountNew(bytes, headers,
                            (WebResultWrapper result) =>
                            {
                                if (result.Code == StatusMessage.OK)
                                {
                                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "提交成功", ToastType = ToastTypeEnum.Success });
                                }
                                else
                                {
                                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = result.Msg, ToastType = ToastTypeEnum.Failure });
                                }

                            },
                           () =>
                           {
                               GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "网络错误", ToastType = ToastTypeEnum.Failure });
                           }));

                }
                else if (code.Length > 20)
                {
                    SpecialCode(code);
                }
                else
                {
                    NormalCode(code);
                }

            }
        }

        private void NormalCode(string code)
        {
            User user = User_Data_Manager.Data;

            if (user.GiftList.ContainsKey(code))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经使用了兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            List<CodeConfig> list = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            List<CodeConfig> configs = list.Where(m => m.code == code && m.Id < 200).ToList();

            if (configs.Count != 1)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有这个兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兑换成功", ToastType = ToastTypeEnum.Success });

            CodeConfig config = configs[0];

            List<Item> items = new List<Item>();

            long gold = 0;

            for (int i = 0; i < config.ItemTypeList.Count(); i++)
            {
                int quantity = 1;
                if (config.ItemQuanlityList != null && config.ItemQuanlityList.Count() > i)
                {
                    quantity = config.ItemQuanlityList[i];
                }

                ItemType type = (ItemType)config.ItemTypeList[i];

                if (type == ItemType.Gold)
                {
                    gold += GoldUnit * quantity;
                }
                else
                {
                    Item item = ItemHelper.BuildItem(type, config.ItemIdList[i], 0, quantity);
                    items.Add(item);
                }
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Important = 1,
                Message = BattleMsgHelper.BuildGiftPackMessage("兑换码奖励", 0, gold, items)
            });

            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

            user.GiftList[code] = true;
        }

        private void SpecialCode(string code)
        {
            User user = User_Data_Manager.Data;

            if (user.GiftList.ContainsKey(code))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经使用了兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //if (UserData.tapAccount == "")
            //{
            //    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先在其他设置里面,绑定Tap帐号", ToastType = ToastTypeEnum.Failure });
            //    return;
            //}

            if (user.DeviceId == "")
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "存档Id错误", ToastType = ToastTypeEnum.Failure });
                return;
            }

            CodeConfig config = CodeConfigCategory.Instance.GetSpeicalConfig(code);

            if (config == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有这个兑换码", ToastType = ToastTypeEnum.Failure });
                return;
            }

            user.GiftList[code] = true;

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兑换成功", ToastType = ToastTypeEnum.Success });

            if (config.Type == 99)
            {
                user.AdData.SaveCode(code);
            }
            else
            {
                List<Item> items = new List<Item>();

                long gold = 0;

                for (int i = 0; i < config.ItemTypeList.Count(); i++)
                {
                    int quantity = 1;
                    if (config.ItemQuanlityList != null && config.ItemQuanlityList.Count() > i)
                    {
                        quantity = config.ItemQuanlityList[i];
                    }

                    ItemType type = (ItemType)config.ItemTypeList[i];

                    if (type == ItemType.Gold)
                    {
                        gold += GoldUnit * quantity;
                    }
                    else
                    {
                        Item item = ItemHelper.BuildItem(type, config.ItemIdList[i], 0, quantity);
                        items.Add(item);
                    }
                }

                if (gold > 0)
                {
                    user.AddExpAndGold(0, gold);
                }

                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Important = 1,
                    Message = BattleMsgHelper.BuildGiftPackMessage("兑换码奖励", 0, gold, items)
                });

                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }

        /// <summary>
        /// 验证字符串：
        /// 1. 只能包含汉字、英文字母、数字
        /// 2. 不能包含空格或其他特殊字符
        /// 3. 总字符数不能超过 6 个
        /// </summary>
        public static bool IsValid(string input)
        {
            // 1. 非空检查
            if (string.IsNullOrEmpty(input))
                return false;

            // 2. 长度检查：C# 中 string.Length 返回的是字符数（Char Count）
            // 一个汉字、一个字母、一个数字都算 1 个字符
            if (input.Length > 6)
                return false;

            // 3. 正则检查：只允许汉字、字母、数字
            //^          : 字符串开头
            // [\u4e00-\u9fa5] : 匹配常用汉字 Unicode 范围
            // a-zA-Z     : 匹配大小写英文字母
            // 0-9        : 匹配数字
            // +          : 至少出现一次（如果允许空串可改为 *，但前面已做空值判断）
            // $          : 字符串结尾
            string pattern = @"^[\u4e00-\u9fa5a-zA-Z0-9]+$";

            return Regex.IsMatch(input, pattern);
        }



    }
}