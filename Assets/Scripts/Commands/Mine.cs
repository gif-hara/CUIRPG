using System.Collections;

namespace HK.MineTerminal
{
    /// <summary>
    /// ポイントを加算する<see cref="ICommand"/>
    /// </summary>
    public sealed class Mine : ICommand
    {
        public string Name => "mine";

        public IEnumerator Invoke(CommandData data, IInteractor interactor)
        {
            OperatingSystem.Instance.AddPointFromCpuPower();
            interactor.Send($"Mined! Current point is {OperatingSystem.Instance.Point}");
            yield break;
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Message.Format(this.Name));
        }
    }
}
