using System.Collections;

namespace HK.MineTerminal
{
    /// <summary>
    /// OSの各パラメータをモニターする<see cref="ICommand"/>
    /// </summary>
    public sealed class Monitor : ICommand
    {
        public string Name => "monitor";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count <= 0)
            {
                this.SendHelp(interactor);
            }

            if(data.ContainsOption("-l") || data.ContainsOption("--clear"))
            {
                interactor.ClearHistory();
            }

            var os = OperatingSystem.Instance;

            if (data.ContainsOption("-p") || data.ContainsOption("--point") || data.ContainsOption("--all"))
            {
                interactor.Send(string.Format("Point      : {0,7}", os.Point));
            }

            if (data.ContainsOption("-h") || data.ContainsOption("--heat") || data.ContainsOption("--all"))
            {
                var colorCode = "";
                var colorEndCode = "";
                if(os.Heat >= 0.33f)
                {
                    colorEndCode = "</color>";

                    if(os.Heat >= 0.66f)
                    {
                        colorCode = "<color=red>";
                    }
                    else
                    {
                        colorCode = "<color=yellow>";
                    }
                }
                interactor.Send(string.Format("{0}Heat       : {1,4}%{2}", colorCode, (os.Heat * 100.0f).ToString("0"), colorEndCode));
            }

            if (data.ContainsOption("-c") || data.ContainsOption("--cool") || data.ContainsOption("--all"))
            {
                interactor.Send(string.Format("Cool Power : {0,4}%/s", (os.CoolPowerRate * 100.0f).ToString("0")));
            }

            if (data.ContainsOption("-u") || data.ContainsOption("--cpu") || data.ContainsOption("--all"))
            {
                interactor.Send(string.Format("CPU Power  : {0,7}", os.CpuPower.ToString()));
            }

            yield break;
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
