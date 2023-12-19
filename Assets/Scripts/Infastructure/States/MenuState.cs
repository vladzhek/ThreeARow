using UI;
using UnityEngine;

namespace Infastructure
{
    public class MenuState : IPayloadedState<string>
    {
        private const string GameScene = "Game";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;

        public MenuState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string scene)
        {
            _curtain.Show();
            _sceneLoader.Load(scene, OnLoaded);
        }

        private void OnLoaded()
        {
            _curtain.Hide();
            
            var view = Resources.Load<GameObject>("UI/Menu");
            var menu = Object.Instantiate(view,new Vector3(0,0,0), Quaternion.identity);
            menu.GetComponent<MenuView>().OnStartGame += StartGame;
        }

        private void StartGame()
        {
            _stateMachine.Enter<LoadLevelState, string>(GameScene);
        }

        public void Exit()
        {
            
        }
    }
}