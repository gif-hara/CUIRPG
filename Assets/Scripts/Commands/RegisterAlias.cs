using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.MineTerminal
{
    /// <summary>
    /// エイリアスを登録する<see cref="ICommand"/>
    /// </summary>
    public sealed class RegisterAlias : ICommand
    {
        public string Name => "alias";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count < 2)
            {
                SendHelp(interactor);
                yield break;
            }

            var commandManager = OperatingSystem.Instance.CommandManager;
            commandManager.RegisterAlias(data.Options[0], data.Options[1], interactor);
            
            yield break;
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
