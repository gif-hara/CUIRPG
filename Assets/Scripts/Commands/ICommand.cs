using System.Collections;

namespace HK.MineTerminal
{
    /// <summary>
    /// コマンドのインターフェイス
    /// </summary>
    public interface ICommand
    {
        string Name { get; }

        IEnumerator Invoke(CommandData data, IInteractor interactor);

        void SendHelp(IInteractor interactor);
    }
}
