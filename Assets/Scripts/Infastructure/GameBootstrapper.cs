using UI;
using UnityEngine;

namespace Infastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain CurtainPrefab;
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, Instantiate(CurtainPrefab));
            _game.StateMachine.Enter<BootstrapState>();
            Game.OnReloadGame += ReloadGame;
            
            DontDestroyOnLoad(this);
        }

        private void ReloadGame()
        {
            _game.StateMachine.Enter<LoadLevelState, string>("Game");
        }
    }
}
