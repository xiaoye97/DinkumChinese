using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace DinkumChinese
{
    public static class ILPatch
    {
        /// <summary>
        /// 在IL中替换文本
        /// </summary>
        public static IEnumerable<CodeInstruction> ReplaceIL(IEnumerable<CodeInstruction> instructions, string target, string i18n)
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

        [HarmonyTranspiler, HarmonyPatch(typeof(PlayerDetailManager), "switchToLevelWindow")]
        public static IEnumerable<CodeInstruction> PlayerDetailManager_switchToLevelWindow_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Resident for: ", "居住:");
            instructions = ReplaceIL(instructions, " days", " 天");
            instructions = ReplaceIL(instructions, " months", " 月");
            instructions = ReplaceIL(instructions, " years", " 年");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayTrackingRecipe")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayTrackingRecipe_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Recipe", " 配方");
            instructions = ReplaceIL(instructions, "These items are required to craft ", "制作");
            instructions = ReplaceIL(instructions, "\n Unpin this to stop tracking recipe.", "需要这些物品。\n取消固定来停止跟踪配方");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayRequest")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayRequest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Request for ", "请求 来自");
            instructions = ReplaceIL(instructions, " has asked you to get ", "想向你要");
            instructions = ReplaceIL(instructions, "By the end of the day", "在今天结束之前");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayQuest")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayQuest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " days remaining", " 天剩余");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "pressPinRecipeButton")]
        public static IEnumerable<CodeInstruction> QuestTracker_pressPinRecipeButton_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Recipe", " 配方");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updateLookingAtTask")]
        public static IEnumerable<CodeInstruction> QuestTracker_updateLookingAtTask_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "<sprite=17> Pinned", "<sprite=17> 固定");
            instructions = ReplaceIL(instructions, "<sprite=16> Pinned", "<sprite=16> 固定");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updatePinnedTask")]
        public static IEnumerable<CodeInstruction> QuestTracker_updatePinnedTask_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Request for ", "请求 来自");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updatePinnedRecipeButton")]
        public static IEnumerable<CodeInstruction> QuestTracker_updatePinnedRecipeButton_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "<sprite=13> Track Recipe Ingredients", "<sprite=13> 跟踪配方成分");
            instructions = ReplaceIL(instructions, "<sprite=12> Track Recipe Ingredients", "<sprite=12> 跟踪配方成分");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "fillMissionTextForRecipe")]
        public static IEnumerable<CodeInstruction> QuestTracker_fillMissionTextForRecipe_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Crafting ", "制作 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BulletinBoard), "showSelectedPost")]
        public static IEnumerable<CodeInstruction> BulletinBoard_showSelectedPost_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "EXPIRED", "过期");
            instructions = ReplaceIL(instructions, " Last Day", "最后一天");
            instructions = ReplaceIL(instructions, " Days Remaining", " 天剩余");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BulletinBoard), "getMissionText")]
        public static IEnumerable<CodeInstruction> BulletinBoard_getMissionText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "<sprite=12> Speak to ", "<sprite=12> 告诉 ");
            instructions = ReplaceIL(instructions, "<sprite=12> Hunt down the ", "<sprite=12> 抓捕");
            instructions = ReplaceIL(instructions, " using its last know location on the map", "使用地图上最后知道的位置");
            instructions = ReplaceIL(instructions, "<sprite=13> Visit the location on the map to investigate", "<sprite=13> 访问地图上的位置进行调查");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(AnimalHouseMenu), "openConfirm")]
        public static IEnumerable<CodeInstruction> AnimalHouseMenu_openConfirm_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Sell ", "出售 ");
            instructions = ReplaceIL(instructions, " for <sprite=11>", " 以 <sprite=11>");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(AnimalHouseMenu), "fillData")]
        public static IEnumerable<CodeInstruction> AnimalHouseMenu_fillData_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Year", " 年");
            instructions = ReplaceIL(instructions, " Month", " 月");
            instructions = ReplaceIL(instructions, " Day", " 日");
            instructions = ReplaceIL(instructions, "s", "");
            instructions = ReplaceIL(instructions, "SELL ", "出售 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Task), MethodType.Constructor, new Type[] { typeof(int), typeof(int) })]
        public static IEnumerable<CodeInstruction> Task_Constructor_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Harvest ", "收获 ");
            instructions = ReplaceIL(instructions, "Catch ", "捕捉");
            instructions = ReplaceIL(instructions, " Bugs", " 虫子");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Task), "generateTask")]
        public static IEnumerable<CodeInstruction> Task_generateTask_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Harvest ", "收获");
            instructions = ReplaceIL(instructions, "Chat with ", "聊天和");
            instructions = ReplaceIL(instructions, " residents", "居民");
            instructions = ReplaceIL(instructions, "Bury ", "埋");
            instructions = ReplaceIL(instructions, " Fruit", "水果");
            instructions = ReplaceIL(instructions, "Collect ", "收集");
            instructions = ReplaceIL(instructions, " Shells", " 贝壳");
            instructions = ReplaceIL(instructions, "Sell ", "出售");
            instructions = ReplaceIL(instructions, "Do a job for someone", "为某人做事");
            instructions = ReplaceIL(instructions, "Plant ", "种植");
            instructions = ReplaceIL(instructions, " Wild Seeds", "野生种子");
            instructions = ReplaceIL(instructions, "Dig up dirt ", "挖出泥土");
            instructions = ReplaceIL(instructions, " times", "次");
            instructions = ReplaceIL(instructions, "Catch ", "捕捉");
            instructions = ReplaceIL(instructions, " Bugs", "虫子");
            instructions = ReplaceIL(instructions, "Craft ", "制作");
            instructions = ReplaceIL(instructions, " Items", "物品");
            instructions = ReplaceIL(instructions, "Eat something", "吃点东西");
            instructions = ReplaceIL(instructions, "Make ", "获得");
            instructions = ReplaceIL(instructions, "Spend ", "花费");
            instructions = ReplaceIL(instructions, "Travel ", "旅行");
            instructions = ReplaceIL(instructions, "m on foot.", "米通过徒步");
            instructions = ReplaceIL(instructions, "m by vehicle", "米通过载具");
            instructions = ReplaceIL(instructions, "Cook ", "烹饪");
            instructions = ReplaceIL(instructions, " meat", "肉");
            instructions = ReplaceIL(instructions, " fruit", "水果");
            instructions = ReplaceIL(instructions, "Cook something at the Cooking table", "在烹饪台上做点东西");
            instructions = ReplaceIL(instructions, " tree seeds", "树种子");
            instructions = ReplaceIL(instructions, "crop seeds", "作物种子");
            instructions = ReplaceIL(instructions, "Water ", "浇灌");
            instructions = ReplaceIL(instructions, " crops", "种子");
            instructions = ReplaceIL(instructions, "Smash ", "打碎");
            instructions = ReplaceIL(instructions, " rocks", " 岩石");
            instructions = ReplaceIL(instructions, " ore rocks", "矿石");
            instructions = ReplaceIL(instructions, "Smelt some ore into a bar", "熔炉一些矿石成锭");
            instructions = ReplaceIL(instructions, "Grind ", "磨碎");
            instructions = ReplaceIL(instructions, " stones", "石头");
            instructions = ReplaceIL(instructions, "Cut down ", "修剪");
            instructions = ReplaceIL(instructions, " trees", "树");
            instructions = ReplaceIL(instructions, "Clear ", "清理");
            instructions = ReplaceIL(instructions, " tree stumps", "树桩");
            instructions = ReplaceIL(instructions, "Saw ", "锯");
            instructions = ReplaceIL(instructions, " planks", "木板");
            instructions = ReplaceIL(instructions, " Fish", "鱼");
            instructions = ReplaceIL(instructions, " grass", "草");
            instructions = ReplaceIL(instructions, "Pet an animal", "爱抚动物");
            instructions = ReplaceIL(instructions, "Buy some new clothes", "买些新衣服");
            instructions = ReplaceIL(instructions, "Buy some new furniture", "买些新家具");
            instructions = ReplaceIL(instructions, "Buy some new wallpaper", "买些新壁纸");
            instructions = ReplaceIL(instructions, "Buy some new flooring", "买些新地板");
            instructions = ReplaceIL(instructions, "Compost something", "堆肥");
            instructions = ReplaceIL(instructions, "Craft a new tool", "制作新工具");
            instructions = ReplaceIL(instructions, "Buy ", "购买");
            instructions = ReplaceIL(instructions, " seeds", "种子");
            instructions = ReplaceIL(instructions, "Trap an animal and deliver it", "诱捕动物并运送");
            instructions = ReplaceIL(instructions, "Hunt ", "狩猎");
            instructions = ReplaceIL(instructions, " animals", "动物");
            instructions = ReplaceIL(instructions, "Buy a new tool", "购买新工具");
            instructions = ReplaceIL(instructions, "Break a tool", "损坏一个工具");
            instructions = ReplaceIL(instructions, "Find some burried treasure", "找到一些埋藏的宝藏");
            instructions = ReplaceIL(instructions, "No mission set", "没有任务");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WeatherManager), "currentWeather")]
        public static IEnumerable<CodeInstruction> WeatherManager_currentWeather_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "It is currently ", "当前气温");
            instructions = ReplaceIL(instructions, "° and ", "°并且");
            instructions = ReplaceIL(instructions, "Storming", "暴风雨");
            instructions = ReplaceIL(instructions, "Raining", "降雨");
            instructions = ReplaceIL(instructions, "Foggy", "雾气");
            instructions = ReplaceIL(instructions, "Fine", "天气良好");
            instructions = ReplaceIL(instructions, ". With a", "。还有");
            instructions = ReplaceIL(instructions, " Strong", "强烈的");
            instructions = ReplaceIL(instructions, " Light", "微弱的");
            instructions = ReplaceIL(instructions, " Northern ", "北");
            instructions = ReplaceIL(instructions, " Southern ", "南");
            instructions = ReplaceIL(instructions, " Westernly ", "西");
            instructions = ReplaceIL(instructions, " Easternly ", "东");
            instructions = ReplaceIL(instructions, " Wind.", "风。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WeatherManager), "tomorrowsWeather")]
        public static IEnumerable<CodeInstruction> WeatherManager_tomorrowsWeather_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Tomorrow expect ", "明日天气预报:");
            instructions = ReplaceIL(instructions, "Storms", "暴风雨");
            instructions = ReplaceIL(instructions, "Rain", "降雨");
            instructions = ReplaceIL(instructions, "Fog", "雾气");
            instructions = ReplaceIL(instructions, "Fine Weather", "好天气");
            instructions = ReplaceIL(instructions, ". With", "。还有");
            instructions = ReplaceIL(instructions, " Strong", "强烈的");
            instructions = ReplaceIL(instructions, " Light", "微弱的");
            instructions = ReplaceIL(instructions, " Northern ", "北");
            instructions = ReplaceIL(instructions, " Southern ", "南");
            instructions = ReplaceIL(instructions, " Westernly ", "西");
            instructions = ReplaceIL(instructions, " Easternly ", "东");
            instructions = ReplaceIL(instructions, "Wind. With temperatures around ", "风。温度接近");
            instructions = ReplaceIL(instructions, "°.", "°。");
            return instructions;
        }

        // instructions = ReplaceIL(instructions, "", "");
    }
}
