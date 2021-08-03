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
            return Observable.Create<Unit>(observer =>
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

                    observer.OnNext(Unit.Default);
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                if (data.ContainsOption("-d"))
                {
                    var targetName = data.GetString("-d");
                    if (string.IsNullOrEmpty(targetName))
                    {
                        SendHelp(interactor);
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();

                        return Disposable.Empty;
                    }

                    if (!commandManager.Commands.ContainsKey(targetName))
                    {
                        interactor.Send($"\"{targetName}\" did not exists.");
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();

                        return Disposable.Empty;
                    }

                    return interactor.Confirm($"Are you sure you want to remove the {targetName}? y/n")
                    .Subscribe(x =>
                    {
                        if (x == "y")
                        {
                            commandManager.Remove(targetName);
                        }

                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    });
                }

                if (data.Options.Count < 2)
                {
                    SendHelp(interactor);
                    observer.OnNext(Unit.Default);
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                commandManager.RegisterAlias(data.Options[0], data.Options[1], interactor);

                observer.OnNext(Unit.Default);
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }
    }
}
