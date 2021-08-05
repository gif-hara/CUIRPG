using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Database
{
    /// <summary>
    /// アイテムに付与出来るエフェクトのデータベース
    /// </summary>
    [Serializable]
    public sealed class TitleAddableItemEffect
    {
        public int itemId;

        public string effectType;

        public int value;

        public int weight;
    }
}
