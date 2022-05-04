using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using utiles;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    [SerializeField] public GameObject leaf;
    [SerializeField] public GameObject dropLog;
    private Animator _anim;
    public AnimationCurve clickSpeedToLeafSpeed, clickSpeedToFloatingSpeed;
    public float dropChance = .5f;

    private readonly ArrayList _clicks = new();

    // private float _averageMean = 0f;
    private long _lastClick;
    private float _clickCurrentSpeed;
    private AudioClip[] _rustleAudioClip;
    private AudioClip[] _dropLogAudioClip;

    public float GetSpeed()
    {
        return _clickCurrentSpeed;
    }

    void Start()
    {
        _rustleAudioClip = new[]
        {
            Resources.Load("Raws/rustle_00") as AudioClip,
            Resources.Load("Raws/rustle_01") as AudioClip,
            Resources.Load("Raws/rustle_02") as AudioClip,
            Resources.Load("Raws/rustle_03") as AudioClip,
            Resources.Load("Raws/rustle_04") as AudioClip,
            Resources.Load("Raws/rustle_05") as AudioClip,
            Resources.Load("Raws/rustle_06") as AudioClip
        };
        _dropLogAudioClip = new[]
        {
            Resources.Load("Raws/drop_wood_00") as AudioClip,
            Resources.Load("Raws/drop_wood_01") as AudioClip,
            Resources.Load("Raws/drop_wood_02") as AudioClip,
            Resources.Load("Raws/drop_wood_03") as AudioClip,
            Resources.Load("Raws/drop_wood_04") as AudioClip,
            Resources.Load("Raws/drop_wood_05") as AudioClip
        };
        _anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - _lastClick > 1000)
            _anim.SetTrigger("Down");
    }

    private void OnMouseExit()
    {
        _anim.SetTrigger("Up");
    }

    private void OnMouseUpAsButton()
    {
        if (_clicks.Count > 1 && (long) _clicks[^1] - (long) _clicks[0] > 1000)
            _clicks.RemoveRange(0, _clicks.Count - 2);

        _lastClick = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        _clicks.Add(_lastClick);

        if (_clicks.Count > 1)
        {
            _clickCurrentSpeed = (long) _clicks[^1] - (long) _clicks[^2];
            if (_clickCurrentSpeed > 200)
                _anim.SetTrigger("Bounce1Start");
            else
                _anim.SetTrigger("Bounce2Start");

            _anim.SetFloat("speed", (float) Math.Max(0.6, (330 - _clickCurrentSpeed) / 250f * 1.9f));
            // Debug.Log(speed);
        }
        else
        {
            _anim.SetTrigger("Bounce1Start");
            _anim.SetFloat("speed", 0.6f);
        }

        //Leafs drop
        var leafDropSpeed = _clicks.Count > 1 ? clickSpeedToLeafSpeed.Evaluate(_clickCurrentSpeed) : 0.3f;
        for (int i = -1; i < (int) (leafDropSpeed / 0.6f); i++)
            Instantiate(leaf).GetComponent<LeafDrop>().speed = leafDropSpeed;

        Effect.ClickEffect(Camera.main.ScreenToWorldPoint(Input.mousePosition), utilies.HexToColor("#76E573"));
        Effect.SpawnFloatingText(Input.mousePosition,
            GameObject.Find("DataStorage").GetComponent<DataStorage>().user.ClickPower,
            _clicks.Count > 1 ? clickSpeedToFloatingSpeed.Evaluate(_clickCurrentSpeed) : 2f);

        if (Random.Range(0f, 1f) < dropChance)
        {
            PlaySound(_dropLogAudioClip);
            var log = Instantiate(dropLog);
            log.GetComponent<LogResources>().speed =
                _clicks.Count > 1 ? clickSpeedToLeafSpeed.Evaluate(_clickCurrentSpeed) + 0.1f : 0.4f;
        }
        else
            PlaySound(_rustleAudioClip);

        GameObject.Find("DataStorage").GetComponent<DataStorage>().user.EarnClick();
        GameObject.Find("Header_log_value").GetComponent<Animator>().SetTrigger("Bounce");
    }

    private void PlaySound(IReadOnlyList<AudioClip> audioClips)
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}