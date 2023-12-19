using System;
using Services;
using UI;

namespace Infastructure
{
    public class Game
    {
        public static event Action OnReloadGame;
        public GameStateMachine StateMachine;
        public static CurrencyService CurrencyService;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain);
        }

        public static void ReloadGame()
        {
            OnReloadGame?.Invoke();
        }
    }
}