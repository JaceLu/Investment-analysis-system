using System.Collections;
using System.Collections.Generic;

namespace Lamtip
{
    public class StringKeyPairs<TValue> : IDictionary<string, TValue>
    {

        protected static IComparer<string> ASCIIComparer => Comparer<string>.Create((x, y) => string.CompareOrdinal(x, y));


        protected SortedDictionary<string, TValue> _dictionary { get; }

        public ICollection<string> Keys => ((IDictionary<string, TValue>)_dictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<string, TValue>)_dictionary).Values;

        public int Count => ((IDictionary<string, TValue>)_dictionary).Count;

        public bool IsReadOnly => ((IDictionary<string, TValue>)_dictionary).IsReadOnly;

        public TValue this[string key] { set => this._dictionary[key] = value; get => this._dictionary[key]; }

        /// <summary>
        /// 
        /// </summary>
        public StringKeyPairs()
        {
            _dictionary = new SortedDictionary<string, TValue>(ASCIIComparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public StringKeyPairs(IComparer<string> comparer)
        {
            this._dictionary = new SortedDictionary<string, TValue>(comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public StringKeyPairs(IDictionary<string, TValue> dictionary)
        {
            this._dictionary = new SortedDictionary<string, TValue>(dictionary, ASCIIComparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public StringKeyPairs(IDictionary<string, TValue> dictionary, IComparer<string> comparer)
        {
            this._dictionary = new SortedDictionary<string, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="withEmptyValue"></param>
        /// <returns></returns>
        public string ToString(string separator = "&", bool withEmptyValue = true)
        {
            var res = "";
            foreach (var item in this._dictionary)
            {
                var val = item.Value?.ToString() ?? "";
                if (!withEmptyValue && string.IsNullOrEmpty(val)) continue;
                res = string.IsNullOrEmpty(res) ? res : res + separator;
                res += $"{item.Key}={val}";
            }
            return res;
        }

        public override string ToString()
        {
            return this.ToString("&");
        }

        public void Add(string key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }


        public void Clear()
        {
            _dictionary.Clear();
        }


        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return ((IDictionary<string, TValue>)_dictionary).Remove(item);
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            ((IDictionary<string, TValue>)_dictionary).Add(item);
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return ((IDictionary<string, TValue>)_dictionary).Contains(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, TValue>)_dictionary).GetEnumerator();
        }
    }

}