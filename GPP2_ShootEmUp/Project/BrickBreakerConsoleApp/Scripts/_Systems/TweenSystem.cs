using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class TweenSystem : ISystem
    {
        LTimer timer = new LTimer();
        uint startTick = 0;

        List<TweenComponent> tweenComponents = new List<TweenComponent>();

        private TweenSystem() { }
        private static TweenSystem instance = null;
        public static TweenSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TweenSystem();
                }
                return instance;
            }
        }

        public void Init()
        {
            timer.Start();
        }

        public TweenComponent CreateTweenComponent(Vector2D startPos, Vector2D targetPos, EaseType easeType, int durationInSec, bool loop)
        {
            TweenComponent tweenComponent = new TweenComponent(this, startPos, targetPos, easeType, durationInSec, loop);
            tweenComponents.Add(tweenComponent);
            return tweenComponent;
        }

        private void TweenPosition(TweenComponent tweenComp, float t)
        {
            if (tweenComp.Loop)
            {
                Vector2D EaseResult = new Vector2D((int)(tweenComp.StartPos.X + Ease.SineInOut(t / (tweenComp.Duration * 1000)) * (tweenComp.TargetPos.X - tweenComp.StartPos.X)),
                                                 (int)(tweenComp.StartPos.Y + Ease.SineInOut(t / (tweenComp.Duration * 1000)) * (tweenComp.TargetPos.Y - tweenComp.StartPos.Y)));

                //Console.WriteLine("EaseResult = " + EaseResult);
                tweenComp.GameObject.Transform.Position = EaseResult;

                if (EaseResult.X == tweenComp.TargetPos.X && EaseResult.Y == tweenComp.TargetPos.Y)
                {
                    Vector2D startPosTmp = tweenComp.StartPos;
                    tweenComp.StartPos = tweenComp.TargetPos;
                    //Console.WriteLine("StartPos now = " + tweenComp.StartPos);
                    tweenComp.TargetPos = startPosTmp;
                    //Console.WriteLine("TargetPos now = " + tweenComp.TargetPos);
                }
            }
            else if (!tweenComp.Loop)
            {
                float Percentage = t / (tweenComp.Duration * 1000);
                Percentage = MathHelper.Clamp(Percentage, 0, 1);

                Vector2D EasingResult = new Vector2D((int)(tweenComp.StartPos.X + Ease.SineInOut(Percentage) * (tweenComp.TargetPos.X - tweenComp.StartPos.X)),
                                                     (int)(tweenComp.StartPos.Y + Ease.SineInOut(Percentage) * (tweenComp.TargetPos.Y - tweenComp.StartPos.Y)));

                tweenComp.GameObject.Transform.Position = EasingResult;

                //Console.WriteLine("Percentage : " + Percentage);
                //Console.WriteLine("EasingResult: " + EasingResult);

                if (Percentage == 1)
                {
                    tweenComp.TweenFinished = true;
                }
            }
        }

        public void Update()
        {
            float t = (timer.GetTicks() - startTick);

            foreach (var tweenComp in tweenComponents)
            {
                if (!tweenComp.TweenFinished && tweenComp.GameObject.Active)
                {
                    TweenPosition(tweenComp, t);
                }
            }
        }
    }
}
