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
        public HardFill(ColorRGB targetColor, ColorRGB backColor, int radius)
        {
            _target = targetColor;
            _back = backColor;
            _r = radius;
        }

        public void Apply(ref byte[,,] image)
        {
            int height = image.GetLength(2);
            int width = image.GetLength(3);

            int x, y, z;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    x = image[0, i, j] - _target.R;
                    y = image[1, i, j] - _target.G;
                    z = image[2, i, j] - _target.B;

                    if (x * x + y * y + z * z < _r)
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

        private readonly int _r;
    }
}
