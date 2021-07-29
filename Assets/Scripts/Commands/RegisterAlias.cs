using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// エイリアスを登録する<see cref="ICommand"/>
    /// </summary>
    public sealed class RegisterAlias : ICommand
    {
        public string Name => "alias";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (data.Options.Count < 2)
                {
                    SendHelp(interactor);
                    return Observable.ReturnUnit();
                }

                var commandManager = OperatingSystem.Instance.CommandManager;
                commandManager.RegisterAlias(data.Options[0], data.Options[1], interactor);

                return Observable.ReturnUnit();
            });
        }
    }
}
