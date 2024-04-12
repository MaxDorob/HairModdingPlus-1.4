using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Shashlichnik.HairModdingPlus
{
    public class PawnRenderNode_BackLayerHair : PawnRenderNode
    {
        public PawnRenderNode_BackLayerHair(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }
        protected override string TexPathFor(Pawn pawn)
        {
            var hair = pawn.story.hairDef;
            if (hair is HairDefExt)
            {

            }
            return $"{hair.texPath}_back";
        }
        static string[] directions = { "south", "north", "east", "west" };
        public override Graphic GraphicFor(Pawn pawn)
        {
            if (!directions.Any(x => ContentFinder<Texture2D>.Get($"{TexPathFor(pawn)}_{x}", false) != null)) return null;
            return base.GraphicFor(pawn);
        }
    }
}
