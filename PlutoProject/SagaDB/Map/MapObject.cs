using System;
using SagaLib;

namespace SagaDB.Map
{
    [Serializable]
    public class MapObject
    {
        private int centerX, centerY;
        private byte dir;
        private BitMask flag;
        private string name;
        private int width, height;
        private byte x, y;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Width
        {
            get => width;
            set => width = value;
        }

        public int Height
        {
            get => height;
            set => height = value;
        }

        public int CenterX
        {
            get => centerX;
            set => centerX = value;
        }

        public int CenterY
        {
            get => centerY;
            set => centerY = value;
        }

        public byte X
        {
            get => x;
            set => x = value;
        }

        public byte Y
        {
            get => y;
            set => y = value;
        }


        public BitMask Flag
        {
            get => flag;
            set => flag = value;
        }

        public byte Dir
        {
            get => dir;
            set => dir = value;
        }

        public MapObject Clone
        {
            get
            {
                var obj = new MapObject();
                obj.Name = name;
                obj.width = width;
                obj.height = height;
                obj.centerX = centerX;
                obj.centerY = centerY;
                obj.flag = new BitMask(flag.Value);
                return obj;
            }
        }

        public int[,][] PositionMatrix
        {
            get
            {
                var basic = new int[width, height][];
                for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    basic[i, j] = new int[2];
                    basic[i, j][0] = i - centerX;
                    basic[i, j][1] = j - centerY;
                }

                var theta = dir * 45 * Math.PI / 180;
                var rotationMatrix = new double[2, 2]
                {
                    { Math.Cos(theta), Math.Sin(theta) },
                    { -Math.Sin(theta), Math.Cos(theta) }
                };

                for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    double x, y;
                    x = Math.Round(rotationMatrix[0, 0] * basic[i, j][0] + rotationMatrix[1, 0] * -basic[i, j][1], 3);
                    y = Math.Round(-(rotationMatrix[0, 1] * basic[i, j][0] + rotationMatrix[1, 1] * -basic[i, j][1]),
                        3);
                    if (x > 0)
                        basic[i, j][0] = (int)Math.Ceiling(x);
                    else
                        basic[i, j][0] = (int)Math.Floor(x);
                    if (y > 0)
                        basic[i, j][1] = (int)Math.Ceiling(y);
                    else
                        basic[i, j][1] = (int)Math.Floor(y);
                }

                return basic;
            }
        }
    }
}