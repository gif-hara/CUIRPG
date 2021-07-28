using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HK.Framework.Text
{
	/// <summary>
	/// 文字列を管理するアセット.
	/// </summary>
	[System.Serializable][CreateAssetMenu()]
	public class StringAsset : ScriptableObject
	{
        public static string currentCulture = "ja";

        [System.Serializable]
		public class Finder
		{
			public StringAsset target;

			public string value;

			public string guid;

#if !UNITY_EDITOR
	        private string cachedValue;

	        private bool isInitialize = false;
#endif

			public Finder(StringAsset target, Data data)
			{
				this.target = target;
				this.value = data.value.Default;
				this.guid = data.guid;
			}

			public override string ToString()
			{
#if UNITY_EDITOR
				// 毎回取得することでゲーム中でも値を変更出来るように.
				return this.target.Get(this);
#else
	                // 最適化のためキャッシュさせる.
	                if(!this.isInitialize)
	                {
	                    this.cachedValue = this.target.Get( this );
	                    this.isInitialize = true;
	                }

	                return this.cachedValue;
#endif
			}

			public string Get
			{
				get
				{
					return this.ToString();
				}
			}

			public string Format(params object[] args)
			{
				return this.target.Format(this, args);
			}

			public override int GetHashCode()
			{
				return this.ToString().GetHashCode();
			}

			public bool IsValid
			{
				get { return !string.IsNullOrEmpty(this.guid); }
			}
		}

		[System.Serializable]
		public class Data
		{
			public Data()
			{
				this.value = new Value();
				this.guid = System.Guid.NewGuid().ToString();
			}

			public Value value;

			public string guid;
		}

		public class DataEqualityComparer : IEqualityComparer<Data>
		{
			public bool Equals(Data x, Data y)
			{
				return x.value.Default.CompareTo(y.value.Default) == 0;
			}

			public int GetHashCode(Data obj)
			{
				return obj.value.Default.GetHashCode();
			}
		}

		[System.Serializable]
		public class Value
		{
			public string ja = "";

			public string en = "";

			public string Default
			{
				get
				{
					return this.ja;
				}
			}

			public void Set(string value, string culture)
			{
				switch(culture)
				{
				case "ja":
					this.ja = value;
				break;
				case "en":
					this.en = value;
				break;
				default:
					Debug.AssertFormat(false, "不正な値です. culture = {0}", culture);
				break;
				}
			}

			public string Get(string culture)
			{
				switch(culture)
				{
				case "ja":
					return this.ja;
				case "en":
					return this.en;
				default:
					Debug.AssertFormat(false, "不正な値です. culture = {0}", culture);
					return "";
				}
			}
		}

		public List<Data> database = new List<Data>();

#if !UNITY_EDITOR
		/// <summary>
		/// 検索用のディクショナリ.
		/// </summary>
		private Dictionary<string, Value> findDictionary = null;
#endif

		/// <summary>
		/// 要素を取得する.
		/// </summary>
		/// <param name="finder"></param>
		/// <returns></returns>
		public string Get(Finder finder)
		{
#if UNITY_EDITOR
			var data = this.database.Find(d => d.guid == finder.guid);
			if(data == null)
			{
				Debug.LogError("\"" + finder.value + "\"に対応する値がありませんでした.");
				return "";
			}

			return data.value.Get(currentCulture);
#else
	        if( this.findDictionary == null )
	        {
	            this.findDictionary = new Dictionary<string, Value>();
	            for( int i = 0, imax = this.database.Count; i < imax; i++ )
	            {
	                this.findDictionary.Add( this.database[i].guid, this.database[i].value );
	            }
	        }
			
			Value result;
	        if( !this.findDictionary.TryGetValue( finder.guid, out result ) )
			{
				// 要素がない場合はエラー.
				Debug.LogError( "\"" + finder.value + "\"に対応する文字列がありませんでした." );
	            result.ja = finder.value;
			}
			
			return result.Get(currentCulture);
#endif
		}

#if UNITY_EDITOR
		[ContextMenu("Overlap Check")]
		private void OverlapCheck()
		{
			var overlapList = this.database
				.Where(d => this.database.FindAll(_d => _d.value.Default.CompareTo(d.value.Default) == 0).Count != 1)
				.Distinct(new DataEqualityComparer())
				.ToList();
			if(overlapList.Count > 0)
			{
				foreach(var o in overlapList)
				{
					Debug.LogErrorFormat("\"{0}\"が複数存在します.", o.value.Default);
				}
			}
			else
			{
				Debug.Log("重複していませんでした.");
			}
		}

		[ContextMenu("Print Count")]
		private void PrintCount()
		{
			Debug.LogFormat("{0}には{1}の要素が存在しています.", this.name, this.database.Count);
		}

		public Finder CreateFinder(string defaultString)
		{
			var data = this.database.Find(d => d.value.Default == defaultString);
			Debug.AssertFormat(data != null, "{0}がありません.", defaultString);
			return new Finder(this, data);
		}
#endif
		/// <summary>
		/// string.Formatのラッピング.
		/// </summary>
		/// <param name="finder"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public string Format(Finder finder, params object[] args)
		{
			return string.Format(Get(finder), args);
		}

		public void Add()
		{
			this.database.Add(new Data());
		}
	}
}
