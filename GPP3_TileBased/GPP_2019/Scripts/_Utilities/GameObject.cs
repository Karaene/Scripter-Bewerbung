using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    public class GameObject
    {
        public string Id { get; set; }
        public Tag Tag { get; set; }
        public Transform Transform { get; set; }
        public bool Active { get; set; } = true;

        private List<IEntityComponent> components = new List<IEntityComponent>();
        //private Dictionary<Type , IEntityComponent> components = new Dictionary<Type, IEntityComponent>();

        public GameObject(string id)
        {
            Transform = new Transform(new Vector2D(0, 0), new Size(1,1));
            Id = id;
        }

        public T GetComponent<T>()
        {
            foreach (var comp in components)
            {
                if (comp.GetType() == typeof(T))
                {
                    return (T)comp;
                }
            }
            return default(T);
        }

        public bool ContainsComponent<T>()
        {
            foreach (var comp in components)
            {
                if (comp.GetType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
        }


        public List<IEntityComponent> GetAllComponents()
        {
            return components;
        }

        internal void AddComponent(IEntityComponent component)
        {
            components.Add(component);
            component.GameObject = this;
        }

        public void RemoveComponent(IEntityComponent component)
        {
            foreach (var comp in components)
            {
                if (comp.Equals(component))
                    components.Remove(comp);
                return;
            }
            Console.WriteLine("Could'nt delete compnent " + component + " because I have not found it!");
        }

        public void RemoveAllComponents()
        {
            components.Clear();
        }
    }

    public enum Tag { PLAYER, ENEMY}
}
