using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace HK.MineTerminal
{
    /// <summary>
    /// 指定された時間だけスリープする<see cref="ICommand"/>
    /// </summary>
    public sealed class Sleep : ICommand
    {
        public string Name => "sleep";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count <= 0)
            {
                this.SendHelp(interactor);
                yield break;
            }

            var seconds = default(float);
            var isSilent = data.ContainsOption("-s");
            if(float.TryParse(data.Options[0], out seconds))
            {
                if (!isSilent)
                {
                    interactor.Send("Good night...");
                }

                yield return new WaitForSeconds(seconds);

                if(!isSilent)
                {
                    interactor.Send("Good morning!");
                }
            }
            else
            {
                this.SendHelp(interactor);
            }
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
