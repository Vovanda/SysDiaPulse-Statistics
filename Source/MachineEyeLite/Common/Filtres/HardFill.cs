using static SysDiaPulseCore.Methods.Rgb;

namespace SysDiaPulseCore.Common.Filtres
{
    /// <summary>
    /// The filter replaces the pixel color with the target color
    /// if it is close enough otherwise it will fill it with
    /// the background color
    /// </summary>
    public class HardFill: IFilter
    {

        const int absolute_len = 255 * 255 * 3;

        public HardFill(ColorRGB targetColor, ColorRGB backColor, float prop)
        {
            _target = targetColor;
            _back = backColor;
            _prop = prop;
        }

        public void Apply(ref byte[,,] image)
        {
            int height = image.GetLength(1);
            int width = image.GetLength(2);

            int x, y, z;

            //int x1, y1, z1;
            //int x2, y2, z2;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    x = image[0, i, j] - _back.R;
                    y = image[1, i, j] - _back.G;
                    z = image[2, i, j] - _back.B;

                    if ((x * x + y * y + z * z) > _prop * absolute_len)
                    {
                        image[0, i, j] = _target.R;
                        image[1, i, j] = _target.G;
                        image[2, i, j] = _target.B;
                    }
                    else
                    {
                        image[0, i, j] = _back.R;
                        image[1, i, j] = _back.G;
                        image[2, i, j] = _back.B;
                    }
                }
            }
        }

        private readonly ColorRGB _target;

        private readonly ColorRGB _back;

        private readonly float _prop;
    }
}
