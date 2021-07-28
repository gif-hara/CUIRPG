using UnityEngine;
using HK.Framework.Text;
using System;
using System.Collections.Generic;

namespace HK.MineTerminal
{
    /// <summary>
    /// <see cref="StringAsset"/>を保持するクラス
    /// </summary>
    [CreateAssetMenu(menuName = "MineTerminal/StringAssetHolder")]
    public sealed class StringAssetHolder : ScriptableObject
    {
        public CommandHelpBundle commandHelpBundle = default;

        public StringAsset.Finder commandDoesNotExist = default;

        [Serializable]
        public class CommandHelpBundle
        {
            [SerializeField]
            private List<CommandHelp> list = default;

            private Dictionary<string, CommandHelp> dictionary = null;

            public CommandHelp Get(string commandName)
            {
                if(this.dictionary == null)
                {
                    this.dictionary = new Dictionary<string, CommandHelp>();
                    foreach(var x in this.list)
                    {
                        this.dictionary.Add(x.CommandName, x);
                    }
                }

                return this.dictionary[commandName];
            }
        }

        [Serializable]
        public class CommandHelp
        {
            [SerializeField]
            private string commandName = default;
            public string CommandName => this.commandName;

            [SerializeField]
            private StringAsset.Finder message = default;
            public StringAsset.Finder Message => this.message;
        }
    }
}
