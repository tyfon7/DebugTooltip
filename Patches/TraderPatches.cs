using System.Reflection;
using EFT;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace DebugTooltip;

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
            // Fully qualified because declared with the interface in the name
            return AccessTools.Method(typeof(TraderCard), "UnityEngine.EventSystems.IPointerEnterHandler.OnPointerEnter");
        }

        [PatchPostfix]
        public static void Postfix(Profile.TraderInfo ____trader)
        {
            if (!Settings.ShowDebugInfo.Value)
            {
                return;
            }

            DebugTooltip.SetDebugInfo(new TraderDebugInfo(____trader));
            ItemUiContext.Instance.Tooltip.Show(string.Empty);
        }
    }

    private class TraderCardExitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Fully qualified because declared with the interface in the name
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
