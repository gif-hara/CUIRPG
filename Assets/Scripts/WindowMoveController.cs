using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace HK.CUIRPG
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WindowMoveController : MonoBehaviour, IDragHandler
    {
        [SerializeField]
        private Window window = default;

        public void OnDrag(PointerEventData eventData)
        {
            var pos = this.window.RectTransform.anchoredPosition;
            pos += eventData.delta;
            this.window.RectTransform.anchoredPosition = pos;
        }
    }
}
