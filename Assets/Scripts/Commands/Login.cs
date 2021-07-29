using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// ゲームにログインする<see cref="ICommand"/>
    /// </summary>
    public sealed class Login : ICommand
    {
        public string Name => "login";

        public Login()
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
                var request = new LoginWithCustomIDRequest
                {
                    CustomId = "GettingStartedGuide",
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithCustomID(
                    request,
                    result =>
                    {
                        interactor.Send($"Login! {result.PlayFabId}");
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        interactor.Send("Login Failed...");
                        observer.OnError(new System.Exception(error.ErrorMessage));
                    });

                return Disposable.Empty;
            });
        }
    }
}
