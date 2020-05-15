using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class GameObjectSystem
    {
        
        private Dictionary<string, GameObject> gameObjectDictionary = new Dictionary<string, GameObject>();
        
        private GameObjectSystem() { }
        private static GameObjectSystem instance = null;
        public static GameObjectSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObjectSystem();
                }
                return instance;
            }
        }

        public void RemoveGameObject(string name)
        {
            EventSystem.Instance.RaiseEvent("GameObjectDestroyed", name);
            if (gameObjectDictionary[name] == null)
                return;

            gameObjectDictionary.Remove(name);
        }

        public GameObject CreateGameObject(string name)
        {
            GameObject tmpGO = new GameObject(name);
            gameObjectDictionary.Add(name, tmpGO);
            return tmpGO;
        }

        public GameObject CreateGameObject()
        {
            GameObject tmpGO = new GameObject("");
            return tmpGO;
        }

        public GameObject GetGameObject(string id)
        {
            if(gameObjectDictionary.ContainsKey(id))
                return gameObjectDictionary[id];
            return null;
        }

        public void ShowAllComponents(string name)
        {
            if (!gameObjectDictionary.ContainsKey(name))
            {
                Console.WriteLine("There is no GameObject called: " + name);
                return;
            }
            foreach (var comp in gameObjectDictionary[name].GetAllComponents())
            {
                Console.WriteLine("Component " + comp);
            }
        }
        

    }
}
