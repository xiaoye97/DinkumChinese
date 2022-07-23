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
    public static class StringReturnPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameUnderMapCursor")]
        public static void GenerateMap_getBiomeNameUnderMapCursor_Patch(ref string __result)
        {
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result.StrToI2Str());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameById")]
        public static void GenerateMap_getBiomeNameById_Patch(ref string __result, int id)
        {
            GenerateMap.biomNames biomNames = (GenerateMap.biomNames)id;
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, biomNames.ToString());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getTitleText")]
        public static void PostOnBoard_getTitleText_Patch(PostOnBoard __instance,ref string __result, int postId)
        {
            string titleOri = __instance.getPostPostsById().title.StrToI2Str();
            string title = TextLocData.GetLoc(DinkumChinesePlugin.Inst.PostTextLocList, titleOri);
            __result = title.Replace("<boardRewardItem>", 
                __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>", 
                __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<boardRequestItem>", 
                __instance.getPostPostsById().getBoardRequestItem(postId));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getContentText")]
        public static void PostOnBoard_getContentText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            string textOri = __instance.getPostPostsById().contentText.StrToI2Str();
            string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.PostTextLocList, textOri);
            __result = text.Replace("<boardRewardItem>", 
                __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>", 
                __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<getAnimalsInPhotoList>", 
                __instance.getPostPostsById().getRequirementsNeededInPhoto(postId)).Replace("<boardRequestItem>", 
                __instance.getPostPostsById().getBoardRequestItem(postId));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PickUpNotification), "fillButtonPrompt")]
        public static void PickUpNotification_fillButtonPrompt_Patch(PickUpNotification __instance, string buttonPromptText)
        {
            string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, buttonPromptText);
            __instance.itemText.text = text;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AnimalHouseMenu), "fillData")]
        public static void AnimalHouseMenu_fillData_Patch(AnimalHouseMenu __instance)
        {
            __instance.eatenText.text = __instance.eatenText.text.Replace("Eaten", "喂食");
            __instance.shelterText.text = __instance.shelterText.text.Replace("Shelter", "住所");
            __instance.pettedText.text = __instance.pettedText.text.Replace("Petted", "爱抚");
        }
    }
}
