using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserData
    {
        public static UserData Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new UserData();
                }

                return m_Instance;
            }
        }
        private static UserData m_Instance;

        public IObservable<Unit> SetuppedAsObservable() => m_Setupped;
        private Subject<Unit> m_Setupped = new Subject<Unit>();

        public List<UserItem> UserItems
        {
            get;
            private set;
        }

        public List<UserAlias> UserAliases
        {
            get;
            private set;
        }

        public void Setup(Dictionary<string, UserDataRecord> data)
        {
            UserItems = Setup<List<UserItem>>(Key.Items, data);
            UserAliases = Setup<List<UserAlias>>(Key.Aliases, data);

            m_Setupped.OnNext(Unit.Default);
        }

        private T Setup<T>(string key, Dictionary<string, UserDataRecord> data) where T : new()
        {
            if (data.ContainsKey(key))
            {
                return PlayFabSimpleJson.DeserializeObject<T>(data[key].Value);
            }
            else
            {
                return new T();
            }
        }

        public IObservable<Unit> SendUpdateUserDataRequestAsObservable()
        {
            return Observable.Create<Unit>(observer =>
            {
                var request = new UpdateUserDataRequest
                {
                    Data = ToRequest()
                };
                PlayFabClientAPI.UpdateUserData(
                    request,
                    result =>
                    {
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        observer.OnError(new Exception(error.GenerateErrorReport()));
                    });

                return Disposable.Empty;
            });
        }

        public Dictionary<string, string> ToRequest()
        {
            return new Dictionary<string, string>()
            {
                { Key.Items, PlayFabSimpleJson.SerializeObject(UserItems) },
                { Key.Aliases, PlayFabSimpleJson.SerializeObject(UserAliases) }
            };
        }

        private static class Key
        {
            public static readonly string Items = "Items";

            public static readonly string Aliases = "Aliases";
        }
    }
}
