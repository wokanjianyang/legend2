using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public static class NetworkHelper
    {
        //private static string home = "http://127.0.0.1:11111/public/";
        private static string home = "http://120.76.249.105/public/";
        //private static string home = "http://192.168.10.5:11111/public/";


        public static string[] GetAddressIPs()
        {
            List<string> list = new List<string>();
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                {
                    continue;
                }
                foreach (UnicastIPAddressInformation add in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    list.Add(add.Address.ToString());
                }
            }
            return list.ToArray();
        }

        public static IPEndPoint ToIPEndPoint(string host, int port)
        {
            return new IPEndPoint(IPAddress.Parse(host), port);
        }

        public static IPEndPoint ToIPEndPoint(string address)
        {
            int index = address.LastIndexOf(':');
            string host = address.Substring(0, index);
            string p = address.Substring(index + 1);
            int port = int.Parse(p);
            return ToIPEndPoint(host, port);
        }

        public static string BuildCode()
        {
            //string deviceId = AppHelper.GetDeviceIdentifier();
            //string fileId = User_Data_Manager.Data.DeviceId;
            string skey = AppHelper.GetBaseMd5();
            //string code = EncryptionHelper.AesEncrypt(skey, (deviceId + fileId).Substring(0, 16));

            return skey;
        }

        public static string BuildSign()
        {
            string deviceId = AppHelper.GetDeviceIdentifier();
            string fileId = User_Data_Manager.Data.DeviceId;
            string skey = AppHelper.getKey();

            string code = EncryptionHelper.AesEncrypt(deviceId, skey);
            //Debug.Log("code:" + code);

            code = EncryptionHelper.Md5(code + fileId);
            //Debug.Log("code:" + code);

            return code;
        }


        public static string BuildUpdateParam(User user)
        {
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            paramDict.Add("account", user.Account);
            paramDict.Add("name", user.Name);
            paramDict.Add("power", user.AttributeBonus.GetPowerText());
            paramDict.Add("gold", StringHelper.FormatNumber(user.MagicGold.Data));
            paramDict.Add("level", user.MagicLevel.Data + "");
            paramDict.Add("cycle", user.Cycle.Data + "");

            long advert = user.GetAchievementProgeress(AchievementProType.Advert);
            paramDict.Add("advert", advert + "");

            long fashion = GetTotal(user.Bags, ItemHelper.Fashion_Stone);
            foreach (var sp in user.FashionData)
            {
                if (sp.Value.Data > 0)
                {
                    FashionConfig fashionConfig = FashionConfigCategory.Instance.Get(sp.Key);
                    fashion += sp.Value.Data * fashionConfig.Fee;
                }
            }
            paramDict.Add("fashion", fashion + "");

            long halidom = user.HalidomData.Select(m => m.Value.Data).Sum();
            paramDict.Add("halidom", halidom + "");

            long infiniteMax = user.GetAchievementProgeress(AchievementProType.Infinite);
            paramDict.Add("infinite", infiniteMax + "");


            paramDict.Add("swing", user.WingData.Data + "");

            long strongTotal = user.MagicEquipStrength.Select(m => m.Value.Data).Sum();
            paramDict.Add("strong", strongTotal + "");

            long spirit = user.SpiritRecord.Select(m => m.Value.Level.Data).Sum();
            paramDict.Add("spirit", spirit + "");

            long talent = user.TalentExp.Data / 10000;
            paramDict.Add("talent", talent + "");
            //user.SaveRecordMax((int)AbcType.Talent, talent);

            long minVersion = user.VersionLog.Select(m => m.Key).Min();
            paramDict.Add("minVersion", minVersion + "");

            paramDict.Add("versionCount", user.VersionLog.Count + "");

            paramDict.Add("channel", ConfigHelper.Channel + "");

            if (user.First_Create_Time > 0)
            {
                string createTime = TimeHelper.SecondsToDate(user.First_Create_Time).ToString("yyyy-MM-dd");
                paramDict.Add("accountTime", createTime + "");
            }

            string param = JsonConvert.SerializeObject(paramDict);

            return param;
        }

        private static long GetTotal(List<BoxItem> Bags, int ConfigId)
        {
            long count = Bags.Where(m => m.Item.ConfigId == ConfigId).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }
        private static long GetTotal(List<BoxItem> Bags, int StartId, int EndId)
        {
            long count = Bags.Where(m => m.Item.ConfigId >= StartId && m.Item.ConfigId <= EndId).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }

        public static IEnumerator CreateAccount(string account, string pwd, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("account", account);
            dict.Add("pwd", pwd);
            dict.Add("srvId", ConfigHelper.SrvId + "");

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("create_user", bytes, successAction, failAction);
        }

        public static IEnumerator UpdateInfo(string data, Action<WebResultWrapper> successAction, Action failAction)
        {
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(data);

            return SendRequest("update_info", bytes, successAction, failAction);
        }

        public static IEnumerator UploadData(byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("save_user_file", bytes, headers, successAction, failAction);
        }

        public static IEnumerator CreateAccountNew(byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("create_user_new", bytes, headers, successAction, failAction);
        }

        public static IEnumerator GetDownParam(Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("get_user_file", Encoding.UTF8.GetBytes(""), successAction, failAction);
        }

        public static IEnumerator SaveRank(string accountId, string type, string rank, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("type", type);
            dict.Add("rank", rank);
            dict.Add("accountId", accountId);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("save_rank", bytes, successAction, failAction);
        }

        public static IEnumerator GetRank(string type, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("type", type);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("get_rank", bytes, successAction, failAction);
        }

        public static IEnumerator GetPet(int configId, int count, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("configId", configId + "");
            dict.Add("count", count + "");

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("get_pet", bytes, successAction, failAction);
        }

        public static IEnumerator GetSerial(Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("get_serial", null, successAction, failAction);
        }

        public static IEnumerator DownData(Action<byte[]> successAction, Action failAction)
        {
            string url = home + "down_user_file";

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var db = new DownloadHandlerBuffer())
                {
                    string account = User_Data_Manager.Data.Account;
                    string fileId = User_Data_Manager.Data.DeviceId;
                    string deviceId = AppHelper.GetDeviceIdentifier();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("version", ConfigHelper.Version + "");
                    request.SetRequestHeader("sign", BuildSign());
                    request.SetRequestHeader("code", BuildCode());

                    request.downloadHandler.Dispose();
                    request.downloadHandler = db;
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        //Debug.Log("Down Error:" + request.error);
                        failAction?.Invoke();
                    }
                    else
                    {
                        byte[] data = ((DownloadHandlerBuffer)request.downloadHandler).data;
                        successAction?.Invoke(data);
                    }
                }
            }
        }

        public static IEnumerator SendRequest(string action, byte[] bytes, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest(action, bytes, null, successAction, failAction);
        }
        public static IEnumerator SendRequest(string action, byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            string url = home + action;

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var uh = new UploadHandlerRaw(bytes))
                {
                    User user = User_Data_Manager.Data;
                    string account = user.Account;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string fileId = user.DeviceId;
                    string level = user.MagicLevel.Data + "";
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("level", level);
                    request.SetRequestHeader("channel", ConfigHelper.Channel + "");
                    request.SetRequestHeader("version", ConfigHelper.Version + "");
                    request.SetRequestHeader("sign", BuildSign());
                    request.SetRequestHeader("code", BuildCode());
                    if (action == "save_user_file")
                    {
                        //Debug.Log("Start Serial:" + user.Serial);

                        int serial = user.Serial + 1;
                        request.SetRequestHeader("SaveCount", serial + "");
                    }

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.SetRequestHeader(header.Key, header.Value);
                        }
                    }

                    request.uploadHandler.Dispose();
                    request.uploadHandler = uh;
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        //Debug.Log("Upload Error:" + request.error);
                        failAction?.Invoke();
                    }
                    else
                    {
                        //Debug.Log("Upload complete! Server response: " + request.downloadHandler.text);

                        WebResultWrapper result = JsonConvert.DeserializeObject<WebResultWrapper>(request.downloadHandler.text);

                        if (result.Code == StatusMessage.BlackList)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                        }
                        else if (result.Code == StatusMessage.OldFile)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new NewVersionEvent() { Type = 1 });
                        }
                        else if (result.Version > ConfigHelper.Version)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new NewVersionEvent() { Type = 2, Version = result.Version });
                        }

                        if (action == "save_user_file")
                        {
                            user.Serial++;
                            User_Data_Manager.Save();

                            //Debug.Log("End Serial:" + user.Serial);
                        }

                        successAction?.Invoke(result);
                    }
                }
            }
        }

        public static IEnumerator Loading(Action<WebResultWrapper> successAction, Action failAction)
        {
            User user = User_Data_Manager.Data;
            string accountId = user.AccountId;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("accountId", accountId);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("loading", bytes, successAction, failAction);
        }

        public static IEnumerator Convert‌Store(int storeId, Action<WebResultWrapper> successAction, Action failAction)
        {
            User user = User_Data_Manager.Data;
            string accountId = user.AccountId;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("accountId", accountId);
            dict.Add("storeId", storeId + "");

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("convert_store", bytes, successAction, failAction);
        }

        public static IEnumerator ToLottery(int number, Action<WebResultWrapper> successAction, Action failAction)
        {
            User user = User_Data_Manager.Data;
            string accountId = user.AccountId;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("accountId", accountId);
            dict.Add("number", number + "");

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("to_lottery", bytes, successAction, failAction);
        }
    }
}
