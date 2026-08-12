using System.Reflection;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace DebugTooltip;

internal class ShortcutPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(ItemUiContext), nameof(ItemUiContext.Update));
    }

    [PatchPostfix]
    public static void Postfix()
    {
        if (Settings.ToggleShortcut.Value.IsDown())
        {
            Settings.ShowDebugInfo.Value = !Settings.ShowDebugInfo.Value;
        }
    }
}
