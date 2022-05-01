using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ShopItem : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler
{
    public AnimationCurve onPressCurve;
    public float onPressAnimationDuration = 0.5f;
    private bool _onDownAnimation = false;
    private bool _onUpAnimation = false;

    private float _onPressStartTime;
    private float _onPressAnimationScale;
    public static AudioClip MouseDownAudioClip;
    public static AudioClip MouseUpAudioClip;

    void Start()
    {
        MouseDownAudioClip = Resources.Load("Raws/click_down_003") as AudioClip;
        MouseUpAudioClip = Resources.Load("Raws/click_up_002") as AudioClip;
        _onPressAnimationScale = 1 / onPressAnimationDuration;
    }

    void Update()
    {
        if (!_onDownAnimation && !_onUpAnimation)
            return;
        var t = Time.time - _onPressStartTime;

        float localY=default;
        if (_onDownAnimation)
            localY = -15f * onPressCurve.Evaluate(t * _onPressAnimationScale);
        else if (_onUpAnimation)
            localY = -15f + 15f * onPressCurve.Evaluate(t * _onPressAnimationScale);

        transform.Find("ShopItem_base").localPosition = new Vector2(0, localY);
        
        if (t * _onPressAnimationScale > 1)
        {
            _onDownAnimation = false;
            _onUpAnimation = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<AudioSource>().PlayOneShot(MouseDownAudioClip);
        _onDownAnimation = true;
        _onUpAnimation = false;
        _onPressStartTime = Time.time;

        transform.Find("ShopItem_base").Find("ShopItem_highlight").gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<AudioSource>().PlayOneShot(MouseUpAudioClip);
        _onDownAnimation = false;
        _onUpAnimation = true;
        _onPressStartTime = Time.time;

        transform.Find("ShopItem_base").Find("ShopItem_highlight").gameObject.SetActive(false);
    }
    
}