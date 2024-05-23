using BepInEx;
using I2.Loc;
using I2LocPatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using File = System.IO.File;

namespace DinkumChinese
{
    public static class DumpTool
    {
        /// <summary>
        /// Dump对话
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllConversationObjectDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            List<ConversationObject> conversations = new List<ConversationObject>();
            // 直接从资源搜索单独的Conversation
            conversations.AddRange(Resources.FindObjectsOfTypeAll<ConversationObject>());

            List<string> terms = new List<string>();
            I2File i2File = new I2File();
            i2File.Name = "对话表";
            i2File.Languages = new List<string>() { "English" };

            foreach (var c in conversations)
            {
                // Openings
                if (c.targetOpenings != null && c.targetOpenings.sequence.Length > 0)
                {
                    for (int i = 0; i < c.targetOpenings.sequence.Length; i++)
                    {
                        string term = $"{c.conversationTarget}/{c.name}_Intro_{i.ToString("D3")}";
                        string text = c.targetOpenings.sequence[i].StrToI2Str();
                        dict[term] = text;
                    }
                }
                // Option
                for (int i = 0; i < c.playerOptions.Length; i++)
                {
                    string term = $"{c.conversationTarget}/{c.name}_Option_{i.ToString("D3")}";
                    string text = c.playerOptions[i].StrToI2Str();
                    dict[term] = text;
                }
                // Respone
                for (int i = 0; i < c.targetResponses.Count; i++)
                {
                    var response = c.targetResponses[i];
                    if (response.sequence.Length > 0)
                    {
                        for (int j = 0; j < response.sequence.Length; j++)
                        {
                            string term = $"{c.conversationTarget}/{c.name}_Response_{i.ToString("D3")}_{j.ToString("D3")}";
                            string text = response.sequence[j].StrToI2Str();
                            dict[term] = text;
                        }
                    }
                }
            }
            return dict;
        }

        /// <summary>
        /// Dump对话
        /// </summary>
        /// <returns></returns>
        public static List<string> DumpAllConversationObject()
        {
            List<ConversationObject> conversations = new List<ConversationObject>();
            // 直接从资源搜索单独的Conversation
            conversations.AddRange(Resources.FindObjectsOfTypeAll<ConversationObject>());

            List<string> terms = new List<string>();
            I2File i2File = new I2File();
            i2File.Name = "对话表";
            i2File.Languages = new List<string>() { "English" };

            foreach (var c in conversations)
            {
                // Openings
                if (c.targetOpenings != null && c.targetOpenings.sequence.Length > 0)
                {
                    for (int i = 0; i < c.targetOpenings.sequence.Length; i++)
                    {
                        string term = $"{c.conversationTarget}/{c.name}_Intro_{i.ToString("D3")}";
                        string text = c.targetOpenings.sequence[i];
                        string line = $"{term}\t{text.StrToI2Str()}";
                        if (!string.IsNullOrWhiteSpace(term))
                        {
                            terms.Add(term);
                            TermLine termLine = new TermLine();
                            termLine.Name = term;
                            termLine.Texts = new string[] { text };
                            i2File.Lines.Add(termLine);
                            //LogInfo(line);
                        }
                    }
                }
                // Option
                for (int i = 0; i < c.playerOptions.Length; i++)
                {
                    string term = $"{c.conversationTarget}/{c.name}_Option_{i.ToString("D3")}";
                    string text = c.playerOptions[i];
                    string line = $"{term}\t{text.StrToI2Str()}";
                    if (!string.IsNullOrWhiteSpace(term))
                    {
                        terms.Add(term);
                        TermLine termLine = new TermLine();
                        termLine.Name = term;
                        termLine.Texts = new string[] { text };
                        i2File.Lines.Add(termLine);
                        //LogInfo(line);
                    }
                }
                // Respone
                for (int i = 0; i < c.targetResponses.Count; i++)
                {
                    var response = c.targetResponses[i];
                    if (response.sequence.Length > 0)
                    {
                        for (int j = 0; j < response.sequence.Length; j++)
                        {
                            string term = $"{c.conversationTarget}/{c.name}_Response_{i.ToString("D3")}_{j.ToString("D3")}";
                            string text = response.sequence[j];
                            string line = $"{term}\t{text.StrToI2Str()}";
                            if (!string.IsNullOrWhiteSpace(term))
                            {
                                terms.Add(term);
                                TermLine termLine = new TermLine();
                                termLine.Name = term;
                                termLine.Texts = new string[] { text };
                                i2File.Lines.Add(termLine);
                                //LogInfo(line);
                            }
                        }
                    }
                }
            }
            i2File.WriteCSVTable($"{Paths.GameRootPath}/I2/{i2File.Name}.csv");
            LogInfo($"对话表Dump完毕");
            return terms;
        }

