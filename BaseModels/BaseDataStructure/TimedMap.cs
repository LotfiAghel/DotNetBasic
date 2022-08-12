using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace BaseDataStructure
{
    public class TimedMap<T>
    {	
		class TimedItem
		{
			public string key;
			public T item;
			public DateTime expiration;
			public LinkedListNode<TimedItem> node;
			public Action<bool> removeCallback;
		}

		LinkedList<TimedItem> tsList = new LinkedList<TimedItem>();
		Dictionary<string, TimedItem> itemMap = new Dictionary<string, TimedItem>();
		TimeSpan timeout;               

        public TimedMap(TimeSpan timeout, TimeSpan checkInterval)
		{
			this.timeout = timeout;
			//var _ = Coroutine.Run(() => cleanUp(checkInterval));
		}

		public bool UpdateExpiration(string key)
		{
			TimedItem item;
			if (itemMap.TryGetValue(key, out item))
			{
				tsList.Remove(item.node);
				item.expiration = DateTime.UtcNow.Add(timeout);
				tsList.AddLast(item.node);
				return true;
			}
			return false;
		}

		public bool TryAdd(string key, T item, Action<bool> removeCallback = null)
		{
			TimedItem to = new TimedItem()
			{
				key = key,
				item = item,
				expiration = DateTime.UtcNow.Add(timeout),
                removeCallback = removeCallback
			};
			if (itemMap.TryAdd(key, to))
			{
				to.node = tsList.AddLast(to);
				return true;
			}
			return false;
		}

		public bool TryGet(string key, out T item)
		{
			TimedItem to;
			if (itemMap.TryGetValue(key, out to))
			{
				item = to.item;
				return true;
			}
			item = default(T);
			return false;
		}

		public IEnumerable<string> GetKeys()
		{
			foreach (var item in itemMap.Keys)
			{
				yield return item;
			}
		}

		public IEnumerable<T> GetValues()
		{
			foreach (var item in itemMap.Values)
			{
				yield return item.item;
			}
		}

		public IEnumerable<KeyValuePair<string, T>> GetItems()
        {
            foreach (var item in itemMap)
            {
				yield return new KeyValuePair<string, T>(item.Key, item.Value.item);
            }
        }

		public bool Has(string key)
		{
			return itemMap.ContainsKey(key);
		}

		bool remove(string key, bool timedOut)
		{
			TimedItem to;
            if (itemMap.TryGetValue(key, out to))
            {
                try
                {
                    tsList.Remove(to.node);
                }
                catch { }
                var result = itemMap.Remove(key);
                if (to.removeCallback != null)
                {
					try
					{
						to.removeCallback(timedOut);
					}
					catch (Exception ex)
					{
						Logging.Error("TimedMap.remove", ex + "");
					}
                    to.removeCallback = null;
                }
                return result;
            }
            return false;
		}

		public bool Remove(string key)
		{
			return remove(key, false);
		}

		public int GetCount()
		{
			return itemMap.Count;
		}
		public void onTick()
		{
				DateTime now = DateTime.UtcNow;
				for (int i = 0; i < 100; i++)
				{
					if (tsList.First != null)
					{
						var item = tsList.First.Value;
						if (item.expiration < now)
						{
							remove(item.key, true);
						}
						else
						{
							break;
						}
					}
					else
						break;
				}

		}
		
    }


	

	public class IndexerMap<TValue, TSortObject>: SortedSet<IndexerMap<TValue, TSortObject>.Entry>
	{
		public class Entry
		{
			public TValue v;
			public TSortObject index;
		}
		public class BlahComparer : Comparer<Entry>
		{
			public override int Compare(Entry x, Entry y)
			{
				int v=Comparer<TSortObject>.Default.Compare(x.index, y.index);
				if (v != 0)
					return v;
				return Comparer<TValue>.Default.Compare(x.v, y.v);
			}
		}
		//SortedSet<Entry> sortedset = new SortedSet<Entry>(new BlahComparer());
		public IndexerMap():base(new BlahComparer())
		{
			
		}
		Dictionary<TValue, Entry> map = new Dictionary<TValue, Entry>();

		public Entry addOrUpdate(TValue v, TSortObject a)
		{
			if (map.ContainsKey(v))
			{
				base.Remove(map[v]);
			}
			var t = new Entry()
			{
				v=v,
				index=a
			};
			base.Add(t);
			map[v] = t;
			return t;
		}
		public void remove(TValue v)
		{
			if (map.ContainsKey(v))
			{
				base.Remove(map[v]);
				map.Remove(v);
				
			}
		}
		public void remove0(Entry v)
		{
			base.Remove(v);
			map.Remove(v.v);
		}

		public bool TryGet(TValue v,out Entry res)
		{
			if (map.ContainsKey(v))
			{
				res = map[v];
				return true;
			}
			res = null;
			return false;
		}




		/*public class PeopleEnum : IEnumerator<Entry>, IEnumerator
		{
			public SortedSet<TValue, TSortObject> _people;

			// Enumerators are positioned before the first element
			// until the first MoveNext() call.
			SortedSet<Entry>.Enumerator cur =null;

			public PeopleEnum(SortedSet<TValue, TSortObject> list)
			{
				_people = list;
				cur = _people.sortedset.GetEnumerator();// list.keys.First.Previous;
			}

			public bool MoveNext()
			{
				return cur.MoveNext();
			}

			public void Reset()
			{
				cur = _people.getFirst();
			}


			private bool disposedValue = false;
			public void Dispose()
			{
				cur = null;
				_people = null;
				GC.SuppressFinalize(this);

			}


			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			public Entry Current
			{
				get
				{
					try
					{
						return cur;
					}
					catch (IndexOutOfRangeException)
					{
						throw new InvalidOperationException();
					}
				}
			}
		}
		public IEnumerator<SortedSet<TValue, TSortObject>.Entry> GetEnumerator()
		{
			return new PeopleEnum(this);

		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new PeopleEnum(this);
		}/**/
	}


	class FastList<Tkey, T> : IEnumerable<T>
	{
		public class DData
		{
			public T data;
			public LinkedListNode<DData> ln;
		}
		//HashSet<DData> data;
		LinkedList<DData> keys = new LinkedList<DData>();
		Dictionary<Tkey, DData> index = new Dictionary<Tkey, DData>();

		public int Count() { return index.Count; }

		public T Peek()
		{
			return keys.First.Value.data;
		}
		public T get(Tkey reqId)
		{
			DData o = null;
			index.TryGetValue(reqId, out o);
			if (o == null)
				return default(T);
			return o.data;
		}

		public void remove(Tkey reqId)
		{
			DData o = null;
			index.TryGetValue(reqId, out o);
			if (o == null)
				return;
			keys.Remove(o.ln);
			index.Remove(reqId);
			//data.Remove(o);
		}

		internal bool ContainsKey(Tkey id)
		{
			return index.ContainsKey(id);
		}

		internal void Add(Tkey reqId, T rr)
		{
			var x = new DData()
			{
				data = rr
			};
			index.Add(reqId, x);
			x.ln = keys.AddLast(x);
		}


		public class PeopleEnum : IEnumerator<T>, IEnumerator
		{
			public FastList<Tkey, T> _people;

			// Enumerators are positioned before the first element
			// until the first MoveNext() call.
			LinkedListNode<DData> cur;

			public PeopleEnum(FastList<Tkey, T> list)
			{
				_people = list;
				cur = null;// list.keys.First.Previous;
			}

			public bool MoveNext()
			{
				if (cur == null)
					cur = _people.keys.First;
				else
					cur = cur.Next;
				return cur != null;
			}

			public void Reset()
			{
				cur = _people.keys.First;
			}


			private bool disposedValue = false;
			public void Dispose()
			{
				cur = null;
				_people = null;
				GC.SuppressFinalize(this);

			}



			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			public T Current
			{
				get
				{
					try
					{
						return cur.Value.data;
					}
					catch (IndexOutOfRangeException)
					{
						throw new InvalidOperationException();
					}
				}
			}
		}
		public IEnumerator<T> GetEnumerator()
		{
			return new PeopleEnum(this);

		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new PeopleEnum(this);
		}
	}

	public class FixedSizeMap<TKey, TVAlue2> : SortedSet<FixedSizeMap<TKey, TVAlue2>.Entry>
	{
		public class Entry
		{
			public TVAlue2 value;
			public TKey key;
			
			public long index;
		}
		long gen = 0;
		Dictionary<TVAlue2, HashSet<Entry>> revers=new Dictionary<TVAlue2, HashSet<Entry>>();
		public class BlahComparer : IComparer<Entry>
		{
			public  int Compare(Entry x, Entry y)
			{
				int v = Comparer<long>.Default.Compare(x.index, y.index);
				if (v != 0)
					return v;
				return Comparer<TKey>.Default.Compare(x.key, y.key);
			}
		}
		//SortedSet<Entry> sortedset = new SortedSet<Entry>(new BlahComparer());
		public FixedSizeMap() : base(new BlahComparer())
		{

		}

		public FixedSizeMap(int v) : base(new BlahComparer())
		{
			this.maxSize = v;
		}

		Dictionary<TKey, Entry> map = new Dictionary<TKey, Entry>();
		private int maxSize;

		public void addOrUpdate(TKey key,TVAlue2 value)
		{
			Entry t = null;
			if (map.TryGetValue(key, out t))
			{
				base.Remove(t);
				if(value!=null)
					revers[map[key].value].Remove(map[key]);
			}
			else
			{
				t = new Entry()
				{
					key = key,
				};
				map[key] = t;
			}

			t.value = value;
			var utcNow = DateTime.UtcNow;
			t.index = gen++;
			base.Add(t);//TODO System.ArgumentException: 'At least one object must implement IComparable.'
			HashSet<Entry> le=null;
			if (value!=null && !revers.TryGetValue(value, out le))
			{
				le = new HashSet<Entry>();
				revers[value] = le;
			}
			if(le!=null)
				le.Add(t);
			if (Count > maxSize)
				remove0(Min);


			
		}
		public void remove(TKey key)
		{
			if (map.ContainsKey(key))
				remove0(map[key]);
		}
		public void remove0(Entry v)
		{
			base.Remove(v);
			map.Remove(v.key);
			if(v.value!=null)revers[v.value].Remove(v);
		}

		

		public bool TryGet(TKey v, out TVAlue2 res)
		{
			Entry val;
			if (map.TryGetValue(v,out val))
			{
				res = val.value;
				return true;
			}
			res = default(TVAlue2);
			return false;
		}

		internal void UpdateExpiration(TKey key)
		{
			
			if (!map.ContainsKey(key))
				return;
			TVAlue2 val = map[key].value;
			var t = map[key];
			base.Remove(t);

			t.value = val;
			t.index = gen++;// DateTime.UtcNow;
			
			base.Add(t);
			map[key] = t;
		}
		public void removeAllkeyToValue(TVAlue2 v)
		{
			if (v!=null && revers.ContainsKey(v))
			{
				var l = revers[v];//TODO The given key 'GCS-Bankrupt-1168' was not present in the dictionary in faghat mitune bekhatere chand thearde budan pish biyad
				revers.Remove(v);
				foreach (var entry in l)
				{
					base.Remove(entry);
					map.Remove(entry.key);
				}
				
			}
		}
	}



	public class FixedSizeMap2<TKey, TValue> 
    {
		Dictionary<TKey, TValue> data;
		//IndexerMap<TKey,int> 

	}

}
