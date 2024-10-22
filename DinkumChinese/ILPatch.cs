using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace DinkumChinese
{
    public static class ILPatch
    {
        [HarmonyTranspiler, HarmonyPatch(typeof(AnimalHouseMenu), "fillData")]
        public static IEnumerable<CodeInstruction> AnimalHouseMenu_fillData_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, " Year", " 年");
            instructions = RIL(instructions, " Month", " 月");
            instructions = RIL(instructions, " Day", " 日");
            instructions = RIL(instructions, "s", "");
            instructions = RIL(instructions, "SELL ", "出售 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(AnimalHouseMenu), "openConfirm")]
        public static IEnumerable<CodeInstruction> AnimalHouseMenu_openConfirm_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Sell ", "出售 ");
            instructions = RIL(instructions, " for <sprite=11>", " 以 <sprite=11>");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BankMenu), "convertButton")]
        public static IEnumerable<CodeInstruction> BankMenu_convertButton_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, " [<sprite=11> 500 for <sprite=15> 1]", " [<sprite=11> 500 到 <sprite=15> 1]");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BulletinBoard), "getMissionText")]
        public static IEnumerable<CodeInstruction> BulletinBoard_getMissionText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "<sprite=12> Trade ", "<sprite=12> 交换");
            instructions = RIL(instructions, " with ", "与");
            instructions = RIL(instructions, "<sprite=12> Speak to ", "<sprite=12> 告诉");
            instructions = RIL(instructions, "<sprite=12> Hunt down the ", "<sprite=12> 狩猎");
            instructions = RIL(instructions, " using its last know location on the map", "使用地图上最后知道的位置");
            instructions = RIL(instructions, "<sprite=13> Visit the location on the map to investigate", "<sprite=13> 访问地图上的位置进行调查");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BulletinBoard), "showSelectedPost")]
        public static IEnumerable<CodeInstruction> BulletinBoard_showSelectedPost_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "EXPIRED", "过期");
            instructions = RIL(instructions, " Last Day", "最后一天");
            instructions = RIL(instructions, " Days Remaining", " 天剩余");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BullitenBoardPost), "getBoardRequestItem")]
        public static IEnumerable<CodeInstruction> BullitenBoardPost_getBoardRequestItem_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "any other furniture", "一件家具");
            instructions = RIL(instructions, "any other clothing", "一件衣服");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(TownEventManager), "RunIslandDay", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> TownEventManager_RunIslandDay_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "It's ", "");
            instructions = RIL(instructions, " Day!", "日!");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ConversationManager), "CheckLineForReplacement")]
        public static IEnumerable<CodeInstruction> ConversationManager_checkLineForReplacement_Patch(IEnumerable<CodeInstruction> instructions)
        {
            //instructions = RIL(instructions, "South City", "南部城市");
            instructions = RIL(instructions, "Animal Type", "动物类型");
            instructions = RIL(instructions, "Animal Name", "动物名字");
            instructions = RIL(instructions, "Journal", "日记");
            instructions = RIL(instructions, "Licence", "许可证");
            instructions = RIL(instructions, "Licences", "许可证");
            instructions = RIL(instructions, "Shift", "转移");
            instructions = RIL(instructions, "Shifts", "转移");
            instructions = RIL(instructions, "Airship", "飞艇");
            instructions = RIL(instructions, "Airships", "飞艇");
            instructions = RIL(instructions, "Nomad", "游商");
            instructions = RIL(instructions, "Nomads", "游商");
            instructions = RIL(instructions, "Birthday", "生日");
            instructions = RIL(instructions, "Permit Points", "许可点数");
            instructions = RIL(instructions, "Snag Sizzles", "烤肠");
            instructions = RIL(instructions, "Snag Sizzle", "烤肠");
            instructions = RIL(instructions, "Snag", "香肠");
            instructions = RIL(instructions, "Snags", "香肠");
            //instructions = RIL(instructions, " Day", "日");
            //instructions = RIL(instructions, " Year", "年");
            instructions = RIL(instructions, "reward", "奖励");
            instructions = RIL(instructions, "Animal", "动物");
            instructions = RIL(instructions, "I just love the colours!", "我只是喜欢这些颜色！");
            instructions = RIL(instructions, "I love this one.", "我喜欢这个。");
            instructions = RIL(instructions, "The composition is wonderful", "这组合很奇妙");
            instructions = RIL(instructions, "It speaks to me, you know?", "它在跟我说话，你知道吗？");
            instructions = RIL(instructions, "It makes me feel something...", "它让我有点感觉...");
            instructions = RIL(instructions, "Made by hand by yours truly!", "真正由您亲手制作！");
            instructions = RIL(instructions, "Finished that one off today!", "今天完成了一个！");
            instructions = RIL(instructions, "It feels just right for you, ", "感觉很适合你");
            instructions = RIL(instructions, "The colour is very powerful, y'know?", "颜色很强大，你知道吗？");
            instructions = RIL(instructions, "It will open your chakras, y'know?", "它会打通你的血脉，你知道吗？");
            instructions = RIL(instructions, "Do you feel the energy coming from it?", "你感受到来自它的能量了吗？");
            instructions = RIL(instructions, "I feel good things coming to whoever buys it.", "我觉得买它的人都会有好东西。");
            instructions = RIL(instructions, "The design just came to me, y'know?", "刚想到的设计，你知道吗？");
            instructions = RIL(instructions, "Y'know, that would look great on you, ", "你知道，这对你来说很好看，");
            instructions = RIL(instructions, "I put a lot of myself into this one.", "我把很多心血都投入到了这上面。");
            //instructions = RIL(instructions, "darl", "伙计");
            //instructions = RIL(instructions, "love", "朋友");
            //instructions = RIL(instructions, "possum", "亲爱的");
            instructions = RIL(instructions, "Bug Catching Comp", "捕虫大赛");
            instructions = RIL(instructions, "Fishing Comp", "钓鱼大赛");
            instructions = RIL(instructions, "Comp Log Book", "比赛手册");
            instructions = RIL(instructions, "Beginning...", "开始...");
            instructions = RIL(instructions, "...Nothing happened...", "...没事发生...");
            instructions = RIL(instructions, "s", "");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ExhibitSign), "Start")]
        public static IEnumerable<CodeInstruction> ExhibitSign_Start_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "This exhibit is currently empty.", "此展览目前为空。");
            instructions = RIL(instructions, "We look forward to future donations!", "我们期待未来的捐赠！");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ExhibitSign), "updateMySign")]
        public static IEnumerable<CodeInstruction> ExhibitSign_updateMySign_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "In this exhibit:", "展览的生物有：");
            return instructions;
        }

        // todo
        [HarmonyTranspiler, HarmonyPatch(typeof(FadeBlackness), "fadeInDateText", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> FadeBlackness_fadeInDateText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Year ", "年份 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GiveNPC), "UpdateMenu", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> GiveNPC_UpdateMenu_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Place", "放置");
            instructions = RIL(instructions, "Donate", "捐赠");
            instructions = RIL(instructions, "Sell", "出售");
            instructions = RIL(instructions, "Cancel", "取消");
            instructions = RIL(instructions, "Swap", "交换");
            instructions = RIL(instructions, "Give", "给予");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(InventoryItemDescription), "fillItemDescription")]
        public static IEnumerable<CodeInstruction> InventoryItemDescription_fillItemDescription_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "All year", "全年");
            instructions = RIL(instructions, "Summer", "夏天");
            instructions = RIL(instructions, "Autumn", "秋天");
            instructions = RIL(instructions, "Winter", "冬天");
            instructions = RIL(instructions, "Spring", "春天");
            instructions = RIL(instructions, "Bury", "掩埋");
            instructions = RIL(instructions, "Speeds up certain production devices for up to 14 tiles", "加快某些生产设备的速度，半径范围14格子");
            instructions = RIL(instructions, "Speeds up certain production devices for up to 8 tiles", "加快某些生产设备的速度，半径范围8格子");
            instructions = RIL(instructions, "Can pull materials from storage boxes 5 tiles out", "可以从5格内的箱中取材料");
            instructions = RIL(instructions, "Will try and auto stack materials in storage boxes 10 tiles out", "将尝试自动将材料放入10格内的箱中");
            instructions = RIL(instructions, "Reaches ", "灌溉范围半径");
            instructions = RIL(instructions, " tiles out.\n<color=red>Requires Water Tank</color>", "格。\n<color=red>需要水箱</color>");
            instructions = RIL(instructions, "Provides water to sprinklers ", "向");
            instructions = RIL(instructions, " tiles out.", "格半径内的洒水器提供水源。");
            instructions = RIL(instructions, "Fills animal feeders ", "填充动物饲料 半径范围");
            instructions = RIL(instructions, " tiles out.\n<color=red>Requires Animal Food</color>", "格。\n<color=red>需要动物饲料</color>");
            instructions = RIL(instructions, " x Tiles Wide", "x半径范围");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(MailManager), "getSentByName")]
        public static IEnumerable<CodeInstruction> MailManager_getSentByName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Animal Research Centre", "动物研究中心");
            instructions = RIL(instructions, "Dinkum Dev", "Dinkum开发者");
            instructions = RIL(instructions, "Fishin' Tipster", "钓鱼指引者");
            instructions = RIL(instructions, "Bug Tipster", "捕虫指引者");
            instructions = RIL(instructions, "Unknown", "未知");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(MailManager), "showLetter")]
        public static IEnumerable<CodeInstruction> MailManager_showLetter_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "From ", "来自");
            instructions = RIL(instructions, "<size=18><b>To ", "<size=18><b>致");
            instructions = RIL(instructions, "\n\n<size=18><b>From ", "\n\n<size=18><b>来自");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "getDesiredItemName")]
        public static IEnumerable<CodeInstruction> NPCRequest_getDesiredItemName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "any bug", "任意昆虫");
            instructions = RIL(instructions, "any fish", "任意鱼类");
            instructions = RIL(instructions, "something to eat", "一些食物");
            instructions = RIL(instructions, "something you've made me at a cooking table", "一些你在烹饪台上做的食物");
            instructions = RIL(instructions, "furniture", "家具");
            instructions = RIL(instructions, "clothing", "衣服");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "getDesiredItemNameByNumber")]
        public static IEnumerable<CodeInstruction> NPCRequest_getDesiredItemNameByNumber_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "a ", "");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "getMissionText")]
        public static IEnumerable<CodeInstruction> NPCRequest_getMissionText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "<sprite=12> Bring ", "<sprite=12> 带给 ");
            instructions = RIL(instructions, "<sprite=13> Collect ", "<sprite=13> 收集 ");
            instructions = RIL(instructions, "\n<sprite=12> Bring ", "\n<sprite=12> 带给 ");
            instructions = RIL(instructions, "<sprite=12> Collect ", "<sprite=12> 收集 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCSchedual), "getDaysClosed")]
        public static IEnumerable<CodeInstruction> NPCSchedual_getDaysClosed_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Closed: ", "关门时间: ");
            instructions = RIL(instructions, " and ", " 和 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCSchedual), "getNextDayOffName")]
        public static IEnumerable<CodeInstruction> NPCSchedual_getNextDayOffName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "No Day off", "没有休息日");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCSchedual), "getOpeningHours")]
        public static IEnumerable<CodeInstruction> NPCSchedual_getOpeningHours_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Open: ", "开门时间: ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PlayerDetailManager), "switchToLevelWindow")]
        public static IEnumerable<CodeInstruction> PlayerDetailManager_switchToLevelWindow_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Resident for: ", "居住:");
            instructions = RIL(instructions, " days", " 天");
            instructions = RIL(instructions, " months", " 月");
            instructions = RIL(instructions, " years", " 年");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PostOnBoard), "getPostedByName")]
        public static IEnumerable<CodeInstruction> PostOnBoard_getPostedByName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Town Announcement", "城镇公告");
            instructions = RIL(instructions, "Animal Research Centre", "动物研究中心");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Quest), "getMissionObjText")]
        public static IEnumerable<CodeInstruction> Quest_getMissionObjText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "<sprite=12> Attract a total of 5 permanent residents to move to ", "<sprite=12> 总共吸引5名常住居民搬到");
            instructions = RIL(instructions, "<sprite=12> Talk to ", "\n<sprite=12> 对话");
            instructions = RIL(instructions, " once the Base Tent has been moved", "在移动基地帐篷后");
            instructions = RIL(instructions, "<sprite=13> Craft a ", "<sprite=13> 制作一个");
            instructions = RIL(instructions, " at the crafting table in the Base Tent.\n<sprite=13> Place the  ", "在基地帐篷里的制作桌子上。\n<sprite=13> 放置");
            instructions = RIL(instructions, " down outside.\n<sprite=13> Place Tin Ore into ", "在地上。\n<sprite=13> 将锡矿放入");
            instructions = RIL(instructions, " and wait for it to become ", "并等待它熔炼成");
            instructions = RIL(instructions, ".\n<sprite=12> Take the ", "。\n<sprite=12> 拿着");
            instructions = RIL(instructions, " to ", "到");
            instructions = RIL(instructions, " down outside.\n<sprite=12> Place Tin Ore into ", "。\n<sprite=12> 放置");
            instructions = RIL(instructions, " at the crafting table in the Base Tent.\n<sprite=12> Place the  ", "在基础帐篷的制作桌上。\n<sprite=12> 放置");
            instructions = RIL(instructions, "<sprite=12> Craft a ", "<sprite=12> 制作一个");
            instructions = RIL(instructions, "<sprite=13> Buy the ", "<sprite=13> 购买");
            instructions = RIL(instructions, "\n<sprite=12> Talk to ", "\n<sprite=12> 对话");
            instructions = RIL(instructions, "<sprite=12> Buy the ", "<sprite=12> 购买");
            instructions = RIL(instructions, "[Optional] Complete Daily tasks\n<sprite=12> Place sleeping bag and get some rest.", "[可选]完成日常任务\n<sprite=12>放置睡袋并休息。");
            instructions = RIL(instructions, "<sprite=13> Find something to eat.\n<sprite=12> Talk to ", "<sprite=13> 找点吃的。\n<sprite=12> 对话");
            instructions = RIL(instructions, "<sprite=12> Find something to eat.\n<sprite=12> Talk to ", "<sprite=12> 找点吃的。\n<sprite=12> 对话");
            instructions = RIL(instructions, "<sprite=13> Collect the requested items.\n<sprite=12> Bring items to ", "<sprite=13> 收集请求的物品。\n<sprite=12> 将物品带到");
            instructions = RIL(instructions, "<sprite=12> Collect the requested items.", "<sprite=12> 收集请求的物品。");
            instructions = RIL(instructions, "\n<sprite=12> Bring items to ", "\n<sprite=12> 将物品带到");
            instructions = RIL(instructions, "<sprite=12> Do some favours for John", "<sprite=12> 帮助约翰");
            instructions = RIL(instructions, "<sprite=13> Do some favours for John", "<sprite=13> 帮助约翰");
            instructions = RIL(instructions, "\n<sprite=12> Spend money or sell items in John's store", "\n<sprite=12> 在约翰的店里花钱或卖东西");
            instructions = RIL(instructions, "\n<sprite=13> Spend money or sell items in John's store", "\n<sprite=13> 在约翰的店里花钱或卖东西");
            instructions = RIL(instructions, "\n<sprite=12> Convince John to move in.", "\n<sprite=12> 说服约翰留下来。");
            instructions = RIL(instructions, "<sprite=12> Ask ", "<sprite=12> 询问");
            instructions = RIL(instructions, " about the town to apply for the ", "关于城镇申请");
            instructions = RIL(instructions, "<sprite=12> Place the ", "<sprite=12> 放置");
            instructions = RIL(instructions, "<sprite=12> Wait for construction of the  ", "<sprite=12> 等待");
            instructions = RIL(instructions, " to be completed", "的施工完成");
            instructions = RIL(instructions, "<sprite=12> Place the required items into the construction box at the deed site", "<sprite=12> 将所需物品放入契约现场的材料箱中");
            instructions = RIL(instructions, "<sprite=12> Place ", "<sprite=12> 放置");
            instructions = RIL(instructions, "<sprite=13> Place ", "<sprite=13> 放置");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayQuest")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayQuest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, " days remaining", " 天剩余");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayRequest")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayRequest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Request for ", "请求 来自");
            instructions = RIL(instructions, " has asked you to get ", "想向你要");
            instructions = RIL(instructions, "By the end of the day", "在今天结束之前");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayTrackingRecipe")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayTrackingRecipe_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, " Recipe", " 配方");
            instructions = RIL(instructions, " Construction Requirements", " 制作条件");
            instructions = RIL(instructions, "These items required to start construction on ", "开始制作");
            instructions = RIL(instructions, "These items are required to craft ", "制作");
            instructions = RIL(instructions, "\nUnpin this to stop tracking recipe.", "需要这些物品。\n取消固定来停止跟踪配方");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "fillMissionTextForRecipe")]
        public static IEnumerable<CodeInstruction> QuestTracker_fillMissionTextForRecipe_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Crafting ", "制作 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "pressPinRecipeButton")]
        public static IEnumerable<CodeInstruction> QuestTracker_pressPinRecipeButton_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, " Recipe", " 配方");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updatePinnedTask")]
        public static IEnumerable<CodeInstruction> QuestTracker_updatePinnedTask_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Request for ", "请求 来自");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(RealWorldTimeLight), "showTimeOnClock")]
        public static IEnumerable<CodeInstruction> RealWorldTimeLight_showTimeOnClock_Patch(IEnumerable<CodeInstruction> instructions)
        {
            //instructions = RIL(instructions, "<size=10>PM</size>", "<size=10>下午</size>");
            //instructions = RIL(instructions, "<size=10>AM</size>", "<size=10>上午</size>");
            instructions = RIL(instructions, "Late", "深夜");
            return instructions;
        }

        /// <summary>
        /// 在IL中替换文本
        /// </summary>
        public static IEnumerable<CodeInstruction> RIL(IEnumerable<CodeInstruction> instructions, string target, string i18n)
        {
            bool success = false;
            var list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var ci = list[i];
                if (ci.opcode == OpCodes.Ldstr)
                {
                    if ((string)ci.operand == target)
                    {
                        ci.operand = i18n;
                        success = true;
                    }
                }
            }
            if (!success)
            {
                Debug.LogWarning($"汉化插件欲将{target}替换成{i18n}失败，没有找到目标");
            }
            return list.AsEnumerable();
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(SaveSlotButton), "fillFromSaveSlot")]
        public static IEnumerable<CodeInstruction> SaveSlotButton_fillFromSaveSlot_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Year ", "年份 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(SeasonAndTime), "getLocationName")]
        public static IEnumerable<CodeInstruction> SeasonAndTime_getLocationName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Everywhere", "任意地点");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(UseBook), "plantBookRoutine", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> UseBook_plantBookRoutine_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, " Plant", "植株");
            instructions = RIL(instructions, "Ready for harvest", "可收获");
            instructions = RIL(instructions, "Mature in:\n", "成熟时间:\n");
            instructions = RIL(instructions, " days.", "天后");
            instructions = RIL(instructions, " days", "天后");
            instructions = RIL(instructions, "Plant", "植株");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WeatherManager), "GetWeatherDescription")]
        public static IEnumerable<CodeInstruction> WeatherManager_GetWeatherDescription_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "It is currently {0} ° and ", "当前气温{0}°并且");
            instructions = RIL(instructions, "Storming", "暴风雨");
            instructions = RIL(instructions, "Raining", "降雨");
            instructions = RIL(instructions, "Foggy", "雾气");
            instructions = RIL(instructions, "Fine", "天气良好");
            instructions = RIL(instructions, ". With a", "。还有");
            instructions = RIL(instructions, " Strong", "强烈的");
            instructions = RIL(instructions, " Light", "微弱的");
            instructions = RIL(instructions, " Northern ", "北");
            instructions = RIL(instructions, " Southern ", "南");
            instructions = RIL(instructions, " Westernly ", "西");
            instructions = RIL(instructions, " Easternly ", "东");
            instructions = RIL(instructions, " Wind.", "风。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(LoadingScreenImageAndTips), "OnEnable")]
        public static IEnumerable<CodeInstruction> LoadingScreenImageAndTips_OnEnable_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Photo by ", "拍摄者 ");
            return instructions;
        }

        /// <summary>
        /// 由于作者把地名翻译了但是在图标这里还是用的写死的英文来判断名字，所以导致在地下的时候图标显示不了。
        /// 临时做个处理，等作者把这修了，再删掉这个。
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(RenderMap), "ChangeWorldArea")]
        public static IEnumerable<CodeInstruction> RenderMap_ChangeWorldArea_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = RIL(instructions, "Mine", "矿井");
            instructions = RIL(instructions, "Airport", "飞机场");
            return instructions;
        }

        // instructions = ReplaceIL(instructions, "", "");
    }
}