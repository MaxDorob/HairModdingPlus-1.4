using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Shashlichnik.HairModdingPlus
{
    [HarmonyPatch(typeof(Graphic_Multi), nameof(Graphic_Multi.Init))]
    public static class Patch_BackwardCompatibility
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var _instructions = instructions.ToList();
            bool firstPatch = false;
            for (int i = 0; i < _instructions.Count; i++)
            {
                if (_instructions[i].opcode == OpCodes.Ldloc_0 && _instructions[i + 1].opcode == OpCodes.Ldc_I4_0 && _instructions[i + 3].opcode == OpCodes.Ldnull && !firstPatch)
                {
                    Log.Warning("Called 1");
                    firstPatch = true;
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return CodeInstruction.Call(typeof(Patch_BackwardCompatibility), nameof(MyMethod));
                }
                if (_instructions[i].opcode == OpCodes.Ldloc_1 && _instructions[i + 1].opcode == OpCodes.Ldc_I4_0 && _instructions[i + 3].opcode == OpCodes.Ldnull)
                {
                    Log.Warning("Called 2");
                    yield return new CodeInstruction(OpCodes.Ldloc_2);//str1
                    yield return new CodeInstruction(OpCodes.Ldloc_3);//str2
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return CodeInstruction.Call(typeof(Patch_BackwardCompatibility), nameof(MyMethod2));
                }
                yield return _instructions[i];
            }
        }
        public static void MyMethod(GraphicRequest req, Texture2D[] array)
        {
            var path = req.path;
            Log.Message(1);
            if (array[0] != null || array[1] != null || array[2] != null || array[3] != null || !path.Contains("_back")) return;
            Log.Message(2);
            var newPath = path.Replace("_back", "");
            array[0] = ContentFinder<Texture2D>.Get(newPath + "_north" + "_back", false);
            array[1] = ContentFinder<Texture2D>.Get(newPath + "_east" + "_back", false);
            array[2] = ContentFinder<Texture2D>.Get(newPath + "_south" + "_back", false);
            array[3] = ContentFinder<Texture2D>.Get(newPath + "_west" + "_back", false);
            if (array[0] != null)
            {
                Log.WarningOnce($"Please, change name of {newPath}", 26372458);
            }
        }
        public static void MyMethod2(string path, string str2, Texture2D[] array)
        {
            Log.Message(3);
            if (array[0] != null || array[1] != null || array[2] != null || array[3] != null || !path.Contains("_backm")) return;
            Log.Message(4);
            var newPath = path.Replace("_back", "");
            array[0] = ContentFinder<Texture2D>.Get(newPath + "_north" + "_backm" + str2, false);
            array[1] = ContentFinder<Texture2D>.Get(newPath + "_east" + "_backm" + str2, false);
            array[2] = ContentFinder<Texture2D>.Get(newPath + "_south" + "_backm" + str2, false);
            array[3] = ContentFinder<Texture2D>.Get(newPath + "_west" + "_backm" + str2, false);
            if (array[0] != null)
            {
                Log.WarningOnce($"Please, change name of {newPath}", 26372458);
            }
        }
    }
}
