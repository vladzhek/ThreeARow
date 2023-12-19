using System;
using Gameplay;
using Infastructure;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour
{
    [SerializeField] private GameObject GameContainer;
    [SerializeField] private GameObject EndGameContainer;
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text endGameScoreText;

    public event Action OnMenuClick; 
    public event Action OnRestartClick; 

    void Start()
    {
        GameContainer.SetActive(true);
        EndGameContainer.SetActive(false);
        
        menuBtn.onClick.AddListener(MenuButton);
        restartBtn.onClick.AddListener(RestartButton);
        Game.CurrencyService.OnUpdate += UpdateScore;
        GameObject.FindWithTag("GameController").GetComponent<GameController>().OnEndGame += EndGameplay;
        UpdateScore();
    }

    private void EndGameplay()
    {
        GameContainer.SetActive(false);
        EndGameContainer.SetActive(true);
        endGameScoreText.text = scoreText.text;
    }

    private void UpdateScore()
    {
        scoreText.text = Game.CurrencyService.SoftCoins.ToString();
    }

    private void RestartButton()
    {
        OnRestartClick?.Invoke();
    }

    private void MenuButton()
    {
        OnMenuClick?.Invoke();
    }

    private void OnDisable()
    {
        var currency = Game.CurrencyService;
        currency.AddCurrency(CurrencyType.Hard, currency.SoftCoins);
        currency.SpendCurrency(CurrencyType.Soft, currency.SoftCoins);
    }
}
