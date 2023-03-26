using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using I2LocPatch;
using Mirror;

namespace DinkumChinese
{
    public static class StringReturnPatch
    {
        #region Prefix

        [HarmonyPrefix, HarmonyPatch(typeof(AnimalManager), "capitaliseFirstLetter")]
        public static bool AnimalManager_capitaliseFirstLetter_Patch(ref string __result, string toChange)
        {
            try
            {
                __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, toChange);
                return false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BullitenBoardPost), "getRequirementsNeededInPhoto")]
        public static bool BullitenBoardPost_getRequirementsNeededInPhoto_Patch(BullitenBoardPost __instance, int postId, ref string __result)
        {
            try
            {
                if (!BulletinBoard.board.attachedPosts[postId].isPhotoTask)
                {
                    __result = "";
                    return false;
                }
                List<string> list = new List<string>();
                List<int> list2 = new List<int>();
                string text = "";
                if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Animal)
                {
                    for (int i = 0; i < BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto().Length; i++)
                    {
                        string item = BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].animalName;
                        if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].animalId == 1)
                        {
                            item = AnimalManager.manage.allAnimals[1].GetComponent<FishType>().getFishInvItem().getInvItemName();
                        }
                        else if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].animalId == 2)
                        {
                            item = BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].GetComponent<BugTypes>().bugInvItem().itemName;
                        }
                        if (!list.Contains(item))
                        {
                            list.Add(item);
                            list2.Add(1);
                        }
                        else
                        {
                            list2[list.IndexOf(item)]++;
                        }
                    }
                    for (int j = 0; j < list.Count; j++)
                    {
                        text = ((list2[j] <= 1) ? (text + "" + list[j]) : (text + list2[j] + "" + list[j]));
                        if (j != list.Count - 1)
                        {
                            text = ((j != list.Count - 2 || list.Count <= 1) ? (text + "，") : (text + "和"));
                        }
                    }
                }
                else
                {
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Npc)
                    {
                        __result = NPCManager.manage.NPCDetails[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].NPCName;
                        return false;
                    }
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Location)
                    {
                        __result = "地图上标记处拍照";
                        return false;
                    }
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Carryable)
                    {
                        string itemName;
                        if ((bool)SaveLoad.saveOrLoad.carryablePrefabs[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].GetComponent<SellByWeight>())
                        {
                            itemName = SaveLoad.saveOrLoad.carryablePrefabs[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].GetComponent<SellByWeight>().itemName;
                        }
                        else
                        {
                            itemName = SaveLoad.saveOrLoad.carryablePrefabs[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].name;
                        }
                        __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, itemName);
                        return false;
                    }
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Biome)
                    {
                        __result = "在" + BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnRequiredLocationBiomeName() + "拍照";
                        return false;
                    }
                }
                __result = text;
                return false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Inventory), "getExtraDetails")]
        public static bool Inventory_getExtraDetails_Patch(Inventory __instance, int itemId, ref string __result)
        {
            try
            {
                var _this = __instance;
                string text = "";
                if (_this.allItems[itemId].placeable && _this.allItems[itemId].placeable.tileObjectGrowthStages && !_this.allItems[itemId].consumeable)
                {
                    string text2 = "";
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSummer && _this.allItems[itemId].placeable.tileObjectGrowthStages.growsInWinter && _this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSpring && _this.allItems[itemId].placeable.tileObjectGrowthStages.growsInAutum)
                    {
                        text2 = UIAnimationManager.manage.getCharacterNameTag("全年");
                    }
                    else
                    {
                        text2 += "在";
                        if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSummer)
                        {
                            text2 += UIAnimationManager.manage.getCharacterNameTag("夏天");
                        }
                        if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInAutum)
                        {
                            if (text2 != "在")
                            {
                                text2 += "和";
                            }
                            text2 += UIAnimationManager.manage.getCharacterNameTag("秋天");
                        }
                        if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInWinter)
                        {
                            if (text2 != "在")
                            {
                                text2 += "和";
                            }
                            text2 += UIAnimationManager.manage.getCharacterNameTag("冬天");
                        }
                        if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSpring)
                        {
                            if (text2 != "在")
                            {
                                text2 += "和";
                            }
                            text2 += UIAnimationManager.manage.getCharacterNameTag("春天");
                        }
                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.needsTilledSoil)
                    {
                        text = text + "适合" + text2 + "种植。";
                    }
                    string text4;
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length != 0 || (_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto && _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestSpots.Length != 0))
                    {
                        string text3 = "";
                        bool flag = false;
                        if (_this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length != 0)
                        {
                            if (_this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length > 1)
                            {
                                flag = true;
                            }
                            text3 = _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestDrop.getInvItemName();
                        }
                        else if (_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                        {
                            if (_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestSpots.Length > 1)
                            {
                                flag = true;
                            }
                            text3 = _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestDrop.getInvItemName();
                        }
                        text4 = UIAnimationManager.manage.getItemColorTag(text3);
                    }
                    else
                    {
                        text4 = "???";
                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length != 0)
                    {
                        if (_this.allItems[itemId].burriedPlaceable)
                        {
                            __result = "你得把它埋在地里。" + _this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length.ToString() + "天后它就会长大。";
                            return false;
                        }
                        if (_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                        {
                            text = string.Concat(new string[]
                            {
                        text,
                        "周围需要一些空间，因为它们会在旁边的位置结出",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                        "",
                        text4,
                        "。该植株最多能分出4个分支！"
                            });
                        }
                        else
                        {
                            text = string.Concat(new string[]
                            {
                        text,
                        "需要",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length.ToString(),
                        "天的时间来生长，可收获",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                        "",
                        text4,
                        "。"
                            });
                        }
                    }
                    if (!_this.allItems[itemId].placeable.tileObjectGrowthStages.diesOnHarvest && !_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                    {
                        text = string.Concat(new string[]
                        {
                    text,
                    "后续每",
                    Mathf.Abs(_this.allItems[itemId].placeable.tileObjectGrowthStages.takeOrAddFromStateOnHarvest).ToString(),
                    "天可收获",
                    _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                    "",
                    text4,
                    "。"
                        });
                    }
                    if (!WorldManager.manageWorld.allObjectSettings[_this.allItems[itemId].placeable.tileObjectId].walkable)
                    {
                        text = text + "噢，这还需要" + UIAnimationManager.manage.getItemColorTag("植物支架") + "来附着生长。";
                    }
                }
                __result = text;
                return false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(NetworkMapSharer), "UserCode_RpcDeliverAnimal")]
        public static bool NetworkMapSharer_UserCode_RpcDeliverAnimal_Patch(uint deliveredBy, int animalDelivered, int variationDelivered, int rewardToSend, int trapType)
        {
            try
            {
                if (NetworkIdentity.spawned.ContainsKey(deliveredBy))
                {
                    CharMovement component = NetworkIdentity.spawned[deliveredBy].GetComponent<CharMovement>();
                    var animal = AnimalManager.manage.allAnimals[animalDelivered];
                    // 获取动物名字
                    string animalName = TextLocData.GetLoc(DinkumChinesePlugin.Inst.AnimalsTextLocList, animal.animalName);
                    string str = animalName;
                    if (variationDelivered != 0)
                    {
                        string variationName = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, animal.hasVariation.variationAdjective[variationDelivered]);
                        str = variationName + animalName;
                    }
                    NotificationManager.manage.createChatNotification(component.GetComponent<EquipItemToChar>().playerName + "交付了一只" + str, false);
                    if (component.isLocalPlayer)
                    {
                        if (animalDelivered == 29)
                        {
                            MailManager.manage.sendAChrissyAnimalCapturedLetter(trapType);
                            return false;
                        }
                        MailManager.manage.sendAnAnimalCapturedLetter(rewardToSend, trapType);
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(SeasonAndTime), "capitaliseFirstLetter")]
        public static bool SeasonAndTime_capitaliseFirstLetter_Patch(ref string __result, string toChange)
        {
            try
            {
                __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, toChange);
                return false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }
        }

        #endregion Prefix

        #region Postfix

        [HarmonyPostfix, HarmonyPatch(typeof(AnimalHouseMenu), "fillData")]
        public static void AnimalHouseMenu_fillData_Patch(AnimalHouseMenu __instance)
        {
            try
            {
                __instance.eatenText.text = __instance.eatenText.text.Replace("Eaten", "喂食");
                __instance.shelterText.text = __instance.shelterText.text.Replace("Shelter", "住所");
                __instance.pettedText.text = __instance.pettedText.text.Replace("Petted", "爱抚");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AnimalManager), "fillAnimalLocation")]
        public static void AnimalManager_fillAnimalLocation_Patch(ref string __result)
        {
            try
            {
                __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CustomNetworkManager), "refreshLobbyTypeButtons")]
        public static void CustomNetworkManager_refreshLobbyTypeButtons_Patch(CustomNetworkManager __instance)
        {
            try
            {
                __instance.friendGameText.text = __instance.friendGameText.text.Replace("Friends Only", "仅好友");
                __instance.inviteOnlyText.text = __instance.inviteOnlyText.text.Replace("InviteOnly", "仅邀请");
                __instance.publicGameText.text = __instance.publicGameText.text.Replace("Public", "公开");
                __instance.lanGameText.text = __instance.lanGameText.text.Replace("LAN", "局域网");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameById")]
        public static void GenerateMap_getBiomeNameById_Patch(ref string __result, int id)
        {
            try
            {
                GenerateMap.biomNames biomNames = (GenerateMap.biomNames)id;
                __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, biomNames.ToString());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameUnderMapCursor")]
        public static void GenerateMap_getBiomeNameUnderMapCursor_Patch(ref string __result)
        {
            try
            {
                __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result.StrToI2Str());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Licence), "getConnectedSkillName")]
        public static void Licence_getConnectedSkillName_Patch(ref string __result)
        {
            try
            {
                __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "setRandomBugNoAndLocation")]
        public static void NPCRequest_setRandomBugNoAndLocation_Patch(NPCRequest __instance)
        {
            try
            {
                __instance.itemFoundInLocation = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __instance.itemFoundInLocation);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "setRandomFishNoAndLocation")]
        public static void NPCRequest_setRandomFishNoAndLocation_Patch(NPCRequest __instance)
        {
            try
            {
                __instance.itemFoundInLocation = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __instance.itemFoundInLocation);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PickUpNotification), "fillButtonPrompt", new Type[] { typeof(string), typeof(Sprite) })]
        public static void PickUpNotification_fillButtonPrompt_Patch(PickUpNotification __instance, string buttonPromptText)
        {
            try
            {
                string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, buttonPromptText);
                __instance.itemText.text = text;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PickUpNotification), "fillButtonPrompt", new Type[] { typeof(string), typeof(Input_Rebind.RebindType) })]
        public static void PickUpNotification_fillButtonPrompt_Patch2(PickUpNotification __instance, string buttonPromptText)
        {
            try
            {
                string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, buttonPromptText);
                __instance.itemText.text = text;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getContentText")]
        public static void PostOnBoard_getContentText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            try
            {
                string textOri = __instance.getPostPostsById().contentText.StrToI2Str();
                string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.PostTextLocList, textOri);
                __result = text.Replace("<boardRewardItem>",
                    __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>",
                    __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<getAnimalsInPhotoList>",
                    __instance.getPostPostsById().getRequirementsNeededInPhoto(postId)).Replace("<boardRequestItem>",
                    __instance.getPostPostsById().getBoardRequestItem(postId));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getTitleText")]
        public static void PostOnBoard_getTitleText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            try
            {
                string titleOri = __instance.getPostPostsById().title.StrToI2Str();
                string title = TextLocData.GetLoc(DinkumChinesePlugin.Inst.PostTextLocList, titleOri);
                __result = title.Replace("<boardRewardItem>",
                    __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>",
                    __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<boardRequestItem>",
                    __instance.getPostPostsById().getBoardRequestItem(postId));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestButton), "setUp")]
        public static void QuestButton_setUp_Patch(QuestButton __instance, int questNo)
        {
            try
            {
                if (__instance.isMainQuestButton)
                {
                    string nameOri = QuestManager.manage.allQuests[questNo].QuestName.StrToI2Str();
                    string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                    __instance.buttonText.text = name;
                }
                else if (__instance.isQuestButton)
                {
                }
                else
                {
                    __instance.buttonText.text = __instance.buttonText.text.Replace("Request for ", "来自") + "的请求";
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestNotification), "showQuest")]
        public static void QuestNotification_showQuest_Patch(QuestNotification __instance)
        {
            try
            {
                string nameOri = __instance.displayingQuest.QuestName.StrToI2Str();
                string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                __instance.QuestText.text = name;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "displayMainQuest")]
        public static void QuestTracker_displayMainQuest_Patch(QuestTracker __instance, int questNo)
        {
            try
            {
                string nameOri = QuestManager.manage.allQuests[questNo].QuestName.StrToI2Str();
                string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                string descOri = QuestManager.manage.allQuests[questNo].QuestDescription.StrToI2Str();
                string desc = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, descOri).Replace("<IslandName>", Inventory.inv.islandName);
                __instance.questTitle.text = name;
                __instance.questDesc.text = desc;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "pinTheTask")]
        public static void QuestTracker_pinTheTask_Patch(QuestTracker __instance, QuestTracker.typeOfTask type, int id)
        {
            try
            {
                if (type == QuestTracker.typeOfTask.Quest)
                {
                    if (!QuestManager.manage.isQuestCompleted[id])
                    {
                        string nameOri = QuestManager.manage.allQuests[id].QuestName.StrToI2Str();
                        string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                        string pinText = __instance.pinMissionText.text.Replace(QuestManager.manage.allQuests[id].QuestName, name);
                        __instance.pinMissionText.text = pinText;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        #endregion Postfix
    }
}