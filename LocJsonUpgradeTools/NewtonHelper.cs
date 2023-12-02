using I2LocPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LocJsonUpgradeTools
{
    public class NewtonHelper : IJson
    {
        public T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public string ToJson(object obj, bool prettyPrint)
        {
            return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None);
        }
    }
}
