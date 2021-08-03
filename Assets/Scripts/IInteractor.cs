using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// 対話を行うインターフェイス
    /// </summary>
    public interface IInteractor
    {
        IMessageBroker Broker { get; }

        IReadOnlyReactiveProperty<bool> IsInteractable { get; }

        /// <summary>
        /// <see cref="IInteractor"/>がユーザーへメッセージを送信する
        /// </summary>
        void Send(string message);

        /// <summary>
        /// ユーザーが<see cref="IInteractor"/>へメッセージを送信する
        /// </summary>
        void Receive(string message);

        /// <summary>
        /// <see cref="IInteractor"/>がユーザーへ問いかけを行う
        /// </summary>
        IObservable<string> Confirm(string message);

        /// <summary>
        /// ユーザーとのやり取りを行えないようにする
        /// </summary>
        void Busy();

        /// <summary>
        /// ユーザーとのやり取りを行えるようにする
        /// </summary>
        void Accept();

        /// <summary>
        /// 処理を強制終了する
        /// </summary>
        void Abort();

        /// <summary>
        /// 履歴をクリアする
        /// </summary>
        void ClearHistory();
    }
}
