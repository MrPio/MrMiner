using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LeafDrop : MonoBehaviour
{
    public AnimationCurve moveCurveX, moveCurveY, rotateCurveZ;
    private Vector3 startPoint;
    public float _finalPosY = 1f;
    private PolygonCollider2D TreeCollider;
    [Range(0.001f, 1f)] public float speed;
    private float _timeStart;
    public Vector2 durationRange;
    private float duration;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        TreeCollider = GameObject.Find("LeafSpawnZone").GetComponent<PolygonCollider2D>();
        startPoint = utilies.RandomWorldPointInCollider(TreeCollider);
        transform.position = startPoint;

        // _finalPosY += Random.Range(-1f, 1f) * utilies.GetCameraBounds().y * 0.025f;

        var keyframes = rotateCurveZ.keys;
        keyframes[^1].value = Random.Range(-1f, 1f);
        rotateCurveZ.keys = keyframes;

        GetComponent<SpriteRenderer>().flipX = Random.Range(0f, 1f) < 0.5;
        GetComponent<SpriteRenderer>().flipY = Random.Range(0f, 1f) < 0.5;

        duration = Random.Range(durationRange.x, durationRange.y);
        _spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale *= Random.Range(0.8f, 1.2f);
        
        _timeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var t = Time.time - _timeStart;
        var xFin = utilies.GetCameraBounds().x * 0.06f;
        var pos = new Vector3(
            startPoint.x + xFin * moveCurveX.Evaluate(t * speed),
            startPoint.y - _finalPosY  * moveCurveY.Evaluate(t * speed),
            0f);
        var rotation = transform.rotation;
        var angle = Quaternion.Euler(rotation.x, rotation.y,
            45f * rotateCurveZ.Evaluate(t * speed));
        transform.SetPositionAndRotation(pos, angle);

        if (t > duration)
        {
            var color = _spriteRenderer.color;
            color.a = 1 - moveCurveY.Evaluate((t - duration) * speed*1.5f);
            _spriteRenderer.color = color;
        }

        if (t > duration + 1 / (speed * 1.5f))
            Destroy(gameObject);
    }
}