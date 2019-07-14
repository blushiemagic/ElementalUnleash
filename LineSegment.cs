using System;
using Microsoft.Xna.Framework;

namespace Bluemagic {
public struct LineSegment
{
    public Vector2 Center;
    public float Angle;
    public float Length;

    public LineSegment(Vector2 center, float angle, float length)
    {
        this.Center = center;
        this.Angle = angle;
        this.Length = length;
    }
}}