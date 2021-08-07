using System;
using HK.CUIRPG.Database;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Commands
{
    /// <summary>
    /// インベントリに関する<see cref="ICommand"/>
    /// </summary>
    public sealed class InventoryCommands : ICommand
    {
        public string Name => "inventory";

        public void SendHelp(IInteractor interactor)
        {
            var help = OperatingSystem.Instance.LocalizedMessages.commandHelpBundle.Get(this.Name);
            interactor.Send(help.Description.Format(this.Name));
        }

        public IObservable<Unit> InvokeAsObservable(CommandData data, IInteractor interactor)
        {
            return Observable.Defer(() =>
            {
                if (data.Options.Count <= 0)
                {
                    SendHelp(interactor);
                    return Observable.ReturnUnit();
                }

                if (data.ContainsOption("-l"))
                {
                    var userData = UserData.Instance;
                    for (var i = 0; i < userData.UserItems.Count; i++)
                    {
                        var item = userData.UserItems[i];
                        interactor.Send($"[{i}] {item}");
                        if (data.ContainsOption("detail") || data.ContainsOption("d"))
                        {
                            foreach (var e in item.effects)
                            {
                                interactor.Send($"    {e.Key}:{e.Value}");
                            }
                        }
                    }

                    return Observable.ReturnUnit();
                }

                if (data.ContainsOption("-d"))
                {
                    var targetIndex = -1;
                    if (data.TryGetInt("-d", out targetIndex))
                    {
                        var userData = UserData.Instance;
                        if (targetIndex < 0 || userData.UserItems.Count <= targetIndex)
                        {
                            interactor.Send($"{targetIndex}は存在しません");
                            return Observable.ReturnUnit();
                        }
                        else
                        {
                            var userItem = userData.UserItems[targetIndex];
                            return interactor.Confirm($"{userItem.GetTitleItem().name}を削除します. よろしいですか？ y/n")
                            .SelectMany(x =>
                            {
                                if (x == "y")
                                {
                                    userData.UserItems.Remove(userItem);
                                    userData.SendUpdateUserDataRequestAsObservable()
                                    .Subscribe();
                                }

                                return Observable.ReturnUnit();
                            });
                        }
                    }
                    else
                    {
                        SendHelp(interactor);
                        return Observable.ReturnUnit();
                    }
                }

                return Observable.ReturnUnit();
            });
        }
    }
}
