using BepInEx;
using I2.Loc;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using TMPro;
using System.IO;
using BepInEx.Configuration;

namespace DinkumChinese
{
    [BepInPlugin("xiaoye97.Dinkum.DinkumChinese", "DinkumChinese", "1.0.0")]
    public class DinkumChinesePlugin : BaseUnityPlugin
    {
        public static DinkumChinesePlugin Inst;

        public static bool Pause
        {
            get
            {
                return pause;
            }
            set
            {
                pause = value;
            }
        }

        private static bool pause;

        public ConfigEntry<bool> DevMode;

        public List<TextLocData> DynamicTextLocList = new List<TextLocData>();
        public List<TextLocData> PostTextLocList = new List<TextLocData>();

        private void Awake()
        {
            Inst = this;
            DevMode = Config.Bind<bool>("Dev", "DevMode", false, "开发模式时，可以按快捷键触发开发功能");
            Harmony.CreateAndPatchAll(typeof(DinkumChinesePlugin));
            Harmony.CreateAndPatchAll(typeof(ILPatch));
            Harmony.CreateAndPatchAll(typeof(StringReturnPatch));
            DynamicTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/DynamicTextLoc.txt");
            PostTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/PostTextLoc.json");
        }

        private void Update()
        {
            if (DevMode.Value)
            {
                // Ctrl + 小键盘4 检查括号
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad4))
                {
                    CheckKuoHao();
                }
                // Ctrl + 小键盘5 切换暂停游戏，游戏速度1
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad5))
                {
                    Pause = !Pause;
                    Time.timeScale = Pause ? 0 : 1;
                }
                // Ctrl + 小键盘6 切换暂停游戏，游戏速度10
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad6))
                {
                    Pause = !Pause;
                    Time.timeScale = Pause ? 1 : 10;
                }
                // Ctrl + 小键盘7 dump场景内所有文本，不包括隐藏的文本
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad7))
                {
                    DumpText(false);
                }
                // Ctrl + 小键盘8 dump场景内所有文本，包括隐藏的文本
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad8))
                {
                    DumpText(true);
                }
                // Ctrl + 小键盘9 dump所有不在多语言表格内的对话
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad9))
                {
                    DumpAllConversation();
                }
                // Ctrl + 小键盘3 
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad3))
                {
                    DumpAllPost();
                }
            }
        }

        public static void LogInfo(string log)
        {
            Inst.Logger.LogInfo(log);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(OptionsMenu), "Start")]
        public static void OptionsMenuStartPatch()
        {
            LocalizationManager.CurrentLanguage = "Chinese";
        }

        [HarmonyPrefix, HarmonyPatch(typeof(RealWorldTimeLight), "setUpDayAndDate")]
        public static bool RealWorldTimeLight_setUpDayAndDate_Patch(RealWorldTimeLight __instance)
        {
            __instance.seasonAverageTemp = __instance.seasonAverageTemps[WorldManager.manageWorld.month - 1];
            __instance.DayText.text = __instance.getDayName(WorldManager.manageWorld.day - 1);
            __instance.DateText.text = (WorldManager.manageWorld.day + (WorldManager.manageWorld.week - 1) * 7).ToString("00");
            __instance.SeasonText.text = __instance.getSeasonName(WorldManager.manageWorld.month - 1);
            return false;
        }

        public void DumpAllConversation()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Key\tEnglish");
            var cs = Resources.FindObjectsOfTypeAll<Conversation>();
            foreach (var c in cs)
            {
                // Intro
                for (int i = 0; i < c.startLineAlt.aConverstationSequnce.Length; i++)
                {
                    string key = c.getIntroName(i);
                    if (!LocalizationManager.Sources[0].ContainsTerm(key))
                    {
                        if (!string.IsNullOrWhiteSpace(c.startLineAlt.aConverstationSequnce[i]))
                        {
                            sb.AppendLine($"{key}\t{c.startLineAlt.aConverstationSequnce[i].StrToI2Str()}");
                            LogInfo($"Intro:{key}:{c.startLineAlt.aConverstationSequnce[i].StrToI2Str()}");
                        }
                    }
                }
                // Option
                for (int j = 0; j < c.optionNames.Length; j++)
                {
                    if (!c.optionNames[j].Contains("<"))
                    {
                        string key = c.getOptionName(j);
                        if (!LocalizationManager.Sources[0].ContainsTerm(key))
                        {
                            if (!string.IsNullOrWhiteSpace(c.optionNames[j]))
                            {
                                sb.AppendLine($"{key}\t{c.optionNames[j].StrToI2Str()}");
                                LogInfo($"Option:{key}:{c.optionNames[j].StrToI2Str()}");
                            }
                        }
                    }
                }
                // Respone
                for (int k = 0; k < c.responesAlt.Length; k++)
                {
                    for (int l = 0; l < c.responesAlt[k].aConverstationSequnce.Length; l++)
                    {
                        string key = c.getResponseName(k, l);
                        if (!LocalizationManager.Sources[0].ContainsTerm(key))
                        {
                            if (!string.IsNullOrWhiteSpace(c.responesAlt[k].aConverstationSequnce[l]))
                            {
                                sb.AppendLine($"{key}\t{c.responesAlt[k].aConverstationSequnce[l].StrToI2Str()}");
                                LogInfo($"Respone:{key}:{c.responesAlt[k].aConverstationSequnce[l].StrToI2Str()}");
                            }
                        }
                    }
                }
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/NoTermConversation.csv", sb.ToString());
        }

        public void DumpAllPost()
        {
            StringBuilder sb = new StringBuilder();
            List<BullitenBoardPost> list = new List<BullitenBoardPost>();
            list.Add(BulletinBoard.board.announcementPosts[0]);
            list.Add(BulletinBoard.board.huntingTemplate);
            list.Add(BulletinBoard.board.captureTemplate);
            list.Add(BulletinBoard.board.tradeTemplate);
            list.Add(BulletinBoard.board.photoTemplate);
            list.Add(BulletinBoard.board.cookingTemplate);
            list.Add(BulletinBoard.board.smeltingTemplate);
            list.Add(BulletinBoard.board.compostTemplate);
            list.Add(BulletinBoard.board.sateliteTemplate);
            list.Add(BulletinBoard.board.craftingTemplate);
            list.Add(BulletinBoard.board.shippingRequestTemplate);
            sb.AppendLine($"Key\tEnglish");
            //var ps = Resources.FindObjectsOfTypeAll<BullitenBoardPost>();
            foreach (var p in list)
            {
                sb.AppendLine(p.title);
                sb.AppendLine(p.contentText.StrToI2Str());
                LogInfo($"==========");
                LogInfo($"Title:{p.title}");
                LogInfo($"Text:{p.contentText.StrToI2Str()}");
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/Post.csv", sb.ToString());
        }

        /// <summary>
        /// 检查翻译中的括号是否匹配
        /// </summary>
        public void CheckKuoHao()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            // 索引和Excel表中的行对应的偏移
            int hangOffset = 3;
            int findCount = 0;
            StringBuilder sb = new StringBuilder();
            LogInfo($"开始检查翻译中的括号:");
            Regex reg = new Regex(@"(?is)(?<=\<)[^\>]+(?=\>)");
            var mResourcesCache = Traverse.Create(ResourceManager.pInstance).Field("mResourcesCache").GetValue<Dictionary<string, UnityEngine.Object>>();
            LanguageSourceAsset asset = mResourcesCache.Values.First() as LanguageSourceAsset;
            int len = asset.SourceData.mTerms.Count;
            for (int i = 0; i < len; i++)
            {
                var term = asset.SourceData.mTerms[i];
                if (string.IsNullOrWhiteSpace(term.Languages[3])) continue;
                MatchCollection mc1 = reg.Matches(term.Languages[0]);
                MatchCollection mc2 = reg.Matches(term.Languages[3]);
                if (mc1.Count != mc2.Count)
                {
                    string log = $"行号:{i + hangOffset} Key:{term.Term} 中的括号数量不一致 英文原文有{mc1.Count}对括号 中文中有{mc2.Count}对括号";
                    LogInfo(log);
                    sb.AppendLine(log);
                    findCount++;
                }
                else if (mc1.Count > 0)
                {
                    for (int j = 0; j < mc1.Count; j++)
                    {
                        if (mc1[j].Value != mc2[j].Value)
                        {
                            string log = $"行号:{i + hangOffset} Key:{term.Term} 中的第{j}对括号内容不一致 原文中:<{mc1[j].Value}> 翻译中:<{mc2[j].Value}>";
                            LogInfo(log);
                            sb.AppendLine(log);
                            findCount++;
                        }
                    }
                }
            }
            sw.Stop();
            LogInfo($"检查完毕，找到{findCount}个有问题的项，耗时{sw.ElapsedMilliseconds}ms");
            System.IO.File.WriteAllText($"{Paths.GameRootPath}/CheckKuoHao.txt", sb.ToString());
        }

        /// <summary>
        /// Dump当前的文本
        /// </summary>
        /// <param name="includeInactive"></param>
        public void DumpText(bool includeInactive)
        {
            StringBuilder sb = new StringBuilder();
            var tmps = GameObject.FindObjectsOfType<TextMeshProUGUI>(includeInactive);
            foreach (var tmp in tmps)
            {
                var i2 = tmp.GetComponent<Localize>();
                if (i2 != null) continue;
                sb.AppendLine("===========");
                sb.AppendLine($"path:{GetPath(tmp.transform)}");
                sb.AppendLine($"text:{tmp.text.StrToI2Str()}");
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/TextDump.txt", sb.ToString());
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string GetPath(Transform t)
        {
            List<string> paths = new List<string>();
            StringBuilder sb = new StringBuilder();
            paths.Add(t.name);
            Transform p = t.parent;
            while (p != null)
            {
                paths.Add(p.name);
                p = p.parent;
            }
            for (int i = paths.Count - 1; i >= 0; i--)
            {
                sb.Append(paths[i]);
                if (i != 0)
                {
                    sb.Append('/');
                }
            }
            return sb.ToString();
        }
    }

    public static class TextEx
    {
        public static string newLineChar = "þ";

        public static string StrToI2Str(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return str.Replace("\n", newLineChar).Replace("\r", newLineChar);
        }

        public static string I2StrToStr(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return str.Replace(newLineChar, "\n");
        }
    }
}