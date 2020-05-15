using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class SpawnSystem : ISystem
    {
        LTimer timer = new LTimer();

        private List<SpawnerComponent> _activeSpawner = new List<SpawnerComponent>();

        /*                 EnemyType       EnemyPool   */
        private Dictionary<EnemyType, List<GameObject>> _enemyPools = new Dictionary<EnemyType, List<GameObject>>();

        private List<Dictionary<EnemyType, List<GameObject>>> _spawnerEnemyPools = new List<Dictionary<EnemyType, List<GameObject>>>();

        private SpawnSystem() { }
        private static SpawnSystem instance = null;
        public static SpawnSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpawnSystem();
                }
                return instance;
            }
        }

        public void Init()
        {
            timer.Start();
            
            List<GameObject> gameObjects = new List<GameObject>();

            _enemyPools.Add(EnemyType.NORMAL, gameObjects);
            for (int i = 0; i < 100; i++)
            {
                GameObject enemy = GameObjectSystem.Instance.CreateGameObject("Enemy" + i);
                enemy.Transform.Size = new Size(64, 64);
                enemy.AddComponent(RenderSystem.Instance.CreateSpriteRenderer("Sprites/enemy.png"));
                enemy.AddComponent(PhysicSystem.Instance.CreateBoxCollider(enemy));
                enemy.AddComponent(MovementSystem.Instance.CreateVelocityComponent(new Vector2D(4, 4)));
                enemy.AddComponent(BotControl.Instance.CreateAiComponent());
                enemy.AddComponent(FightSystem.Instance.CreateAttackComponent(AttackType.PUNCH, 10));
                enemy.Active = false;
                
                _enemyPools[EnemyType.NORMAL].Add(enemy);
            }
            
        }
 

        public SpawnerComponent CreateSpawner()
        {
            SpawnerComponent sc = new SpawnerComponent(this);
            _activeSpawner.Add(sc);
            return sc;
        }

        private void SpawnEnemy(SpawnerComponent spawner, EnemyType enemyType)
        {
            foreach (var enemy in _enemyPools[enemyType])
            {
                if (!enemy.Active)
                {
                    enemy.Transform.Position = spawner.GameObject.Transform.Position;
                    enemy.Active = true;
                    break;
                }
            }
        }
        
        public void SpawEnemyHorde(SpawnerComponent spawner, EnemyType enemyType)
        {
            Vector2D distanceBetween = Vector2D.ZERO;
            foreach (var enemy in _enemyPools[enemyType])
            {
                if (!enemy.Active)
                {
                    enemy.Transform.Position = spawner.GameObject.Transform.Position + distanceBetween;
                    distanceBetween += new Vector2D(enemy.Transform.Size.Width/2, 0);
                    --spawner.SpawnAmount;
                    enemy.Active = true;
                    if (spawner.SpawnAmount == 0)
                    {
                        break;
                    }
                }
            }
        }
        
        public void TweenSpawnerBetween(Vector2D startPos, Vector2D targetPos)
        {

        }

        public void Update()
        {
            foreach (var spawner in _activeSpawner)
            {
                
                if (spawner.SpawnAmount > 0 && (timer.GetTicks() > spawner.StartTick + spawner.CoolDown * 1000) && spawner.Mode == Mode.SINGLE)
                {
                    //Console.WriteLine(spawner.GameObject.Id);
                    SpawnEnemy(spawner, spawner.EnemyType);
                    spawner.SpawnAmount -= 1;
                    spawner.StartTick = timer.GetTicks();
                }

                if (spawner.SpawnAmount > 0 && spawner.Mode == Mode.HORDE)
                {
                    SpawEnemyHorde(spawner, EnemyType.NORMAL);
                }

                
            }
        }
    }
    public enum EnemyType { NORMAL }

}
