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
    public class UserData
    {
        static string savePath = "player";

        static string fileName = "wj2.xml"; //文件名
        static string ppKey = "pk";
        static string mdKey = "mk";

        public static User Load()
        {
            User user = null;

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

                            user = JsonConvert.DeserializeObject<User>(str_json, new JsonSerializerSettings
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
                if (user == null)
                {
                    user = new User();
                    //首次初始化
                    user.MagicLevel.Data = 1;
                    user.MagicExp.Data = 0;
                    user.Name = "传奇";
                    user.MapId = ConfigHelper.MapStartId;
                    user.MagicGold.Data = 0;
                    user.First_Create_Time = TimeHelper.ClientNowSeconds();
                }

                if (user.EquipPanelList.Count < 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (!user.EquipPanelList.ContainsKey(i))
                        {
                            user.EquipPanelList[i] = new Dictionary<int, Equip>();
                        }
                    }
                }

                if (user.EquipPanelGoldenList.Count < 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (!user.EquipPanelGoldenList.ContainsKey(i))
                        {
                            user.EquipPanelGoldenList[i] = new Dictionary<int, Equip>();
                        }
                    }
                }

                if (user.DefendData == null)
                {
                    user.DefendData = new DefendData();
                }
                if (!user.DefendData.CountDict.ContainsKey(1))
                {
                    MagicData data = new MagicData();
                    data.Data = 1;
                    user.DefendData.CountDict[1] = data;
                }
                if (!user.DefendData.CountDict.ContainsKey(2))
                {
                    MagicData data = new MagicData();
                    data.Data = 1;
                    user.DefendData.CountDict[2] = data;
                }

                if (user.HeroPhatomData == null)
                {
                    user.HeroPhatomData = new HeroPhatomData();
                    user.HeroPhatomData.Count.Data = 1;
                }
                if (user.InfiniteData == null)
                {
                    user.InfiniteData = new InfiniteData();
                }

                if (user.GetLimitId() <= 1030)
                {
                    CycleConfigCategory.Instance.Init();
                }

                if (user.LegacyData == null)
                {
                    user.LegacyData = new LegacyData();
                    user.LegacyData.GetDropId(1);
                    user.LegacyData.GetDropId(2);
                    user.LegacyData.GetDropId(3);
                    user.LegacyData.GetDropLayer(1, 1);
                    user.LegacyData.GetDropLayer(2, 1);
                    user.LegacyData.GetDropLayer(3, 1);
                }

                if (user.DeviceId == "")
                {
                    user.DeviceId = AppHelper.GetDeviceIdentifier();
                }

                //记录版号
                user.VersionLog[ConfigHelper.Version] = TimeHelper.ClientNowSeconds();
            }
            catch (Exception e)
            {
                Debug.LogError("Load Error:" + e.Message);
            }

            return user;
        }

        public static void Save()
        {
            if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
            {
                return;
            }
            var user = GameProcessor.Inst.User;
            //user.LastOut = TimeHelper.ClientNowSeconds();

            //序列化
            string str_json = JsonConvert.SerializeObject(user, new JsonSerializerSettings
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