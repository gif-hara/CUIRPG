using System.Collections;
using System.Collections.Generic;
using HK.CUIRPG.Database;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="TitleData"/>に関する拡張関数
    /// </summary>
    public static class TitleDataExtensions
    {
        /// <summary>
        /// <paramref name="titleItemId"/>に紐づく<see cref="TitleAddableItemEffect"/>を返す
        /// </summary>
        public static Dictionary<string, List<TitleAddableItemEffect>> GetFromTitleItemId(this IEnumerable<TitleAddableItemEffect> self, int titleItemId)
        {
            var result = new Dictionary<string, List<TitleAddableItemEffect>>();

            foreach (var i in self)
            {
                if (i.itemId != titleItemId)
                {
                    continue;
                }

                if (!result.ContainsKey(i.effectType))
                {
                    result.Add(i.effectType, new List<TitleAddableItemEffect>());
                }

                result[i.effectType].Add(i);
            }

            return result;
        }
    }
}
