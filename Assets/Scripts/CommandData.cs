using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// ユーザーが入力したコマンドデータ
    /// </summary>
    public sealed class CommandData
    {
        public string Name { get; }

        public readonly List<string> Options = new List<string>();

        private const char Space = ' ';

        private const char SingleQuote = '\'';

        private const char DoubleQuote = '\"';

        private enum QuoteMode
        {
            None,
            Single,
            Double,
        }

        public bool IsValid { get; }

        public CommandData(string data)
        {
            // e.g. $mine --head "hello world"loop 3 'this is "hoge"'
            // [mine, --head, hello world, loop, 3, this is "hoge"]
            var parsedData = new StringBuilder();
            var index = 0;
            var quoteMode = QuoteMode.None;
            while(data.Length > index)
            {
                var c = data[index];
                index++;
                if(c == Space && quoteMode == QuoteMode.None)
                {
                    if(parsedData.Length > 0)
                    {
                        this.Options.Add(parsedData.ToString());
                        parsedData.Clear();
                    }
                }
                else if(c == SingleQuote || c == DoubleQuote)
                {
                    var q = c == SingleQuote ? QuoteMode.Single : QuoteMode.Double;
                    if(quoteMode == QuoteMode.None)
                    {
                        quoteMode = q;
                    }
                    else if((c == SingleQuote && quoteMode == QuoteMode.Single) || (c == DoubleQuote && quoteMode == QuoteMode.Double))
                    {
                        quoteMode = QuoteMode.None;
                        if(parsedData.Length > 0)
                        {
                            this.Options.Add(parsedData.ToString());
                            parsedData.Clear();
                        }
                    }
                    else
                    {
                        parsedData.Append(c);
                    }
                }
                else
                {
                    parsedData.Append(c);
                }
            }

            if(parsedData.Length > 0)
            {
                this.Options.Add(parsedData.ToString());
            }

            if(this.Options.Count > 0)
            {
                this.IsValid = true;
                this.Name = this.Options[0];
                Options.RemoveAt(0);
            }
            else
            {
                this.IsValid = false;
            }
        }

        public bool ContainsOption(string value)
        {
            return this.Options.FindIndex(x => x == value) >= 0;
        }

        public string GetString(string optionName)
        {
            if(!ContainsOption(optionName))
            {
                return "";
            }

            var index = this.Options.FindIndex(x => x == optionName);
            if(this.Options.Count - 1 == index)
            {
                return "";
            }

            return this.Options[index + 1];
        }

        public bool TryGetInt(string optionName, out int result)
        {
            var value = GetString(optionName);
            return int.TryParse(value, out result);
        }

        public bool TryGetFloat(string optionName, out float result)
        {
            var value = GetString(optionName);
            return float.TryParse(value, out result);
        }
    }
}
