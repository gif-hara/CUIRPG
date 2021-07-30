using System;
using HK.CUIRPG.Database;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// インベントリに関する<see cref="ICommand"/>
    /// </summary>
    public sealed class InventoryCommands : ICommand
    {
        public string Name => "inventory";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                var userData = UserData.Instance;
                for (var i = 0; i < userData.UserItems.Count; i++)
                {
                    interactor.Send($"[{i}] {userData.UserItems[i]}");
                }

                return Observable.ReturnUnit();
            });
        }
    }
}
