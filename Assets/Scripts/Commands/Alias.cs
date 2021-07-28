using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.MineTerminal
{
    /// <summary>
    /// エイリアスを実行する<see cref="ICommand"/>
    /// </summary>
    public sealed class Alias : ICommand
    {
        public string Name => "registered_alias";

        private readonly string aliasName;

        private readonly string commandData;

        public Alias(string aliasName, string commandData)
        {
            this.aliasName = aliasName;
            this.commandData = commandData;
        }

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            yield return OperatingSystem.Instance.CommandManager.InvokeCoroutine(this.commandData, interactor);
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.aliasName, this.commandData));
        }
    }
}
