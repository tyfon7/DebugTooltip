using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace DebugTooltip
{
    internal static class TooltipPatches
    {
        private const string CopyHint = "<color=grey>ctrl-c to copy ID, ctrl-shift-c to copy all</color>";

        public static void Enable()
        {
            new TooltipPatch().Enable();
            new PriceTooltipPatch().Enable();
        }

        private class TooltipPatch : ModulePatch
        {
            private static TooltipCopier Copier;

            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.DeclaredMethod(typeof(SimpleTooltip), nameof(SimpleTooltip.Show));
            }

            [PatchPrefix]
            public static void Prefix(SimpleTooltip __instance, ref string text, ref float delay)
            {
                if (__instance != ItemUiContext.Instance.Tooltip || !Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                DebugInfo debugInfo = DebugTooltip.GetDebugInfo();
                if (debugInfo == null)
                {
                    return;
                }

                if (Copier == null)
                {
                    Copier = __instance.GetOrAddComponent<TooltipCopier>();
                }

                Copier.SetText(debugInfo.ToShortString(), debugInfo.ToString());

                // Ain't nobody got time for that
                delay = 0f;

                if (!string.IsNullOrEmpty(text))
                {
                    text += "\n\n";
                }

                text += debugInfo.ToString();

                if (Settings.ShowCopyPrompt.Value)
                {
                    text += CopyHint;
                }

                DebugTooltip.Clear();
            }
        }

        private class PriceTooltipPatch : ModulePatch
        {
            private static TooltipCopier Copier;
            private static TextMeshProUGUI DebugText;

            protected override MethodBase GetTargetMethod()
            {
                Settings.ShowDebugInfo.Subscribe(enabled =>
                {
                    if (!enabled && DebugText != null)
                    {
                        UnityEngine.Object.Destroy(DebugText.gameObject);
                        DebugText = null;
                    }
                });

                return AccessTools.DeclaredMethod(typeof(PriceTooltip), nameof(PriceTooltip.Show));
            }

            [PatchPrefix]
            public static void Prefix(PriceTooltip __instance, TextMeshProUGUI ____price)
            {
                if (__instance != ItemUiContext.Instance.PriceTooltip || !Settings.ShowDebugInfo.Value)
                {
                    return;
                }

                DebugInfo debugInfo = DebugTooltip.GetDebugInfo();
                if (debugInfo == null)
                {
                    return;
                }

                if (Copier == null)
                {
                    Copier = __instance.GetOrAddComponent<TooltipCopier>();
                }

                Copier.SetText(debugInfo.ToShortString(), debugInfo.ToString());

                if (DebugText == null)
                {
                    DebugText = UnityEngine.Object.Instantiate(____price, ____price.transform.parent.parent, false);
                    DebugText.color = Color.white;
                }

                DebugText.text = debugInfo.ToString();

                if (Settings.ShowCopyPrompt.Value)
                {
                    DebugText.text += CopyHint;
                }

                DebugTooltip.Clear();
            }
        }
    }
}
