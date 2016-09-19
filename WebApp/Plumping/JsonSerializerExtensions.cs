using System.Collections.Generic;
using ServiceStack.Text;
namespace WebApp.Plumping {
    public static class JsonSerializerExtensions {
        public static T Clone<T>(this T t) where T : class {
            if (t == null) return null;
            return t.SerializeJson().DeserializeJson<T>();
        }
        public static T TranslateFromJsonObjectTo<T>(this JsonObject jo) where T : class, new() {
            return (jo == null) ? null : jo.SerializeJson().DeserializeJson<T>();
        } 
        public static JsonObject TranslateToJsonObject(this object o) {
            return (o == null) ? null : o.SerializeJson().DeserializeJson<JsonObject>();
        }
        public static T TranslateObjectTo<T>(this object obj) where T : class, new() {
            return (obj == null) ? null : obj.SerializeJson().DeserializeJson<T>();
        }
        public static string SerializeJson<T>(this T t) where T : class {
            return (t == null) ? @"" : JsonSerializer.SerializeToString(t).Replace(@"\r", @"").Replace(@"\n", @"").Replace(@"\t", @"");
        }
        public static T DeserializeJson<T>(this string s) where T : class {
            return s.IsNullOrWhiteSpace() ? null : JsonSerializer.DeserializeFromString<T>(s);
        }
        public static object DeserializeJson(this string s) {
            if (s.IsJsonString()) { //dictionary branches
                var dict1 = s.DeserializeJsonString();
                var keys = dict1.Keys.ToArray();
                foreach (var key in keys) {
                    dict1[key] = DeserializeJson((string)dict1[key]);
                }
                return dict1; 
            }
            else if (s.IsJsonArray()) { //list branches
                var list1 = s.DeserializeJsonArray();
                return list1; 
            }
            return s; //text leaf node
        }

        //public static ConcurrentDictionary<string, string> MergeJsonObjects(this ConcurrentDictionary<string, string> target, ConcurrentDictionary<string, string> source) {
        //    var dict1 = target.TranslateToJsonObject();
        //    var dict2 = source.TranslateToJsonObject();
        //    dict1.MergeJsonObjects(dict2);
        //    return dict1.TranslateObjectTo<ConcurrentDictionary<string, string>>(); 
        //}
        //public static string MergeJsonObjects(this string target, string source) {
        //    var obj1 = target.DeserializeJson();
        //    var obj2 = source.DeserializeJson();
        //    obj1.MergeJsonObjects(obj2);
        //    if (obj1 is JsonObject) return ((JsonObject)obj1).SerializeJson();
        //    if (obj1 is JsonArrayObjects) return ((JsonArrayObjects)obj1).SerializeJson();
        //    return target;
        //}
        ////this can be used after getting object from DeserializeJson() function, with default conflict resolver
        //public static void MergeJsonObjects(this object target, object source) {
        //    MergeJsonObjects(target, source, (x, y) => {
        //        if (x.IsInteger() && y.IsInteger()) return x.ToInteger() > y.ToInteger() ? x : y;
        //        if (x.IsBoolean() && y.IsBoolean()) return (x.ToBoolean() || y.ToBoolean()).ToString();
        //        return x; //can't make a decision, pick default first one
        //    });
        //}

        //this can be used after getting object from DeserializeJson() function
        //LIMITATION: when use json array, its children JsonObjects must have same structure
        //each JsonObject is compared as a string as a whole, it will not be parsed further down
        //complex asymmetric objects cannot be merged, only support symmetric simple key-value pairs e.g.,
        //"{\"employees\": [{ \"givenName\":\"John\" , \"familyName\":\"Doe\" }, " +
        //                 "{ \"givenName\":\"Anna\" , \"familyName\":\"Smith\" }, " +
        //                 "{ \"givenName\":\"Peter\" , \"familyName\":\"Jones\" }]}";

        //public static void MergeJsonObjects(this object target, object source, Func<string, string, string> resolveConflict) {
        //    if (target is JsonObject && source is JsonObject) {
        //        var dictTarget = (JsonObject) target;
        //        var dictSource = (JsonObject) source;
        //        foreach (var pair in dictSource) {
        //            string t;
        //            string k = pair.Key;
        //            string s = pair.Value;
        //            dictTarget.TryGetValue(k, out t);
        //            if (t.IsNullOrWhiteSpace()) {
        //                dictTarget.Add(k, s);  
        //            } else {
        //                dictTarget[k] = resolveConflict(s, t);
        //            }
        //        }
        //    } else if (target is JsonArrayObjects && source is JsonArrayObjects) {
        //        var listTarget = (JsonArrayObjects)target;
        //        var listSource = (JsonArrayObjects)source;
        //        foreach (var str in listSource) {
        //            if (!listTarget.Contains(str)) listTarget.Add(str);
        //        }
        //    }
        //}
        public static Dictionary<string, object> DeserializeJsonString(this string s) {
            if (s.IsJsonString())
                return JsonSerializer.DeserializeFromString<Dictionary<string, object>>(s);
            return null;
        }
        public static List<string> DeserializeJsonArray(this string s) {
            if (s.IsJsonArray())
                return JsonSerializer.DeserializeFromString<List<string>>(s);
            return null;
        }
        public static bool IsJsonString(this string s) {
            if (s.IsNullOrWhiteSpace()) return false;
            s = s.Trim();
            return s.StartsWith("{") && s.EndsWith("}");
        }
        public static bool IsJsonArray(this string s) {
            if (s.IsNullOrWhiteSpace()) return false;
            s = s.Trim();
            return s.StartsWith("[") && s.EndsWith("]");
        }
    }
}
