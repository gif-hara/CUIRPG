using System.Collections;
using HK.Framework.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.MineTerminal
{
    /// <summary>
    /// カルチャーに関する<see cref="ICommand"/>
    /// </summary>
    public sealed class Culture : ICommand
    {
        public string Name => "culture";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count <= 0)
            {
                this.SendHelp(interactor);
                yield break;
            }

            StringAsset.currentCulture = data.Options[0];

            yield break;
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
