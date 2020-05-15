using System;
using System.Collections.Generic;
using System.Text;

// Look if your System is placed inside a folder like this:
// namespace GPP_2019.Reference
// if this is the case, change it too this:
// namespace GPP_2019
namespace GPP_2019.Reference
{
    // Every System needs to inherit from ISystem
    class SomeSystem : ISystem
    {
        // Every System is build as a Singleton because it doesn't make sense to have more than one instance of a system//
        // You can copy the following code to achieve the Singleton behaviour for your System.                          //
        // Just replace "SomeSystem" with your specific System Class name                                               //
        private static SomeSystem instance = null;                                                                      //
        private SomeSystem() { }                                                                                        //         
        public static SomeSystem Instance                                                                               //
        {                                                                                                               //
            get                                                                                                         //
            {                                                                                                           //
                if (instance == null)                                                                                   //
                {                                                                                                       //
                    instance = new SomeSystem();                                                                        //
                }                                                                                                       //
                return instance;                                                                                        //
            }                                                                                                           //
        }                                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Most system have a List of their specific components, so that the system can operate on every specific Component 
        // The List Type should be IEntityComponent if possible, but you can also get more specific like in this case here, if it doesnt need to be too abstract
        private List<SomeComponent> _someComponentsList = new List<SomeComponent>();

        // Each System needs to have this so called "factory-Method" !
        // Factory methods are used to create the specific Components the system is responsible for.
        // It provides, that every component is part of the _someComponentsList.
        // With this method setup, the system knows about every SomeComponent ever created and can operate on them!
        internal IEntityComponent CreateSomeComponent()
        {
            SomeComponent someComponent = new SomeComponent(this);
            _someComponentsList.Add(someComponent);
            return someComponent;
        }

        // Down here, you have space to provide every logic needed to operate on someComponets
        // Here is some example method:                                                     //
        private void SetHealth(string id, int health)                                       //
        {                                                                                   //
            foreach (var someComponent in _someComponentsList)                              //
            {                                                                               //
                if (someComponent.GameObject.Id == id)                                      //
                {                                                                           //
                    someComponent.Health = health;                                          //
                }                                                                           //
            }                                                                               //
        }                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////
        
        // You can also add an Update Method here, if your system needs to be updated regularly
        // For example like this:                                                            //
        public void Update() { }                                                             //
        ///////////////////////////////////////////////////////////////////////////////////////


        // If your System needs to communicate with another System, use the following:   /////////////////////////////////////////////////////////////////////////////
        private void AddListeners()                                                                                                                                 //
        {                                                                                                                                                           //
            // The EventSystem provide us two useful methodes                                                                                                       //
            // AddListener() and                                                                                                                                    //
            // RaiseEvent()                                                                                                                                         //
                                                                                                                                                                    //
            // This is the RaiseEvent Method, wich will trigger every System with AddListener("NameOfEvent", ...)                                                   //
            EventSystem.Instance.RaiseEvent("NameOfEvent");                                                                                                         //
            // You can also past some information through your events, by using parameters.                                                                         //
            // Here is an example:                                                                                                                                  //
            int value = 5;                                                                                                                                          //
            string data = "";                                                                                                                                       //
            EventSystem.Instance.RaiseEvent("NameOfEvent", value, data);                                                                                            //
            // You can add as many parameters as you need!                                                                                                          //
                                                                                                                                                                    //
            // This is the AddListener() Method, wich will listen for raised Events                                                                                 //
            // You can copy the AddListener() Method down there, because the Syntax is always the same                                                              //
            /*                                Here insert Name of your Eevent             Here you specify, if your Event should called only once or multiple times*/
            EventSystem.Instance.AddListener("NameOfEvent", new EventSystem.EventListener(false)                                                                    //
            {                                                                                                                                                       //
                /*        this are the parameters from RaiseEvent()*/                                                                                               //
                Method = (object[] parameters) => {                                                                                                                 //
                    /* Here you insert your code */                                                                                                                 //
                    // Could also be anotherMethodCall()                                                                                                            //
                    // If you want to get the parameters of your Event just use parameters[0], parameters[1], ... for the first, second and so on parameter         //
                }                                                                                                                                                   //
            });                                                                                                                                                     //
        }                                                                                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
