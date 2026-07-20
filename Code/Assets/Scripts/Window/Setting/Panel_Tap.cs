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
using TapSDK.CloudSave;

namespace Game
{
    public class Panel_Tap : MonoBehaviour
    {
        public Text txt_Info;
        public Text txt_Memo;

        public Text Txt_DeviceId;
        public Text Txt_FileId;

        public Button btn_Bind;

        public Text Txt_Save_Auto;

        public Button btn_Save;
        public Text Txt_Save;

        public Button btn_Load;
        public Text Txt_Load;

        private const int CdSaveTime = 1800;
        private const int CdLoadTime = 3600 * 1;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Bind.onClick.AddListener(this.OnClick_Change);
            this.btn_Save.onClick.AddListener(this.OnClick_Save);
            this.btn_Load.onClick.AddListener(this.OnClick_Load);

            this.Init();
        }

        float currentRoundTime = 0;
        private void Update()
        {
            //if (ConfigHelper.Channel != ConfigHelper.Channel_Tap)
            //{
            this.currentRoundTime += Time.unscaledDeltaTime;
            if (this.currentRoundTime >= 1.0)
            {
                this.currentRoundTime = 0;
                Show();
            }
            //}
        }

        private void CheckShow()
        {
            User user = User_Data_Manager.Data;
            string account = user.Account;

            if (account == "")
            {
                this.btn_Bind.gameObject.SetActive(true);
                this.btn_Save.gameObject.SetActive(false);
                this.btn_Load.gameObject.SetActive(false);
            }
            else
            {
                this.btn_Bind.gameObject.SetActive(false);
                this.btn_Save.gameObject.SetActive(true);
                this.btn_Load.gameObject.SetActive(true);

                this.txt_Memo.text = buildMeme(user.TapUUID);
            }
        }

        private string buildMeme(string account)
        {
            return "您已经登录了Tap账号，可以使用Tap云存档了\n" +
                "游戏不会自动存档，请按自身需求，\n" +
                "手动点击保存存档按钮，手动存档\n"
                + "当前存档ID：" + account;

        }

        private void Show()
        {
            this.CheckShow();

            User user = User_Data_Manager.Data;
            string account = user.Account;
            if (account == "")
            {
                return;
            }

            //long autoTime = Math.Max(user.SaveTicketTime, user.SaveTickeTimeHand);
            //if (autoTime > 0)
            //{
            //    Txt_Save_Auto.text = "最后存档时间:" + TimeHelper.SecondsToDate(autoTime).AddHours(8).ToString("G");
            //}
            //else
            //{
            //    Txt_Save_Auto.text = "还没有自动存档";
            //}

            long now = TimeHelper.ClientNowSeconds();
            long cdSaveTime = now - user.SaveTickeTimeHand;

#if UNITY_EDITOR
            cdSaveTime = 1900;
#endif
            if (cdSaveTime > CdSaveTime)
            {
                btn_Save.gameObject.SetActive(true);
                Txt_Save.gameObject.SetActive(false);
            }
            else
            {
                btn_Save.gameObject.SetActive(false);
                Txt_Save.gameObject.SetActive(true);
                Txt_Save.text = TimeSpan.FromSeconds(CdSaveTime - cdSaveTime).ToString(@"hh\:mm\:ss");
            }

            long cdLoadTime = now - user.LoadTicketTime;
#if UNITY_EDITOR
            cdLoadTime = 8000;
#endif
            if (cdLoadTime > CdLoadTime)
            {
                btn_Load.gameObject.SetActive(true);
                Txt_Load.gameObject.SetActive(false);
            }
            else
            {
                btn_Load.gameObject.SetActive(false);
                Txt_Load.gameObject.SetActive(true);
                Txt_Load.text = TimeSpan.FromSeconds(CdLoadTime - cdLoadTime).ToString(@"hh\:mm\:ss");
            }
        }

        public void Init()
        {
            User user = User_Data_Manager.Data;

            this.Txt_Save_Auto.text = "";
            this.Txt_FileId.text = "存档Id:" + user.DeviceId;
            this.Txt_DeviceId.text = "设备Id:" + AppHelper.GetDeviceIdentifier();

            this.Show();
        }

        public void OnClick_Change()
        {
            this.bindTap();
        }

