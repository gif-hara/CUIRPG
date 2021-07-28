using System.Collections;
using HK.CUIRPG.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System;

namespace HK.CUIRPG
{
    /// <summary>
    /// ループしてコマンドを実行する<see cref="ICommand"/>
    /// </summary>
    public sealed class Run : ICommand
    {
        public string Name => "run";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public System.IObservable<Unit> Invoke(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (data.Options.Count <= 0)
                {
                    this.SendHelp(interactor);
                    return Observable.ReturnUnit();
                }

                var targetCommandData = data.Options[0];
                int loopCount;
                if (!data.TryGetInt("-l", out loopCount))
                {
                    loopCount = -1;
                }

                float sleepSeconds;
                if (!data.TryGetFloat("-s", out sleepSeconds))
                {
                    sleepSeconds = 0.0f;
                }

                var stream = Observable.EveryUpdate()
                    .SelectMany(_ => OperatingSystem.Instance.CommandManager.InvokeCoroutine(targetCommandData, interactor));

                if (sleepSeconds > 0.0f)
                {
                    stream = stream
                    .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(sleepSeconds)).AsUnitObservable());
                }

                return stream;
            });
        }
    }
}
