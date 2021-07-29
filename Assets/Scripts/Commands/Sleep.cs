using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// 指定された時間だけスリープする<see cref="ICommand"/>
    /// </summary>
    public sealed class Sleep : ICommand
    {
        public string Name => "sleep";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (data.Options.Count <= 0)
                {
                    this.SendHelp(interactor);
                    return Observable.ReturnUnit();
                }

                var seconds = default(float);
                var isSilent = data.ContainsOption("-s");
                if (float.TryParse(data.Options[0], out seconds))
                {
                    if (!isSilent)
                    {
                        interactor.Send("Good night...");
                    }

                    return Observable.Timer(TimeSpan.FromSeconds(seconds))
                    .Do(_ =>
                    {
                        if (!isSilent)
                        {
                            interactor.Send("Good morning!");
                        }
                    })
                    .AsUnitObservable();
                }
                else
                {
                    this.SendHelp(interactor);
                    return Observable.ReturnUnit();
                }
            });
        }
    }
}
