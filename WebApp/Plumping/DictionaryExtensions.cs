using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
namespace WebApp.Plumping {
    public static class DictionaryExtensions {
        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IDictionary<TKey, TValue> input) {
            return input == null ? null : new ConcurrentDictionary<TKey, TValue>(input);
        }
        public static ConcurrentDictionary<string, TValue> ToConcurrentDictionaryIgnoreKeyCase<TValue>(this IDictionary<string, TValue> input) {
            return input == null ? null : new ConcurrentDictionary<string, TValue>(input, StringComparer.OrdinalIgnoreCase);
        }
        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> input) {
            return input == null ? null : new ConcurrentDictionary<TKey, TValue>(input);
        }
        public static void Replace<TKey, TVal>(this IDictionary<TKey, TVal> dict, TVal val, Func<TVal, bool> conditions) {
            var list = dict.Where(p => conditions(p.Value)).ToList();
            list.ForEach(p => dict[p.Key] = p.Value);
        }
        //try get value
        public static TVal Get<TKey, TVal>(this ConcurrentDictionary<TKey, TVal> dict, TKey key) {
            TVal val;
            return dict.TryGetValue(key, out val) ? val : default(TVal);
        }
        public static ConcurrentDictionary<TKey, TVal> Update<TKey, TVal>(this ConcurrentDictionary<TKey, TVal> target, ConcurrentDictionary<TKey, TVal> source) {
            source.Keys.ForEach(key => {
                TVal newValue, oldValue;
                if (source.TryGetValue(key, out newValue) && target.TryGetValue(key, out oldValue))
                    target.TryUpdate(key, newValue, oldValue);
            });
            return target;
        }
        public static bool Delete<TKey, TVal>(this ConcurrentDictionary<TKey, TVal> dict, TKey key) {
            TVal val;
            return dict.TryRemove(key, out val);
        }
        //try get value
        public static TVal Get<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key) {
            TVal val;
            return dict.TryGetValue(key, out val) ? val : default(TVal);
        }
        public static IDictionary<TKey, TVal> Update<TKey, TVal>(this IDictionary<TKey, TVal> target, IDictionary<TKey, TVal> source) {
            source.Keys.ForEach(key => {
                TVal newValue, oldValue;
                if (source.TryGetValue(key, out newValue) && target.TryGetValue(key, out oldValue))
                    target[key] = newValue;
            });
            return target;
        }
        public static bool Delete<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key) {
            TVal val;
            return dict.Remove(key);
        }
        public static IDictionary<TKey, TVal> AddRange<TKey, TVal>(this IDictionary<TKey, TVal> target, IDictionary<TKey, TVal> source) {
            if (target == null) target = new Dictionary<TKey, TVal>();
            source.ForEach(p => target.Add(p));
            return target;
        }
    }
}
