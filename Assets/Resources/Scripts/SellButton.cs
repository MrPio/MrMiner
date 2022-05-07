using Unity.VisualScripting;
using UnityEngine;
using utiles;

public class SellButton : MonoBehaviour
{
    public AudioClip[] sold,damage;
    public AudioClip beep;
    public AnimationCurve onPressCurve, timeToNextSell;
    public Transform coin;
    public float timeToStart = 1f;
    public int maxClicks = 100;
    public SpriteRenderer glow, coinSpriteRenderer;
    public float cooldownTime = 5;

    private static AudioClip _mouseDownAudioClip;
    private static AudioClip _mouseUpAudioClip;
    private AudioSource _audioSource;
    private bool _onDownAnimation, _onUpAnimation;
    private float _timeStart;
    private float _shopBaseInitialLocalPosY;
    private float _lastSell;
    private Camera _camera;
    private DataStorage _dataStorage;
    private int _clicks;
    private SpriteRenderer _spriteRenderer;
    private Color _lastColor, _lastColorCoin;
    private bool _cool = true;

    private void Start()
    {
        _dataStorage = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        _mouseDownAudioClip = Resources.Load("Raws/click_down_003") as AudioClip;
        _mouseUpAudioClip = Resources.Load("Raws/click_up_002") as AudioClip;
        _audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>()
            .GetComponent<AudioSource>();
        _shopBaseInitialLocalPosY = transform.localPosition.y;
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_onDownAnimation && !_onUpAnimation)
            return;

        var t = Time.time - _timeStart;
        float localY = default;
        if (_onDownAnimation)
            localY = -0.18f * onPressCurve.Evaluate(t * 1 / 0.06f);
        else if (_onUpAnimation)
            localY = -0.18f + 0.18f * onPressCurve.Evaluate(t * 1 / 0.06f);

        transform.localPosition = new Vector2(0, _shopBaseInitialLocalPosY + localY);
        coin.localPosition = new Vector2(0, _shopBaseInitialLocalPosY + localY);

        if (_onDownAnimation)
        {
            if (t > timeToStart && Time.time - _lastSell > timeToNextSell.Evaluate(t))
            {
                ++_clicks;
                _cool = false;
                _lastSell = Time.time;
                Effect.ClickEffect(_camera.ScreenToWorldPoint(Input.mousePosition), utilies.HexToColor("#A5FAFF"));
                Effect.SpawnFloatingText(Input.mousePosition, _dataStorage.user.ClickPowerCoin, 1.6f, "#FFD87C");
                _dataStorage.user.EarnClickCoin();
                _audioSource.PlayOneShot(sold[Random.Range(0, sold.Length)]);

                _lastColor = Color.Lerp(utilies.HexToColor("#FFDA00"), utilies.HexToColor("#EC0005"),
                    (float) _clicks / maxClicks);
                _spriteRenderer.color = _lastColor;

                _lastColorCoin = Color.Lerp(Color.white, utilies.HexToColor("#EC0005"),
                    (float) _clicks / maxClicks);
                coinSpriteRenderer.color = _lastColorCoin;

                var glowColor = _lastColor;
                glowColor.a = 0.3f + (float) _clicks / maxClicks;
                glow.color = glowColor;
            }
        }

        if (_onUpAnimation && !_cool)
        {

            var heat = cooldownTime * _clicks / maxClicks;
            if (t / heat >= 1)
            {
                _clicks = 0;
                _cool = true;
                _audioSource.PlayOneShot(beep);
            }
            var color = _spriteRenderer.color = Color.Lerp(_lastColor, utilies.HexToColor("#FFDA00"), t / heat);
            coinSpriteRenderer.color = Color.Lerp(_lastColorCoin, Color.white, t / heat);
            color.a = 1.3f - t / heat;
            glow.color = color;
        }
    }

    private void OnMouseDown()
    {
        _audioSource.PlayOneShot(_mouseDownAudioClip);
        _timeStart = Time.time;
        _onDownAnimation = true;
        _onUpAnimation = false;
    }

    private void OnMouseUp()
    {
        _audioSource.PlayOneShot(_mouseUpAudioClip);
        _timeStart = Time.time;
        _onDownAnimation = false;
        _onUpAnimation = true;
    }


    private void OnMouseUpAsButton()
    {
    }
}