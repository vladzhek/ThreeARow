using System;
using System.Collections;
using Gameplay;
using Infastructure;
using Services;
using UnityEngine;

public class BallMono : MonoBehaviour
{
    private const string LeftLineTag = "Left";
    private const string MidLineTag = "Mid";
    private const string RightLineTag = "Right";
    
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private ParticleSystem _explosionPS;
    private Rigidbody2D _rb;
    
    private EBallColor _colorType;
    private Color _color;
    public int Index { get; set; }

    public event Action<BallMono, ELine> OnLineEnter;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetColor(EBallColor type)
    {
        _colorType = type;
        var color = type switch
        {
            EBallColor.Red => Color.red ,
            EBallColor.Green => Color.green,
            EBallColor.Blue => Color.cyan,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        _sprite.color = color;
        _color = color;
    }

    public EBallColor GetColor()
    {
        return _colorType;
    }

    public void BallDestroy()
    {
        StartCoroutine(CheckAndDestroy());
    }

    private IEnumerator CheckAndDestroy()
    {
        if (_rb == null) yield break;
        
        while (true)
        {
            yield return null;
            if (_rb.velocity.y == 0)
            {
                var ps = Instantiate(_explosionPS, gameObject.transform.position, Quaternion.identity);
                ps.startColor = _color;
                Destroy(gameObject);
                BallReward();
                yield break;
            }
        }
    }

    private void BallReward()
    {
        switch(_colorType)
        {
            case EBallColor.Red:
                Game.CurrencyService.AddCurrency(CurrencyType.Soft, 1);
                break;
            case EBallColor.Green:
                Game.CurrencyService.AddCurrency(CurrencyType.Soft, 2);
                break;
            case EBallColor.Blue:
                Game.CurrencyService.AddCurrency(CurrencyType.Soft, 3);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        };
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(LeftLineTag))
        {
            OnLineEnter?.Invoke(this, ELine.Left);
        }
        else if (col.CompareTag(MidLineTag))
        {
            OnLineEnter?.Invoke(this, ELine.Mid);
        }
        else if (col.CompareTag(RightLineTag))
        {
            OnLineEnter?.Invoke(this, ELine.Right);
        }
    }
}