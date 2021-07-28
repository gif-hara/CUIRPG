using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UniRx;
using UnityEngine.UI;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="Window"/>のフォーカスを制御するクラス
    /// </summary>
    public sealed class WindowFocusController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private Window owner = default;

        [SerializeField]
        private Image raycastTarget = default;

        void Awake()
        {
            OperatingSystem.CurrentWindow
                .SubscribeWithState(this, (x, _this) =>
                {
                    _this.raycastTarget.raycastTarget = x != _this.owner;
                })
                .AddTo(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OperatingSystem.CurrentWindow.Value = this.owner;
        }
    }
}