        public void OnClick_Load()
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Hero);

            btn_Load.gameObject.SetActive(false);
            this.txt_Info.text = "读档中...";

            loadTapData();

        }

        public void OnClick_Save()
        {
            btn_Save.gameObject.SetActive(false);
            this.txt_Info.text = "存档中...";

            saveTapData();
        }

        private async Task loadTapData()
        {
            User user = User_Data_Manager.Data;
            user.LoadTicketTime = TimeHelper.ClientNowSeconds();

            try
            {
                List<ArchiveData> archives = await TapTapCloudSave.GetArchiveList();

                if (archives.Count <= 0)
                {
                    this.txt_Info.text = "您还没有云存档";
                    return;
                }

                string uuid = archives[archives.Count - 1].Uuid;  //读取最后一份
                string fileId = archives[archives.Count - 1].FileId;  //读取最后一份
                int serial = archives[archives.Count - 1].Playtime;

                byte[] data = await TapTapCloudSave.GetArchiveData(uuid, fileId);

                string str_json = Encoding.UTF8.GetString(data);

                Debug.Log(str_json);

                if (GameProcessor.Inst.LoadInit(str_json, "", "", serial))
                {
                    this.txt_Info.text = "读取存档成功,请退出重进";
                    User_Data_Manager.Save();
                    //GameProcessor.Inst.SaveData(); ;
                }
                else
                {
                    this.txt_Info.text = "读取失败,存档损坏,取消读档,请退出重进";
                    user.LoadTicketTime = TimeHelper.ClientNowSeconds() - CdLoadTime + 10;

                }
                Application.Quit();

                // 处理存档列表
            }
            catch (TapException ex)
            {
                // 处理错误，可使用 ex.Code 与 ex.Message
                this.txt_Info.text = "读档失败：" + ex.message;
                user.LoadTicketTime = TimeHelper.ClientNowSeconds() - CdLoadTime + 10;
            }

        }

        private async Task saveTapData()
        {
            User user = User_Data_Manager.Data;
            user.SaveTickeTimeHand = TimeHelper.ClientNowSeconds();

            try
            {
                string fileName = user.DeviceId;
                string time = DateTime.Now.ToShortTimeString();
                int serial = user.Serial + 1;

                // 存档元信息
                ArchiveMetadata metadata = new ArchiveMetadata(
                    archiveName: fileName,
                    archiveSummary: "user_File",
                    archiveExtra: time,
                    archivePlaytime: serial  // 创建时间
                );

                // 存档文件路径（单个存档文件大小不超过10MB）
                string archiveFilePath = User_Data_Manager.GetTapPath();

                User_Data_Manager.SaveTap();

                // 存档封面路径（可选，封面大小不超过512KB）
                string archiveCoverPath = "";

                if (user.TapUUID == "")
                {
                    ArchiveData archive = await TapTapCloudSave.CreateArchive(metadata, archiveFilePath, archiveCoverPath);

                    user.TapUUID = archive.Uuid;


                    Debug.Log("save success:" + archive.Uuid);
                }
                else
                {
                    string archiveUuid = user.TapUUID;


                    ArchiveData updated = await TapTapCloudSave.UpdateArchive(archiveUuid, metadata, archiveFilePath, archiveCoverPath);
                }
            }
            catch (TapException ex)
            {
                // 处理错误，可使用 ex.Code 与 ex.Message
                this.txt_Info.text = "存档失败：" + ex.message;
                user.SaveTickeTimeHand = TimeHelper.ClientNowSeconds() - CdSaveTime;
            }
        }


        private async Task bindTap()
        {
            try
            {
                // 定义授权范围
                List<string> scopes = new List<string> { TapTapLogin.TAP_LOGIN_SCOPE_PUBLIC_PROFILE };

                // 发起 Tap 登录
                var userInfo = await TapTapLogin.Instance.LoginWithScopes(scopes.ToArray());
                Debug.Log($"登录成功，当前用户 ID：{userInfo.unionId}");

                TapTapAccount account = await TapTapLogin.Instance.GetCurrentTapAccount();
                if (account != null)
                {
                    // 用户已登录
                    AccessToken accessToken = account.accessToken;
                    string openId = account.openId;
                    string name = account.name;

                    Debug.Log("unionId:" + account.unionId);

                    User user = User_Data_Manager.Data;
                    user.Name = name;
                    user.Account = account.openId;


                    this.CheckShow();
                }
                else
                {
                    // 用户未登录
                }
            }
            catch (Exception exception)
            {
                Debug.Log($"登录失败，出现异常：{exception}");
            }
        }
    }
}
