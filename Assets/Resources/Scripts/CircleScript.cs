﻿using UnityEngine;
using UnityEngine.Serialization;

public class CircleScript : MonoBehaviour
{
    [Range(1, 100)] public const float Velocity = 14.0f;
    [Range(0.1f, 1)] public float duration = 0.6f;

    private float _elapsed;

    private void Update()
    {
        _elapsed += Time.deltaTime;
        var localScale = transform.localScale;
        localScale = new Vector2(localScale.x,
            localScale.y - localScale.y * _elapsed / (duration * Random.Range(60, 100)));
        transform.localScale = localScale;
        var color = GetComponent<SpriteRenderer>().color;
        color.a = 1 - _elapsed / duration;
        GetComponent<SpriteRenderer>().color = color;
        if (_elapsed > duration)
            Destroy(gameObject);
    }
}