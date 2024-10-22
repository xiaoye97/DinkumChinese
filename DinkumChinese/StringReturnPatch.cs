using HarmonyLib;
using I2LocPatch;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public static bool BullitenBoardPost_getRequirementsNeededInPhoto_Patch2(BullitenBoardPost __instance, int postId, ref string __result)
        {
            try
            {
                if (!BulletinBoard.board.attachedPosts[postId].isPhotoTask)
                {
                    __result = "";
                    return false;
                }
                List<string> nameList = new List<string>();
                List<int> countList = new List<int>();
                string text = "";
                // 如果要拍的是动物
                if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Animal)
                {
                    for (int i = 0; i < BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto().Length; i++)
                    {
                        string animalName = BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].GetAnimalName();
                        if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].animalId == 1)
                        {
                            animalName = AnimalManager.manage.allAnimals[1].GetComponent<FishType>().getFishInvItem().getInvItemName();
                        }
                        else if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].animalId == 2)
                        {
                            animalName = BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.animalsRequiredInPhoto()[i].GetComponent<BugTypes>().bugInvItem().itemName;
                        }
                        if (!nameList.Contains(animalName))
                        {
                            nameList.Add(animalName);
                            countList.Add(1);
                        }
                        else
                        {
                            countList[nameList.IndexOf(animalName)]++;
                        }
                    }
                    for (int j = 0; j < nameList.Count; j++)
                    {
                        // 不要原有逻辑了，太罗嗦，重新写一个
                        text += $"{countList[j]}{nameList[j]} ";
                    }
                }
                else
                {
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Npc)
                    {
                        __result = NPCManager.manage.NPCDetails[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].GetNPCName();
                        return false;
                    }
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Location)
                    {
                        __result = "地图上标记处拍照";
                        return false;
                    }
                    if (BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.getSubjectType() == PhotoChallengeManager.photoSubject.Carryable)
                    {
                        if ((bool)SaveLoad.saveOrLoad.carryablePrefabs[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].GetComponent<SellByWeight>())
                        {
                            __result = SaveLoad.saveOrLoad.carryablePrefabs[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].GetComponent<SellByWeight>().itemName;
                        }
                        else
                        {
                            __result = SaveLoad.saveOrLoad.carryablePrefabs[BulletinBoard.board.attachedPosts[postId].myPhotoChallenge.returnSubjectId()[0]].name;
                        }
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
        public static bool Inventory_getExtraDetails_Patch2(Inventory __instance, int itemId, ref string __result)
        {
            try
            {
                var _this = __instance;
                var item = _this.allItems[itemId];
                var placeable = item.placeable;
                string text = "";
                if (placeable && placeable.tileObjectGrowthStages && !item.consumeable)
                {
                    var tileObjectGrowthStages = placeable.tileObjectGrowthStages;
                    string text2 = "";
                    if (tileObjectGrowthStages.growsInSummer && tileObjectGrowthStages.growsInWinter && tileObjectGrowthStages.growsInSpring && tileObjectGrowthStages.growsInAutum)
                    {
                        text2 = UIAnimationManager.manage.GetCharacterNameTag("all year");
                    }
                    else
                    {
                        if (tileObjectGrowthStages.growsInSummer)
                        {
                            text2 += UIAnimationManager.manage.GetCharacterNameTag("Summer");
                        }
                        if (tileObjectGrowthStages.growsInAutum)
                        {
                            text2 += UIAnimationManager.manage.GetCharacterNameTag("Autumn");
                        }
                        if (tileObjectGrowthStages.growsInWinter)
                        {
                            text2 += UIAnimationManager.manage.GetCharacterNameTag("Winter");
                        }
                        if (tileObjectGrowthStages.growsInSpring)
                        {
                            text2 += UIAnimationManager.manage.GetCharacterNameTag("Spring");
                        }
                    }
                    if (tileObjectGrowthStages.needsTilledSoil)
                    {
                        text = "适合在" + text2 + "种植。";
                    }
                    string text3 = "";
                    if (tileObjectGrowthStages.harvestSpots.Length != 0 || (tileObjectGrowthStages.steamsOutInto && tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestSpots.Length != 0))
                    {
                        string text4 = "";
                        if (tileObjectGrowthStages.harvestSpots.Length != 0)
                        {
                            text4 = tileObjectGrowthStages.harvestDrop.getInvItemName();
                        }
                        else if (tileObjectGrowthStages.steamsOutInto)
                        {
                            text4 = tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestDrop.getInvItemName();
                        }
                        text3 = UIAnimationManager.manage.GetItemColorTag(text4);
                    }
                    else
                    {
                        text3 = "???";
                    }

                    var objectStages = tileObjectGrowthStages.objectStages;
                    var steamsOutInto = tileObjectGrowthStages.steamsOutInto;
                    if (objectStages.Length != 0)
                    {
                        if (item.burriedPlaceable)
                        {
                            __result = "你得把它埋在地里。它会在" + objectStages.Length + "天后长大。";
                            return false;
                        }
                        if (steamsOutInto)
                        {
                            text += $"周围需要一些空间，因为它们会在旁边的位置结出{steamsOutInto.tileObjectGrowthStages.harvestSpots.Length}{text3}。该植株最多能分出4个分支！";
                        }
                        else
                        {
                            text += $"需要{objectStages.Length}天的时间来生长，可收获{tileObjectGrowthStages.harvestSpots.Length}{text3}。";
                        }
                    }
                    if (!tileObjectGrowthStages.diesOnHarvest && !steamsOutInto)
                    {
                        text += $"后续每{Mathf.Abs(tileObjectGrowthStages.takeOrAddFromStateOnHarvest)}天可收获{tileObjectGrowthStages.harvestSpots.Length}{text3}。";
                    }
                    if (!WorldManager.Instance.allObjectSettings[item.placeable.tileObjectId].walkable)
                    {
                        text += $"哦，这还需要{UIAnimationManager.manage.GetItemColorTag("藤架")}来附着生长。";
                    }
                    if (tileObjectGrowthStages.canGrowInTilledWater)
                    {
                        text += "哦，如果你把它们种在浅水区，你就不需要给它们浇水了！";
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

        [HarmonyPostfix, HarmonyPatch(typeof(PickUpNotification), "fillButtonPrompt", new Type[] { typeof(string), typeof(Sprite) })]
        public static void PickUpNotification_fillButtonPrompt_Patch(PickUpNotification __instance, string buttonPromptText)
        {
            try
            {
                //Debug.Log($"PickUpNotification.fillButtonPrompt1 buttonPromptText:{buttonPromptText}");
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
                //Debug.Log($"PickUpNotification.fillButtonPrompt2 buttonPromptText:{buttonPromptText}");
                string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, buttonPromptText);
                __instance.itemText.text = text;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        //[HarmonyPrefix, HarmonyPatch(typeof(NotificationManager), "showButtonPrompt")]
        //public static bool NotificationManager_showButtonPrompt_Patch(NotificationManager __instance, ref string promptText)
        //{
        //    try
        //    {
        //        Debug.Log($"NotificationManager.showButtonPrompt promptText:{promptText}");
        //        string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, promptText);
        //        promptText = text;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //    }
        //    return true;
        //}

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getContentText")]
        public static void PostOnBoard_getContentText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            try
            {
                string textOri = __instance.getPostPostsById().contentText;
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
                string titleOri = __instance.getPostPostsById().title;
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
                    string nameOri = QuestManager.manage.allQuests[questNo].QuestName;
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
                string nameOri = __instance.displayingQuest.QuestName;
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
                string nameOri = QuestManager.manage.allQuests[questNo].QuestName;
                string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                string descOri = QuestManager.manage.allQuests[questNo].QuestDescription;
                string desc = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, descOri).Replace("<IslandName>", Inventory.Instance.islandName);
                __instance.questTitle.text = name;
                __instance.questDesc.text = desc;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "updatePinnedTask")]
        public static void QuestTracker_updatePinnedTask_Patch(QuestTracker __instance)
        {
            try
            {
                if (__instance.pinnedType == QuestTracker.typeOfTask.Quest)
                {
                    if (!QuestManager.manage.isQuestCompleted[__instance.pinnedId])
                    {
                        string nameOri = QuestManager.manage.allQuests[__instance.pinnedId].QuestName;
                        string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                        string pinText = __instance.pinMissionText.text.Replace(QuestManager.manage.allQuests[__instance.pinnedId].QuestName, name);
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