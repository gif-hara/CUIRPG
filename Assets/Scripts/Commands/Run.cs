using System.Collections;
using HK.CUIRPG.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using System;
using UniRx.Diagnostics;

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

                var stream = Observable.Interval(TimeSpan.FromSeconds(sleepSeconds))
                    .SelectMany(_ => OperatingSystem.Instance.CommandManager.InvokeCoroutine(targetCommandData, interactor));

                if (loopCount > 0)
                {
                    stream = stream.Take(loopCount);
                }

                return stream;
            });
        }
    }
}
