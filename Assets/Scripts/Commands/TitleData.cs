using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

namespace HK.CUIRPG
{
    /// <summary>
    /// タイトルデータに関するする<see cref="ICommand"/>
    /// </summary>
    public sealed class TitleData : ICommand
    {
        public string Name => "titledata";

        public TitleData()
        {
        }

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description);
        }

        public System.IObservable<Unit> Invoke(CommandData data, IInteractor interactor)
        {
            return Observable.Create<Unit>(observer =>
            {
                var request = new GetTitleDataRequest();

                PlayFabClientAPI.GetTitleData(
                    request,
                    result =>
                    {
                        foreach (var i in result.Data)
                        {
                            interactor.Send($"{i.Key}:{i.Value}");
                        }

                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        interactor.Send($"{error.ErrorMessage}");
                        observer.OnError(new System.Exception(error.ErrorMessage));
                    });

                return Disposable.Empty;
            });
        }
    }
}
