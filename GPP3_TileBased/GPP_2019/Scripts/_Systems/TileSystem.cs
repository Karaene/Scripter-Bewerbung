using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GPP_2019
{
    class TileSystem : ISystem
    {
        int[,] background;
        int[,] foreground;
        int[,] doors;
        int[,] walls;

        string[] lines;

        int lengthx = 0;
        int lengthy = 0;
        public int CurrentLevel { get; set; }
        public double TileSize { get; set; } = 64;
        private List<IEntityComponent> tiles = new List<IEntityComponent>();
        private List<GameObject> tileList = new List<GameObject>();
        private List<Spritesheet> sheetBackground = new List<Spritesheet>();
        private List<Spritesheet> sheetForeground = new List<Spritesheet>();
        private static TileSystem instance = null;
        public static TileSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TileSystem();
                }
                return instance;
            }
        }

        public void ReadTxt(String path)
        {
            lines = System.IO.File.ReadAllLines(path);
            lengthx = lines[1].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            lengthy = lines[0].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

        }

        private void InterpreteFile(int type)
        {
            int counter = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                counter = 0;
                foreach (var ch in lines[i].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int x = 0;
                    Int32.TryParse(ch, out x);
                    if (type == 0)
                    {
                        background[i, counter] = x;
                    }
                    else if (type == 1)
                    {
                        foreground[i, counter] = x;
                    }
                    else if (type == 2)
                    {
                        walls[i, counter] = x;
                    }
                    else if (type == 3)
                    {
                        doors[i, counter] = x;
                    }
                    counter++;
                }
            }
        }

        //Adjust tilesize to level size/mapsize 
        private void AdjustTileSize()
        {
            double tilemapSize = TileSize * background.GetLength(0);
            double multiplicator = LevelManager.LEVEL_WIDTH / tilemapSize;
            TileSize = TileSize * multiplicator;
        }

        public TileComponent CreateTileComponent(int id, bool walkable, bool door, int level)
        {
            TileComponent tileComponent = new TileComponent(this, id, walkable, door, level);
            tiles.Add(tileComponent);
            return tileComponent;
        }

        public void CreateForeground(string path, string sprite, int level)
        {
            ReadTxt(path);
            foreground = new int[lengthx, lengthy];
            InterpreteFile(1);

            int size = GetSheetCount(foreground) + 1;
            LoadSpriteSheets(size, sprite, 1);
            Console.WriteLine();


            double posx = 0 - LevelManager.SCREEN_OFFSET_X + TileSize / 2;
            double posy = 0 - LevelManager.SCREEN_OFFSET_Y + TileSize / 2;
            int type = 0;
            for (int i = 0; i < foreground.GetLength(0); i++)
            {
                for (int j = 0; j < foreground.GetLength(1); j++)
                {
                    type = foreground[i, j];
                    if (type != -1)
                    {
                        AddForeGroundSpritePointer(type);
                        GameObject tile = GameObjectSystem.Instance.CreateGameObject("tile_f" + i + "," + j + "," + level);
                        tile.Transform.Size = new Size(TileSize, TileSize);
                        tile.Transform.Position = new Vector2D(posx, posy);
                        tile.AddComponent(TileSystem.Instance.CreateTileComponent(type, true, false, level));
                        tile.AddComponent(RenderSystem.Instance.CreateTileSheetRenderer(sheetForeground[type], new Vector2D(TileSize, TileSize), Layer.FOREGROUND, type));
                        tileList.Add(tile);
                    }
                    posx += TileSize;
                    if (posx >= LevelManager.LEVEL_WIDTH - LevelManager.SCREEN_OFFSET_X)
                    {
                        posx = 0 - LevelManager.SCREEN_OFFSET_X + TileSize / 2;
                    }

                }
                posy += TileSize;
            }

        }
        public void LoadSpriteSheets(int size, String sprite, int id)
        {
            for (int i = 0; i < size; i++)
            {
                Spritesheet spritesheet = RenderSystem.Instance.GetSheet(sprite, new Vector2D(TileSize, TileSize));
                if (id == 0)
                {
                    sheetBackground.Add(spritesheet);
                }
                else
                {
                    sheetForeground.Add(spritesheet);
                }
            }
        }
        private void AddForeGroundSpritePointer(int id)
        {
            sheetForeground[id].IsSpriteFlipped = false;
            if (id <= 31)
            {
                sheetForeground[id].animationPointer = 0;
                sheetForeground[id].spritePointer = id;
            }
            else if (id <= 63)
            {
                sheetForeground[id].animationPointer = 1;
                sheetForeground[id].spritePointer = id - 32;
            }
            else if (id <= 95)
            {
                sheetForeground[id].animationPointer = 2;
                sheetForeground[id].spritePointer = id - 32 * 2;
            }
            else if (id <= 127)
            {
                sheetForeground[id].animationPointer = 3;
                sheetForeground[id].spritePointer = id - 32 * 3;
            }
        }

        private void AddBackgroundSpritePointer(int id)
        {
            sheetBackground[id].IsSpriteFlipped = false;
            if (id <= 31)
            {
                sheetBackground[id].animationPointer = 0;
                sheetBackground[id].spritePointer = id;
            }
            else if (id <= 63)
            {
                sheetBackground[id].animationPointer = 1;
                sheetBackground[id].spritePointer = id - 32;
            }
            else if (id <= 95)
            {
                sheetBackground[id].animationPointer = 2;
                sheetBackground[id].spritePointer = id - 32 * 2;
            }

            else if (id <= 127)
            {
                sheetBackground[id].animationPointer = 3;
                sheetBackground[id].spritePointer = id - 32 * 3;
            }

        }

        private int GetSheetCount(int[,] array)
        {
            int type = 0;
            int tempHighest = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    type = array[i, j];
                    if (type > tempHighest)
                    {
                        tempHighest = type;
                    }
                }
            }
            return tempHighest;
        }
        public void CreateBackgroundTiles(String path, String colliderpath, String doorpath, String sprite, int level)
        {
            ReadTxt(path);
            background = new int[lengthy, lengthx];
            InterpreteFile(0);
            ReadTxt(colliderpath);
            walls = new int[lengthx, lengthy];
            InterpreteFile(2);

            ReadTxt(doorpath);
            doors = new int[lengthx, lengthy];
            InterpreteFile(3);

            CurrentLevel = level;
            int size = GetSheetCount(background) + 1;
            LoadSpriteSheets(size, sprite, 0);

            Console.WriteLine(sheetBackground.Count);

            double posx = 0 - LevelManager.SCREEN_OFFSET_X + TileSize / 2;
            double posy = 0 - LevelManager.SCREEN_OFFSET_Y + TileSize / 2;
            int type = 0;
            int typeCollider = 1;
            int doorCollider = 1;
            for (int i = 0; i < background.GetLength(0); i++)
            {
                for (int j = 0; j < background.GetLength(1); j++)
                {
                    type = background[i, j];
                    typeCollider = walls[i, j];
                    doorCollider = doors[i, j];
                    AddBackgroundSpritePointer(type);

                    GameObject tile = GameObjectSystem.Instance.CreateGameObject("tile" + i + "," + j + "," + level);
                    tile.Transform.Size = new Size(TileSize, TileSize);
                    tile.Transform.Position = new Vector2D(posx, posy);
                    tile.AddComponent(RenderSystem.Instance.CreateTileSheetRenderer(sheetBackground[type], new Vector2D(TileSize, TileSize), Layer.BACKGROUND, type));

                    if (typeCollider == 0)
                    {
                        tile.AddComponent(TileSystem.Instance.CreateTileComponent(type, false, false, level));
                        tile.AddComponent(PhysicSystem.Instance.CreateBoxCollider(tile, 0, 0));
                        tile.AddComponent(PhysicSystem.Instance.CreateObstacleComponent());
                    }
                    else if (doorCollider == 0)
                    {
                        tile.AddComponent(TileSystem.Instance.CreateTileComponent(type, true, true, level));
                        tile.AddComponent(PhysicSystem.Instance.CreateBoxCollider(tile, 0, 0));
                        tile.AddComponent(PhysicSystem.Instance.CreateObstacleComponent());
                    }
                    else
                    {
                        tile.AddComponent(TileSystem.Instance.CreateTileComponent(type, true, false, level));
                    }
                    // tile.AddComponent(AStarPathfinding.Instance.CreateNodeComponent(tile)); 
                    tileList.Add(tile);

                    posx += TileSize;
                    if (posx >= LevelManager.LEVEL_WIDTH - LevelManager.SCREEN_OFFSET_X)
                    {
                        posx = 0 - LevelManager.SCREEN_OFFSET_X + TileSize / 2;
                    }

                }
                posy += TileSize;
            }

        }
        public void TurnOffEverything()
        {
            foreach (var tile in tileList)
            {
                tile.Active = false;
            }
            sheetBackground.Clear();
            sheetForeground.Clear();
        }
        public void SwapLevels(int id)
        {
            foreach (var tile in tileList)
            {
                //turn on startscreen tiles and turn of level1
                if (id == 0)
                {
                    CurrentLevel = 0;
                    Program.Instance.SwapEnemies();
                    if (tile.GetComponent<TileComponent>().Level == 0)
                    {
                        tile.Active = true;
                    }
                    else
                    {
                        tile.Active = false;
                    }
                    //update player pos
                    if (tile.GetComponent<TileComponent>().door == true)
                    {
                        PlayerControl.Instance.Player.GameObject.Transform.Position = new Vector2D(tile.Transform.Position.X, tile.Transform.Position.Y);
                    }

                }
                //turn on level1 tiles and turn of startscreen
                else if (id == 1)
                {
                    CurrentLevel = 1;
                    // Program.Instance.CreateEnemiesFloor1();
                    Program.Instance.SwapEnemies2();
                    if (tile.GetComponent<TileComponent>().Level == 1)
                    {
                        tile.Active = true;
                    }
                    else
                    {
                        tile.Active = false;
                    }

                    //update player pos
                    if (tile.GetComponent<TileComponent>().door == true)
                    {
                        PlayerControl.Instance.Player.GameObject.Transform.Position = new Vector2D(tile.Transform.Position.X, tile.Transform.Position.Y - 1500);
                    }

                }
            }
        }


        private void PrintMatrix()
        {
            for (int i = 0; i < background.GetLength(0); i++)
            {
                for (int j = 0; j < background.GetLength(1); j++)
                {
                    Console.Write(background[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

    }
}
