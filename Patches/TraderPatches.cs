using EFT;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace DebugTooltip
{
    internal class TraderPatches
    {
        public static void Enable()
        {
            new TraderCardEnterPatch().Enable();
            new TraderCardExitPatch().Enable();
        }

        private class TraderCardEnterPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(TraderCard), "UnityEngine.EventSystems.IPointerEnterHandler.OnPointerEnter");
            }

            [PatchPostfix]
            public static void Postfix(Profile.TraderInfo ___traderInfo_0)
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                DebugTooltip.SetDebugInfo(new TraderDebugInfo(___traderInfo_0));
                ItemUiContext.Instance.Tooltip.Show(string.Empty);
            }
        }

        private class TraderCardExitPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(TraderCard), "UnityEngine.EventSystems.IPointerExitHandler.OnPointerExit");
            }

            [PatchPostfix]
            public static void Postfix()
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                ItemUiContext.Instance.Tooltip.Close();
            }
        }
    }
}
