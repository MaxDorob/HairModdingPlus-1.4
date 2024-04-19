using HarmonyLib;
using RimWorld;
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
    [HarmonyPatch(typeof(Widgets),nameof(Widgets.DefIcon))]
    public static class BackLayerForStylingStation_Patch
    {
        static Dictionary<HairDef, Texture2D> textures = new Dictionary<HairDef, Texture2D>();
        public static void Prefix(Rect rect, Def def, float scale, ref Material material)
        {
            if (def is HairDef hairDef)
            {
                if (!textures.TryGetValue(hairDef, out var texture))
                {
                    texture = ContentFinder<Texture2D>.Get($"{hairDef.texPath}_back_south", false) ?? ContentFinder<Texture2D>.Get($"{hairDef.texPath}_south_back", false);
                    textures.Add(hairDef, texture);
                }

                if (texture != null)
                {
                    Widgets.DrawTextureFitted(rect, texture, scale, material);
                }
            }
        }

    }
}
