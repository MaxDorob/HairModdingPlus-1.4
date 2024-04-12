using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Shashlichnik.HairModdingPlus
{
    public class HairModdingPlus : Mod
    {
        public HairModdingPlus(ModContentPack content) : base(content)
        {
            new Harmony(nameof(HairModdingPlus)).PatchAll();
        }
    }
}
