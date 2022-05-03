using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public AnimationCurve animationX, animationY;
    public float duration;
    [Range(0, 200)] public float horizontalDelta = 40;
    private float _startTime;
    private Vector2 _startPose;

    public void Start()
    {
        _startTime = Time.time;
        _startPose = transform.position;
    }

    void Update()
    {
        var t = (Time.time - _startTime) / duration;
        transform.position = new Vector2(
            _startPose.x + horizontalDelta * animationX.Evaluate(t),
            _startPose.y - (Camera.main.rect.height - _startPose.y) * (animationY.Evaluate(t))
        );
        foreach (var text in transform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            var col = text.color;
            col.a = 1 - animationY.Evaluate(t);
            text.color = col;
        }

        if (t > 1)
            Destroy(gameObject);
    }
}