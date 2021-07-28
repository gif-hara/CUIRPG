using HK.CUIRPG.Events;
using UnityEngine;
using UniRx;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="string"/>に関する拡張機能
    /// </summary>
    public static class StringExtensions
    {
        public static string Format(this string self, params object[] args)
        {
            return string.Format(self, args);
        }
    }
}
