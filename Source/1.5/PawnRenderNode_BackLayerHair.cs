using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Shashlichnik.HairModdingPlus
{
    public class PawnRenderNode_BackLayerHair : PawnRenderNode_Hair
    {
        public PawnRenderNode_BackLayerHair(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }
        static HashSet<HairDef> HairBackLayer = new HashSet<HairDef>(DefDatabase<HairDef>.DefCount);
        static PawnRenderNode_BackLayerHair()
        {
            foreach (var hair in DefDatabase<HairDef>.AllDefs)
            {
                var hairExt = hair.GetModExtension<HairDefExt>();
                if (hairExt?.noGraphics ?? false)
                {
                    continue;
                }
                if (directions.Any(dir => ContentFinder<Texture2D>.Get($"{hair.texPath}_back_{dir}", false) != null || ContentFinder<Texture2D>.Get($"{hair.texPath}_{dir}_back", false) != null))
                {
                    HairBackLayer.Add(hair);
                }
            }
        }
        protected virtual IEnumerable<HairDef> HairsWithLayer => HairBackLayer;
        protected override string TexPathFor(Pawn pawn)
        {
            var hair = pawn.story.hairDef;
            if (hair?.texPath == null) return null;
            return $"{hair.texPath}_back";
        }
        protected internal static string[] directions = { "south", "north", "east", "west" };

        public override Graphic GraphicFor(Pawn pawn)
        {
            var hair = pawn.story.hairDef;
            if (hair == null || pawn.DevelopmentalStage.Baby() || pawn.DevelopmentalStage.Newborn()) return null;
            var hairExt = hair.GetModExtension<HairDefExt>();
            if (hairExt != null && hairExt.noGraphics) return null;
            if (!HairsWithLayer.Contains(hair)) return null;
            string texPath = TexPathFor(pawn);
            if (texPath == null) return null;
            ShaderTypeDef overrideShaderTypeDef = hairExt?.overrideShaderTypeDef ?? hair.overrideShaderTypeDef;
            return GraphicFor(texPath, overrideShaderTypeDef, ColorFor(pawn));
        }
        public virtual Graphic GraphicFor(string texPath, ShaderTypeDef overrideShaderTypeDef, Color color) => GraphicDatabase.Get<Graphic_MultiNoFlip>(texPath, overrideShaderTypeDef?.Shader ?? ShaderDatabase.CutoutHair, Vector2.one, color);
    }
}
