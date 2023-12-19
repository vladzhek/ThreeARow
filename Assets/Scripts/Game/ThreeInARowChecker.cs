using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Gameplay
{
    public class ThreeInARowChecker
    {
        public event Action<Dictionary<BallMono, ELine>> OnDestroyLine; 
        private Dictionary<BallMono, ELine> _ballToLine = new();

        public async UniTask CheckThreeInARow(Dictionary<BallMono, ELine> ballToLine)
        {
            await UniTask.WhenAll(
                CheckHorizontal(ballToLine),
                CheckVertical(ballToLine),
                CheckDiagonalTopLeft(ballToLine),
                CheckDiagonalTopRight(ballToLine)
            );
        }
        
        private async UniTask CheckHorizontal(Dictionary<BallMono, ELine> ballToLine)
        {
            var leftLineBalls = ballToLine.Where(pair => pair.Value == ELine.Left).Select(pair => pair.Key).ToList();
            var midLineBalls = ballToLine.Where(pair => pair.Value == ELine.Mid).Select(pair => pair.Key).ToList();
            var rightLineBalls = ballToLine.Where(pair => pair.Value == ELine.Right).Select(pair => pair.Key).ToList();

            for (var i = 0; i < leftLineBalls.Count; i++)
            {
                var currentBallLeft = leftLineBalls[i];
                var currentIndexLeft = i;

                if (currentIndexLeft > midLineBalls.Count - 1 || currentIndexLeft > rightLineBalls.Count - 1)
                    continue;

                var nextBallMid = midLineBalls[currentIndexLeft];
                var nextNextBallRight = rightLineBalls[currentIndexLeft];

                if (currentBallLeft.GetColor() == nextBallMid.GetColor() && currentBallLeft.GetColor() == nextNextBallRight.GetColor())
                {
                    DestroyBalls(ballToLine, new List<BallMono> { currentBallLeft, nextBallMid, nextNextBallRight });
                    return;
                }
            }
        }

        private async UniTask CheckVertical(Dictionary<BallMono, ELine> ballToLine)
        {
            var lines = Enum.GetValues(typeof(ELine)).Length;
            var balls = ballToLine.Keys.ToList();

            foreach (var line in Enumerable.Range(0, lines))
            {
                for (var i = 0; i < balls.Count; i++)
                {
                    var matchingBalls = new List<BallMono> { balls[i] };
            
                    for (var j = i + 1; j < balls.Count; j++)
                    {
                        if (ballToLine[balls[i]] == (ELine)line && ballToLine[balls[j]] == (ELine)line &&
                            balls[i].GetColor() == balls[j].GetColor())
                        {
                            matchingBalls.Add(balls[j]);
                        }
                    }

                    if (matchingBalls.Count >= 3)
                    {
                        DestroyBalls(ballToLine, matchingBalls);
                        return;
                    }
                }
            }
        }

        private async UniTask CheckDiagonalTopLeft(Dictionary<BallMono, ELine> ballToLine)
        {
            var leftBall = ballToLine.FirstOrDefault(pair => pair.Value == ELine.Left && pair.Key.Index == 1).Key;
            var midBall = ballToLine.FirstOrDefault(pair => pair.Value == ELine.Mid && pair.Key.Index == 2).Key;
            var rightBall = ballToLine.FirstOrDefault(pair => pair.Value == ELine.Right && pair.Key.Index == 3).Key;

            if (leftBall == null || midBall == null || rightBall == null)
                return;

            if (leftBall.GetColor() == midBall.GetColor() && leftBall.GetColor() == rightBall.GetColor())
            {
                DestroyBalls(ballToLine, new List<BallMono> { leftBall, midBall, rightBall });
            }
        }

        private async UniTask CheckDiagonalTopRight(Dictionary<BallMono, ELine> ballToLine)
        {
            var leftBall = ballToLine.FirstOrDefault(pair => pair.Value == ELine.Left && pair.Key.Index == 3).Key;
            var midBall = ballToLine.FirstOrDefault(pair => pair.Value == ELine.Mid && pair.Key.Index == 2).Key;
            var rightBall = ballToLine.FirstOrDefault(pair => pair.Value == ELine.Right && pair.Key.Index == 1).Key;

            if (leftBall == null || midBall == null || rightBall == null)
                return;

            if (leftBall.GetColor() == midBall.GetColor() && leftBall.GetColor() == rightBall.GetColor())
            {
                DestroyBalls(ballToLine, new List<BallMono> { leftBall, midBall, rightBall });
            }
        }

        private async void DestroyBalls(Dictionary<BallMono, ELine> ballToLine, IEnumerable<BallMono> ballsToDestroy)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.65f));

            foreach (var ball in ballsToDestroy)
            {
                if (ballToLine.ContainsKey(ball))
                {
                    var line = ballToLine[ball];
                    var removedIndex = ball.Index;
                    
                    ball.BallDestroy();
                    ballToLine.Remove(ball);
                    
                    var ballsOnLine = ballToLine.Where(pair => pair.Value == line).Select(pair => pair.Key).ToList();
                    ballsOnLine.Sort((b1, b2) => b1.Index.CompareTo(b2.Index));

                    for (var i = 0; i < ballsOnLine.Count; i++)
                    {
                        if(ballsOnLine[i].Index > removedIndex)
                            ballsOnLine[i].Index -= 1;
                    }
                }
            }
            
            OnDestroyLine?.Invoke(ballToLine);
        }
    }
}