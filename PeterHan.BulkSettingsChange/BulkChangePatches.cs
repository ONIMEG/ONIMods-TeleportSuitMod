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

using HarmonyLib;
using PeterHan.PLib.Actions;
using PeterHan.PLib.AVC;
using PeterHan.PLib.Core;
using PeterHan.PLib.Database;
using PeterHan.PLib.PatchManager;
using System.Collections.Generic;
using KMod;
using UnityEngine;
using TeleportSuitMod;
using TeleportSuitMod.PeterHan.BulkSettingsChange;
using System;
namespace TeleportSuitMod.PeterHan.BulkSettingsChange
{
    /// <summary>
    /// Patches which will be applied via annotations for Bulk Settings Change.
    /// 
    /// This code took inspiration from https://github.com/0Mayall/ONIBlueprints/
    /// </summary>
    public sealed class BulkChangePatches
    {
        //internal delegate void UpdateForbidden(GameObject go);

        /// <summary>
        /// The action to bring up the bulk change tool.
        /// </summary>
        //internal static PAction BulkChangeAction { get; private set; }

        /// <summary>
        /// Forbid Items support is easy!
        /// </summary>
        //internal static bool CanForbidItems { get; private set; }

        //internal static readonly Tag Forbidden = new Tag("Forbidden");

        //[PLibMethod(RunAt.BeforeDbInit)]
        //internal static void BeforeDbInit() {
        //	var icon = SpriteRegistry.GetToolIcon();
        //	Assets.Sprites.Add(icon.name, icon);
        //}

        //public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods) {
        //	base.OnAllModsLoaded(harmony, mods);
        //	CanForbidItems = PPatchTools.GetTypeSafe(
        //		"PeterHan.ForbidItems.ForbidItemsPatches", "ForbidItems") != null;
        //	if (CanForbidItems)
        //		PUtil.LogDebug("Adding tool for Forbid Items");
        //}


        //[PLibMethod(RunAt.OnEndGame)]
        //internal static void OnEndGame() {
        //	PUtil.LogDebug("Destroying BulkParameterMenu");
        //	BulkParameterMenu.DestroyInstance();
        //}

        /// <summary>
        /// Applied to PlayerController to load the change settings tool into the available
        /// tool list.
        /// </summary>
        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public static class PlayerController_OnPrefabInit_Patch
        {
            internal static void Postfix(PlayerController __instance)
            {
                // Create list so that new tool can be appended at the end
                var interfaceTools = new List<InterfaceTool>(__instance.tools);
                var bulkChangeTool = new GameObject("TeleportRestrictTool");
                var tool = bulkChangeTool.AddComponent<BulkChangeTool>();
                // Reparent tool to the player controller, then enable/disable to load it
                bulkChangeTool.transform.SetParent(__instance.gameObject.transform);
                bulkChangeTool.SetActive(true);
                bulkChangeTool.SetActive(false);
                //PUtil.LogDebug("Created BulkChangeTool");
                // Add tool to tool list
                interfaceTools.Add(tool);
                __instance.tools = interfaceTools.ToArray();
            }
        }

        public static PAction BulkChangeAction { get; set; }

        [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public static class ToolMenu_CreateBasicTools_Patch
        {
            internal static void Postfix(ToolMenu __instance)
            {
                __instance.basicTools.Add(ToolMenu.CreateToolCollection(TeleportSuitStrings.TELEPORT_RESTRICT_TOOL.
                    TOOL_TITLE, TeleportSuitStrings.TELEPORT_RESTRICT_TOOL.TOOL_ICON_NAME, BulkChangeAction.GetKAction(),
                    "TeleportRestrictTool", TeleportSuitStrings.TELEPORT_RESTRICT_TOOL.TOOL_DESCRIPTION, false));
            }
        }


        [HarmonyPatch(typeof(ToolMenu), "OnPrefabInit")]
        public static class ToolMenu_OnPrefabInit_Patch
        {
            internal static void Postfix()
            {
                BulkParameterMenu.CreateInstance();
            }
        }
    }
}
