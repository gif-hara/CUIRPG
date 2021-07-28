using UniRx;
using UnityEngine;

namespace HK.MineTerminal
{
    /// <summary>
    /// ウィンドウを制御するクラス
    /// </summary>
    public sealed class Window : MonoBehaviour
    {
        [SerializeField]
        private RectTransform cachedTransform = default;

        public readonly IMessageBroker Broker = new MessageBroker();

        public RectTransform RectTransform => this.cachedTransform;
    }
}
