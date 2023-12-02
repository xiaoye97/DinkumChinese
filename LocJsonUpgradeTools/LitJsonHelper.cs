using LitJson;

namespace I2LocPatch
{
    public class LitJsonHelper : IJson
    {
        public T FromJson<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }

        public string ToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }

        public string ToJson(object obj, bool prettyPrint)
        {
            return JsonMapper.ToJson(obj);
        }
    }
}
