using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using SA.Android.Utilities;
using System.Linq;
using Game.Data;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;

namespace Game
{
    public class User_Data_Manager
    {
        public static User Data;

        static string savePath = "player";

        static string fileName = "wj2.xml"; //文件名
        static string ppKey = "pk";
        static string mdKey = "mk";

        public static void Load()
        {

            string filePath = GetSavePath();
            Debug.Log($"存档路径：{filePath}");

            try
            {
                if (File.Exists(filePath))
                {
                    //PlayerPrefs.DeleteAll();
                    string key = PlayerPrefs.GetString(ppKey);
                    string mkey = PlayerPrefs.GetString(mdKey);

                    if (mkey == AppHelper.GetDeviceIdentifier())
                    {
                        //读取文件
                        StreamReader sr = new StreamReader(filePath);
                        string str_json = sr.ReadToEnd();
                        sr.Close();

                        if (str_json.Length > 0)
                        {
                            str_json = EncryptionHelper.AesDecrypt(str_json, key);

                            Data = JsonConvert.DeserializeObject<User>(str_json, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            });
                            //Debug.Log("成功读取");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception innerEx = ex.InnerException;
                Debug.LogError($"真实错误类型: {innerEx.GetType().Name}");
                Debug.LogError($"真实错误消息: {innerEx.Message}");
                Debug.LogError($"堆栈跟踪: {innerEx.StackTrace}");

                // 如果内部异常还有内部异常，可能需要递归查看
                if (innerEx.InnerException != null)
                {
                    Debug.LogError($"深层原因: {innerEx.InnerException.Message}");
                }

                Debug.LogError(ex.Message);
            }

            try
            {
                if (Data == null)
                {
                    Data = new User();
                    //首次初始化
                    Data.MagicLevel.Data = 1;
                    Data.MagicExp.Data = 0;
                    Data.Name = "传奇";
                    Data.MapId = ConfigHelper.MapStartId;
                    Data.MagicGold.Data = 0;
                    Data.First_Create_Time = TimeHelper.ClientNowSeconds();
                }

                if (Data.EquipPanelList.Count < 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (!Data.EquipPanelList.ContainsKey(i))
                        {
                            Data.EquipPanelList[i] = new Dictionary<int, Equip>();
                        }
                    }
                }

                if (Data.EquipPanelGoldenList.Count < 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (!Data.EquipPanelGoldenList.ContainsKey(i))
                        {
                            Data.EquipPanelGoldenList[i] = new Dictionary<int, Equip>();
                        }
                    }
                }

                if (Data.DeviceId == "")
                {
                    Data.DeviceId = AppHelper.GetDeviceIdentifier();
                }

                //记录版号
                Data.VersionLog[ConfigHelper.Version] = TimeHelper.ClientNowSeconds();
            }
            catch (Exception e)
            {
                Debug.LogError("Load Error:" + e.Message);
            }

            //return user;
        }

        public static void Save()
        {
            //if (GameProcessor.Inst == null || User_Data_Manager.Data == null)
            //{
            //    return;
            //}
            //var user = User_Data_Manager.Data;
            //user.LastOut = TimeHelper.ClientNowSeconds();

            //序列化
            string str_json = JsonConvert.SerializeObject(Data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            string key = Guid.NewGuid().ToString().Substring(0, 16);
            //Debug.Log("save key" + key);


            //加密
            str_json = EncryptionHelper.AesEncrypt(str_json, key);

            string filePath = GetSavePath();             //文件路径

            try
            {
                File.WriteAllText(filePath, str_json);
                PlayerPrefs.SetString(ppKey, key);
                PlayerPrefs.SetString(mdKey, AppHelper.GetDeviceIdentifier());
                PlayerPrefs.Save();

                //Debug.Log("saved successfully.");
            }
            catch (Exception ex)
            {

                Debug.Log("saved Error." + ex.Message);
            }
        }


        public static string GetSavePath()
        {
            string folderPath = Path.Combine(Application.persistentDataPath, savePath); //文件夹路径

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                File.Create(filePath).Dispose();
            }

            return filePath;
        }
    }
}