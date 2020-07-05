using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SysDiaPulseCore.Common.Filtres
{
    class FastHough
    {
        public void Apply(ref byte[,,] image)
        {
            int height = image.GetLength(2);
            int width = image.GetLength(3);
        }

        public byte[,,] fht2(byte[,,] image)
        {

        }

        public byte[,,] mergeHT(byte[,,] h0, byte[,,] h1)
        {
            int o = h0.GetLength(1);
            int m = h0.GetLength(2); //height
            int n0 = h0.GetLength(3); //width
            int n = 2 * n0;
            byte[,,] h = new byte[o, m, n];
            float r = (n0 - 1) / (n - 1);
            int t0, s;
            for (int i = 1, t = 0; i < n; i++, t++)
            {
                t0 = (int)(t * r + 0.5);
                s = t - t0;
            }
            return h;
        }
    }
}
