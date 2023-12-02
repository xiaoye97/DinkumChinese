namespace I2LocPatch
{
    public interface IJson
    {
        string ToJson(object obj);

        string ToJson(object obj, bool prettyPrint);

        T FromJson<T>(string json);
    }
}