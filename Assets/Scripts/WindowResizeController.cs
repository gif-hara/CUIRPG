using HK.CUIRPG.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HK.CUIRPG
{
    /// <summary>
    /// ウィンドウのサイズを変更するクラス
    /// </summary>
    public sealed class WindowResizeController : MonoBehaviour, IDragHandler
    {
        public enum Horizontal
        {
            None,
            Left,
            Right,
        }

        public enum Vertical
        {
            None,
            Top,
            Bottom,
        }

        [SerializeField]
        private Window window = default;

        [SerializeField]
        private Horizontal horizontal = default;

        [SerializeField]
        private Vertical vertical = default;

        public void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;
            if(this.horizontal != Horizontal.None)
            {
                var velocity = delta.x;
                var size = this.window.RectTransform.sizeDelta;
                size.x += this.horizontal == Horizontal.Left ? -velocity : velocity;
                this.window.RectTransform.sizeDelta = size;
                if(this.horizontal == Horizontal.Left)
                {
                    var pos = this.window.RectTransform.anchoredPosition;
                    pos.x += velocity;
                    this.window.RectTransform.anchoredPosition = pos;
                }
            }
            if(this.vertical != Vertical.None)
            {
                var velocity = delta.y;
                var size = this.window.RectTransform.sizeDelta;
                size.y += this.vertical == Vertical.Top ? velocity : -velocity;
                this.window.RectTransform.sizeDelta = size;
                if (this.vertical == Vertical.Top)
                {
                    var pos = this.window.RectTransform.anchoredPosition;
                    pos.y += velocity;
                    this.window.RectTransform.anchoredPosition = pos;
                }
            }

            this.window.Broker.Publish(WindowEvents.Resize.Get(this.window.RectTransform.anchoredPosition, this.window.RectTransform.sizeDelta));
        }
    }
}
