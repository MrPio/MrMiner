using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utiles;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    public GameObject leaf;
    public GameObject dropLog;
    public AnimationCurve clickSpeedToLeafSpeed, clickSpeedToFloatingSpeed;
    public float dropChance = .5f;
    
    private Animator _anim;
    private readonly ArrayList _clicks = new();
    private long _lastClick;
    private float _clickCurrentSpeed;
    private AudioClip[] _rustleAudioClip;
    private AudioClip[] _dropLogAudioClip;
    private static readonly int Down = Animator.StringToHash("Down");
    private static readonly int Up = Animator.StringToHash("Up");
    private static readonly int Bounce1Start = Animator.StringToHash("Bounce1Start");
    private static readonly int Bounce2Start = Animator.StringToHash("Bounce2Start");
    private static readonly int Speed = Animator.StringToHash("speed");
    private Camera _camera;
    private User _user;
    private static readonly int Bounce = Animator.StringToHash("Bounce");
    private Animator _headerLogValueAnimator;

    private void Start()
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
        _camera = Camera.main;
        _user = GameObject.Find("DataStorage").GetComponent<DataStorage>().user;
        _headerLogValueAnimator = GameObject.Find("Header_log_value").GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - _lastClick > 1000)
            _anim.SetTrigger(Down);
    }

    private void OnMouseExit()
    {
        _anim.SetTrigger(Up);
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
            _anim.SetTrigger(_clickCurrentSpeed > 200 ? Bounce1Start : Bounce2Start);
            _anim.SetFloat(Speed, (float) Math.Max(0.6, (330 - _clickCurrentSpeed) / 250f * 1.9f));
        }
        else
        {
            _anim.SetTrigger(Bounce1Start);
            _anim.SetFloat(Speed, 0.6f);
        }

        //Leafs drop
        var leafDropSpeed = _clicks.Count > 1 ? clickSpeedToLeafSpeed.Evaluate(_clickCurrentSpeed) : 0.3f;
        for (var i = -1; i < (int) (leafDropSpeed / 0.6f); i++)
            Instantiate(leaf).GetComponent<LeafDrop>().speed = leafDropSpeed;

        Effect.ClickEffect(_camera.ScreenToWorldPoint(Input.mousePosition), utilies.HexToColor("#76E573"));
        Effect.SpawnFloatingText(Input.mousePosition,
            _user.ClickPower,
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

        _user.EarnClick();
        _headerLogValueAnimator.SetTrigger(Bounce);
    }

    private void PlaySound(IReadOnlyList<AudioClip> audioClips)
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}