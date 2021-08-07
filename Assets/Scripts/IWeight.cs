using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// 重みを持つインターフェイス
    /// </summary>
    public interface IWeight
    {
        /// <summary>
        /// 重み
        /// </summary>
        int Weight { get; }
    }
}
