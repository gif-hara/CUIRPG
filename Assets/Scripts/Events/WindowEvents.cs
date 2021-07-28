using HK.Framework.EventSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.MineTerminal.Events
{
    /// <summary>
    /// <see cref="Window"/>に関するイベント
    /// </summary>
    public sealed class WindowEvents
    {
        /// <summary>
        /// ウィンドウサイズが変更された際のイベント
        /// </summary>
        public sealed class Resize : Message<Resize, Vector2, Vector2>
        {
            public Vector2 NewPosition => this.param1;

            public Vector2 NewSize => this.param2;
        }

        /// <summary>
        /// ウィンドウを閉じるリクエストイベント
        /// </summary>
        public sealed class RequestRemove : Message<RequestRemove>
        {
        }

        /// <summary>
        /// ウィンドウを閉じた際のイベント
        /// </summary>
        public sealed class Removed : Message<Removed>
        {
        }
    }
}
