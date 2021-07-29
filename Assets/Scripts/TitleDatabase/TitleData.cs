using System.Collections.Generic;
using PlayFab.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TitleData
    {
        public static TitleData Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new TitleData();
                }

                return m_Instance;
            }
        }
        private static TitleData m_Instance;

        public IReadOnlyList<Item> Items => m_Items;

        private List<Item> m_Items = new List<Item>();

        public void Setup(Dictionary<string, string> data)
        {
            m_Items = PlayFabSimpleJson.DeserializeObject<List<Item>>(data["Items"]);
        }
    }
}
