using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    public interface IEntityComponent
    {
        ISystem System { get; }
        GameObject GameObject { get; set; }
        Type Type { get; set; }

    }

    public enum Type { RENDER, INPUT, BUTTON, HEALTH, PHYSICS, PLAYER, CAMERA, BOX_COLLIDER, CIRCLE_COLLIDER, SPAWNER, VELOCITY, AI, WEAPON, BULLET, SOUND}
}