        /// <summary>
        /// Dump物品
        /// </summary>
        /// <returns></returns>
        public static List<string> DumpAllItem()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Key\tEnglish");
            List<string> keys = new List<string>();
            foreach (var item in Inventory.Instance.allItems)
            {
                int id = Inventory.Instance.getInvItemId(item);
                string nameKey = "InventoryItemNames/InvItem_" + id.ToString();
                string descKey = "InventoryItemDescriptions/InvDesc_" + id.ToString();
                string line = nameKey + "\t" + item.itemName;
                //LogInfo(line);
                if (keys.Contains(nameKey))
                {
                    string log = $"出现重复的key {nameKey} 已阻止此项添加";
                    DinkumChinesePlugin.LogInfo(log);
                }
                else
                {
                    keys.Add(nameKey);
                    sb.AppendLine(line);
                }
                line = descKey + "\t" + item.itemDescription;
                //LogInfo(line);
                if (keys.Contains(descKey))
                {
                    string log = $"出现重复的key {descKey} 已阻止此项添加";
                    DinkumChinesePlugin.LogInfo(log);
                }
                else
                {
                    keys.Add(descKey);
                    sb.AppendLine(line);
                }
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/物品表.csv", sb.ToString());
            LogInfo("物品表Dump完毕");
            return keys;
        }

        /// <summary>
        /// Dump邮件
        /// </summary>
        public static void DumpAllMail()
        {
            List<TextLocData> list = new List<TextLocData>();
            foreach (var item in Resources.FindObjectsOfTypeAll<LetterTemplate>())
            {
                list.Add(new TextLocData(item.letterText, ""));
            }
            RemoveDuplicates(list);
            var json = DinkumChinesePlugin.Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/MailTextLoc.json", json);
            LogInfo("Mail表Dump完毕");
            //Debug.Log(json);
        }

