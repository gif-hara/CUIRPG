using System;
using System.Collections;
using System.Collections.Generic;
using HK.CUIRPG.Database;
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

                var userData = UserData.Instance;
                var items = userData.UserItems;
                items.Add(new UserItem
                {
                    titleItemId = Database.TitleData.Instance.Items[0].id
                });

                var request = new UpdateUserDataRequest
                {
                    Data = userData.ToRequest()
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
