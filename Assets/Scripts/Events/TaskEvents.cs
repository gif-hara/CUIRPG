using HK.Framework.EventSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.MineTerminal.Events
{
    /// <summary>
    /// <see cref="ITask"/>に関するイベント
    /// </summary>
    public sealed class TaskEvents
    {
        /// <summary>
        /// タスクが開始した際のイベント
        /// </summary>
        public sealed class Enter : Message<Enter, ITask>
        {
            public ITask Task => this.param1;
        }

        /// <summary>
        /// タスクが終了した際のイベント
        /// </summary>
        public sealed class Exit : Message<Exit, ITask>
        {
            public ITask Task => this.param1;
        }
    }
}
