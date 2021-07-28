using HK.CUIRPG.Events;
using UnityEngine;
using UniRx;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="Window"/>に関する拡張機能
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// <see cref="WindowEvents.RequestRemove"/>のイベントを受け取ったら即座に<see cref="Window"/>を削除する
        /// </summary>
        public static void ObserveRemoveOnImmediate(this Window self)
        {
            self.Broker.Receive<WindowEvents.RequestRemove>()
                .SubscribeWithState(self, (_, _this) =>
                {
                    Object.Destroy(_this.gameObject);
                    _this.Broker.Publish(WindowEvents.Removed.Get());
                })
                .AddTo(self);
        }
    }
}
