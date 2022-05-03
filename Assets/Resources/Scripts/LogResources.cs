using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using utiles;
using Random = UnityEngine.Random;

public class LogResources : MonoBehaviour
{
    public AnimationCurve moveCurveX, moveCurveY, rotateCurveZ;
    [Range(0.001f, 1f)] public float speed = .5f;
    private float _timeStart;
    public float finalPosY = 1.2f;
    private Vector2 _startPoint;
    private bool _left;
    private float _totalAngle;
    private float _finalPosX;

    void Start()
    {
        _left = Random.Range(0f, 1f) < .5f;
        var spawnZone = GameObject.Find(_left ? "LogSpawnZone_001" : "LogSpawnZone_002");
        _startPoint = utilies.RandomWorldPointInCollider(spawnZone.GetComponent<PolygonCollider2D>());
        GetComponent<SpriteRenderer>().flipX = Random.Range(0f, 1f) < .5f;
        _totalAngle = Random.Range(360 * 2f, 360 * 3f);

        _finalPosX = Random.Range(0, utilies.GetCameraBounds().x * 0.8f);
        _finalPosX = _left ? -_finalPosX : _finalPosX;
        finalPosY += Random.Range(-0.3f, 0.3f);

        transform.localScale *= Random.Range(0.8f, 1.2f);

        _timeStart = Time.time;
    }

    void Update()
    {
        var t = Time.time - _timeStart;
        if(t * speed>1)
            return;
        
        var pos = new Vector3(
            _startPoint.x + (_finalPosX - _startPoint.x) * moveCurveX.Evaluate(t * speed),
            _startPoint.y + (finalPosY - _startPoint.y) * moveCurveY.Evaluate(t * speed),
            0f);
        var rotation = transform.rotation;
        var angle = Quaternion.Euler(rotation.x, rotation.y,
            _totalAngle * rotateCurveZ.Evaluate(t * speed));
        transform.SetPositionAndRotation(pos, angle);
    }

    private void OnMouseEnter()
    {
        transform.localScale *= 1.3f;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        transform.localScale /= 1.3f;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnMouseUp()
    {
        GameObject.Find("Main Camera").GetComponent<AudioSource>()
            .PlayOneShot(Resources.Load("Raws/badge") as AudioClip);
        Effect.ClickEffect(Camera.main.ScreenToWorldPoint(Input.mousePosition),utilies.HexToColor("#F8DB95"));
        GameObject.Find("Header_log_value").GetComponent<Animator>().SetTrigger("Bounce");
        GameObject.Find("Header_log_value").GetComponent<ColorFade>().FadeToColor(utilies.HexToColor("#FFFD73"), typeof(TextMeshProUGUI));
        Destroy(gameObject);
    }
}