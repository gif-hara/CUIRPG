using System;
using System.Collections;
using UniRx;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// コマンドのインターフェイス
    /// </summary>
    public interface ICommand
    {
        string Name { get; }

        IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor);

        void SendHelp(IInteractor interactor);
    }
}
