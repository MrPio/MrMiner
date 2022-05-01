using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class utilies : MonoBehaviour
{
    /// <summary>
    /// returns a Vector2 containing max x and max y in WorldPoint
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetCameraBounds()
    {
        var orthographicSize = Camera.main.orthographicSize;
        return new Vector2(
            orthographicSize * Screen.width / Screen.height,
            orthographicSize * Screen.height / Screen.width
        );
    }

    public static Vector2 RandomWorldPointInCollider(Collider2D collider)
    {
        Vector2 point = default;
        var count = 0;
        do
        {
            if (++count > 10000)
                throw new Exception("cannot find the point inside the collider within 10_000 tries!");
            var randomViewPortPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
            point = Camera.main.ViewportToWorldPoint(randomViewPortPoint);
        } while (!collider.OverlapPoint(point));

        return point;
    }
 
    public static Color HexToColor(string hex)
    {
        return ColorUtility.TryParseHtmlString(hex, out var newCol) ? newCol : Color.black;
    }
}