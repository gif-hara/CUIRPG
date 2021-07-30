using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using System;
using UnityEngine;
using HK.CUIRPG.Events;
using System.Threading;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// コマンドを管理するクラス
    /// </summary>
    public sealed class CommandManager
    {
        public readonly Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();

        public IObservable<RegisteredAliasCommand> OnRegisteredAliasCommandAsObservable() => m_OnRegisteredAliasCommand;
        private Subject<RegisteredAliasCommand> m_OnRegisteredAliasCommand = new Subject<RegisteredAliasCommand>();

        public class RegisteredAliasCommand
        {
            public string aliasName;

            public string commandData;
        }

        public CommandManager()
        {
            this.Add(new Sleep());
            this.Add(new RegisterAlias());
            this.Add(new Help());
            this.Add(new Run());
            this.Add(new Login());
            this.Add(new TitleData());
            this.Add(new Search());
            this.Add(new InventoryCommands());
        }

        public void Add(ICommand command)
        {
            this.Add(command, command.Name);
        }

        public void Add(ICommand command, string name)
        {
            this.Commands.Add(name, command);
        }

        public void RegisterAlias(string aliasName, string targetCommandData, IInteractor interactor)
        {
            if (this.Commands.ContainsKey(aliasName))
            {
                interactor.Send($"\"{aliasName}\" already exists.");
                return;
            }

            var alias = new Alias(aliasName, targetCommandData);
            this.Add(alias, aliasName);
            interactor.Send($"success register alias!");
            m_OnRegisteredAliasCommand.OnNext(new RegisteredAliasCommand
            {
                aliasName = aliasName,
                commandData = targetCommandData
            });
        }

        public void RegisterAlias(string aliasName, string targetCommandData)
        {
            if (this.Commands.ContainsKey(aliasName))
            {
                return;
            }

            var alias = new Alias(aliasName, targetCommandData);
            this.Add(alias, aliasName);
            m_OnRegisteredAliasCommand.OnNext(new RegisteredAliasCommand
            {
                aliasName = aliasName,
                commandData = targetCommandData
            });
        }

        public void SetupAlias(List<UserAlias> userAliases)
        {
            foreach (var i in userAliases)
            {
                var alias = new Alias(i.aliasName, i.commandData);
                this.Add(alias, i.aliasName);
            }
        }

        public void Invoke(string data, IInteractor interactor)
        {
            var disposable = this.InvokeAsObservable(data, interactor)
                .Subscribe();
            interactor.Broker.Receive<IInteractorEvents.Aborted>()
                .TakeUntil(interactor.IsInteractable.Where(x => x))
                .Take(1)
                .Subscribe(_ =>
                {
                    disposable.Dispose();
                    interactor.Abort();
                });
        }

        public IObservable<Unit> InvokeAsObservable(string data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (string.IsNullOrEmpty(data))
                {
                    return Observable.ReturnUnit();
                }

                var commandData = new CommandData(data);

                if (!commandData.IsValid)
                {
                    return Observable.ReturnUnit();
                }

                if (!this.Commands.ContainsKey(commandData.Name))
                {
                    interactor.Send($"\"{commandData.Name}\" did not exist.");
                    return Observable.ReturnUnit();
                }

                interactor.Busy();

                return this.Commands[commandData.Name].InvokeAsObservable(commandData, interactor)
                .DoOnCompleted(() =>
                {
                    interactor.Accept();
                });
            });
        }
    }
}
