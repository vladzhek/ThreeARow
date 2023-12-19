using System;
using System.Collections.Generic;
using System.Linq;
using Infastructure;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        private const int BallsToEndGame = 3;
        private const int MaxBallsOnLines = 9;
        
        [SerializeField] private GameObject _pendulum;
        [SerializeField] private BallMono _ballPrefab;
        [SerializeField] private SpriteRenderer _cooldown;
        private BallSpawnService _ballSpawnService;
        private ThreeInARowChecker _threeInARowChecker;
        private BallMono _pendulumBall;
        private bool _endGame;

        private Dictionary<BallMono, ELine> _ballToLine = new();

        public event Action OnEndGame; 
        
        private bool isReloading = false;
        private float reloadTimer;

        private void Start()
        {
            _pendulumBall = _pendulum.GetComponent<BallMono>();

            var color = (EBallColor)Random.Range(0, 3);
            _ballSpawnService = new BallSpawnService(color, _ballPrefab);
            _threeInARowChecker = new ThreeInARowChecker();
            _threeInARowChecker.OnDestroyLine += LineDestroy;
            
            _endGame = false;
            reloadTimer = Constants.RELOAD_TIME;
            PendulumColor(color);
        }

        private void LineDestroy(Dictionary<BallMono, ELine> lines)
        {
            _ballToLine = lines;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && !_endGame && !isReloading)
            {
                var ballPrefab = _ballSpawnService.SpawnBall(_pendulum.transform);
                PendulumColor(_ballSpawnService.GetNextColor());
                ballPrefab.OnLineEnter += AddLineFromObject;
                isReloading = true;
                _cooldown.material.SetFloat("_CD", Constants.RELOAD_TIME);
            }
            
            if (isReloading)
            {
                reloadTimer -= Time.deltaTime;
                _cooldown.material.SetFloat("_CD", reloadTimer);
                if (reloadTimer <= 0)
                {
                    isReloading = false;
                    reloadTimer = Constants.RELOAD_TIME;
                }
            }
        }

        private void PendulumColor(EBallColor color)
        {
            _pendulumBall.SetColor(color);
        }

        private void AddLineFromObject(BallMono ball, ELine line)
        {
            if (_ballToLine.ContainsKey(ball)) return;

            var count = _ballToLine.Count(pair => pair.Value == line);
            
            ball.Index = count + 1;
            if (count == BallsToEndGame)
            {
                EndGame();
                return;
            }
            
            _ballToLine.Add(ball, line);

            _threeInARowChecker?.CheckThreeInARow(_ballToLine);

            if (_ballToLine.Count != MaxBallsOnLines) return;
            EndGame();
        }

        private void EndGame()
        {
            _endGame = true; 
            OnEndGame?.Invoke();
        }
    }
}