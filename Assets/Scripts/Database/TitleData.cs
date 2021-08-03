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

        public IReadOnlyList<TitleItem> Items => m_Items;

        public IReadOnlyList<TitleField> Fields => m_Fields;

        private List<TitleItem> m_Items = new List<TitleItem>();

        private List<TitleField> m_Fields = new List<TitleField>();

        public void Setup(Dictionary<string, string> data)
        {
            m_Items = PlayFabSimpleJson.DeserializeObject<List<TitleItem>>(data["Items"]);
            m_Fields = PlayFabSimpleJson.DeserializeObject<List<TitleField>>(data["Fields"]);
        }
    }
}
