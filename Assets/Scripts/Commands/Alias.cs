using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
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

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.aliasName, this.commandData));
        }

        public System.IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return OperatingSystem.Instance.CommandManager.InvokeAsObservable(this.commandData, interactor);
        }
    }
}
