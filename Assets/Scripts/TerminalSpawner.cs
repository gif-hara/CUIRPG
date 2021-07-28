using HK.Framework.EventSystems;
using HK.MineTerminal.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace HK.MineTerminal
{
    /// <summary>
    /// <see cref="TerminalController"/>を生成するクラス
    /// </summary>
    public sealed class TerminalSpawner : MonoBehaviour
    {
        [SerializeField]
        private TerminalController prefab = default;

        [SerializeField]
        private RectTransform parent = default;

        void Awake()
        {
            Broker.Global.Receive<TaskEvents.Enter>()
                .Where(x => x.Task is Session)
                .SubscribeWithState(this, (x, _this) =>
                {
                    var session = (Session)x.Task;
                    var terminal = Instantiate(_this.prefab, _this.parent, false);
                    terminal.Initialize(session);
                })
                .AddTo(this);
        }
    }
}
