using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Database
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class UserItem
    {
        public int titleItemId;

        public Dictionary<string, int> effects = new Dictionary<string, int>();

        public TitleItem GetTitleItem() => TitleData.Instance.Items.FirstOrDefault(x => x.id == titleItemId);

        public static UserItem New(int titleItemId)
        {
            var instance = new UserItem
            {
                titleItemId = titleItemId
            };

            var targetEffects = TitleData.Instance.AddableItemEffects.GetFromTitleItemId(titleItemId);
            foreach (var i in targetEffects)
            {
                var effect = i.Value.Lottery();
                if (effect.value <= 0)
                {
                    continue;
                }

                instance.effects.Add(i.Key, effect.value);
            }

            return instance;
        }

        public override string ToString()
        {
            var titleItem = TitleData.Instance.Items.FirstOrDefault(x => x.id == titleItemId);
            return $"{titleItem.name}";
        }
    }
}
