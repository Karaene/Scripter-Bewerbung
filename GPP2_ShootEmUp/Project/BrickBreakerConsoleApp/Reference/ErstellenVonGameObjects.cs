using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp.Reference
{
    // Hier ist ein Beispiel, wie GameObjects erstellt werden und man Components hinzufügt
    class ErstellenVonGameObjects
    {
        public void BuildGameObjects()
        {
            // GameObjects werden über die factory-methode vom GameObjectSystem erzeugt!
            // So kennt das GameObjectSystem jedes erstellte GameObject im Spiel
            GameObject gameObject = GameObjectSystem.Instance.CreateGameObject("NamensID");
            // Um Componenten hinzuzufügen, benutzt du die AddComponent Methode von GameObject
            // Diese Methode setzt nämlich gleichzeitig die GameObject Property von der Component,
            // sodass sie weiß, zu welchem GameObject sie gehört! (someComponent.GameObject = gameObject)
            // Ausserdem wird sie über AddComponent der Luste von Componenten des gameObject zugeordnet.
            gameObject.AddComponent(SomeSystem.Instance.CreateSomeComponent());
        }
    }
}
