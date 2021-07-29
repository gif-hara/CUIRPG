using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Database
{
    /// <summary>
    /// アイテムのデータベース
    /// </summary>
    [Serializable]
    public sealed class TitleItem
    {
        public int id;

        public string name;
    }
}