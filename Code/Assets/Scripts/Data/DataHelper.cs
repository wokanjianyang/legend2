using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;
using Game.Data;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;

namespace Game
{
    public class DataHelper
    {
        static string savePath = "user";
        static string fileName = "data.json"; //文件名

        static string backPath = "back";
        static string[] BackFileName = { "back1.json", "back2.json", "back3.json" };

        public static long StartTime = 0;

        public static PlayerData Load()
        {
            PlayerData data = null;

            string filePath = GetSavePath();

            try
            {
                if (File.Exists(filePath))
                {
                    string key = AppHelper.GetDeviceIdentifier();

                    //读取文件
                    StreamReader sr = new StreamReader(filePath);
                    string str_json = sr.ReadToEnd();
                    sr.Close();

                    if (str_json.Length > 0)
                    {
                        str_json = EncryptionHelper.AesDecrypt(str_json, key);

                        data = JsonConvert.DeserializeObject<PlayerData>(str_json, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        });
                        //Debug.Log("成功读取");
                    }

                    if (data == null)
                    {
                        for (int i = 0; i < BackFileName.Length; i++)
                        {
                            data = LoadBack(i);

                            if (data != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            if (data == null)
            {
                data = new PlayerData();
            }

            return data;
        }

        public static PlayerData LoadBack(int index)
        {
            Debug.Log("读取备份文件:" + index);

            string filePath = GetBackPath(index);
            string key = AppHelper.GetDeviceIdentifier();

            //读取文件
            StreamReader sr = new StreamReader(filePath);
            string str_json = sr.ReadToEnd();
            sr.Close();

            if (str_json.Length <= 0)
            {
                return null;
            }
            //Debug.Log("备份str_json:" + index + " " + str_json);

            str_json = EncryptionHelper.AesDecrypt(str_json, key);

            //Debug.Log("备份json:" + index + " " + str_json);

            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(str_json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            return data;
        }

        public static void Save()
        {
            if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
            {
                return;
            }

            PlayerData data = new PlayerData();
            //data.Init(GameProcessor.Inst.User);

            //序列化
            string str_json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            string key = AppHelper.GetDeviceIdentifier();
            //加密
            str_json = EncryptionHelper.AesEncrypt(str_json, key);

            string filePath = GetSavePath();             //文件路径

            try
            {
                File.WriteAllText(filePath, str_json);
                Debug.Log("saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.Log("saved Error." + ex.Message);
            }
        }

        public static void SaveBack(int index)
        {
            if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
            {
                return;
            }

            //序列化
            PlayerData data = new PlayerData();
            //data.Init(GameProcessor.Inst.User);

            string str_json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            string key = AppHelper.GetDeviceIdentifier();
            //加密
            str_json = EncryptionHelper.AesEncrypt(str_json, key);

            string filePath = GetBackPath(index);             //文件路径

            try
            {
                File.WriteAllText(filePath, str_json);
                Debug.Log("save back successfully.");
            }
            catch (Exception ex)
            {
                Debug.Log("saved back Error." + ex.Message);
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

        public static string GetBackPath(int index)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, backPath); //文件夹路径

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string backFileName = BackFileName[index];
            string filePath = Path.Combine(folderPath, backFileName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                File.Create(filePath).Dispose();
            }

            return filePath;
        }
    }
}