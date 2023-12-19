using UI;

namespace Infastructure
{
    public class LoadLevelState : IPayloadedState<string>, IState
    {
        private const string InitialPointTag = "PlayerInitialPoint";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string scene)
        {
            _curtain.Show();
            _sceneLoader.Load(scene, OnLoaded);
        }
        
        public void Enter()
        {
            _curtain.Show();
            OnLoaded();
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _curtain.Hide();
        }
    }
}