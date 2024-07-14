using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine.EventSystems;

namespace DebugTooltip
{
    internal class QuestPatches
    {
        public static void Enable()
        {
            new QuestListItemPatch().Enable();
            new NotesTaskPatch().Enable();
        }

        private class QuestListItemPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(QuestListItem), nameof(QuestListItem.Init));
            }

            [PatchPostfix]
            public static void Postfix(QuestListItem __instance, QuestClass quest)
            {
                var hoverTrigger = __instance.GetOrAddComponent<HoverTrigger>();

                void OnHoverStart(PointerEventData eventData)
                {
                    if (Settings.ShowDebugInfo.Value)
                    {
                        DebugTooltip.SetDebugInfo(new QuestDebugInfo(quest));
                        ItemUiContext.Instance.Tooltip.Show(string.Empty);
                    }
                }

                hoverTrigger.OnHoverStart += OnHoverStart;
                new R.UIElement(__instance).UI.AddDisposable(() => hoverTrigger.OnHoverStart -= OnHoverStart);

                hoverTrigger.OnHoverEnd -= OnHoverEnd;
                hoverTrigger.OnHoverEnd += OnHoverEnd;
            }

            private static void OnHoverEnd(PointerEventData eventData)
            {
                ItemUiContext.Instance.Tooltip.Close();
            }
        }

        private class NotesTaskPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.DeclaredMethod(typeof(NotesTask), nameof(NotesTask.Show));
            }

            [PatchPostfix]
            public static void Postfix(NotesTask __instance, QuestClass quest)
            {
                var hoverTrigger = __instance.GetOrAddComponent<HoverTrigger>();

                void OnHoverStart(PointerEventData eventData)
                {
                    if (Settings.ShowDebugInfo.Value)
                    {
                        DebugTooltip.SetDebugInfo(new QuestDebugInfo(quest));
                        ItemUiContext.Instance.Tooltip.Show(string.Empty);
                    }
                }

                hoverTrigger.OnHoverStart += OnHoverStart;
                new R.UIElement(__instance).UI.AddDisposable(() => hoverTrigger.OnHoverStart -= OnHoverStart);

                hoverTrigger.OnHoverEnd -= OnHoverEnd;
                hoverTrigger.OnHoverEnd += OnHoverEnd;
            }

            private static void OnHoverEnd(PointerEventData eventData)
            {
                ItemUiContext.Instance.Tooltip.Close();
            }
        }
    }
}
