using HarmonyLib;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DebugTooltip
{
    internal class TooltipCopier : MonoBehaviour
    {
        private string shortText;
        private string longText;

        private static PropertyInfo SystemCopyBufferProperty;

        public void Awake()
        {
            SystemCopyBufferProperty = AccessTools.Property(typeof(GUIUtility), "systemCopyBuffer");
        }

        public void SetText(string id, string all)
        {
            this.shortText = StripTags(id);
            this.longText = StripTags(all);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                var ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl);
                var shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                if (ctrlDown && shiftDown)
                {
                    SystemCopyBufferProperty.SetValue(null, longText, null);
                    return;
                }

                if (ctrlDown)
                {
                    SystemCopyBufferProperty.SetValue(null, shortText, null);
                }
            }
        }

        private string StripTags(string input)
        {
            return Regex.Replace(input, "</?color[^>]*>", string.Empty);
        }
    }
}
