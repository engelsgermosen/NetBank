using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NetBank.Core.Application.Helpers
{
    public static class SessionHelper
    {
        public static void Set<T>(this ISession session,string key, T value)
        {
            session.SetString(key,JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session,string key)
        {
            var values = session.GetString(key);
            return values == null ? default : JsonConvert.DeserializeObject<T>(values);
        }
    }
}
