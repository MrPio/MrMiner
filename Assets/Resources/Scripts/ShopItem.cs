using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Vector2 = UnityEngine.Vector2;

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
    public static AudioClip NotEnoughMoney;
    public static AudioClip Bought;
    public int index = 0;
    private float _shopBaseInitialLocalPosY;

    void Start()
    {
        MouseDownAudioClip = Resources.Load("Raws/click_down_003") as AudioClip;
        MouseUpAudioClip = Resources.Load("Raws/click_up_002") as AudioClip;
        Bought = Resources.Load("Raws/buy") as AudioClip;
        NotEnoughMoney = Resources.Load("Raws/fx_no_buy") as AudioClip;
        _onPressAnimationScale = 1 / onPressAnimationDuration;
        _shopBaseInitialLocalPosY = transform.Find("ShopItem_base").localPosition.y;
    }

    void Update()
    {
        if (!_onDownAnimation && !_onUpAnimation)
            return;
        var t = Time.time - _onPressStartTime;

        float localY = default;
        if (_onDownAnimation)
            localY = -15f * onPressCurve.Evaluate(t * _onPressAnimationScale);
        else if (_onUpAnimation)
            localY = -15f + 15f * onPressCurve.Evaluate(t * _onPressAnimationScale);

        transform.Find("ShopItem_base").localPosition = new Vector2(0, _shopBaseInitialLocalPosY + localY);

        if (t * _onPressAnimationScale > 1)
        {
            _onDownAnimation = false;
            _onUpAnimation = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Buy();
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

    public void TurnUpdateMode()
    {
        var mode = false; //todo

        var shopBase = transform.Find("ShopItem_base").gameObject;
        var shopShadow = transform.Find("ShopItem_shadow").gameObject;
        var rectBase = shopBase.GetComponent<RectTransform>().rect;
        var rectShadow = shopShadow.GetComponent<RectTransform>().rect;
        var rectPrice = shopBase.transform.Find("ShopItem_price").GetComponent<RectTransform>().rect;
        var rectLog = shopBase.transform.Find("ShopItem_log").GetComponent<RectTransform>().rect;

        if (mode)
        {
            rectBase.height = 218;
            rectShadow.height = 218;
            rectPrice.y = -82;
            rectLog.y = -115;
        }
        else
        {
            rectBase.height = 177;
            rectShadow.height = 177;
            rectPrice.y = -95;
            rectLog.y = -128;
        }

        shopBase.GetComponent<RectTransform>().sizeDelta = new Vector2(rectBase.width, rectBase.height);
        shopShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(rectShadow.width, rectShadow.height);
        shopBase.transform.Find("ShopItem_price").GetComponent<RectTransform>().position =
            new Vector2(rectPrice.x, rectPrice.y);
        shopBase.transform.Find("ShopItem_log").GetComponent<RectTransform>().position =
            new Vector2(rectLog.x, rectLog.y);
        ;
    }

    public void TurnAvailability(bool force=false)
    {
        var mode = BigInteger.Compare(GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Logs,
            GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index].CurrentCost) >= 0;

        if (!force&&GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index].available == mode)
            return;
        //Debug.Log("INVERTO--->"+index);

        GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index].available = mode;
        
        var shopBase = transform.Find("ShopItem_base");
        var shopAvailability = shopBase.Find("ShopItem_availability").gameObject;
        var priceText = shopBase.transform.Find("ShopItem_price").GetComponent<TextMeshProUGUI>();

        var trasp = mode ? Color.clear : new Color(0f, 0f, 0f, 0.4f);
        shopAvailability.GetComponent<ColorFade>().FadeToColor(trasp,typeof(Image));

        var color = mode ? Color.white : Color.gray;
        priceText.GetComponent<ColorFade>().FadeToColor(color,typeof(TextMeshProUGUI));
    }

    public void Buy()
    {
        var user = GameObject.Find("DataStorage").GetComponent<DataStorage>().user;
        if (GameObject.Find("DataStorage").GetComponent<DataStorage>().user
            .Buy(GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index]))
            GetComponent<AudioSource>().PlayOneShot(Bought);
        else
            GetComponent<AudioSource>().PlayOneShot(NotEnoughMoney);
    }
}