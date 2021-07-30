using System;
using HK.CUIRPG.Database;
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
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (data.Options.Count <= 0)
                {
                    SendHelp(interactor);
                    return Observable.ReturnUnit();
                }

                var userData = UserData.Instance;
                if (data.ContainsOption("-i"))
                {
                    for (var i = 0; i < userData.UserItems.Count; i++)
                    {
                        interactor.Send($"[{i}] {userData.UserItems[i].ToString()}");
                    }
                }

                return Observable.ReturnUnit();
            });
        }
    }
}
