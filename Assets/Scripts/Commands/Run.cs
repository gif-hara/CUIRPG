using System.Collections;
using HK.MineTerminal.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace HK.MineTerminal
{
    /// <summary>
    /// ループしてコマンドを実行する<see cref="ICommand"/>
    /// </summary>
    public sealed class Run : ICommand
    {
        public string Name => "run";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count <= 0)
            {
                this.SendHelp(interactor);
                yield break;
            }

            var targetCommandData = data.Options[0];
            int loopCount;
            if(!data.TryGetInt("-l", out loopCount))
            {
                loopCount = -1;
            }

            float sleepSeconds;
            if(!data.TryGetFloat("-s", out sleepSeconds))
            {
                sleepSeconds = 0.0f;
            }

            while(loopCount != 0)
            {
                yield return OperatingSystem.Instance.CommandManager.InvokeCoroutine(targetCommandData, interactor);

                if(sleepSeconds > 0.0f)
                {
                    yield return new WaitForSeconds(sleepSeconds);
                }
                loopCount--;
            }
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
