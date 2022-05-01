using UnityEngine;

public class Circle
{
    public float Radius { get; }
    public Vector2 Center { get; }

    public Circle(float radius, Vector2 center)
    {
        this.Radius = radius;
        this.Center = center;
    }
    public Circle(Vector2 center)
    {
        Radius = 0f;
        this.Center = center;
    }


    public Vector2 GetPointFromAngle(float angleRadiants)
    {
        var vec= new Vector2((Mathf.Cos(angleRadiants) * Radius + Center.x),
                           (Mathf.Sin(angleRadiants) * Radius + Center.y));
        return vec;
    }

    public float GetAngleByPoint(Vector2 point)
    {
        float radius = Mathf.Sqrt(Mathf.Pow(Center.x - point.x, 2) + Mathf.Pow(Center.y - point.y, 2));
        if (point.y > Center.y)
            return -(Mathf.Acos((+point.x - Center.x) / radius) / Mathf.PI * 180f);
        return Mathf.Acos((+point.x - Center.x) / radius) / Mathf.PI * 180f;
    }

    public float GetDistaceFromCenter(Vector2 point)
    {
        return Mathf.Sqrt(Mathf.Pow(Center.x - point.x, 2) + Mathf.Pow(Center.y - point.y, 2));
    }
}
