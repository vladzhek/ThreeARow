using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private Button _startBtn;
    
    public event Action OnStartGame;

    private void Start()
    {
        _startBtn.onClick.AddListener(StartButton);
    }

    private void StartButton()
    {
        OnStartGame?.Invoke();
        Destroy(gameObject);
    }
}
