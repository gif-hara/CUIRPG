using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserItem
    {
        public int titleItemId;

        public TitleItem TitleItem => TitleData.Instance.Items.FirstOrDefault(x => x.id == titleItemId);

        public override string ToString()
        {
            var titleItem = TitleData.Instance.Items.FirstOrDefault(x => x.id == titleItemId);
            return $"{titleItem.name}";
        }
    }
}
