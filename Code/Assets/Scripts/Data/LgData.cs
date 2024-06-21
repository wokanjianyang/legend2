using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class LgData
    {
        private double o1;
        private double o2;
        private double data;

        private string key;
        private string text;

        public LgData()
        {
            o1 = RandomHelper.RandomNumber(2, 10);
            o2 = RandomHelper.RandomNumber(100000, 200000);
            data = 0;
            key = Guid.NewGuid().ToString();
            text = "";
        }

        public double Data
        {
            get
            {
                if (data == 0)
                {
                    return data;
                }
                else
                {
                    if (!Check())
                    {
                        return 0;
                    }

                    return (data - o2) / o1;
                }
            }
            set
            {
                data = value * o1 + o2;
                this.text = EncryptionHelper.Md5(data + key);
            }
        }

        public bool Check()
        {
            if (text == null || text == "")
            {
                return true;
            }

            string md5 = EncryptionHelper.Md5(data + key);

            if (this.text != md5)
            {
                //数据校检失败
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return false;
            }

            return true;
        }
    }
}
