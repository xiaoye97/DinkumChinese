using BepInEx;
using I2.Loc;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using TMPro;

namespace DinkumChinese
{
    [BepInPlugin("xiaoye97.Dinkum.DinkumChinese", "DinkumChinese", "1.0.0")]
    public class DinkumChinesePlugin : BaseUnityPlugin
    {
        public static DinkumChinesePlugin Inst;
        public ModLocalization ModLoc;

        void Awake()
        {
            Inst = this;
            ModLoc = new ModLocalization();
            ModLoc.LoadModLoc();
            Harmony.CreateAndPatchAll(typeof(DinkumChinesePlugin));
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad4))
            {
                CheckKuoHao();
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

        [HarmonyPostfix, HarmonyPatch(typeof(TextMeshProUGUI), "OnEnable")]
        public static void TextMeshProUGUIOnEnablePatch(TextMeshProUGUI __instance)
        {
            ModLocalization.Instance.FixTMPText(__instance);
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
    }
}
