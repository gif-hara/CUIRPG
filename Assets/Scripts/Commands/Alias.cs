using System.Collections;
using System.Linq;
using HK.CUIRPG.Database;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// エイリアスを実行する<see cref="ICommand"/>
    /// </summary>
    public sealed class Alias : ICommand
    {
        public string Name => "registered_alias";

        public readonly string AliasName;

        public readonly string CommandData;

        public Alias(string aliasName, string commandData)
        {
            this.AliasName = aliasName;
            this.CommandData = commandData;
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.AliasName, this.CommandData));
        }

        public System.IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            var commandData = $"{CommandData} {string.Join(" ", data.Options)}";
            return OperatingSystem.Instance.CommandManager.InvokeAsObservable(commandData, interactor);
        }
    }
}
