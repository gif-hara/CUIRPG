using UnityEngine;
using HK.Framework.Text;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="StringAsset"/>を保持するクラス
    /// </summary>
    [CreateAssetMenu(menuName = "CUIRPG/StringAssetHolder")]
    public sealed class StringAssetHolder : ScriptableObject
    {
        public CommandHelpBundle commandHelpBundle = default;

        [Multiline]
        public string commandDoesNotExist = default;

        [Serializable]
        public class CommandHelpBundle
        {
            [SerializeField]
            private List<CommandHelp> list = default;

            private Dictionary<string, CommandHelp> dictionary = null;

            public CommandHelp Get(string commandName)
            {
                if (this.dictionary == null)
                {
                    this.dictionary = new Dictionary<string, CommandHelp>();
                    foreach (var x in this.list)
                    {
                        this.dictionary.Add(x.CommandName, x);
                    }
                }

                Assert.IsTrue(this.dictionary.ContainsKey(commandName), $"{commandName}は存在しません");

                return this.dictionary[commandName];
            }
        }

        [Serializable]
        public class CommandHelp
        {
            [SerializeField]
            private string commandName = default;
            public string CommandName => this.commandName;

            [SerializeField, Multiline]
            private string description = default;
            public string Description => this.description;
        }
    }
}
