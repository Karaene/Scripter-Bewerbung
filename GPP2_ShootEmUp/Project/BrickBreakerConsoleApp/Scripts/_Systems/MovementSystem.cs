using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class MovementSystem : ISystem
    {
        private List<VelocityComponent> _velocityComponents = new List<VelocityComponent>();
        private List<InputComponent> _activeInputs = new List<InputComponent>();

        private List<GameObject> _Moveables = new List<GameObject>();

        private bool updateMoveables = false;

        private MovementSystem() { }
        private static MovementSystem instance = null;
        public static MovementSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MovementSystem();
                }
                return instance;
            }
        }

        public InputComponent CreateInput(InputType inputType)
        {
            InputComponent input = new InputComponent(this, inputType);
            _activeInputs.Add(input);

            return input;
        }

        public IEntityComponent CreateVelocityComponent(Vector2D velocity)
        {
            VelocityComponent vc = new VelocityComponent(this, velocity);
            _velocityComponents.Add(vc);
            updateMoveables = true;
            return vc;
        }

        private void UpdateMoveables()
        {
            updateMoveables = false;
            foreach (var vc in _velocityComponents)
            {
                if (vc.GameObject.ContainsComponent(Type.INPUT))
                {
                    _Moveables.Add(vc.GameObject);
                }
            }
        }

        public void Update()
        {
            if(updateMoveables)
                UpdateMoveables();

            foreach (var moveable in _Moveables)
            {
                InputComponent tmpInput = (InputComponent)moveable.GetComponent(Type.INPUT);
                VelocityComponent velocity = (VelocityComponent)moveable.GetComponent(Type.VELOCITY);

                if (tmpInput.MoveRight && (tmpInput.GameObject.Transform.Position.X <= LevelManager.LEVEL_WIDTH - tmpInput.GameObject.Transform.Size.Width / 2 - LevelManager.SCREEN_OFFSET_X))
                {
                    tmpInput.GameObject.Transform.Position = new Vector2D(tmpInput.GameObject.Transform.Position.X + velocity.Velocity.X, tmpInput.GameObject.Transform.Position.Y);
                }
                if (tmpInput.MoveLeft && (tmpInput.GameObject.Transform.Position.X >= -LevelManager.SCREEN_OFFSET_X + tmpInput.GameObject.Transform.Size.Width / 2))
                {
                    tmpInput.GameObject.Transform.Position = new Vector2D(tmpInput.GameObject.Transform.Position.X - velocity.Velocity.X, tmpInput.GameObject.Transform.Position.Y);
                }
                if (tmpInput.MoveUp && (tmpInput.GameObject.Transform.Position.Y >= -LevelManager.SCREEN_OFFSET_Y + tmpInput.GameObject.Transform.Size.Height / 2))
                {
                    tmpInput.GameObject.Transform.Position = new Vector2D(tmpInput.GameObject.Transform.Position.X, tmpInput.GameObject.Transform.Position.Y - velocity.Velocity.Y);
                }
                if (tmpInput.MoveDown && (tmpInput.GameObject.Transform.Position.Y <= LevelManager.LEVEL_HEIGHT - tmpInput.GameObject.Transform.Size.Height / 2 - LevelManager.SCREEN_OFFSET_Y))
                {
                    tmpInput.GameObject.Transform.Position = new Vector2D(tmpInput.GameObject.Transform.Position.X, tmpInput.GameObject.Transform.Position.Y + velocity.Velocity.Y);
                }
            }
        }
    }
}
