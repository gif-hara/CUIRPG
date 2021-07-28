using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UniRx;
using HK.MineTerminal.Events;

namespace HK.MineTerminal
{
    /// <summary>
    /// ターミナルを制御するクラス
    /// </summary>
    [RequireComponent(typeof(Window))]
    public sealed class TerminalController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform root = default;

        [SerializeField]
        private TMP_InputField history = default;

        [SerializeField]
        private TMP_InputField inputField = default;

        private RectTransform historyTransform;

        private RectTransform inputFieldTransform;

        private readonly StringBuilder historyBuilder = new StringBuilder();

        private Session session;

        private int currentUpdateCount = -1;

        private Window owner;

        void Start()
        {
            this.historyTransform = (RectTransform)this.history.transform;
            this.inputFieldTransform = (RectTransform)this.inputField.transform;
            this.CalculateSize();
            this.inputField.onSubmit.AddListener(s =>
            {
                this.session.Receive(s);
                this.inputField.text = "";

                Observable.NextFrame().SubscribeWithState(this, (_, _this) =>
                {
                    _this.inputField.ActivateInputField();
                    _this.inputField.text = "";
                });
            });

            this.owner = this.GetComponent<Window>();
            Assert.IsNotNull(this.owner);

            this.owner.Broker.Receive<WindowEvents.Resize>()
                .SubscribeWithState(this, (_, _this) => _this.CalculateSize())
                .AddTo(this);

            this.owner.Broker.Receive<WindowEvents.Removed>()
                .SubscribeWithState(this, (_, _this) =>
                {
                    OperatingSystem.Instance.RemoveTask(_this.session);
                })
                .AddTo(this);

            this.owner.ObserveRemoveOnImmediate();

            OperatingSystem.CurrentWindow
                .Where(x => x == this.owner)
                .SubscribeWithState(this, (x, _this) =>
                {
                    _this.inputField.ActivateInputField();
                })
                .AddTo(this);
        }

        void Update()
        {
            var isFocus = OperatingSystem.CurrentWindow.Value == this.owner;
            if (isFocus && !this.session.IsInteractable.Value && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                this.session.Broker.Publish(IInteractorEvents.Aborted.Get());
            }

            if (this.session.HistoryUpdateCount != this.currentUpdateCount)
            {
                this.historyBuilder.Clear();
                var histories = this.session.Histories.ToArray();
                for (var i = 0; i < histories.Length; i++)
                {
                    var command = histories[i];
                    if ((i + 1) < this.session.Histories.Count)
                    {
                        this.historyBuilder.AppendLine(command);
                    }
                    else
                    {
                        this.historyBuilder.Append(command);
                    }
                }
                this.history.text = this.historyBuilder.ToString();

                // 何かしら入力していないとサイズ計算がおかしくなるので適当な文字列を入れておく
                var tempCommand = this.inputField.text;
                this.inputField.text = "calculateSize";

                this.CalculateSize();
                this.history.textComponent.ForceMeshUpdate();

                // 一旦履歴をアクティブにして最後尾の行を表示する
                if (isFocus)
                {
                    this.history.ActivateInputField();
                    this.history.MoveToEndOfLine(false, true);
                }

                // サイズ計算が終わったので入力中の文字を代入する
                this.inputField.text = tempCommand;

                this.currentUpdateCount = this.session.HistoryUpdateCount;

                if (isFocus)
                {
                    Observable.NextFrame().SubscribeWithState(this, (_, _this) => _this.inputField.ActivateInputField());
                }
            }
        }

        public void Initialize(Session session)
        {
            this.session = session;
        }

        private void CalculateSize()
        {
            var rootRect = this.root.rect;
            var sizeDelta = this.historyTransform.sizeDelta;
            if (this.history.text.Length <= 0)
            {
                sizeDelta.y = 0.0f;
            }
            else
            {
                var fontHeight = this.history.textComponent.preferredHeight / GetNewLineCount(this.history.text);
                var preferredHeight = this.history.textComponent.preferredHeight;
                var maxHeight = rootRect.height - fontHeight;

                sizeDelta.y = Mathf.Min(preferredHeight, maxHeight);
            }
            this.historyTransform.sizeDelta = sizeDelta;
            var inputFieldRect = this.inputFieldTransform.rect;
            inputFieldRect.height = rootRect.height - sizeDelta.y;
            this.inputFieldTransform.sizeDelta = new Vector2(sizeDelta.x, inputFieldRect.height);
        }

        private static int GetNewLineCount(string value)
        {
            var result = 0;
            foreach (var s in value)
            {
                if (s == '\n')
                {
                    result++;
                }
            }

            return result + 1;
        }
    }
}
