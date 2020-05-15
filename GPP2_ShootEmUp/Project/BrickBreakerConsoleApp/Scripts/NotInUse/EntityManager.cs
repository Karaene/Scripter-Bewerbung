using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    public sealed class EntityManager
    {
        //Singleton Pattern
        private EntityManager() { }
        private static EntityManager instance = null;
        public static EntityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityManager();
                }
                return instance;
            }
        }

        #region Attributes
        private Dictionary<int, List<IEntityComponent>> gameEntities = new Dictionary<int, List<IEntityComponent>>();
        #endregion

        #region Methodes
        public void AddEntity(int id)
        {
            List<IEntityComponent> components = new List<IEntityComponent>();
            gameEntities.Add(id, components);
        }

        public void DeleteEntity(int id)
        {
            if (gameEntities[id] != null)
            {
                gameEntities.Remove(id);
            }
            else
            {
                Console.WriteLine("Error: Entity wit Id " + id + " not found!");
            }

        }

        public void AddComponent(int id, IEntityComponent component)
        {
            if (gameEntities[id] != null)
            {
                gameEntities[id].Add(component);
            }
            else
            {
                Console.WriteLine("Error: Entity with Id " + id + " not found!");
            }
        }

        public void DeleteComponent(int id, IEntityComponent component)
        {
            if (gameEntities[id] != null)
            {
                if (gameEntities[id].Contains(component))
                {
                    gameEntities[id].Remove(component);
                }
                else
                {
                    Console.WriteLine("Error: Entity " + id + " does not have a " + component + " !");
                }
            }
            else
            {
                Console.WriteLine("Error: Entity with Id " + id + " not found!");
            }
        }

        public List<IEntityComponent> GetEntity(int id)
        {
            return gameEntities[id];
        }

        public void ShowComponents(int id)
        {
            if (gameEntities[id] != null)
            {
                if (gameEntities[id].Count > 0)
                {
                    Console.WriteLine("Entity " + id + " contains the following components: ");
                    foreach (var component in gameEntities[id])
                    {
                        Console.WriteLine(component);
                    }
                }
                else
                {
                    Console.WriteLine(" Entity " + id + " does not have a component!");
                }
            }
            else
            {
                Console.WriteLine("Error: Entity with Id " + id + " not found!");
            }
        }

        public void Update()
        {
            foreach (var entity in gameEntities)
            {
                foreach (var component in entity.Value)
                {
                    //component.Update();
                }
            }
        }
        #endregion
    }
}
