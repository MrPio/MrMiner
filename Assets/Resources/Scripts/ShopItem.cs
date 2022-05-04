using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mono.Cecil;
using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Vector2 = UnityEngine.Vector2;

public class ShopItem : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler
{
    public float downTimeForUpdate = 1;
    public GameObject shopItemHold;
    public AnimationCurve onPressCurve;
    public float onPressAnimationDuration = 0.5f;
    private bool _onDownAnimation;
    private bool _onUpAnimation;
    private float _downTime;

    private float _onPressStartTime;
    private float _onPressAnimationScale;
    public static AudioClip MouseDownAudioClip;
    public static AudioClip MouseUpAudioClip;
    public static AudioClip NotEnoughMoney;
    public static AudioClip Bought;
    public static AudioClip Charge;
    public int index;
    private float _shopBaseInitialLocalPosY;
    private bool _updateMode;
    private bool _holdStarted;

    void Start()
    {
        MouseDownAudioClip = Resources.Load("Raws/click_down_003") as AudioClip;
        MouseUpAudioClip = Resources.Load("Raws/click_up_002") as AudioClip;
        Bought = Resources.Load("Raws/buy") as AudioClip;
        NotEnoughMoney = Resources.Load("Raws/fx_no_buy") as AudioClip;
        Charge = Resources.Load("Raws/charge_01") as AudioClip;
        _onPressAnimationScale = 1 / onPressAnimationDuration;
        _shopBaseInitialLocalPosY = transform.Find("ShopItem_base").localPosition.y;
    }

    void Update()
    {
        var t = Time.time - _onPressStartTime;

        if (!_onDownAnimation && !_onUpAnimation)
        {
            var color = shopItemHold.GetComponent<Image>().color;
            if (color.a > 0.01f)
            {
                color.a = 1 - t * 3;
                shopItemHold.GetComponent<Image>().color = color;
            }

            return;
        }


        if (t * _onPressAnimationScale <= 1f)
        {
            float localY = default;
            if (_onDownAnimation)
                localY = -15f * onPressCurve.Evaluate(t * _onPressAnimationScale);
            else if (_onUpAnimation)
                localY = -15f + 15f * onPressCurve.Evaluate(t * _onPressAnimationScale);

            transform.Find("ShopItem_base").localPosition = new Vector2(0, _shopBaseInitialLocalPosY + localY);
        }

        if (t * _onPressAnimationScale > 1f)
        {
            //_onDownAnimation = false;
            _onUpAnimation = false;
            if (!_onDownAnimation)
                return;

            if (!_holdStarted)
            {
                shopItemHold.GetComponent<RectTransform>().position = Input.mousePosition;
                GetComponent<AudioSource>().PlayOneShot(Charge);
            }

            _holdStarted = true;
            shopItemHold.GetComponent<RectTransform>().localScale =
                Vector2.one * (0.5f + 18f * (t * _onPressAnimationScale - 1f) /
                    (_onPressAnimationScale * downTimeForUpdate));

            var color = shopItemHold.GetComponent<Image>().color;
            color.a = (t * _onPressAnimationScale - 1f) / (_onPressAnimationScale * downTimeForUpdate * 1.1f);
            shopItemHold.GetComponent<Image>().color = color;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_downTime > downTimeForUpdate)
            BuyUpgrade();
        else
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
        _holdStarted = false;
        GetComponent<AudioSource>().PlayOneShot(MouseUpAudioClip);
        _onDownAnimation = false;
        _onUpAnimation = true;
        _downTime = Time.time - _onPressStartTime;
        Debug.Log("DOWN time--->" + _downTime);
        _onPressStartTime = Time.time;

        transform.Find("ShopItem_base").Find("ShopItem_highlight").gameObject.SetActive(false);
    }

    public void TurnUpdateModeIfNecessary()
    {
        var building = GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index];
        if (_updateMode == building.CheckForUpgrade())
            return;

        _updateMode = !_updateMode;
        var trigger = "Close";
        if (_updateMode)
            trigger = "Open";
        GetComponent<Animator>().SetTrigger(trigger);
        /*var mode = false; //todo

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
            new Vector2(rectLog.x, rectLog.y);*/
    }

    public void TurnAvailability(bool force = false)
    {
        var mode = BigInteger.Compare(GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Logs,
            GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index].CurrentCost) >= 0;

        if (!force && GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index].available ==
            mode)
            return;
        //Debug.Log("INVERTO--->"+index);

        GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index].available = mode;

        var shopBase = transform.Find("ShopItem_base");
        var shopAvailability = shopBase.Find("ShopItem_availability").gameObject;
        var priceText = shopBase.transform.Find("ShopItem_price").GetComponent<TextMeshProUGUI>();

        var traspFinal = mode ? Color.clear : new Color(0f, 0f, 0f, 0.4f);
        var traspInitial = !mode ? Color.clear : new Color(0f, 0f, 0f, 0.4f);
        shopAvailability.GetComponent<ColorFade>().FadeToColor(traspInitial, traspFinal, typeof(Image));

        var colorFinal = mode ? Color.green : Color.gray;
        var colorInitial = !mode ? Color.green : Color.gray;
        priceText.GetComponent<ColorFade>().FadeToColor(colorInitial, colorFinal, typeof(TextMeshProUGUI));
    }

    private void Buy()
    {
        var building = GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index];
        if (GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buy(building))
        {
            GetComponent<AudioSource>().PlayOneShot(Bought);
            TurnUpdateModeIfNecessary();
        }
        else
            GetComponent<AudioSource>().PlayOneShot(NotEnoughMoney);
    }

    private void BuyUpgrade()
    {
        var building = GameObject.Find("DataStorage").GetComponent<DataStorage>().user.Buildings[index];
        if (GameObject.Find("DataStorage").GetComponent<DataStorage>().user.BuyUpgrade(building))
        {
            var shopBase = transform.Find("ShopItem_base").transform;
            shopBase.Find("ShopItem_price_upgrade").GetComponent<TextMeshProUGUI>().text =
                utilies.NumberToFormattedString(building.CalculateUpgradeCost());
            shopBase.Find("ShopItem_version").GetComponent<TextMeshProUGUI>().text = "lv." + building.Version;

            GetComponent<AudioSource>().PlayOneShot(Bought);
        }
        else
            GetComponent<AudioSource>().PlayOneShot(NotEnoughMoney);
    }
}