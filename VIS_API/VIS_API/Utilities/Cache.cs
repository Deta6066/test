namespace VIS_API.Utilities
{
    public static class Cache
    {
        private static readonly Dictionary<string, object> _dict = new();

        public static void Set(string key, object value)
        {
            if (_dict.ContainsKey(key))
                _dict[key] = value;
            else
                _dict.Add(key, value);
        }

        public static string? Get(string key)
        {
            if (_dict.ContainsKey(key))
                return _dict[key] as string;
            else
                return null;
        }

        public static T? Get<T>(string key) where T : class
        {
            if (_dict.ContainsKey(key))
                return _dict[key] as T;
            else
                return null;
        }

        public static void Remove(string key)
        {
            _dict.Remove(key);
        }
    }
}
