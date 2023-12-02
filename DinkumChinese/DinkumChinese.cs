using TMPro;
using I2.Loc;
using System;
using BepInEx;
using XYModLib;
using I2LocPatch;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Text;
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
        public const string Version = "1.18.0";
        public static DinkumChinesePlugin Inst;

        public static Queue<TextMeshProUGUI> waitShowTMPs = new Queue<TextMeshProUGUI>();

        public List<TextLocData> AnimalsTextLocList = new List<TextLocData>();

        public UIWindow DebugWindow;

        public ConfigEntry<bool> DevMode;

        public ConfigEntry<bool> DontLoadLocOnDevMode;

        public List<TextLocData> DynamicTextLocList = new List<TextLocData>();

        public string ErrorStr;

        public UIWindow ErrorWindow;

        public List<TextLocData> HoverTextLocList = new List<TextLocData>();

        public bool IsPluginLoaded;

        public ConfigEntry<bool> LogNoTranslation;

        public List<TextLocData> MailTextLocList = new List<TextLocData>();

        public List<TextLocData> NPCNameTextLocList = new List<TextLocData>();

        public List<TextLocData> PostTextLocList = new List<TextLocData>();

        public List<TextLocData> QuestTextLocList = new List<TextLocData>();

        public List<TextLocData> TipsTextLocList = new List<TextLocData>();

        private static IJson _json;

        private static bool pause;

        private Vector2 cv;

        private bool isChatHide;

        private int lastChatCount;

        private float showChatCD;

        private float tipsCD = 15;

        public static IJson Json
        {
            get
            {
                if (_json == null)
                {
                    _json = new LitJsonHelper();
                }
                return _json;
            }
        }

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

        public static void LogInfo(string log)
        {
            Inst.Logger.LogInfo(log);
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
                    string log = $"行号:{i + hangOffset} Key:{term.Term} 中的括号数量不一致 英文原文有{mc1.Count}对括号 中文中有{mc2.Count}对括号 原文:{term.Languages[0]} 中文:{term.Languages[3]}";
                    LogInfo(log);
                    sb.AppendLine(log);
                    findCount++;
                }
                else if (mc1.Count > 0)
                {
                    List<string> oriList = new List<string>();
                    List<string> locList = new List<string>();
                    for (int j = 0; j < mc1.Count; j++)
                    {
                        if (!oriList.Contains(mc1[j].Value))
                        {
                            oriList.Add(mc1[j].Value);
                        }
                        if (!locList.Contains(mc2[j].Value))
                        {
                            locList.Add(mc2[j].Value);
                        }
                        //if (mc1[j].Value != mc2[j].Value)
                        //{
                        //    string log = $"行号:{i + hangOffset} Key:{term.Term} 中的第{j}对括号内容不一致 原文中:<{mc1[j].Value}> 翻译中:<{mc2[j].Value}>";
                        //    LogInfo(log);
                        //    sb.AppendLine(log);
                        //    findCount++;
                        //}
                    }
                    for (int j = 0; j < oriList.Count; j++)
                    {
                        string ori = oriList[j];
                        if (!locList.Contains(ori))
                        {
                            string log = $"行号:{i + hangOffset} Key:{term.Term} 中的原文有括号<{ori}>，而译文中不存在";
                            LogInfo(log);
                            sb.AppendLine(log);
                            findCount++;
                        }
                    }
                    for (int j = 0; j < oriList.Count; j++)
                    {
                        string loc = locList[j];
                        if (!oriList.Contains(loc))
                        {
                            string log = $"行号:{i + hangOffset} Key:{term.Term} 中的译文有括号<{loc}>，而原文中不存在";
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

        public void DebugWindowGUI()
        {
            GUILayout.BeginVertical("功能区", GUI.skin.window);
            if (GUILayout.Button("[Ctrl+数字键5] 切换暂停游戏，游戏速度1"))
            {
                Pause = !Pause;
                Time.timeScale = Pause ? 0 : 1;
            }
            if (GUILayout.Button("[Ctrl+数字键6] 切换暂停游戏，游戏速度10"))
            {
                Pause = !Pause;
                Time.timeScale = Pause ? 1 : 10;
            }
            if (GUILayout.Button("检查括号(需要已汉化状态)"))
            {
                CheckKuoHao();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Dump", GUI.skin.window);
            if (GUILayout.Button("[Ctrl+数字键7] dump场景内所有文本，不包括隐藏的文本"))
            {
                DumpTool.DumpText(false);
            }
            if (GUILayout.Button("[Ctrl+数字键8] dump场景内所有文本，包括隐藏的文本"))
            {
                DumpTool.DumpText(true);
            }
            if (GUILayout.Button("一键导出全部原文(需要未汉化状态)"))
            {
                List<string> ignoreTermList = new List<string>();
                var list1 = DumpTool.DumpAllConversationObject();
                var list2 = DumpTool.DumpAllItem();
                ignoreTermList.AddRange(list1);
                ignoreTermList.AddRange(list2);
                I2LocPatchPlugin.Instance.DumpAllLocRes(ignoreTermList);
                DumpTool.DumpAllPost();
                DumpTool.DumpAllQuest();
                DumpTool.DumpAllMail();
                DumpTool.DumpAllTips();
                DumpTool.DumpAnimals();
                DumpTool.DumpNPCNames();
                DumpTool.DumpHoverText();
                DumpTool.DumpInventoryLootTableTimeWeatherMaster_locationName();
                DumpTool.DumpMapIcon();
            }

            GUILayout.EndVertical();
        }

        public void ErrorWindowFunc()
        {
            GUILayout.Label("请注意检查是否有新版本汉化");
            GUILayout.Label("Dinkum汉化交流QQ频道:7opslk1lrt");
            GUILayout.Label(ErrorStr);
        }

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

        public void LogFlagTrue()
        {
            IsPluginLoaded = true;
        }

        /// <summary>
        /// 当游戏开始时只需要一次的处理
        /// </summary>
        public void OnGameStartOnceFix()
        {
            ReplaceNPCNames();
            ReplaceHoverTexts();
        }

        /// <summary>
        /// 替换NPC的名字
        /// </summary>
        public void ReplaceNPCNames()
        {
            List<NPCDetails> coms = new List<NPCDetails>();
            coms.AddRange(Resources.FindObjectsOfTypeAll<NPCDetails>());
            foreach (var com in coms)
            {
                string cnText = TextLocData.GetLoc(NPCNameTextLocList, com.NPCName);
                com.NPCName = cnText;
            }
        }

        public void ReplaceHoverTexts()
        {
            List<HoverToolTipOnButton> coms = new List<HoverToolTipOnButton>();
            coms.AddRange(Resources.FindObjectsOfTypeAll<HoverToolTipOnButton>());
            foreach (var com in coms)
            {
                string cnText = TextLocData.GetLoc(HoverTextLocList, com.hoveringText);
                com.hoveringText = cnText;
            }
        }

        private void Awake()
        {
            Inst = this;
            DevMode = Config.Bind<bool>("Dev", "DevMode", false, "开发模式时，可以按快捷键触发开发功能");
            DontLoadLocOnDevMode = Config.Bind<bool>("Dev", "DontLoadLocOnDevMode", true, "开发模式时，不加载DynamicText Post Quest翻译，方便dump");
            LogNoTranslation = Config.Bind<bool>("Tool", "LogNoTranslation", true, "可以输出没翻译的目标");
            DebugWindow = new UIWindow("汉化测试工具[Ctrl+数字键4] Dinkum汉化交流QQ频道:7opslk1lrt");
            DebugWindow.WindowRect.position = new Vector2(500, 100);
            DebugWindow.OnWinodwGUI = DebugWindowGUI;
            ErrorWindow = new UIWindow($"汉化出现错误 {PluginName} v{Version}");
            ErrorWindow.OnWinodwGUI = ErrorWindowFunc;
            try
            {
                Harmony.CreateAndPatchAll(typeof(OtherPatch));
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
            Invoke("OnGameStartOnceFix", 2f);
            DynamicTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/DynamicTextLoc.csv");
            NPCNameTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/NPCNamesLoc.csv");
            HoverTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/HoverTextLoc.csv");
            PostTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/PostTextLoc.json");
            QuestTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/QuestTextLoc.json");
            TipsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/TipsTextLoc.json");
            MailTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/MailTextLoc.json");
            AnimalsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/AnimalsTextLoc.json");
        }

        private void OnGUI()
        {
            DebugWindow.OnGUI();
            ErrorWindow.OnGUI();
            if (tipsCD > 0)
            {
                GUILayout.Label($"[{(int)tipsCD}s]温馨提示：汉化mod是开源免费的，不需要花钱买，Dinkum汉化交流QQ频道:7opslk1lrt");
            }
        }

        private void Update()
        {
            if (tipsCD > 0)
            {
                tipsCD -= Time.deltaTime;
            }
            if (DevMode.Value)
            {
                // Ctrl + 小键盘4 切换GUI
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha4))
                {
                    DebugWindow.Show = !DebugWindow.Show;
                }
                // Ctrl + 小键盘5 切换暂停游戏，游戏速度1
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha5))
                {
                    Pause = !Pause;
                    Time.timeScale = Pause ? 0 : 1;
                }
                // Ctrl + 小键盘6 切换暂停游戏，游戏速度10
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha6))
                {
                    Pause = !Pause;
                    Time.timeScale = Pause ? 1 : 10;
                }
                // Ctrl + 小键盘7 dump场景内所有文本，不包括隐藏的文本
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha7))
                {
                    DumpTool.DumpText(false);
                }
                // Ctrl + 小键盘8 dump场景内所有文本，包括隐藏的文本
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha8))
                {
                    DumpTool.DumpText(true);
                }
            }
            FixChatFont();
        }
    }
}