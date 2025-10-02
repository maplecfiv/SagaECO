using System;
using System.Collections;
using System.Collections.Generic;

namespace SagaDB.Actor
{
    /// <summary>
    ///     变量不存在是自动返回默认Null值的变量存储器
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class VariableHolder<K, T> : IDictionary<K, T>
    {
        private readonly T nullValue;
        public Dictionary<K, T> content = new Dictionary<K, T>();

        public VariableHolder(T nullValue)
        {
            this.nullValue = nullValue;
        }

        //#region IEnumerable<KeyValuePair<K,T>> Members

        public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
        {
            return content.GetEnumerator();
        }

        //#endregion

        //#region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return content.GetEnumerator();
        }

        //#endregion

        //#region IDictionary<K,T> Members

        public void Add(K key, T value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(K key)
        {
            return content.ContainsKey(key);
        }

        public ICollection<K> Keys => content.Keys;

        public bool Remove(K key)
        {
            if (content.ContainsKey(key))
                return content.Remove(key);
            return false;
        }

        public bool TryGetValue(K key, out T value)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Values => content.Values;

        public T this[K key]
        {
            get
            {
                if (content.ContainsKey(key))
                    return content[key];
                return nullValue;
            }
            set
            {
                if (content.ContainsKey(key))
                    content[key] = value;
                else
                    content.Add(key, value);
            }
        }

        //#endregion

        //#region ICollection<KeyValuePair<K,T>> Members

        public void Add(KeyValuePair<K, T> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<K, T> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<K, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count => content.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public bool Remove(KeyValuePair<K, T> item)
        {
            throw new NotImplementedException();
        }

        //#endregion
    }
}