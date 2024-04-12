using DubsBadHygiene;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    public static class Patch_DubsBadHygiene
    {
		//Somehow Dubs made public variable into private nested class. Hmmm 0_o
		static Type H_GetBodyPos = AccessTools.TypeByName("DubsBadHygiene.Patches.HarmonyPatches").GetNestedType("H_GetBodyPos", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		public static void ChangeOffsetOfBackLayer(Pawn pawn,ref Vector3 onHeadLoc)
        {
			//copied from H_GetBodyPos.Postfix
			if (pawn.CurJob == null)
			{
				return;
			}
			if (pawn.CurJob.def.GetType() != typeof(WashingJobDef))
			{
				return;
			}
			if (!pawn.health.hediffSet.HasHediff(DubDef.Washing, false))
			{
				return;
			}
			//
			//Hope wont hit perfomance
			onHeadLoc.y -= (float)new Traverse(H_GetBodyPos).Field("bodyoffset").GetValue();
		}
    }
}
