using EFT.UI;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace DebugTooltip
{
    internal class ItemPatches
    {
        public static void Enable()
        {
            new GridItemViewPatch().Enable();
            new TradingItemViewPatch().Enable();
            new ModSlotViewPatch().Enable();
            new ModSlotArmorPatch().Enable();
        }

        private class GridItemViewPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(GridItemView), nameof(GridItemView.ShowTooltip));
            }

            [PatchPrefix]
            public static void Prefix(GridItemView __instance)
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                DebugTooltip.SetDebugInfo(new ItemDebugInfo(__instance.Item));
            }
        }

        private class TradingItemViewPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(TradingItemView), nameof(TradingItemView.ShowTooltip));
            }

            [PatchPrefix]
            public static void Prefix(TradingItemView __instance)
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                DebugTooltip.SetDebugInfo(new ItemDebugInfo(__instance.Item));
            }
        }

        private class ModSlotViewPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.DeclaredMethod(typeof(ModSlotView), nameof(ModSlotView.OnPointerEnter));
            }

            [PatchPrefix]
            public static void Prefix(ModSlotView __instance)
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                if (__instance.ContainedItemView == null)
                {
                    DebugTooltip.SetDebugInfo(new EmptySlotDebugInfo(__instance.Slot));
                }
            }

            [PatchPostfix]
            public static void Postfix(ModSlotView __instance)
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                if (__instance.ContainedItemView == null && !ItemUiContext.Instance.Tooltip.isActiveAndEnabled)
                {
                    ItemUiContext.Instance.Tooltip.Show(string.Empty);
                }
            }
        }

        private class ModSlotArmorPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(ModSlotView), nameof(ModSlotView.method_16));
            }

            [PatchPostfix]
            public static void Postfix(ModSlotView __instance, ref bool __result)
            {
                if (!Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                if (__result)
                {
                    DebugTooltip.SetDebugInfo(new ItemDebugInfo(__instance.Slot.ContainedItem));
                }
            }
        }
    }
}
