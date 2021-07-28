using System.Collections;

namespace HK.MineTerminal
{
    /// <summary>
    /// OSを増設する<see cref="ICommand"/>
    /// </summary>
    public sealed class Expansion : ICommand
    {
        public string Name => "exp";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            if(data.Options.Count <= 0)
            {
                this.SendHelp(interactor);
                yield break;
            }

            var os = OperatingSystem.Instance;

            if(data.ContainsOption("-f") || data.ContainsOption("--confirm"))
            {
                interactor.Send("TODO confirm");
                yield break;
            }

            foreach(var o in data.Options)
            {
                switch(o)
                {
                    case "-c":
                        if (!os.CanAddCoolPower())
                        {
                            interactor.Send("買えないよ");
                            yield break;
                        }
                        else
                        {
                            os.AddCoolPower();
                            yield break;
                        }
                    case "-u":
                        if (!os.CanAddCpuPower())
                        {
                            interactor.Send("買えないよ");
                            yield break;
                        }
                        else
                        {
                            os.AddCpuPower();
                            yield break;
                        }
                }
            }
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
