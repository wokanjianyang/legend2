using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class RecordData
    {
        public MagicData Data { get; } = new MagicData();

        public string Text { get; set; }

        public void AddRecord()
        {
            Data.Data++;

            Encryption();
        }

        public bool Check()
        {
            if (Data.Data <= 0)
            {
                return true;
            }

            //序列化
            string str_json = Data.Data + "";

            string md5 = EncryptionHelper.Md5(str_json);

            if (Text != md5)
            {
                //数据校检失败
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经修改了存档", ToastType = ToastTypeEnum.Failure });
                //GameProcessor.Inst.isCheckError = true;
                return false;
            }

            return true;
        }

        private void Encryption()
        {
            string str_json = Data.Data + "";

            string md5 = EncryptionHelper.Md5(str_json);

            this.Text = md5;
        }
    }
}
