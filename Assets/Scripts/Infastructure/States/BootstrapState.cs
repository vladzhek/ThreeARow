
using Services;

namespace Infastructure
{
    public class BootstrapState : IState
    {
        private const string Bootstrap = "Bootstrap";
        private const string Menu = "Menu";
        private readonly GameStateMachine _stateMachine;
        
        private SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine,SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(Bootstrap, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<MenuState, string>(Menu);
        
        private void RegisterServices()
        {
            Game.CurrencyService = new CurrencyService();
        }

        public void Exit()
        {
            
        }
    }
}