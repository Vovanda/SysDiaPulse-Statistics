﻿

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SysDiaPulseCore.Methods
{
    public static partial class Rgb
    {
        public struct ColorRGB
        {
            public byte R;
            public byte G;
            public byte B;
        }
    }

    //Copying from https://archive.codeplex.com/?p=rasterconversion
    public static partial class Rgb
    {
        /// <summary>
        /// Метод загружает растровые изображения без блокирования файла
        /// (как это делает конструктор Bitmap(fileName)).
        /// </summary>
        /// <param name="fileName">Имя файла для загрузки.</param>
        /// <returns>Экземпляр Bitmap.</returns>
        public static Bitmap LoadBitmap(string fileName)
        {
            using FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            return new Bitmap(fs);
        }

        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат byte[,,].
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Байтовый массив с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe static byte[,,] BitmapToByteRgbQ(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (byte* _res = res)
                {
                    byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++);
                            ++_b;
                            *_g = *(curpos++);
                            ++_g;
                            *_r = *(curpos++);
                            ++_r;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат double[,,].
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Mассив double с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe static double[,,] BitmapToDoubleRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            double[,,] res = new double[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (double* _res = res)
                {
                    double* _r = _res, _g = _res + 1, _b = _res + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++); _b += 3;
                            *_g = *(curpos++); _g += 3;
                            *_r = *(curpos++); _r += 3;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        /// <summary>
        /// Функция предназначена для создания нового экземпляра класса Bitmap на 
        /// базе имеющейся в виде byte[,,]-массива информацией о яркости каждого пиксела.
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="rgb">Byte массив с данными о яркости каждой компоненты
        /// каждого пиксела</param>
        /// <returns>Новый экземпляр класса Bitmap</returns>
     
        public unsafe static Bitmap RgbToBitmap(byte[,,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        *(curpos++) = rgb[2, h, w];
                        *(curpos++) = rgb[1, h, w];
                        *(curpos++) = rgb[0, h, w];
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }

        public unsafe static Bitmap RgbToBitmapQ(byte[,,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                fixed (byte* _rgb = rgb)
                {
                    byte* _r = _rgb, _g = _rgb + width * height, _b = _rgb + 2 * width * height;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *(curpos++) = *_b; ++_b;
                            *(curpos++) = *_g; ++_g;
                            *(curpos++) = *_r; ++_r;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }

        /// <summary>
        /// Функция предназначена для создания нового экземпляра класса Bitmap на 
        /// базе имеющейся в виде byte[,,]-массива информацией о яркости каждого пиксела.
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="rgb">Double массив с данными о яркости каждой компоненты
        /// каждого пиксела</param>
        /// <returns>Новый экземпляр класса Bitmap</returns>
        public unsafe static Bitmap RgbToBitmap(double[,,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                fixed (double* _rgb = rgb)
                {
                    double* _r = _rgb, _g = _rgb + 1, _b = _rgb + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *(curpos++) = Limit(*_b); _b += 3;
                            *(curpos++) = Limit(*_g); _g += 3;
                            *(curpos++) = Limit(*_r); _r += 3;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }

        private static byte Limit(double x)
        {
            if (x < 0)
                return 0;
            if (x > 255)
                return 255;
            return (byte)x;
        }

    }
}
