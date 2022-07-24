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
            instructions = ReplaceIL(instructions, "<sprite=12> Hunt down the ", "<sprite=12> 狩猎");
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

        // todo
        [HarmonyTranspiler, HarmonyPatch(typeof(GiftedItemWindow), "giveItemDelay", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> GiftedItemWindow_giveItemDelay_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "New Licence!", "新许可证！");
            instructions = ReplaceIL(instructions, " Level ", " 等级 ");
            instructions = ReplaceIL(instructions, "On ya!", "哦耶！");
            instructions = ReplaceIL(instructions, "You received", "你获得了");
            instructions = ReplaceIL(instructions, "New Crafting Recipe", "新制作配方");
            instructions = ReplaceIL(instructions, "An item was sent to your Mailbox", "物品送去了你的邮箱");
            instructions = ReplaceIL(instructions, "Your pockets were full!", "你的口袋满了！");
            return instructions;
        }

        // todo
        [HarmonyTranspiler, HarmonyPatch(typeof(CameraController), "moveCameraToShowPos", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> CameraController_moveCameraToShowPos_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " is visiting the island!", "正在拜访该岛！");
            instructions = ReplaceIL(instructions, "Someone is visiting the island!", "有人在拜访这个岛！");
            instructions = ReplaceIL(instructions, "No one is visiting today...", "今天没有人来。。。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ConversationManager), "talkToNPC")]
        public static IEnumerable<CodeInstruction> ConversationManager_talkToNPC_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "A new deed is available!", "新契约可用！");
            instructions = ReplaceIL(instructions, "Talk to Fletch to apply for deeds.", "与Fletch谈谈申请契约。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(EquipItemToChar), "OnDestroy")]
        public static IEnumerable<CodeInstruction> EquipItemToChar_OnDestroy_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " has left", " 离开了");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(EquipItemToChar), "UserCode_RpcCharacterJoinedPopup")]
        public static IEnumerable<CodeInstruction> EquipItemToChar_UserCode_RpcCharacterJoinedPopup_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Welcome to ", "欢迎来到");
            instructions = ReplaceIL(instructions, " has joined", " 加入了");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(LicenceManager), "checkForUnlocksOnLevelUp")]
        public static IEnumerable<CodeInstruction> LicenceManager_checkForUnlocksOnLevelUp_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "A new Licence is available!", "新的许可证可用！");
            instructions = ReplaceIL(instructions, "A new deed is available!", "新契约可用！");
            instructions = ReplaceIL(instructions, "Talk to Fletch to apply for deeds.", "与Fletch谈谈申请契约。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NetworkMapSharer), "UserCode_RpcAddToMuseum")]
        public static IEnumerable<CodeInstruction> NetworkMapSharer_UserCode_RpcAddToMuseum_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Donated by ", "捐赠方：");
            return instructions;
        }

        // todo
        [HarmonyTranspiler, HarmonyPatch(typeof(NetworkNavMesh), "waitForNameToChange", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> NetworkNavMesh_waitForNameToChange_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " has joined", " 加入了");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "acceptRequest")]
        public static IEnumerable<CodeInstruction> NPCRequest_acceptRequest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Request added to Journal", "请求已添加到日记本");
            instructions = ReplaceIL(instructions, "This request must be completed by the end of the day.", "此请求必须在当天结束前完成。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PostOnBoard), "acceptTask")]
        public static IEnumerable<CodeInstruction> PostOnBoard_acceptTask_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Request added to Journal by ", "请求已添加到日记账由");
            instructions = ReplaceIL(instructions, "Request added to Journal", "请求已添加到日记账");
            instructions = ReplaceIL(instructions, "A location was added to your map.", "已将位置添加到地图中。");
            instructions = ReplaceIL(instructions, "This request has a time limit.", "此请求有时间限制。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PostOnBoard), "completeTask")]
        public static IEnumerable<CodeInstruction> PostOnBoard_completeTask_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Request Completed by ", "请求完成由");
            instructions = ReplaceIL(instructions, "Investigation Request Complete!", "调查请求完成！");
            instructions = ReplaceIL(instructions, "Request Complete!", "请求完成！");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestManager), "completeQuest")]
        public static IEnumerable<CodeInstruction> QuestManager_completeQuest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "A new deed is available!", "新契约可用！");
            instructions = ReplaceIL(instructions, "Talk to Fletch to apply for deeds.", "与Fletch谈谈申请契约。");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(Quest), "getMissionObjText")]
        public static IEnumerable<CodeInstruction> Quest_getMissionObjText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "<sprite=12> Attract a total of 5 permanent residents to move to ", "<sprite=12> 总共吸引5名常住居民搬到");
            instructions = ReplaceIL(instructions, "<sprite=12> Talk to ", "\n<sprite=12> 对话");
            instructions = ReplaceIL(instructions, " once the Base Tent has been moved", "在移动基地帐篷后");
            instructions = ReplaceIL(instructions, "<sprite=13> Craft a ", "<sprite=13> 制作一个");
            instructions = ReplaceIL(instructions, "at the crafting table in the Base Tent.\n<sprite=13> Place the  ", "在基地帐篷里的制作桌子上。\n<sprite=13> 放置");
            instructions = ReplaceIL(instructions, " down outside.\n<sprite=13> Place Tin Ore into ", "在地上。\n<sprite=13> 将锡矿放入");
            instructions = ReplaceIL(instructions, " and wait for it to become ", "并等待它熔炼成");
            instructions = ReplaceIL(instructions, ".\n<sprite=12> Take the ", "。\n<sprite=12> 拿着");
            instructions = ReplaceIL(instructions, " to ", "到");
            instructions = ReplaceIL(instructions, " down outside.\n<sprite=12> Place Tin Ore into ", "。\n<sprite=12> 放置");
            instructions = ReplaceIL(instructions, "at the crafting table in the Base Tent.\n<sprite=12> Place the  ", "在基础帐篷的制作桌上。\n<sprite=12> 放置");
            instructions = ReplaceIL(instructions, "<sprite=12> Craft a ", "<sprite=12> 制作一个");
            instructions = ReplaceIL(instructions, "<sprite=13> Buy the ", "<sprite=13> 购买");
            instructions = ReplaceIL(instructions, "\n<sprite=12> Talk to ", "\n<sprite=12> 对话");
            instructions = ReplaceIL(instructions, "<sprite=12> Buy the ", "<sprite=12> 购买");
            instructions = ReplaceIL(instructions, "[Optional] Complete Daily tasks\n<sprite=12> Place sleeping bag and get some rest.", "[可选]完成日常任务\n<sprite=12>放置睡袋并休息。");
            instructions = ReplaceIL(instructions, "<sprite=13> Find something to eat.\n<sprite=12> Talk to ", "<sprite=13> 找点吃的。\n<sprite=12> 对话");
            instructions = ReplaceIL(instructions, "<sprite=12> Find something to eat.\n<sprite=12> Talk to ", "<sprite=12> 找点吃的。\n<sprite=12> 对话");
            instructions = ReplaceIL(instructions, "<sprite=13> Collect the requested items.\n<sprite=12> Bring items to ", "<sprite=13> 收集请求的物品。\n<sprite=12> 将物品带到");
            instructions = ReplaceIL(instructions, "<sprite=12> Collect the requested items.", "<sprite=12> 收集请求的物品。");
            instructions = ReplaceIL(instructions, "\n<sprite=12> Bring items to ", "\n<sprite=12> 将物品带到");
            instructions = ReplaceIL(instructions, "<sprite=12> Do some favours for John", "<sprite=12> 帮助John");
            instructions = ReplaceIL(instructions, "<sprite=13> Do some favours for John", "<sprite=13> 帮助John");
            instructions = ReplaceIL(instructions, "\n<sprite=12> Spend money or sell items in John's store", "\n<sprite=12> 在John的店里花钱或卖东西");
            instructions = ReplaceIL(instructions, "\n<sprite=13> Spend money or sell items in John's store", "\n<sprite=13> 在John的店里花钱或卖东西");
            instructions = ReplaceIL(instructions, "\n<sprite=12> Convince John to move in.", "\n<sprite=12> 说服John留下来。");
            instructions = ReplaceIL(instructions, "<sprite=12> Ask ", "<sprite=12> 询问");
            instructions = ReplaceIL(instructions, " about the town to apply for the ", "关于城镇申请");
            instructions = ReplaceIL(instructions, "<sprite=12> Place the ", "<sprite=12> 放置");
            instructions = ReplaceIL(instructions, "<sprite=12> Wait for construction of the  ", "<sprite=12> 等待");
            instructions = ReplaceIL(instructions, " to be completed", "的施工完成");
            instructions = ReplaceIL(instructions, "<sprite=12> Place the required items into the construction box at the deed site", "<sprite=12> 将所需物品放入契约现场的材料箱中");
            instructions = ReplaceIL(instructions, "<sprite=12> Place ", "<sprite=12> 放置");
            instructions = ReplaceIL(instructions, "<sprite=13> Place ", "<sprite=13> 放置");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(InventoryItemDescription), "fillItemDescription")]
        public static IEnumerable<CodeInstruction> InventoryItemDescription_fillItemDescription_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "All year", "全年");
            instructions = ReplaceIL(instructions, "Summer", "夏天");
            instructions = ReplaceIL(instructions, "Autum", "秋天");
            instructions = ReplaceIL(instructions, "Winter", "冬天");
            instructions = ReplaceIL(instructions, "Spring", "春天");
            instructions = ReplaceIL(instructions, "Bury", "掩埋");
            instructions = ReplaceIL(instructions, "Speeds up certain production devices for up to 12 tiles", "加快某些生产设备的速度，最大范围12格子");
            instructions = ReplaceIL(instructions, "Reaches ", "达到");
            instructions = ReplaceIL(instructions, " tiles out.\n<color=red>Requires Water Tank</color>", "格子。\n<color=red>需要水箱</color>");
            instructions = ReplaceIL(instructions, "Provides water to sprinklers ", "向");
            instructions = ReplaceIL(instructions, " tiles out.", "格范围内子灌溉。");
            instructions = ReplaceIL(instructions, "Fills animal feeders ", "填充动物饲料");
            instructions = ReplaceIL(instructions, " tiles out.\n<color=red>Requires Animal Food</color>", "格。\n<color=red>需要动物饲料</color>");

            return instructions;
        }

        // instructions = ReplaceIL(instructions, "", "");
    }
}
