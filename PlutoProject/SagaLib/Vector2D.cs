using System;

namespace SagaLib
{
    public class Vector2D
    {
        public Vector2D(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }

        public ushort X { get; set; }

        public ushort Y { get; set; }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D((ushort)(a.X + b.X), (ushort)(a.Y + b.Y));
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D((ushort)(b.X - a.X), (ushort)(b.Y - a.Y));
        }

        public static ushort GetDistance(Vector2D a, Vector2D b)
        {
            var c = b - a;
            return (ushort)Math.Sqrt(c.X * c.X + c.Y * c.Y);
        }
    }
}