
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
    public class PawnRenderNode_FrontLayerHair : PawnRenderNode_BackLayerHair
    {
        static HashSet<HairDef> HairFrontLayer = new HashSet<HairDef>();
        public PawnRenderNode_FrontLayerHair(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }
        static PawnRenderNode_FrontLayerHair()
        {
            foreach (var hair in DefDatabase<HairDef>.AllDefs)
            {
                var hairExt = hair.GetModExtension<HairDefExt>();
                if (hairExt?.noGraphics ?? false)
                {
                    continue;
                }
                if (directions.Any(dir => ContentFinder<Texture2D>.Get($"{hair.texPath}_front_{dir}", false) != null))
                {
                    HairFrontLayer.Add(hair);
                }
            }
        }
        protected override IEnumerable<HairDef> HairsWithLayer => HairFrontLayer;
        protected override string TexPathFor(Pawn pawn)
        {
            var hair = pawn.story.hairDef;
            if (hair?.texPath == null) return null;
            return $"{hair.texPath}_front";
        }
    }
}
