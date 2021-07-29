using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// アイテムを探す<see cref="ICommand"/>
    /// </summary>
    public sealed class Search : ICommand
    {
        public string Name => "search";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Create<Unit>(observer =>
            {
                interactor.Send("Search...");

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "Hoge", "Foo2"}
                    }
                };
                PlayFabClientAPI.UpdateUserData(
                    request,
                    result =>
                    {
                        interactor.Send("Search Complete!");
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        interactor.Send(error.ErrorMessage);
                        observer.OnError(new Exception(error.GenerateErrorReport()));
                    });

                return Disposable.Empty;
            });
        }
    }
}
