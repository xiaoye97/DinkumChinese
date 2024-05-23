using HarmonyLib;
using I2.Loc;
using I2LocPatch;
using UnityEngine;

namespace DinkumChinese
{
    public static class StartTranslatePatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(AnimalManager), "Start")]
        public static void AnimalManager_Start_Patch()
        {
            var mgr = AnimalManager.manage;
            foreach (var a in mgr.allAnimals)
            {
                a.animalName = TextLocData.GetLoc(DinkumChinesePlugin.Inst.AnimalsTextLocList, a.animalName);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LoadingScreenImageAndTips), "OnEnable")]
        public static void LoadingScreenImageAndTips_OnEnable_Patch(LoadingScreenImageAndTips __instance)
        {
            for (int i = 0; i < __instance.tips.Length; i++)
            {
                string ori = __instance.tips[i];
                for (int j = 0; j < DinkumChinesePlugin.Inst.TipsTextLocList.Count; j++)
                {
                    // 如果已经翻译过，则跳过
                    if (DinkumChinesePlugin.Inst.TipsTextLocList[j].Loc == ori)
                    {
                        goto Finish;
                    }
                }
                string t = TextLocData.GetLoc(DinkumChinesePlugin.Inst.TipsTextLocList, ori);
                if (t == ori)
                {
                    Debug.Log($"LoadingScreenImageAndTips 有待翻译的文本:[{t}]，请添加到TipsTextLocList");
                }
                else
                {
                    __instance.tips[i] = t;
                }
                Finish:;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MailManager), "Start")]
        public static void MailManager_Start_Patch()
        {
            foreach (var item in Resources.FindObjectsOfTypeAll<LetterTemplate>())
            {
                item.letterText = TextLocData.GetLoc(DinkumChinesePlugin.Inst.MailTextLocList, item.letterText);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(OptionsMenu), "Start")]
        public static void OptionsMenuStartPatch()
        {
            LocalizationManager.CurrentLanguage = "Chinese";
        }
    }
}