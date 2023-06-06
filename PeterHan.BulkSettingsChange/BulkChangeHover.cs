﻿/*
 * Copyright 2023 Peter Han
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using TeleportSuitMod;
using System.Collections.Generic;

namespace TeleportSuitMod.PeterHan.BulkSettingsChange
{
    /// <summary>
    /// The hover popup shown when the bulk change tool is invoked.
    /// </summary>
    internal sealed class BulkChangeHover : HoverTextConfiguration
    {
        public override void UpdateHoverElements(List<KSelectable> selected)
        {
            string key = BulkParameterMenu.Instance.SelectedKey;
            if (key != null)
            {
                var mode = BulkToolMode.FromKey(key);
                var hoverInstance = HoverTextScreen.Instance;
                // Find the active mode
                var drawer = hoverInstance.BeginDrawing();
                // Draw the tool title
                drawer.BeginShadowBar(false);
                drawer.DrawText(mode?.Title ?? TeleportSuitStrings.TELEPORT_RESTRICT_TOOL.TOOL_TITLE, ToolTitleTextStyle);
                // Draw the instructions
                ActionName = mode?.Name ?? TeleportSuitStrings.TELEPORT_RESTRICT_TOOL.TOOL_TITLE;
                DrawInstructions(hoverInstance, drawer);
                drawer.EndShadowBar();
                drawer.EndDrawing();
            }
        }
    }
}
