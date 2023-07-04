using TMPro;
using I2.Loc;
using System;
using BepInEx;
using XYModLib;
using System.IO;
using I2LocPatch;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DinkumChinese
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class DinkumChinesePlugin : BaseUnityPlugin
    {
        public const string GUID = "xiaoye97.Dinkum.DinkumChinese";
        public const string PluginName = "DinkumChinese";
        public const string Version = "1.14.0";
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
        public ConfigEntry<bool> DontLoadLocOnDevMode;
        public ConfigEntry<bool> LogNoTranslation;

        public List<TextLocData> DynamicTextLocList = new List<TextLocData>();
        public List<TextLocData> PostTextLocList = new List<TextLocData>();
        public List<TextLocData> QuestTextLocList = new List<TextLocData>();
        public List<TextLocData> TipsTextLocList = new List<TextLocData>();
        public List<TextLocData> MailTextLocList = new List<TextLocData>();
        public List<TextLocData> AnimalsTextLocList = new List<TextLocData>();

        public UIWindow DebugWindow;
        public UIWindow ErrorWindow;
        public string ErrorStr;
        public bool IsPluginLoaded;

        private void Awake()
        {
            Inst = this;
            DevMode = Config.Bind<bool>("Dev", "DevMode", false, "开发模式时，可以按快捷键触发开发功能");
            DontLoadLocOnDevMode = Config.Bind<bool>("Dev", "DontLoadLocOnDevMode", true, "开发模式时，不加载DynamicText Post Quest翻译，方便dump");
            LogNoTranslation = Config.Bind<bool>("Tool", "LogNoTranslation", true, "可以输出没翻译的目标");
            DebugWindow = new UIWindow("汉化测试工具[Ctrl+小键盘4]");
            DebugWindow.OnWinodwGUI = DebugWindowGUI;
            ErrorWindow = new UIWindow($"汉化出现错误 {PluginName} v{Version}");
            ErrorWindow.OnWinodwGUI = ErrorWindowFunc;
            try
            {
                Harmony.CreateAndPatchAll(typeof(DinkumChinesePlugin));
                Harmony.CreateAndPatchAll(typeof(ILPatch));
                Harmony.CreateAndPatchAll(typeof(StringReturnPatch));
                Harmony.CreateAndPatchAll(typeof(StartTranslatePatch));
                Harmony.CreateAndPatchAll(typeof(SpritePatch));
            }
            catch (ExecutionEngineException ex)
            {
                ErrorStr = $"汉化出现错误。推测是由于用户名或者游戏路径中包含非英文字符导致。\n异常信息:\n{ex}";
                ErrorWindow.Show = true;
            }
            catch (Exception ex)
            {
                ErrorStr = $"汉化出现错误。\n异常信息:\n{ex}";
                ErrorWindow.Show = true;
            }
            if (DevMode.Value && DontLoadLocOnDevMode.Value)
            {
                return;
            }
            Invoke("LogFlagTrue", 2f);
            DynamicTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/DynamicTextLoc.txt");
            PostTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/PostTextLoc.json");
            QuestTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/QuestTextLoc.json");
            TipsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/TipsTextLoc.json");
            MailTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/MailTextLoc.json");
            AnimalsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/AnimalsTextLoc.json");
        }

        public void LogFlagTrue()
        {
            IsPluginLoaded = true;
        }

        public void ErrorWindowFunc()
        {
            GUILayout.Label("请注意检查是否有新版本汉化");
            GUILayout.Label(ErrorStr);
        }

        private void Start()
        {
            OnGameStartOnceFix();
        }

        private void Update()
        {
            if (DevMode.Value)
            {
                // Ctrl + 小键盘4 切换GUI
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad4))
                {
                    DebugWindow.Show = !DebugWindow.Show;
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
            }
            FixChatFont();
        }

        private void OnGUI()
        {
            DebugWindow.OnGUI();
            ErrorWindow.OnGUI();
        }

        private Vector2 cv;

        public void DebugWindowGUI()
        {
            GUILayout.BeginVertical("功能区", GUI.skin.window);
            if (GUILayout.Button("[Ctrl+小键盘5] 切换暂停游戏，游戏速度1"))
            {
                Pause = !Pause;
                Time.timeScale = Pause ? 0 : 1;
            }
            if (GUILayout.Button("[Ctrl+小键盘6] 切换暂停游戏，游戏速度10"))
            {
                Pause = !Pause;
                Time.timeScale = Pause ? 1 : 10;
            }
            if (GUILayout.Button("检查括号"))
            {
                CheckKuoHao();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Dump", GUI.skin.window);
            if (GUILayout.Button("[Ctrl+小键盘7] dump场景内所有文本，不包括隐藏的文本"))
            {
                DumpText(false);
            }
            if (GUILayout.Button("[Ctrl+小键盘8] dump场景内所有文本，包括隐藏的文本"))
            {
                DumpText(true);
            }
            if (GUILayout.Button("dump所有不在多语言表格内的对话(需要未汉化状态)"))
            {
                DumpAllConversation();
            }
            if (GUILayout.Button("dump post(需要未汉化状态)"))
            {
                DumpAllPost();
            }
            if (GUILayout.Button("dump quest(需要未汉化状态)"))
            {
                DumpAllQuest();
            }
            if (GUILayout.Button("dump mail(需要未汉化状态)"))
            {
                DumpAllMail();
            }
            if (GUILayout.Button("dump tips(需要未汉化状态)"))
            {
                DumpAllTips();
            }
            if (GUILayout.Button("dump animals(需要未汉化状态)"))
            {
                DumpAnimals();
            }
            if (GUILayout.Button("dump没翻译key的物品(需要未汉化状态)"))
            {
                DumpAllUnTermItem();
            }
            GUILayout.EndVertical();
        }

        private int lastChatCount;
        private bool isChatHide;
        private float showChatCD;

        public void FixChatFont()
        {
            if (ChatBox.chat != null)
            {
                if (isChatHide)
                {
                    showChatCD -= Time.deltaTime;
                    if (showChatCD < 0)
                    {
                        isChatHide = false;
                        foreach (var chat in ChatBox.chat.chatLog)
                        {
                            chat.contents.enabled = false;
                            chat.contents.enabled = true;
                        }
                    }
                }
                if (ChatBox.chat.chatLog.Count != lastChatCount)
                {
                    lastChatCount = ChatBox.chat.chatLog.Count;
                    isChatHide = true;
                    showChatCD = 0.5f;
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

        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getIntroName")]
        public static bool Conversation_getIntroName(Conversation __instance, ref string __result, int i)
        {
            if (Inst.DevMode.Value && Inst.DontLoadLocOnDevMode.Value) return true;
            string result = $"{__instance.saidBy}/{__instance.gameObject.name}_Intro_{i.ToString("D3")}";
            __result = result;
            if (!LocalizationManager.Sources[0].ContainsTerm(result))
            {
                if (__instance.startLineAlt.aConverstationSequnce.Length > i)
                {
                    if (string.IsNullOrWhiteSpace(__instance.startLineAlt.aConverstationSequnce[i]))
                    {
                        __result = result;
                    }
                    else
                    {
                        __result = result + "_" + __instance.startLineAlt.aConverstationSequnce[i].GetHashCode();
                    }
                }
            }
            if (Inst.DevMode.Value)
                Debug.Log($"Conversation_getIntroName {__result}");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getOptionName")]
        public static bool Conversation_getOptionName(Conversation __instance, ref string __result, int i)
        {
            if (Inst.DevMode.Value && Inst.DontLoadLocOnDevMode.Value) return true;
            string result = $"{__instance.saidBy}/{__instance.gameObject.name}_Option_{i.ToString("D3")}";
            __result = result;
            if (!LocalizationManager.Sources[0].ContainsTerm(result))
            {
                if (__instance.optionNames.Length > i)
                {
                    if (string.IsNullOrWhiteSpace(__instance.optionNames[i]))
                    {
                        __result = result;
                    }
                    else
                    {
                        __result = result + "_" + __instance.optionNames[i].GetHashCode();
                    }
                }
            }
            if (Inst.DevMode.Value)
                Debug.Log($"Conversation_getOptionName {__result}");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getResponseName")]
        public static bool Conversation_getResponseName(Conversation __instance, ref string __result, int i, int r)
        {
            if (Inst.DevMode.Value && Inst.DontLoadLocOnDevMode.Value) return true;
            string result = $"{__instance.saidBy}/{__instance.gameObject.name}_Response_{i.ToString("D3")}_{r.ToString("D3")}";
            __result = result;
            if (!LocalizationManager.Sources[0].ContainsTerm(result))
            {
                if (__instance.responesAlt.Length > i)
                {
                    if (__instance.responesAlt[i].aConverstationSequnce.Length > r)
                    {
                        if (string.IsNullOrWhiteSpace(__instance.responesAlt[i].aConverstationSequnce[r]))
                        {
                            __result = result;
                        }
                        else
                        {
                            __result = result + "_" + __instance.responesAlt[i].aConverstationSequnce[r].GetHashCode();
                        }
                    }
                }
            }
            if (Inst.DevMode.Value)
                Debug.Log($"Conversation_getResponseName {__result}");
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LocalizationManager), "TryGetTranslation")]
        public static void Localize_OnLocalize(string Term, bool __result)
        {
            if (Inst.IsPluginLoaded && Inst.LogNoTranslation.Value)
            {
                if (!__result)
                {
                    Debug.LogWarning($"LocalizationManager获取翻译失败:Term:{Term}");
                }
            }
        }

        public static Queue<TextMeshProUGUI> waitShowTMPs = new Queue<TextMeshProUGUI>();

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
                    string log = $"行号:{i + hangOffset} Key:{term.Term} 中的括号数量不一致 英文原文有{mc1.Count}对括号 中文中有{mc2.Count}对括号 原文:{term.Languages[0]} 中文:{term.Languages[3]}";
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

        /// <summary>
        /// 当游戏开始时只需要一次的处理
        /// </summary>
        public void OnGameStartOnceFix()
        {
            // 动物的生物群系翻译
            //AnimalManager.manage.northernOceanFish.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.northernOceanFish.locationName);
            //AnimalManager.manage.southernOceanFish.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.southernOceanFish.locationName);
            //AnimalManager.manage.riverFish.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.riverFish.locationName);
            //AnimalManager.manage.mangroveFish.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.mangroveFish.locationName);
            //AnimalManager.manage.billabongFish.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.billabongFish.locationName);
            //AnimalManager.manage.topicalBugs.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.topicalBugs.locationName);
            //AnimalManager.manage.desertBugs.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.desertBugs.locationName);
            //AnimalManager.manage.bushlandBugs.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.bushlandBugs.locationName);
            //AnimalManager.manage.pineLandBugs.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.pineLandBugs.locationName);
            //AnimalManager.manage.plainsBugs.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.plainsBugs.locationName);
            //AnimalManager.manage.underWaterOceanCreatures.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.underWaterOceanCreatures.locationName);
            //AnimalManager.manage.underWaterRiverCreatures.locationName =
            //    TextLocData.GetLoc(DynamicTextLocList, AnimalManager.manage.underWaterRiverCreatures.locationName);
        }

        #region Dump

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
            LogInfo($"Dump完毕,{Paths.GameRootPath}/I2/TextDump.txt");
        }

        public void DumpAllConversation()
        {
            List<Conversation> conversations = new List<Conversation>();
            // 直接从资源搜索单独的Conversation
            conversations.AddRange(Resources.FindObjectsOfTypeAll<Conversation>());

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"Key\tEnglish");
            List<string> terms = new List<string>();
            I2File i2File = new I2File();
            i2File.Name = "NoTermConversation";
            i2File.Languages = new List<string>() { "English" };

            foreach (var c in conversations)
            {
                // Intro
                for (int i = 0; i < c.startLineAlt.aConverstationSequnce.Length; i++)
                {
                    string key = c.getIntroName(i);
                    if (!LocalizationManager.Sources[0].ContainsTerm(key))
                    {
                        if (!string.IsNullOrWhiteSpace(c.startLineAlt.aConverstationSequnce[i]))
                        {
                            string term = $"{key}_{c.startLineAlt.aConverstationSequnce[i].GetHashCode()}";
                            string line = $"{term}\t{c.startLineAlt.aConverstationSequnce[i].StrToI2Str()}";
                            if (terms.Contains(term))
                            {
                                string log = $"重复的term，忽略。{line}";
                                Logger.LogError(log);
                            }
                            else
                            {
                                terms.Add(term);
                                TermLine termLine = new TermLine();
                                termLine.Name = term;
                                termLine.Texts = new string[] { c.startLineAlt.aConverstationSequnce[i] };
                                i2File.Lines.Add(termLine);
                                //sb.AppendLine(line);
                                LogInfo(line);
                            }
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
                                string term = $"{key}_{c.optionNames[j].GetHashCode()}";
                                string line = $"{term}\t{c.optionNames[j].StrToI2Str()}";
                                if (terms.Contains(term))
                                {
                                    string log = $"重复的term，忽略。{line}";
                                    Logger.LogError(log);
                                }
                                else
                                {
                                    terms.Add(term);
                                    //sb.AppendLine(line);
                                    TermLine termLine = new TermLine();
                                    termLine.Name = term;
                                    termLine.Texts = new string[] { c.optionNames[j] };
                                    i2File.Lines.Add(termLine);
                                    LogInfo(line);
                                }
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
                                string term = $"{key}_{c.responesAlt[k].aConverstationSequnce[l].GetHashCode()}";
                                string line = $"{term}\t{c.responesAlt[k].aConverstationSequnce[l].StrToI2Str()}";
                                if (terms.Contains(term))
                                {
                                    string log = $"重复的term，忽略。{line}";
                                    Logger.LogError(log);
                                }
                                else
                                {
                                    terms.Add(term);
                                    //sb.AppendLine(line);
                                    TermLine termLine = new TermLine();
                                    termLine.Name = term;
                                    termLine.Texts = new string[] { c.responesAlt[k].aConverstationSequnce[l] };
                                    i2File.Lines.Add(termLine);
                                    LogInfo(line);
                                }
                            }
                        }
                    }
                }
            }
            i2File.WriteCSVTable($"{Paths.GameRootPath}/I2/{i2File.Name}.csv");
            LogInfo($"Dump {i2File.Name}完毕");
        }

        public void DumpAllPost()
        {
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
            List<TextLocData> list2 = new List<TextLocData>();
            foreach (var p in list)
            {
                list2.Add(new TextLocData(p.title, ""));
                list2.Add(new TextLocData(p.contentText, ""));
            }
            var json = JsonConvert.SerializeObject(list2, Formatting.Indented);
            File.WriteAllText($"{Paths.GameRootPath}/I2/PostTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllQuest()
        {
            var mgr = QuestManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            foreach (var q in mgr.allQuests)
            {
                list.Add(new TextLocData(q.QuestName, ""));
                list.Add(new TextLocData(q.QuestDescription, ""));
            }
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText($"{Paths.GameRootPath}/I2/QuestTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllMail()
        {
            var mgr = MailManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            list.Add(new TextLocData(mgr.animalResearchLetter.letterText, ""));
            list.Add(new TextLocData(mgr.returnTrapLetter.letterText, ""));
            list.Add(new TextLocData(mgr.devLetter.letterText, ""));
            list.Add(new TextLocData(mgr.catalogueItemLetter.letterText, ""));
            list.Add(new TextLocData(mgr.craftmanDayOff.letterText, ""));
            foreach (var m in mgr.randomLetters) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.thankYouLetters) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.didNotFitInInvLetter) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.fishingTips) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.bugTips) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.licenceLevelUp) list.Add(new TextLocData(m.letterText, ""));
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText($"{Paths.GameRootPath}/I2/MailTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllTips()
        {
            var mgr = GameObject.FindObjectOfType<LoadingScreenImageAndTips>(true);
            List<TextLocData> list = new List<TextLocData>();
            foreach (var tip in mgr.tips) list.Add(new TextLocData(tip, ""));
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText($"{Paths.GameRootPath}/I2/TipsTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAnimals()
        {
            var mgr = AnimalManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            foreach (var a in mgr.allAnimals) list.Add(new TextLocData(a.animalName, ""));
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText($"{Paths.GameRootPath}/I2/AnimalsTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllUnTermItem()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Key\tEnglish");
            List<string> keys = new List<string>();
            foreach (var item in Inventory.inv.allItems)
            {
                int id = Inventory.inv.getInvItemId(item);
                string nameKey = "InventoryItemNames/InvItem_" + id.ToString();
                string descKey = "InventoryItemDescriptions/InvDesc_" + id.ToString();
                if (!LocalizationManager.Sources[0].ContainsTerm(nameKey))
                {
                    string line = nameKey + "\t" + item.itemName;
                    LogInfo(line);
                    if (keys.Contains(nameKey))
                    {
                        string log = $"出现重复的key {nameKey} 已阻止此项添加";
                        Logger.LogError(log);
                    }
                    else
                    {
                        keys.Add(nameKey);
                        sb.AppendLine(line);
                    }
                }
                if (!LocalizationManager.Sources[0].ContainsTerm(descKey))
                {
                    string line = descKey + "\t" + item.itemDescription;
                    LogInfo(line);
                    if (keys.Contains(descKey))
                    {
                        string log = $"出现重复的key {descKey} 已阻止此项添加";
                        Logger.LogError(log);
                    }
                    else
                    {
                        keys.Add(descKey);
                        sb.AppendLine(line);
                    }
                }
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/UnTermItem.csv", sb.ToString());
        }

        #endregion Dump
    }
}