using System.Reflection;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;

namespace DebugTooltip;

internal class TooltipCopier : MonoBehaviour
{
    private string _shortText;
    private string _altShortText;
    private string _longText;

    private static PropertyInfo SystemCopyBufferProperty;

    public void Awake()
    {
        SystemCopyBufferProperty = AccessTools.Property(typeof(GUIUtility), "systemCopyBuffer");
    }

    public void SetDebugInfo(DebugInfo debugInfo)
    {
        _shortText = StripTags(debugInfo.ToShortString());
        _altShortText = StripTags(debugInfo.ToAltShortString());
        _longText = StripTags(debugInfo.ToString());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl);
            var shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            var altDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

            if (ctrlDown && shiftDown)
            {
                SystemCopyBufferProperty.SetValue(null, _longText, null);
                return;
            }

            if (ctrlDown)
            {
                SystemCopyBufferProperty.SetValue(null, _shortText, null);
            }

            if (altDown)
            {
                SystemCopyBufferProperty.SetValue(null, _altShortText, null);
            }
        }
    }

    private string StripTags(string input)
    {
        return Regex.Replace(input, "</?color[^>]*>", string.Empty);
    }
}
