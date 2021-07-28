using HK.Framework.EventSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Events
{
    /// <summary>
    /// <see cref="IInteractor"/>に関するイベント
    /// </summary>
    public sealed class IInteractorEvents
    {
        /// <summary>
        /// ユーザーからメッセージを受け取った際のイベント
        /// </summary>
        public sealed class Received : Message<Received, CommandData>
        {
            public CommandData CommandData => this.param1;
        }

        /// <summary>
        /// ユーザーから強制中止を受け取った際のイベント
        /// </summary>
        public sealed class Aborted : Message<Aborted>
        {

        }
    }
}
