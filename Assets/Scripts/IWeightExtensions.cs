using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="IWeight"/>に関する拡張関数
    /// </summary>
    public static class IWeightExtensions
    {
        /// <summary>
        /// 抽選を行う
        /// </summary>
        public static T Lottery<T>(this IEnumerable<T> self) where T : IWeight
        {
            var max = 0;
            foreach (var i in self)
            {
                max += i.Weight;
            }

            var current = 0;
            var random = Random.Range(0, max);
            foreach (var i in self)
            {
                if (random >= current && random < current + i.Weight)
                {
                    return i;
                }

                current += i.Weight;
            }

            Assert.IsTrue(false, "未定義の動作です");
            return default(T);
        }
    }
}
