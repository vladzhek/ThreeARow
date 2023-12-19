using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class BallSpawnService
    {
        private BallMono _ballPrefab;
        private EBallColor _currentColor;
        
        public Dictionary<BallMono, EBallColor> balls = new();
        
        public BallSpawnService(EBallColor startColor,BallMono ballPrefab)
        {
            _currentColor = startColor;
            _ballPrefab = ballPrefab;
        }

        public BallMono SpawnBall(Transform pendulum)
        {
            var prefab = GameObject.Instantiate(_ballPrefab, pendulum.position, Quaternion.identity);
            var newColor = (EBallColor)Random.Range(0, 3);

            prefab.SetColor(_currentColor);
            balls.Add(prefab, _currentColor);
            _currentColor = newColor;
            return prefab;
        }

        public EBallColor GetNextColor()
        {
            return _currentColor;
        }
    }
}