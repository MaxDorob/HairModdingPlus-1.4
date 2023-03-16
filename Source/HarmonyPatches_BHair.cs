﻿using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    [StaticConstructorOnStartup]
    public class HarmonyPatches_BHair
    {
        public static bool loadedAlienRace = false;
        public static bool loadedRimWorldChildren = false;
        public static bool loadedBabiesAndChildren = false;
        public static bool loadedGradientHair = false;
        public static bool loadedShowHair = false;
        public static bool loadedHatDisplaySelection = false;

        public static bool colonistBarFirstDraw = true;

        static HarmonyPatches_BHair()
        {
            Harmony harmony = new Harmony(id: "butterfish.hairmoddingplus");
            Harmony.DEBUG = false;

            /*foreach (ModContentPack mod in LoadedModManager.RunningModsListForReading)
            {
                Log.Message("Butterfish: Loaded mod " + mod.PackageId);
            }*/
            

            harmony.Patch(original: AccessTools.Method(type: typeof(PawnGraphicSet),
                                                       name: "ResolveAllGraphics"),
                          prefix: null,
                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_Core),
                                                     methodName: "UseModifiedGraphicParams"));

            harmony.Patch(original: AccessTools.Method(type: typeof(PawnRenderer),
                                                       name: "RenderPawnInternal"), 
                          prefix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_Core),
                                                    methodName: "RecalcRootLocY"), 
                          postfix: null);

            harmony.Patch(original: AccessTools.Method(type: typeof(PawnRenderer),
                                                       name: "DrawHeadHair"),
                          prefix: null,
                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_Core),
                                                     methodName: "DrawBackHairLayer"));

            try
            {
                ((Action)(() =>
                {
                    if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "edb.preparecarefully"))
                    {
                        harmony.Patch(original: AccessTools.Method(type: typeof(EdB.PrepareCarefully.ProviderPawnLayers),
                                                                   name: "InitializeDefaultPawnLayers"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_PrepareCarefully),
                                                                 methodName: "PCCompat_AddHairColor2Layer"));

                        harmony.Patch(original: AccessTools.Method(type: typeof(EdB.PrepareCarefully.ProviderPawnLayers),
                                                                   name: "InitializeAlienPawnLayers"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_PrepareCarefully),
                                                                 methodName: "PCCompat_AddHairColor2Layer"));

                        harmony.Patch(original: AccessTools.Method(type: typeof(EdB.PrepareCarefully.ProviderPawnLayers),
                                                                   name: "InitializeHairOptions"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_PrepareCarefully),
                                                                 methodName: "PCCompat_GetHairOptions"));

                        harmony.Patch(original: AccessTools.Method(type: typeof(EdB.PrepareCarefully.PawnLayerHair),
                                                                   name: "GetSelectedColor"),
                                      prefix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_PrepareCarefully),
                                                                 methodName: "PCCompat_GetSelectedColor"),
                                      postfix: null);

                        harmony.Patch(original: AccessTools.Method(type: typeof(EdB.PrepareCarefully.PawnLayerHair),
                                                                   name: "SelectColor"),
                                      prefix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_PrepareCarefully),
                                                                 methodName: "PCCompat_SelectColor"),
                                      postfix: null);

                        harmony.Patch(original: AccessTools.Method(type: typeof(EdB.PrepareCarefully.DefaultPawnCompRules),
                                                                   name: "InitializeRulesForSaving"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_PrepareCarefully),
                                                                 methodName: "PCCompat_IncludeCompToSave"));
                    }
                }))();
            }
            catch (TypeLoadException) { }

            try
            {
                ((Action)(() =>
                {
                    if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "erdelf.humanoidalienraces"))
                    {
                        loadedAlienRace = true;

                        harmony.Patch(original: AccessTools.Method(type: typeof(AlienRace.HarmonyPatches),
                                                                   name: "GetPawnHairMesh"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_AlienRace),
                                                                 methodName: "ARCompat_CopyModifiedPawnHairMesh"));
                    }
                }))();
            }
            catch (TypeLoadException) { }

            //try
            //{
            //    ((Action)(() =>
            //    {
            //        if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "children.and.pregnancy"))
            //        {
            //            loadedRimWorldChildren = true;

            //            harmony.Patch(original: AccessTools.Method(type: typeof(RimWorldChildren.PawnRenderer_RenderPawnInternal_Patch),
            //                                                       name: "GetHairMesh"),
            //                          prefix: null,
            //                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_RimWorldChildren),
            //                                                     methodName: "RCCompat_CopyModifiedPawnHairMesh"));
            //        }
            //    }))();
            //}
            //catch (TypeLoadException) { }

            //try
            //{
            //    ((Action)(() =>
            //    {
            //        if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "babies.and.children.continued"))
            //        {
            //            loadedBabiesAndChildren = true;
            //        }

            //        if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "babies.and.children.continued.13"))
            //        {
            //            loadedBabiesAndChildren = true;

            //            harmony.Patch(original: AccessTools.Method(type: typeof(BabiesAndChildren.Harmony.ChildrenSizePatch),
            //                                                       name: "PawnRenderer_DrawHairHead_Patch"),
            //                          prefix: null,
            //                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_BabiesAndChildren),
            //                                                     methodName: "BCCompat_CopyModifiedPawnHairMesh"));
            //        }
            //    }))();
            //}
            //catch (TypeLoadException) { }

            try
            {
                ((Action)(() =>
                {
                    if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "showhair.kv.rw"))
                    {
                        loadedShowHair = true;

                        harmony.Patch(original: AccessTools.Method(type: typeof(ShowHair.Patch_PawnRenderer_DrawHeadHair),
                                                                   name: "HideHats"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_ShowHair),
                                                                 methodName: "SHCompat_CopyHideHair"));

                        harmony.Patch(original: AccessTools.Method(type: typeof(ShowHair.HairUtilityFactory).GetNestedType("HairUtility", BindingFlags.Static | BindingFlags.NonPublic),
                                                                   name: "TryGetCustomHairMat"),
                                      prefix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_ShowHair),
                                                                methodName: "SHCompat_OverrideTryGetCustomHairMat"),
                                      postfix: null);
                    }
                }))();
            }
            catch (TypeLoadException) { }

            //try
            //{
            //    ((Action)(() =>
            //    {
            //        if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "killface.facialstuff"))
            //        {
            //            harmony.Patch(original: AccessTools.Method(type: typeof(FacialStuff.CompBodyAnimator),
            //                                                       name: "DrawBody"),
            //                          prefix: null,
            //                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_FacialStuff),
            //                                                     methodName: "FSCompat_GetBaseDrawLocY"));

            //            harmony.Patch(original: AccessTools.Method(type: typeof(FacialStuff.HumanHeadDrawer),
            //                                                       name: "DrawHairAndHeadGear",
            //                                                       parameters: new Type[] { typeof(Vector3), typeof(Vector3), typeof(RotDrawMode), typeof(Quaternion), typeof(bool), typeof(bool), typeof(Vector3) } ),
            //                          prefix: null,
            //                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_FacialStuff),
            //                                                     methodName: "FSCompat_DrawBackHairLayer"));

            //            harmony.Unpatch(original: AccessTools.Method(type: typeof(PawnRenderer),
            //                                                         name: "RenderPawnInternal"),
            //                            type: HarmonyPatchType.Prefix,
            //                            harmonyID: "butterfish.hairmoddingplus");

            //            harmony.Unpatch(original: AccessTools.Method(type: typeof(PawnRenderer),
            //                                                         name: "DrawHeadHair"),
            //                            type: HarmonyPatchType.Postfix,
            //                            harmonyID: "butterfish.hairmoddingplus");
            //        }
            //    }))();
            //}
            //catch (TypeLoadException) { }

            try
            {
                ((Action)(() =>
                {
                    if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "velc.hatsdisplayselection"))
                    {
                        loadedHatDisplaySelection = true;

                        //Hats Display Selection overwrites the calling of DrawHeadHair, so call from RenderPawnInternal instead

                        harmony.Patch(original: AccessTools.Method(type: typeof(PawnRenderer),
                                                                   name: "RenderPawnInternal"),
                                      prefix: null,
                                      postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_HatDisplaySelection),
                                                                 methodName: "HDCompat_DrawBackHairLayer"));

                        harmony.Unpatch(original: AccessTools.Method(type: typeof(PawnRenderer),
                                                                     name: "DrawHeadHair"),
                                        type: HarmonyPatchType.Postfix,
                                        harmonyID: "butterfish.hairmoddingplus");
                    }
                }))();
            }
            catch (TypeLoadException) { }

            if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId.Replace("_steam", "").Replace("_copy", "") == "automatic.gradienthair"))
            {
                loadedGradientHair = true;

                harmony.Patch(original: AccessTools.Method(type: typeof(Page_ConfigureStartingPawns),
                                                           name: "DrawPawnList"),
                          prefix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_Core),
                                                    methodName: "PreloadCacheBugfix"),
                          postfix: null);

                harmony.Patch(original: AccessTools.Method(type: typeof(ColonistBar),
                                                           name: "ColonistBarOnGUI"),
                          prefix: null,
                          postfix: new HarmonyMethod(methodType: typeof(ButterfishHairModdingPlus.Patch_Core),
                                                     methodName: "OnLoadPortraitsBugfix"));
            }

        }
    }
}
