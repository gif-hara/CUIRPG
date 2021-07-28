using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using System;
using UnityEngine;
using HK.MineTerminal.Events;
using System.Threading;

namespace HK.MineTerminal
{
    /// <summary>
    /// コマンドを管理するクラス
    /// </summary>
    public sealed class CommandManager
    {
        public readonly Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();

        public CommandManager()
        {
            this.Add(new Sleep());
            this.Add(new RegisterAlias());
            this.Add(new Help());
            this.Add(new Run());
            this.Add(new Culture());
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
        }

        public void Invoke(string data, IInteractor interactor)
        {
            var coroutine = OperatingSystem.Instance.StartCoroutine(this.InvokeCoroutine(data, interactor));
            interactor.Broker.Receive<IInteractorEvents.Aborted>()
                .TakeUntil(interactor.IsInteractable.Where(x => x))
                .Take(1)
                .Subscribe(_ =>
                {
                    OperatingSystem.Instance.StopCoroutine(coroutine);
                    interactor.Abort();
                });
        }

        public IEnumerator InvokeCoroutine(string data, IInteractor interactor)
        {
            if (string.IsNullOrEmpty(data))
            {
                yield break;
            }

            var commandData = new CommandData(data);

            if (!commandData.IsValid)
            {
                yield break;
            }

            if (!this.Commands.ContainsKey(commandData.Name))
            {
                interactor.Send($"\"{commandData.Name}\" did not exist.");
                yield break;
            }

            interactor.Busy();

            yield return this.Commands[commandData.Name].Invoke(commandData, interactor);

            interactor.Accept();
        }
    }
}
