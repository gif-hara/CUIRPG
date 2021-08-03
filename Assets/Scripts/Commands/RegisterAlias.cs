using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Commands
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
                var commandManager = OperatingSystem.Instance.CommandManager;

                if (data.ContainsOption("-l"))
                {
                    var aliasCommands = commandManager.Commands
                    .Where(x => x.Value is Alias);
                    foreach (var i in aliasCommands)
                    {
                        var alias = (Alias)i.Value;
                        interactor.Send($"{alias.AliasName} {alias.CommandData}");
                    }

                    return Observable.ReturnUnit();
                }

                if (data.Options.Count < 2)
                {
                    SendHelp(interactor);
                    return Observable.ReturnUnit();
                }

                commandManager.RegisterAlias(data.Options[0], data.Options[1], interactor);

                return Observable.ReturnUnit();
            });
        }
    }
}
