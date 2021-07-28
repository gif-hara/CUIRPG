using System;
using System.Collections;
using UniRx;

namespace HK.CUIRPG
{
    /// <summary>
    /// コマンドのインターフェイス
    /// </summary>
    public interface ICommand
    {
        string Name { get; }

        IObservable<Unit> Invoke(CommandData data, IInteractor interactor);

        void SendHelp(IInteractor interactor);
    }
}
