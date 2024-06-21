using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public static class PotentialHelper
    {
        //范围 0-10000 ,每2000为一个层级

        public static int GetRarity(int potential)
        {
            return potential / 2000;
        }
    }
}