using BepInEx;
using I2.Loc;
using HarmonyLib;

namespace DinkumChinese
{
    [BepInPlugin("xiaoye97.Dinkum.DinkumChinese", "DinkumChinese", "1.0.0")]
    public class DinkumChinesePlugin : BaseUnityPlugin
    {
        public static DinkumChinesePlugin Inst;
        void Awake()
        {
            Inst = this;
            Harmony.CreateAndPatchAll(typeof(DinkumChinesePlugin));
        }

        public static void LogInfo(string log)
        {
            Inst.Logger.LogInfo(log);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(OptionsMenu), "Start")]
        public static void OptionsMenuStartPatch()
        {
            LocalizationManager.CurrentLanguage = "Chinese";
        }

        [HarmonyPrefix, HarmonyPatch(typeof(RealWorldTimeLight), "setUpDayAndDate")]
        public static bool RealWorldTimeLight_setUpDayAndDate_Patch(RealWorldTimeLight __instance)
        {
            __instance.seasonAverageTemp = __instance.seasonAverageTemps[WorldManager.manageWorld.month - 1];
            __instance.DayText.text = __instance.getDayName(WorldManager.manageWorld.day - 1);
            __instance.DateText.text = (WorldManager.manageWorld.day + (WorldManager.manageWorld.week - 1) * 7).ToString("00");
            __instance.SeasonText.text = __instance.getSeasonName(WorldManager.manageWorld.month - 1);
            return false;
        }

        public void SetChinese()
        {
            LocalizationManager.CurrentLanguage = "Chinese";
        }
    }
}
