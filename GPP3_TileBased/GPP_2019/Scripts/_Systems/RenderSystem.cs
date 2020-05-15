using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace GPP_2019
{
    public class RenderSystem : ISystem
    {
        private static IntPtr _Window = IntPtr.Zero;
        private static IntPtr _Renderer = IntPtr.Zero;

        IntPtr NULL = IntPtr.Zero;

        private IntPtr BackgroundTexture = SDL_CreateTexture(_Renderer, SDL_PIXELFORMAT_RGBA8888, 2, Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
        private IntPtr MiddleGroundTexture = SDL_CreateTexture(_Renderer, SDL_PIXELFORMAT_RGBA8888, 2, Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
        private IntPtr ForegroundTexture = SDL_CreateTexture(_Renderer, SDL_PIXELFORMAT_RGBA8888, 2, Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
        private IntPtr UITexture = SDL_CreateTexture(_Renderer, SDL_PIXELFORMAT_RGBA8888, 2, Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);


        private bool drawOutline = false;
        private List<GameObject> drawOutlineGOs = new List<GameObject>();

        private SDL_Rect camera;

        private List<SpriteRendererComponent> _activeRenderComponents = new List<SpriteRendererComponent>();
        private List<TextComponent> _activeTextComponents = new List<TextComponent>();

        private RenderSystem()
        {
            Init();
        }
        private static RenderSystem instance = null;
        public static RenderSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderSystem();
                }
                return instance;
            }
        }

        private bool Init()
        {
            AddListeners();

            bool success = true;

            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                Program.Error("Init failed");
                success = false;
            }

            _Window = SDL_CreateWindow
            ("Forager2.0",
                SDL_WINDOWPOS_UNDEFINED,
                SDL_WINDOWPOS_UNDEFINED,
                Program.SCREEN_WIDTH,
                Program.SCREEN_HEIGHT,
                SDL_WindowFlags.SDL_WINDOW_SHOWN
            );

            //SDL_SetWindowFullscreen(_Window, (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

            if (_Window == null)
            {
                Program.Error("Window Creation failed");
                success = false;
            }

            _Renderer = SDL_CreateRenderer(_Window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            return success;
        }
        internal IEntityComponent CreateSpriteRenderer(string filePath, Layer layer)
        {
            Sprite sprite = new Sprite(_Renderer, filePath);
            SpriteRendererComponent rc = new SpriteRendererComponent(this, sprite);
            rc.Layer = layer;
            _activeRenderComponents.Add(rc);
            return rc;
        }

        internal Spritesheet GetSheet(string filePath, Vector2D spriteSize)
        {
            Spritesheet spritesheet = new Spritesheet(_Renderer, filePath, spriteSize);
            return spritesheet;
        }

        internal IEntityComponent CreateTileSheetRenderer(Spritesheet sheet, Vector2D spriteSize, Layer layer, int id)
        {
            SpriteRendererComponent rc = new SpriteRendererComponent(this, id, sheet);
            rc.Layer = layer;
            _activeRenderComponents.Add(rc);
            return rc;
        }

        internal IEntityComponent CreateSpritesheetRenderer(string filePath, Vector2D spriteSize, int frames, int speed, Layer layer)
        {
            Spritesheet spritesheet = new Spritesheet(_Renderer, filePath , spriteSize);
            SpriteRendererComponent rc = new SpriteRendererComponent(this, spritesheet, frames, speed);
            rc.Layer = layer;
            _activeRenderComponents.Add(rc);
            return rc;
        }

        internal IEntityComponent CreateTextComponent()
        {
            Text text = new Text(0,0,0,0);
            TextComponent tc = new TextComponent(this, text);
            _activeTextComponents.Add(tc);
            return tc;
        }

        public void ChangeWindowSize(int w , int h)
        {
            SDL_SetWindowSize(_Window, w, h);
        }
        public void Render()
        {
            SDL_RenderClear(_Renderer);

            foreach (var rc in _activeRenderComponents)
            {
                //Center Render
                rc.Rectangle = new Rectangle(rc.GameObject.Transform);
                rc.Rectangle.rect.x = (int)(rc.Rectangle.rect.x - rc.GameObject.Transform.Size.Width / 2) - camera.x;
                rc.Rectangle.rect.y = (int)(rc.Rectangle.rect.y - rc.GameObject.Transform.Size.Height / 2) - camera.y;

                if (rc.BAnimated)
                {
                    rc.Frames++;
                    if (rc.Frames >= rc.Speed)
                    {
                        ((Spritesheet)rc.Sprite).spritePointer++;
                        rc.Frames = 0;
                    }

                    if (rc.animationState[1] && !rc.animationState[3])
                    {
                        ((Spritesheet)rc.Sprite).IsSpriteFlipped = true;
                    }

                    if (rc.animationState[3] && !rc.animationState[1])
                    {
                        ((Spritesheet)rc.Sprite).IsSpriteFlipped = false;
                    }

                    if (((Spritesheet)rc.Sprite).spritePointer >= rc.Sprite.source_Rect.w / ((Spritesheet)rc.Sprite).sprite_Rect.w)
                        ((Spritesheet)rc.Sprite).spritePointer = 0;
                }

                /*
                if (rc.GameObject.Active && CollisionSystem.Instance.InsideCameraFrustrum(rc.GameObject))
                {
                    //Console.WriteLine("Camera: " + camera.x + " / " + camera.y + " | Camera width: " + camera.w + " / " + camera.h);
                    //Console.WriteLine("Needs rendering: " + rc.GameObject.Id + " / " + rc.GameObject.Transform.Position);

                    if(rc.Layer == Layer.BACKGROUND)
                    {
                        SDL_SetRenderTarget(_Renderer, BackgroundTexture);
                        SDL_RenderClear(_Renderer);
                        SDL_RenderCopy(_Renderer, BackgroundTexture, rc.Rectangle.rect,ref NULL);
                    }
                        

                    if(rc.Layer == Layer.MIDDLEGROUND)
                        SDL_SetRenderTarget(_Renderer, MiddleGroundTexture);

                    if (rc.Layer == Layer.FOREGROUND)
                        SDL_SetRenderTarget(_Renderer, ForegroundTexture);

                    if (rc.Layer == Layer.UI)
                        SDL_SetRenderTarget(_Renderer, UITexture);

                    
                    rc.Sprite.Render(_Renderer, rc.Rectangle.rect, rc.GameObject.Transform.Rotation);
                }
                */

                /*
                if (rc.GameObject.Active)
                    rc.Sprite.Render(_Renderer, rc.Rectangle.rect, rc.GameObject.Transform.Rotation);
                */
            }

            foreach (var rc in _activeRenderComponents)
            {
                if (rc.GameObject.Active && CollisionSystem.Instance.InsideCameraFrustrum(rc.GameObject) && rc.Layer == Layer.BACKGROUND)
                {
                    //Console.WriteLine("Camera: " + camera.x + " / " + camera.y + " | Camera width: " + camera.w + " / " + camera.h);
                    //Console.WriteLine("Needs rendering: " + rc.GameObject.Id + " / " + rc.GameObject.Transform.Position);



                    rc.Sprite.Render(_Renderer, rc.Rectangle.rect, rc.GameObject.Transform.Rotation);
                }
            }
            foreach (var rc in _activeRenderComponents)
            {
                if (rc.GameObject.Active && CollisionSystem.Instance.InsideCameraFrustrum(rc.GameObject) && rc.Layer == Layer.MIDDLEGROUND)
                {
                    rc.Sprite.Render(_Renderer, rc.Rectangle.rect, rc.GameObject.Transform.Rotation);
                }
            }
            foreach (var rc in _activeRenderComponents)
            {
                if (rc.GameObject.Active && CollisionSystem.Instance.InsideCameraFrustrum(rc.GameObject) && rc.Layer == Layer.FOREGROUND)
                {
                    rc.Sprite.Render(_Renderer, rc.Rectangle.rect, rc.GameObject.Transform.Rotation);
                }
            }
            foreach (var rc in _activeRenderComponents)
            {
                if (rc.GameObject.Active && CollisionSystem.Instance.InsideCameraFrustrum(rc.GameObject) && rc.Layer == Layer.UI)
                {
                    rc.Sprite.Render(_Renderer, rc.Rectangle.rect, rc.GameObject.Transform.Rotation);
                }
            }


            foreach (var text in _activeTextComponents)
            {
                text.Text.Message_rect.x = (int)text.GameObject.Transform.Position.X;
                text.Text.Message_rect.y = (int)text.GameObject.Transform.Position.Y;
                text.Text.Message_rect.w = (int)text.GameObject.Transform.Size.Width;
                text.Text.Message_rect.h = (int)text.GameObject.Transform.Size.Height;

                //text.Text.Message_rect.x = text.Text.Message_rect.x - camera.x;
                //text.Text.Message_rect.y = text.Text.Message_rect.y - camera.y;

                text.Text.Message_rect.x -= (int)text.GameObject.Transform.Size.Width / 2;
                text.Text.Message_rect.y -= (int)text.GameObject.Transform.Size.Height / 2;

                if (text.GameObject.Active)
                    text.Text.Render(_Renderer);

            }

            if (drawOutline)
            {
                foreach (var obj in drawOutlineGOs)
                {
                    if(obj.GetComponent<BoxColliderComponent>() != null)
                    {
                        SDL_Rect rect = new SDL_Rect();

                        //Get Centered Position
                        rect.x = (int)(obj.Transform.Position.X - (int)obj.Transform.Size.Width / 2) - camera.x;
                        rect.y = (int)(obj.Transform.Position.Y - (int)obj.Transform.Size.Height / 2) - camera.y;
                        //Calculate eventual Offset caused by smaller/bigger box
                        rect.x += (int)(obj.Transform.Size.Width - obj.GetComponent<BoxColliderComponent>().Size.Width) / 2;
                        rect.y += (int)(obj.Transform.Size.Height - obj.GetComponent<BoxColliderComponent>().Size.Height) / 2;

                        rect.w = (int)obj.GetComponent<BoxColliderComponent>().Size.Width;
                        rect.h = (int)obj.GetComponent<BoxColliderComponent>().Size.Height;
                        SDL_SetRenderDrawColor(_Renderer, 0, 0, 0, 255);
                        SDL_RenderDrawRect(_Renderer, ref rect);
                    }

                    if (obj.GetComponent<CircleColliderComponent>() != null)
                    {
                        int x = (int)obj.Transform.Position.X - camera.x;
                        int y = (int)obj.Transform.Position.Y - camera.y;

                        Sphere tmpSphere = new Sphere(new Vector2D(x,y), obj.GetComponent<CircleColliderComponent>().Radius);
                        
                        SDL_SetRenderDrawColor(_Renderer, 0, 0, 0, 255);
                        tmpSphere.DrawSphere(_Renderer);
                    }
                }
            }

            SDL_RenderPresent(_Renderer);
            //Console.WriteLine("Camera.Y: " + camera.y + " / Camera.X: " + camera.x);
        }

        private IEnumerable<SpriteRendererComponent> SubsetThatNeedsRendering()
        {
            
            yield break;
        }


        private void AddListeners()
        {
            EventSystem.Instance.AddListener("Cameraupdate", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => { camera = (SDL_Rect)parameters[0]; }
            });

            EventSystem.Instance.AddListener("DrawOutline", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    GameObject obj = (GameObject)parameters[0];
                    drawOutlineGOs.Add(obj);
                    drawOutline = true;

                }
            });

            EventSystem.Instance.AddListener("StopDrawOutline", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    drawOutlineGOs.Clear();
                    drawOutline = false;
                }
            });

            EventSystem.Instance.AddListener("KeyDown", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    SDL_Keycode key = (SDL_Keycode)parameters[0];
                    GameObject player = (GameObject)parameters[1];
                    if (key != SDL_Keycode.SDLK_w && key != SDL_Keycode.SDLK_a && key != SDL_Keycode.SDLK_s && key != SDL_Keycode.SDLK_d)
                    {
                        return;
                    }

                    //Hier laufanimation Start
                    SpriteRendererComponent rc = player.GetComponent<SpriteRendererComponent>();

                    ((Spritesheet)(rc.Sprite)).animationPointer = 1;

                    if (key == SDL_Keycode.SDLK_w)
                    {
                        rc.animationState[0] = true;
                    }
                    if (key == SDL_Keycode.SDLK_a)
                    {
                        rc.animationState[1] = true;
                    }
                    if (key == SDL_Keycode.SDLK_s)
                    {
                        rc.animationState[2] = true;
                    }
                    if (key == SDL_Keycode.SDLK_d)
                    {
                        rc.animationState[3] = true;
                    }

                    if (rc.animationState[1] && rc.animationState[3] && !rc.animationState[0] && !rc.animationState[2])
                    {
                        ((Spritesheet)(rc.Sprite)).animationPointer = 0;
                    }
                }
            });

            EventSystem.Instance.AddListener("KeyUp", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    SDL_Keycode key = (SDL_Keycode)parameters[0];
                    GameObject player = (GameObject)parameters[1];
                    if (key != SDL_Keycode.SDLK_w && key != SDL_Keycode.SDLK_a && key != SDL_Keycode.SDLK_s && key != SDL_Keycode.SDLK_d)
                    {
                        return;
                    }

                    SpriteRendererComponent rc = player.GetComponent<SpriteRendererComponent>();

                    if (key == SDL_Keycode.SDLK_w)
                    {
                        rc.animationState[0] = false;
                    }
                    if (key == SDL_Keycode.SDLK_a)
                    {
                        rc.animationState[1] = false;
                    }
                    if (key == SDL_Keycode.SDLK_s)
                    {
                        rc.animationState[2] = false;
                    }
                    if (key == SDL_Keycode.SDLK_d)
                    {
                        rc.animationState[3] = false;
                    }

                    //Please make a function that determents the appropriate animation instead of this IF hell....there are also IFs in the Listener above

                    if (rc.animationState[1] && !rc.animationState[3] || !rc.animationState[1] && rc.animationState[3])
                    {
                        ((Spritesheet)(rc.Sprite)).animationPointer = 1;
                    }

                    if (rc.animationState[1] && rc.animationState[3] && !rc.animationState[0] && !rc.animationState[2])
                    {
                        ((Spritesheet)(rc.Sprite)).animationPointer = 0;
                    }

                    if (!rc.animationState[0] && !rc.animationState[1] && !rc.animationState[2] && !rc.animationState[3])
                    {
                        ((Spritesheet)(rc.Sprite)).animationPointer = 0;
                    }

                }
            });
        }
        
    }
}

public enum Layer { BACKGROUND, MIDDLEGROUND, FOREGROUND, UI}
