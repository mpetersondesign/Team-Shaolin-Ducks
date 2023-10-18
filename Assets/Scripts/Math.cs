using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public const float DegToRad = System.MathF.PI / 180;

    public static Vector2 RotateVecRad(Vector2 vec, float rad)
    {
        float c = System.MathF.Cos(rad);
        float s = System.MathF.Sin(rad);

        return new Vector2(vec.x * c - vec.y * s, vec.x * s + vec.y * c);
    }

    public static Vector3 RotateVecRad(Vector3 vec, float rad)
    {
        float c = System.MathF.Cos(rad);
        float s = System.MathF.Sin(rad);

        return new Vector3(vec.x * c - vec.y * s, vec.x * s + vec.y * c, vec.z);
    }

    public static Vector2 RotateVecDeg(Vector2 vec, float deg)
    {
        return RotateVecRad(vec, DegToRad);
    }

    public static Vector3 RotateVecDeg(Vector3 vec, float deg)
    {
        return RotateVecRad(vec, DegToRad);
    }

    public static Vector2 Reflect(Vector2 vec, Vector2 surface_normal)
    {
        Vector2 normal_comp = Dot(vec, surface_normal) * surface_normal;
        Vector2 parallel_comp = vec - normal_comp;

        return parallel_comp - normal_comp;
    }

    public static Vector3 Reflect(Vector3 vec, Vector3 surface_normal)
    {
        Vector3 normal_comp = Dot(vec, surface_normal) * surface_normal;
        Vector3 parallel_comp = vec - normal_comp;

        return parallel_comp - normal_comp;
    }

    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
}
