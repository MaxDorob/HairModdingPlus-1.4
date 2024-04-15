using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Shashlichnik.HairModdingPlus
{
    public class Graphic_MultiNoFlip : Graphic_Multi
    {
        public override void Init(GraphicRequest req)
        {
            this.data = req.graphicData;
            this.path = req.path;
            this.maskPath = req.maskPath;
            this.color = req.color;
            this.colorTwo = req.colorTwo;
            this.drawSize = req.drawSize;
            Texture2D[] array = new Texture2D[this.mats.Length];
            array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
            array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
            array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
            array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
            if (array[0] == null)
            {
                array[0] = ContentFinder<Texture2D>.Get(req.path, false);
            }
            if (array.All(x => x == null))
            {
                Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>());
                return;
            }
            Texture2D[] array2 = new Texture2D[this.mats.Length];
            if (req.shader.SupportsMaskTex())
            {
                string str = this.maskPath.NullOrEmpty() ? this.path : this.maskPath;
                string str2 = this.maskPath.NullOrEmpty() ? "m" : string.Empty;
                array2[0] = ContentFinder<Texture2D>.Get(str + "_north" + str2, false);
                array2[1] = ContentFinder<Texture2D>.Get(str + "_east" + str2, false);
                array2[2] = ContentFinder<Texture2D>.Get(str + "_south" + str2, false);
                array2[3] = ContentFinder<Texture2D>.Get(str + "_west" + str2, false);
            }
            for (int i = 0; i < this.mats.Length; i++)
            {
                if (array[i] == null)
                {
                    mats[i] = BaseContent.BadMat;
                    continue;
                }
                MaterialRequest req2 = default(MaterialRequest);
                req2.mainTex = array[i];
                req2.shader = req.shader;
                req2.color = this.color;
                req2.colorTwo = this.colorTwo;
                req2.maskTex = array2[i];
                req2.shaderParameters = req.shaderParameters;
                req2.renderQueue = req.renderQueue;
                this.mats[i] = MaterialPool.MatFrom(req2);
            }
        }
    }
}
