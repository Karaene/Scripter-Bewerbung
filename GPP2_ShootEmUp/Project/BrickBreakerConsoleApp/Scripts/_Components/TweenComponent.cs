using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class TweenComponent : IEntityComponent
    {
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }
        public Type Type { get; set; }
        public int Duration { get; set; }
        public Vector2D TargetPos { get; set; }
        public Vector2D StartPos { get; set; }
        public EaseType EaseType { get; set; }
        public bool TweenFinished { get; set; } = false;
        public bool Loop { get; set; } = false;

        public TweenComponent(ISystem system, Vector2D startPos, Vector2D targetPos, EaseType easeType, int duration, bool loop)
        {
            System = system;
            StartPos = startPos;
            TargetPos = targetPos;
            Duration = duration;
            EaseType = easeType;
            Loop = loop;
        }
    }

    public enum EaseType {  ELASTIC_IN, ELASTIC_OUT, ELASTIC_IN_OUT,
                            QUAD_IN, QUAD_OUT, QUAD_IN_OUT,
                            CUBE_IN, CUBE_OUT, CUBE_IN_OUT,
                            QUART_IN, QUART_OUT, QUART_IN_OUT,
                            QUINT_IN, QUINT_OUT, QUINT_IN_OUT,
                            SINE_IN, SINE_OUT, SINE_IN_OUT,
                            BOUNCE_IN, BOUNCE_OUT, BOUNCE_IN_OUT,
                            CIRC_IN, CIRC_OUT, CIRC_IN_OUT,
                            EXPO_IN, EXPO_OUT, EXPO_IN_OUT,
                            BACK_IN, BACK_OUT, BACK_IN_OUT
    }
}
