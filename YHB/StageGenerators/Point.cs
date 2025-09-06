using NUnit.Framework;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.StageGenerators
{
    // 안 쓸듯
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
            => obj is Point point && X == point.X && Y == point.Y;

        public override int GetHashCode()
            => HashCode.Combine(X, Y);
        public override string ToString()
            => $"({X}, {Y})";

        /// <summary>
        /// Get distance between two points.
        /// </summary>
        /// <returns>Get Manhattan Distance</returns>
        public static int Distance(Point point1, Point point2)
        {
            int xDistance = Mathf.Abs(point2.X - point1.X);
            int yDistance = Mathf.Abs(point2.Y - point1.Y);

            return xDistance + yDistance;
        }

        public static bool operator ==(Point point1, Point point2)
            => point1.X == point2.X && point1.Y == point2.Y;
        public static bool operator !=(Point point1, Point point2)
            => point1.X != point2.X || point1.Y != point2.Y;

        public static Point operator +(Point point1, Point point2)
            => new Point(point1.X + point2.X, point1.Y + point2.Y);
        public static Point operator -(Point point1, Point point2)
            => new Point(point1.X - point2.X, point1.Y - point2.Y);
    }
}
