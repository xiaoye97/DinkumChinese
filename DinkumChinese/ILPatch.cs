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
    }
}
