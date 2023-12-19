using UnityEngine.SceneManagement;

namespace Infastructure
{
    public class RestartLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public RestartLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        public void Enter(string sceneName)
        {
            _sceneLoader.LoadAnyScene(sceneName, Load);
        }

        private void Load()
        {
            _stateMachine.Enter<LoadLevelState>();
        }

        public void Exit()
        {
            
        }
    }
}