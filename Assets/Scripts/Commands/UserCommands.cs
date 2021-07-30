using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// ユーザーに関する<see cref="ICommand"/>
    /// </summary>
    public sealed class UserCommands : ICommand
    {
        public string Name => "user";

        public void SendHelp(IInteractor interactor)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            throw new NotImplementedException();
        }
    }
}
