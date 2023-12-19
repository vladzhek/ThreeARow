using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded)
        {
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
        }
        
        private IEnumerator LoadScene(string nextScene, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }

        public void LoadAnyScene(string name, Action onLoaded)
        {
            _coroutineRunner.StartCoroutine(LoadAnySceneAsync(name, onLoaded));
        }
        
        private IEnumerator LoadAnySceneAsync(string nextScene, Action onLoaded)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}