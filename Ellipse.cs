using System;
using Microsoft.Xna.Framework;

namespace Bluemagic {
/*
 * Contains code for ellipse collision detection
 */
public class Ellipse
{
    public static bool Collides(Vector2 ellipsePos, Vector2 ellipseDim, Vector2 boxPos, Vector2 boxDim)
    {
        Vector2 ellipseCenter = ellipsePos + 0.5f * ellipseDim;
        float x = 0f;
        float y = 0f;
        if(boxPos.X > ellipseCenter.X)
        {
            x = boxPos.X - ellipseCenter.X;
        }
        else if(boxPos.X + boxDim.X < ellipseCenter.X)
        {
            x = boxPos.X + boxDim.X - ellipseCenter.X;
        }
        if(boxPos.Y > ellipseCenter.Y)
        {
            y = boxPos.Y - ellipseCenter.Y;
        }
        else if(boxPos.Y + boxDim.Y < ellipseCenter.Y)
        {
            y = boxPos.Y + boxDim.Y - ellipseCenter.Y;
        }
        float a = ellipseDim.X / 2f;
        float b = ellipseDim.Y / 2f;
        return (x * x) / (a * a) + (y * y) / (b * b) < 1;
    }
}}