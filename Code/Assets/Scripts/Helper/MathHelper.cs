using System;
using UnityEngine;

namespace Game
{
    public static class MathHelper
    {
        public static float RadToDeg(float radians)
        {
            return (float)(radians * 180 / System.Math.PI);
        }

        public static float DegToRad(float degrees)
        {
            return (float)(degrees * System.Math.PI / 180);
        }

        public static long GetSequence2(long level)
        {
            return level * (level + 1) / 2;
        }

        public static long GetSeqByType(int type, long level, long bc)
        {

            if (type == 0)  //0，不增加
            {
                return bc;
            }
            else if (type == 1)  //1，固定增加，等级*基础
            {
                return level * bc;
            }
            else if (type == 2)//2，线性增加，1+2+3+4
            {
                return GetSequence2(level) * bc;
            }
            else if (type == 3)  //3，指数增加，pow(2,10)
            {
                return (long)Math.Pow(bc, level);
            }

            return 0;
        }

        public static long GetRiseByType(int type, long level, long bc)
        {
            if (type == 0)  //0，不增加
            {
                return 0;
            }
            else if (type == 1)  //1，固定增加，等级*基础
            {
                return bc;
            }
            else if (type == 2)//2，线性增加，1+2+3+4
            {
                return level * bc;
            }
            else if (type == 3)  //3，指数增加，pow(2,10)
            {
                return (long)Math.Pow(bc, level - 1);
            }

            return 0;
        }

        public static double ConvertionDropRate(long rate, int rise)
        {
            double r = 0;

            for (int i = 1; i < 1000; i++)
            {
                int pr = (i - 1) * rise + 100;

                if (rate >= pr)
                {
                    r += 1;
                }
                else
                {
                    r += rate * 1.0 / pr;
                    break;
                }
                rate -= pr;
            }
            return r;
        }

        public static int CalOfflineDropCount(double killRecord, double killCount, double rate)
        {
            int oldCount = (int)(killRecord / rate);

            int newCount = (int)((killRecord + killCount) / rate);

            return newCount - oldCount;
        }

        public static int CalRefineStone(int equipLevel, int riseStone)
        {
            int count = (equipLevel * 3 / 20 + riseStone);
            //Debug.Log("RefineStone:" + count);
            return count;
        }

        public static int RandomBurstMul(double rs)
        {
            if (rs <= 0)
            {
                return 0;
            }

            //if (rs >= 300)
            //{
            //    return 3;
            //}

            int count = (int)(rs / 100);
            int rate = (int)(rs - count * 100);

            if (RandomHelper.RandomCritRate(rate))
            {
                count++;
            }

            return count;
        }

        public static double CalRealResist(double val)
        {
            double r = 0;

            while (val > 0)
            {
                double temp = Math.Min(val, 70);
                val = val - temp;

                r += (100 - r) * temp / 100;
            }

            return r;
        }

        public static int RandomArrayIndex(int[] array, double rise)
        {
            int t = RandomHelper.RandomNumber(array[0], array[array.Length - 1] + 1);
            t = (int)Math.Round(t / rise);

            int index = Array.BinarySearch(array, t);

            if (index < 0)
            {
                index = ~index;
            }

            return array.Length - index;
        }


        public static int GetMiddleNumber(int a, int b, int c)
        {
            int max = Math.Max(a, Math.Max(b, c));
            int min = Math.Min(a, Math.Min(b, c));
            return a + b + c - max - min;
        }
    }
}