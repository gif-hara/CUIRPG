using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab.Json;
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

        public List<UserItem> UserItems
        {
            get;
            private set;
        }

        public void Setup(Dictionary<string, UserDataRecord> data)
        {
            if (data.ContainsKey(Key.Items))
            {
                UserItems = PlayFabSimpleJson.DeserializeObject<List<UserItem>>(data[Key.Items].Value);
            }
            else
            {
                UserItems = new List<UserItem>();
            }
        }

        public Dictionary<string, string> ToRequest()
        {
            return new Dictionary<string, string>()
            {
                { Key.Items, PlayFabSimpleJson.SerializeObject(UserItems) }
            };
        }

        private static class Key
        {
            public static readonly string Items = "Items";
        }
    }
}
