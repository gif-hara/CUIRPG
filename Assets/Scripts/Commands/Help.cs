using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.MineTerminal
{
    /// <summary>
    /// 全てのコマンドのヘルプを閲覧できる<see cref="ICommand"/>
    /// </summary>
    public sealed class Help : ICommand
    {
        public string Name => "help";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count <= 0)
            {
                foreach (var c in OperatingSystem.Instance.CommandManager.Commands)
                {
                    c.Value.SendHelp(interactor);
                }
            }
            else
            {
                var targetCommandName = data.Options[0];
                var commands = OperatingSystem.Instance.CommandManager.Commands;
                if(commands.ContainsKey(targetCommandName))
                {
                    commands[targetCommandName].SendHelp(interactor);
                }
                else
                {
                    interactor.Send(OperatingSystem.Instance.LocalizedMessages.commandDoesNotExist.Get);
                }
            }

            yield break;
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
