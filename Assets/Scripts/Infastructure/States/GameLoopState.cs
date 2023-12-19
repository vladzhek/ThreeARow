using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infastructure
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private const string GameScene = "Game";
        private const string MenuScene = "Menu";
        
        public GameLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            var view = Resources.Load<GameObject>("UI/HUD");
            var hud = Object.Instantiate(view,new Vector3(0,0,0), Quaternion.identity);
            hud.GetComponent<HudView>().OnMenuClick += Menu;
            hud.GetComponent<HudView>().OnRestartClick += RestartLevel;
        }

        private void RestartLevel()
        {
            _stateMachine.Enter<RestartLevelState, string>(GameScene);
        }

        private void Menu()
        {
            _stateMachine.Enter<MenuState, string>(MenuScene);
        }
    }
}