using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using BepInEx;
using TMPro;
using I2.Loc;
using UnityEngine;

namespace DinkumChinese
{
    /// <summary>
    /// 这里是针对游戏中没有翻译组件的地方进行翻译的处理
    /// </summary>
    public class ModLocalization
    {
        public static ModLocalization Instance;
        public List<ModLocData> LocList = new List<ModLocData>();
        public List<ModLocDataRuntime> LocRuntimeList = new List<ModLocDataRuntime>();

        /// <summary>
        /// 深度0名字集合 所有翻译内容的绑定物体的第0层的名字
        /// </summary>
        public List<string> Depth0NameList = new List<string>();

        public ModLocalization()
        {
            Instance = this;
        }

        /// <summary>
        /// 加载Mod翻译
        /// </summary>
        public void LoadModLoc()
        {
            string path = $"{Paths.PluginPath}/DinkumChinese.json";
            FileInfo fileInfo = new FileInfo(path);
            // 如果文件不存在，则创建一个默认的文件
            if (!fileInfo.Exists)
            {
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                CreateDefault(path);
            }
            else
            {
                string json = File.ReadAllText(path);
                LocList = JsonConvert.DeserializeObject<List<ModLocData>>(json);
                if (LocList == null)
                {
                    CreateDefault(path);
                }
            }
            // 根据LocList生成LocRuntimeList
            foreach (var loc in LocList)
            {
                ModLocDataRuntime runtime = new ModLocDataRuntime();
                runtime.Key = loc.Key;
                if (loc.Bind.Contains("/"))
                {
                    var binds = loc.Bind.Split('/');
                    for (int i = binds.Length - 1; i >= 0; i--)
                    {
                        runtime.Bind.Add(binds[i]);
                    }
                }
                else
                {
                    runtime.Bind.Add(loc.Bind);
                }
                LocRuntimeList.Add(runtime);
            }
            // 生成索引
            foreach (var loc in LocRuntimeList)
            {
                string depth0 = loc.Bind[0];
                if (!Depth0NameList.Contains(depth0))
                {
                    Depth0NameList.Add(depth0);
                }
            }
            //string LocRuntimeListStr = JsonConvert.SerializeObject(LocRuntimeList, Formatting.Indented);
            //string Depth0NameListStr = JsonConvert.SerializeObject(Depth0NameList, Formatting.Indented);
            //Debug.Log($"LocRuntimeList:\n{LocRuntimeListStr}");
            //Debug.Log($"Depth0NameList:\n{Depth0NameListStr}");
        }

        private void CreateDefault(string path)
        {
            LocList = new List<ModLocData>();
            ModLocData data = new ModLocData();
            data.Key = "Key";
            data.Bind = "Root/Title";
            LocList.Add(data);
            string json = JsonConvert.SerializeObject(LocList, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// 检查并处理文本
        /// </summary>
        /// <param name="tmp"></param>
        public void FixTMPText(TextMeshProUGUI tmp)
        {
            // 如果名字不在列表则不处理，节约性能
            if (!Depth0NameList.Contains(tmp.name)) return;
            // 如果此文本已经有翻译组件，则跳过
            if (tmp.GetComponent<Localize>() != null) return;

            bool hasKey = false;
            string key = "";
            // 开始匹配翻译
            foreach (var loc in LocRuntimeList)
            {
                // 第0层匹配的话则继续，否则直接跳过
                if (loc.Bind[0] == tmp.name)
                {
                    // 如果只有一个绑定，则说明已经找到目标
                    if (loc.Bind.Count == 1)
                    {
                        hasKey = true;
                        key = loc.Key;
                        break;
                    }
                    Transform p = tmp.transform.parent;
                    for (int i = 1; i < loc.Bind.Count; i++)
                    {
                        if (p != null && p.name == loc.Bind[i])
                        {
                            p = p.parent;
                            if (i == loc.Bind.Count - 1)
                            {
                                hasKey = true;
                                key = loc.Key;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            if (hasKey)
            {
                //Debug.Log($"对文本【{tmp.text}】添加翻译key:{key}");
                var localize = tmp.gameObject.AddComponent<Localize>();
                localize.SetTerm(key);
            }
        }
    }

    public class ModLocData
    {
        public string Key;
        public string Bind;
    }

    public class ModLocDataRuntime
    {
        public string Key;
        public List<string> Bind = new List<string>();
    }
}