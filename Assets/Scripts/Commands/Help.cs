using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// 全てのコマンドのヘルプを閲覧できる<see cref="ICommand"/>
    /// </summary>
    public sealed class Help : ICommand
    {
        public string Name => "help";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public System.IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (data.Options.Count <= 0)
                {
                    foreach (var c in OperatingSystem.Instance.CommandManager.Commands)
                    {
                        c.Value.SendHelp(interactor);
                    }
                    return Observable.ReturnUnit();
                }
                else
                {
                    var targetCommandName = data.Options[0];
                    var commands = OperatingSystem.Instance.CommandManager.Commands;
                    if (commands.ContainsKey(targetCommandName))
                    {
                        commands[targetCommandName].SendHelp(interactor);
                    }
                    else
                    {
                        interactor.Send(OperatingSystem.Instance.LocalizedMessages.commandDoesNotExist.Format(targetCommandName));
                    }

                    return Observable.ReturnUnit();
                }
            });
        }
    }
}