        /// <summary>
        /// Dump告示
        /// </summary>
        public static void DumpAllPost()
        {
            List<BullitenBoardPost> list = new List<BullitenBoardPost>();
            list.AddRange(Resources.FindObjectsOfTypeAll<BullitenBoardPost>());
            List<TextLocData> list2 = new List<TextLocData>();
            foreach (var p in list)
            {
                list2.Add(new TextLocData(p.title, ""));
                list2.Add(new TextLocData(p.contentText, ""));
            }
            RemoveDuplicates(list2);
            var json = DinkumChinesePlugin.Json.ToJson(list2, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/PostTextLoc.json", json);
            LogInfo("Post表Dump完毕");
            //Debug.Log(json);
        }

        /// <summary>
        /// Dump任务
        /// </summary>
        public static void DumpAllQuest()
        {
            var mgr = QuestManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            foreach (var q in mgr.allQuests)
            {
                list.Add(new TextLocData(q.QuestName, ""));
                list.Add(new TextLocData(q.QuestDescription, ""));
            }
            RemoveDuplicates(list);
            var json = DinkumChinesePlugin.Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/QuestTextLoc.json", json);
            LogInfo("Quest表Dump完毕");
            //Debug.Log(json);
        }

        public static void DumpAllTips()
        {
            var mgr = GameObject.FindObjectOfType<LoadingScreenImageAndTips>(true);
            List<TextLocData> list = new List<TextLocData>();
            foreach (var tip in mgr.tips) list.Add(new TextLocData(tip, ""));
            RemoveDuplicates(list);
            var json = DinkumChinesePlugin.Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/TipsTextLoc.json", json);
            LogInfo("Tips表Dump完毕");
            //Debug.Log(json);
        }

        public static void DumpAllUnTermItem()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Key\tEnglish");
            List<string> keys = new List<string>();
            foreach (var item in Inventory.Instance.allItems)
            {
                int id = Inventory.Instance.getInvItemId(item);
                string nameKey = "InventoryItemNames/InvItem_" + id.ToString();
                string descKey = "InventoryItemDescriptions/InvDesc_" + id.ToString();
                if (!LocalizationManager.Sources[0].ContainsTerm(nameKey))
                {
                    string line = nameKey + "\t" + item.itemName;
                    LogInfo(line);
                    if (keys.Contains(nameKey))
                    {
                        string log = $"出现重复的key {nameKey} 已阻止此项添加";
                        DinkumChinesePlugin.LogInfo(log);
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
                        DinkumChinesePlugin.LogInfo(log);
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

        public static void DumpAnimals()
        {
            var mgr = AnimalManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            foreach (var a in mgr.allAnimals) list.Add(new TextLocData(a.animalName, ""));
            RemoveDuplicates(list);
            var json = DinkumChinesePlugin.Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/AnimalsTextLoc.json", json);
            LogInfo("Animals表Dump完毕");
            //Debug.Log(json);
        }

        public static void DumpHoverText()
        {
            List<HoverToolTipOnButton> list = new List<HoverToolTipOnButton>();
            list.AddRange(Resources.FindObjectsOfTypeAll<HoverToolTipOnButton>());
            List<string> result = new List<string>();
            foreach (var item in list)
            {
                if (!result.Contains(item.hoveringText))
                {
                    result.Add(item.hoveringText);
                }
            }
            result.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (string text in result)
            {
                sb.AppendLine(text);
            }
            try
            {
                string path = $"{Paths.GameRootPath}/I2/HoverText.csv";
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.WriteAllText(path, sb.ToString());
                LogInfo($"HoverText表Dump完毕");
            }
            catch (Exception ex)
            {
                I2LocPatchPlugin.LogError("写入文件出现异常");
                I2LocPatchPlugin.LogError(ex.ToString());
            }
        }

        public static void DumpNPCNames()
        {
            List<NPCDetails> details = new List<NPCDetails>();
            details.AddRange(Resources.FindObjectsOfTypeAll<NPCDetails>());
            List<string> npcNames = new List<string>();
            foreach (NPCDetails npc in details)
            {
                if (!npcNames.Contains(npc.NPCName))
                {
                    npcNames.Add(npc.NPCName);
                }
                if (npc.randomNames != null && npc.randomNames.Length > 0)
                {
                    foreach (string npcName in npc.randomNames)
                    {
                        if (!npcNames.Contains(npcName))
                        {
                            npcNames.Add(npcName);
                        }
                    }
                }
            }
            npcNames.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (var text in npcNames)
            {
                sb.AppendLine(text);
            }
            try
            {
                string path = $"{Paths.GameRootPath}/I2/NPC姓名表.csv";
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.WriteAllText(path, sb.ToString());
                LogInfo($"NPC姓名表Dump完毕");
            }
            catch (Exception ex)
            {
                I2LocPatchPlugin.LogError("写入文件出现异常");
                I2LocPatchPlugin.LogError(ex.ToString());
            }
        }

        public static void DumpInventoryLootTableTimeWeatherMaster_locationName()
        {
            List<InventoryLootTableTimeWeatherMaster> list = new List<InventoryLootTableTimeWeatherMaster>();
            list.AddRange(Resources.FindObjectsOfTypeAll<InventoryLootTableTimeWeatherMaster>());
            List<string> result = new List<string>();
            foreach (var item in list)
            {
                if (!result.Contains(item.locationName))
                {
                    result.Add(item.locationName);
                }
            }
            result.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (string text in result)
            {
                sb.AppendLine(text);
            }
            try
            {
                string path = $"{Paths.GameRootPath}/I2/LocationNameText.csv";
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.WriteAllText(path, sb.ToString());
                LogInfo($"LocationNameText表Dump完毕");
            }
            catch (Exception ex)
            {
                I2LocPatchPlugin.LogError("写入文件出现异常");
                I2LocPatchPlugin.LogError(ex.ToString());
            }
        }

        public static void DumpMapIcon()
        {
            List<mapIcon> list = new List<mapIcon>();
            list.AddRange(Resources.FindObjectsOfTypeAll<mapIcon>());
            List<string> result = new List<string>();
            foreach (var item in list)
            {
                if (!result.Contains(item.IconName))
                {
                    result.Add(item.IconName);
                }
            }
            result.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (string text in result)
            {
                sb.AppendLine(text);
            }
            try
            {
                string path = $"{Paths.GameRootPath}/I2/MapIconText.csv";
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                File.WriteAllText(path, sb.ToString());
                LogInfo($"MapIconText表Dump完毕");
            }
            catch (Exception ex)
            {
                I2LocPatchPlugin.LogError("写入文件出现异常");
                I2LocPatchPlugin.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Dump当前的文本
        /// </summary>
        /// <param name="includeInactive"></param>
        public static void DumpText(bool includeInactive)
        {
            StringBuilder sb = new StringBuilder();
            var tmps = GameObject.FindObjectsOfType<TextMeshProUGUI>(includeInactive);
            foreach (var tmp in tmps)
            {
                var i2 = tmp.GetComponent<Localize>();
                if (i2 != null) continue;
                sb.AppendLine("===========");
                sb.AppendLine($"path:{tmp.transform.GetPath()}");
                sb.AppendLine($"text:{tmp.text.StrToI2Str()}");
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/TextDump.txt", sb.ToString());
            LogInfo($"Dump完毕,{Paths.GameRootPath}/I2/TextDump.txt");
        }

        public static void LogInfo(string log)
        {
            DinkumChinesePlugin.LogInfo(log);
        }

        public static void RemoveDuplicates(List<TextLocData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                TextLocData tmp = list[i];
                // 去掉空的
                if (string.IsNullOrWhiteSpace(tmp.Ori))
                {
                    list.RemoveAt(i);
                    i--;
                    continue;
                }
                for (int j = i + 1; j < list.Count; j++)
                {
                    TextLocData tmp2 = list[j];
                    if (tmp2.Ori == tmp.Ori)
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
}