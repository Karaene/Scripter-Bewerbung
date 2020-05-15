using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace BrickBreakerConsoleApp
{
    class Game
    {
        #region Attributes and Properties
        private const double MS_PER_UPDATE = 10;

        private static int _Points = 0;
        private static int _Multiplier = 1;
        private static Text _PointsText = new Text(0, 0, 300, 25);
        private static Text _EndScore;

        public bool SpeedUp { get; set; } = false;
        public int PointMultiplier { get { return _Multiplier; } set { _Multiplier = value; } }
            
        public int playerLifes = 3;
        public bool TwoPlayerMode { get; set; } = false;
        public bool PlayerOneMoveLeft { get; set; } = false;
        public bool PlayerOneMoveRight { get; set; } = false;
        public bool PlayerTwoMoveLeft { get; set; } = false;
        public bool PlayerTwoMoveRight { get; set; } = false;

        private IntPtr renderer;

        private static List<Brick> _Bricks;
        private static List<Brick> _BricksHit;

        //Bricks
        const int BRICKS = 36; //36
        Brick[] bricks = new Brick[BRICKS];

        private static List<IRenderable> _RenderObjects = new List<IRenderable>();
        private static List<IUpdateable> _UpdateObjects = new List<IUpdateable>();
        
        private static Paddle _Paddle_Bottom = new Paddle(Paddle.Placed.BOTTOM);
        private static Paddle _Paddle_Top = new Paddle(Paddle.Placed.TOP);
        public const int PADDLE_WIDTH = 100;
        public const int PADDL_HEIGHT = 25;

        
        public bool Space { get; set; }
        public bool IsRunning { get; set; }

        public static Button _StartButton = new Button();

        //private static Color _BallColor = new Color(255, 0, 255);
        private static Ball _Ball = new Ball();
        private static Boss _Boss;
        
        //Playingfield
        private static Sprite _Background;
        private static Color _BackgroundColor = new Color(0, 100, 100);
        private static PlayingField _PlayingField;

        private static double _Elapsed = 0;

        private static bool _Set_Ball_Player_Bottom = true;

        private Sound sound;
        private Event ev;
        private Random rnd;

        private Timer myTimer = new Timer();
        private static Text _MyTimerText = new Text(0, 0, 175, 25);

        //Used to save the last hit Paddle
        private bool hitBottom = false;
        private bool hitTop = false;

        public bool bossSpawned = false;

        private static State state = State.START_GAME;
        #endregion

        #region Constructors
        public Game(IntPtr renderer, int width, int height)
        {
            this.renderer = renderer;
            Color color = new Color(0, 191, 255);
            _Background = new Sprite(renderer, "background.png");
            _PlayingField = new PlayingField(width, height, _Background);
            //_PlayingField = new PlayingField(width, height, _BackgroundColor);

            _Bricks = new List<Brick>();
            _BricksHit = new List<Brick>();
            /*
            _Paddle_Bottom = new Paddle((int)(_PlayingField.Width / 2), _PlayingField.Height - 50, PADDLE_WIDTH, PADDL_HEIGHT, new Sprite(renderer, "paddle.png"));
            _Paddle_Top = new Paddle((int)(_PlayingField.Width /2), 50, PADDLE_WIDTH, PADDL_HEIGHT, new Sprite(renderer, "paddle.png"));
            */
            _Paddle_Bottom = new Paddle((int)(_PlayingField.Width / 2), _PlayingField.Height - 50, 100, 25, new Sprite(renderer, "paddle.png"), Paddle.Placed.BOTTOM);
            _Paddle_Top = new Paddle((int)(_PlayingField.Width / 2), 25, 100, 25, new Sprite(renderer, "paddle.png"), Paddle.Placed.TOP);

            _Paddle_Bottom.Transform.Position = new Vector2D(_Paddle_Bottom.Transform.Position.X - _Paddle_Bottom.Transform.Dimension.Width / 2, _Paddle_Bottom.Transform.Position.Y);
            _Paddle_Top.Transform.Position = new Vector2D(_Paddle_Top.Transform.Position.X - _Paddle_Top.Transform.Dimension.Width / 2, _Paddle_Top.Transform.Position.Y);
            
            _Ball = new Ball((int)(_Paddle_Bottom.Transform.Position.X + _Paddle_Bottom.Transform.Dimension.Width / 2), (int)_Paddle_Bottom.Transform.Position.Y - 15, new Sprite(renderer,"ball2.png"));
            
            _StartButton = new Button((int)_PlayingField.Width / 2, _PlayingField.Height - 150, 200, 100, new Sprite(renderer, "startButton.png"));
            

            
            _PointsText.SetText(" Lives : 0   BossLife : 0   Points: 0 ");
            rnd = new Random();
            sound = new Sound();
            _EndScore = new Text(_PlayingField.Width/2 - 450, _PlayingField.Height/5, 900, 80);
            _EndScore.SetText(" ");
        }
        #endregion

        #region Methodes
        public void ShowMenu()
        {
            EventManager eventManager = new EventManager(this);
            
            InitializeMenu();

            IsRunning = true;

            while (IsRunning)
            {
                eventManager.UpdateInputEvents();

                if (_StartButton.clicked)
                {
                    _RenderObjects.Clear();
                    IsRunning = false;
                }

                Render(renderer, _RenderObjects);
            }

            if (_StartButton.clicked)
            {
                _StartButton.clicked = false;
                Start();
            }
        }

        public void Start()
        {
            EventManager eventManager = new EventManager(this);

            IsRunning = true;

            double previous = SDL_GetTicks();
            double lag = 0.0;

            AddUpdateObjects();
            AddRenderObjects();

            myTimer.start();
            while (IsRunning)
            {
                double current = SDL_GetTicks();
                _Elapsed = current - previous;
                previous = current;
                lag += _Elapsed;
                
                eventManager.UpdateInputEvents();

                while (lag >= MS_PER_UPDATE)
                {
                    Update();
                  //PowerUpManager.UpdateBuffs(_Paddle_Top, _Paddle_Bottom, _Ball);
                    lag -= MS_PER_UPDATE;
                }
                Render(renderer, _RenderObjects);
            }
            return;
        }

        private void GameOver()
        {
            EventManager eventManager = new EventManager(this);
            InitializeMenu();
            IsRunning = true;
            _RenderObjects.Add(_EndScore);
            int _TimeBonus = (int)(myTimer.get_ticks() / 1000f) / 3;
            int _FinalPoints = _Points - _TimeBonus;
            while (IsRunning)
            {
                _EndScore.SetText("Points: " +  _Points + "  Time needed:" + _TimeBonus + "  Endpoints: " + _FinalPoints);
                eventManager.UpdateInputEvents();
                if (_StartButton.clicked)
                {
                    _RenderObjects.Clear();
                    _UpdateObjects.Clear();
                    playerLifes = 3;
                    _Points = 0;
                    _Ball.Speed = Ball.START_SPEED;
                    myTimer.stop();
                    IsRunning = false;
                }

                Render(renderer, _RenderObjects);
            }

            if (_StartButton.clicked)
            {
                _StartButton.clicked = false;
                Start();
            }
        }

        private void Update()
        {
            //......................................................................................................................................................................................................
            
            if (bossSpawned)
            {
                _PointsText.SetText(" Lives: " + playerLifes + "   BossLife: " + _Boss.GetLive() + "   Points: " + _Points + "   Time: " + (int)(myTimer.get_ticks() / 1000f));
            }
            else
            {
                _PointsText.SetText(" Lives: " + playerLifes + "   Points: " + _Points + "   Time: " + (int)(myTimer.get_ticks() / 1000f));
            }
            foreach (IUpdateable obj in _UpdateObjects)
            {
                obj.Update();
            }

            UpdateEvent();

            if (_Ball.catched)
            {
                if (_Set_Ball_Player_Bottom)
                {
                    _Ball.FollowPaddle(_Paddle_Bottom);
                }
                else
                {
                    _Ball.FollowPaddle(_Paddle_Top);
                }
            }
            Ball_Outside_Handler();
            CheckForBrickCollision(_Ball, bricks);
            if(bossSpawned == true)
            {
                BossCollision(_Ball,_Boss);
            }

            PaddleEvents();
            if (Collisions.Collision_Ball_Paddle(_Ball, _Paddle_Top) == true)
            {
                hitTop = true;
                hitBottom = false;
            }
            if (Collisions.Collision_Ball_Paddle(_Ball, _Paddle_Bottom) == true)
            {
                hitBottom = true;
                hitTop = false;
            }
            Collisions.Collision_Wall_Handler(_Ball, _PlayingField);

            if (Space)
            {
                if (_Ball.catched)
                {
                    _Ball.catched = false;
                }
            }

            if (playerLifes == 0)
            {
                GameOver();
            }

            if (IsBricksEmpty() && !bossSpawned)
            {
                SpawnBoss();
            }

            if (bossSpawned && _Boss.bossLife <= 0)
            {
                DespawnBoss();
                GameOver();
            }
        }

        private bool IsBricksEmpty()
        {
            bool isBricksEmpty = true;
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i] != null)
                {
                    isBricksEmpty = false;
                }
            }
            return isBricksEmpty;
        }

        private void UpdateEvent()
        {
            if (ev != null)
            {
                ev.Update();
                ev.CheckCollision();
                ev.CheckOutOfBounds(_PlayingField.Height);
                ev.CreateEvent();
                if (ev.EventFinished)
                {
                    _RenderObjects.Remove(ev);
                    ev = null;
                }
            }
        }

        private void InitializeMenu()
        {
            CreateBricks(4, 9);

            _RenderObjects.Add(_PlayingField);
            _RenderObjects.Add(_StartButton);

            for (int i = 0; i < BRICKS; i++)
            {
                _RenderObjects.Add(bricks[i]);
            }
        }

        private void AddRenderObjects()
        {
            _RenderObjects.Add(_PlayingField);
            _RenderObjects.Add(_PointsText);
            _RenderObjects.Add(_Paddle_Bottom);
            _RenderObjects.Add(_Paddle_Top);
            _RenderObjects.Add(_Ball);

            for (int i = 0; i < BRICKS; i++)
            {
                _RenderObjects.Add(bricks[i]);
            }
        }
        
        private void AddUpdateObjects()
        {
            _UpdateObjects.Add(_Paddle_Bottom);
            _UpdateObjects.Add(_Paddle_Top);
            _UpdateObjects.Add(_Ball);
        }

        public void SwitchPlayerMode()
        {
            TwoPlayerMode = !TwoPlayerMode;
            if (!TwoPlayerMode)
                _Paddle_Top.Transform.Position = new Vector2D(_Paddle_Bottom.Transform.Position.X, _Paddle_Top.Transform.Position.Y);
            return;
        }


        private void Render(IntPtr renderer, List<IRenderable> renderObjects)
        {
            //Render every renderableObject
            foreach (var renderObject in renderObjects)
            {
                if (renderObject != null)
                {
                    renderObject.Render(renderer);
                }
            }
            SDL_RenderPresent(renderer);
        }
        
        
        private void Ball_Outside_Handler()
        {
            if (_Ball.Transform.Position.Y < 0)
            {
                playerLifes--;
                _Ball.catched = true;
                _Ball.Speed = Ball.START_SPEED;
                _Set_Ball_Player_Bottom = false;
            }
            else if (_Ball.Transform.Position.Y > _PlayingField.Height)
            {
                playerLifes--;
                _Ball.catched = true;
                _Ball.Speed = Ball.START_SPEED;
                _Set_Ball_Player_Bottom = true;
            }
        }

        void CreateBricks(int rows, int columns)
        {
            Dimension brick_Dimension = new Dimension();
            int offset = 0;

            if (_PlayingField.Width == 1920)
            {
                brick_Dimension = new Dimension(150, 40); //123, 70
                offset = 5;
            }
            else if (_PlayingField.Width == 1280)
            {
                brick_Dimension = new Dimension(150, 40); //84, 45
                offset = 2;
            }
            else if (_PlayingField.Width == 720)
            {
                brick_Dimension = new Dimension(150, 40); //47, 26
                offset = 1;
            }

            for (int n = 0, x = _PlayingField.Width / 5, y = (int)((_PlayingField.Height - (brick_Dimension.Height + offset) * rows) / 2); n < BRICKS; n++, x += ((int)brick_Dimension.Width + offset))
            {
                if (x > _PlayingField.Width - (_PlayingField.Width / 5) - offset) //If x is near the right edge of the screen
                {
                    x = _PlayingField.Width / 5; //We start going from the left again
                    y += (int)brick_Dimension.Height + offset; //And move down a little
                }
                Vector2D position = new Vector2D(x, y);
                bricks[n] = new Brick(renderer, position, brick_Dimension, 3);
            }
        }

        
        public void CheckForBrickCollision(Ball ball, Brick[] bricks)
        {
            for (int n = 0; n < bricks.Length; n++)
            {
                if (bricks[n] != null)
                {
                    if (Collisions.CheckForCollision(ball, bricks[n]) )
                    {
                       
                        Collisions.Collision_Brick_Handler(ball, bricks[n]);
                       
                        //Create an Event if there is none yet (Only one allowed at once)
                        if (ev == null)
                        {
                            // ~65% chance
                            int randomNumber = rnd.Next(0, 100);
                            if (randomNumber <= 65)
                            {

                                    if (hitBottom == true)
                                    {
                                        // ev = new Event(ball, _Paddle_Bottom, _Boss, this);
                                        //ev.FallDownFromBoss();
                                        ev = new Event(ball, _Paddle_Bottom, bricks[n], this);
                                        ev.FallDown();
                                        _RenderObjects.Add(ev);

                                    }
                                    else if (hitTop == true)
                                    {
                                        // ev = new Event(ball, _Paddle_Top, _Boss, this);
                                        //ev.FallDownFromBoss();
                                        ev = new Event(ball, _Paddle_Top, bricks[n], this);
                                        ev.FallDown();
                                        _RenderObjects.Add(ev);
                                    }                                                            
                            }
                        }
                        _Points += 2 * _Multiplier;

                        if (bricks[n].LoseLive() == 0)
                        {
                            _RenderObjects.Remove(bricks[n]);
                            bricks[n] = null;
                            _Ball.Accelerate();
                        }
                        break;
                    }
                }
            }
        }

        private void BossCollision(Ball ball, Boss boss)
        {
            if (Collisions.CheckForCollision(ball, boss))
            {
                Collisions.Collision_Brick_Handler(ball, boss);
                if (ev == null)
                {
                    // ~65% chance
                    int randomNumber = rnd.Next(0, 100);
                    if (randomNumber <= 65)
                    {
                        if (hitBottom == true)
                        {
                            ev = new Event(ball, _Paddle_Bottom, _Boss, this);
                            ev.FallDownFromBoss();
                            //ev = new Event(ball, _Paddle_Bottom, bricks[n], this);
                            //ev.FallDown();
                            _RenderObjects.Add(ev);

                        }
                        else if (hitTop == true)
                        {
                            ev = new Event(ball, _Paddle_Top, _Boss, this);
                            ev.FallDownFromBoss();
                            //ev = new Event(ball, _Paddle_Bottom, bricks[n], this);
                            //ev.FallDown();
                            _RenderObjects.Add(ev);
                        }
                    }
                }
                   boss.bossLife -= ball.damage;
            }
        }

        public void SpawnBoss()
        {
            _Boss = new Boss(_PlayingField.Width / 2, _PlayingField.Height / 5, 200, 217, new Sprite(renderer, "boss5.png"), 20, _Ball);
            bossSpawned = true;
            _RenderObjects.Add(_Boss);
            _Ball.Transform.Position = new Vector2D(_Paddle_Bottom.Transform.Position.X + _Paddle_Bottom.Transform.Dimension.Width / 2, _Paddle_Bottom.Transform.Position.Y - 15);
        }

        public void DespawnBoss()
        {
            bossSpawned = false;
            _RenderObjects.Remove(_Boss);
        }


        public void PaddleMovement(int mouseposition)
        {
            if (TwoPlayerMode)
            {
                _Paddle_Bottom.PaddleMovement(mouseposition);
            }
            else
            {
                _Paddle_Top.PaddleMovement(mouseposition);
                _Paddle_Bottom.PaddleMovement(mouseposition);
            }
        }

        public void PaddleEvents()
        {
            if (PlayerOneMoveLeft) { _Paddle_Bottom.MovePaddle(Paddle.Direction.LEFT, _PlayingField); }
            else if (PlayerOneMoveRight) { _Paddle_Bottom.MovePaddle(Paddle.Direction.RIGHT, _PlayingField); }

            if (PlayerTwoMoveLeft) { _Paddle_Top.MovePaddle(Paddle.Direction.LEFT, _PlayingField); }
            else if (PlayerTwoMoveRight) { _Paddle_Top.MovePaddle(Paddle.Direction.RIGHT, _PlayingField); }

            if (SpeedUp)
            {
                _Paddle_Top.ChangeSpeed(Paddle.Speed.FAST);
                _Paddle_Bottom.ChangeSpeed(Paddle.Speed.FAST);
            }
            else
            {
                _Paddle_Top.ChangeSpeed(Paddle.Speed.NORMAL);
                _Paddle_Bottom.ChangeSpeed(Paddle.Speed.NORMAL);
            }
        }
        #endregion

        #region Enums
        public enum State { START_GAME, PLAY_GAME, GAME_OVER }
        #endregion
    }
}