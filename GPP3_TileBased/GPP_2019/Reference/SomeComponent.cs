using System;
using System.Collections.Generic;
using System.Text;

// Look if your System is placed inside a folder like this:
// namespace GPP_2019.Reference
// if this is the case, change it too this:
// namespace GPP_2019
namespace GPP_2019.Reference
{
    // Jede Component muss von IEntityComponent erben und die Schnittstellenmember implementieren!
    // Schnittstellenmember stehen in IEntityComponent, oder wenn du mit der Maus über das rot gegringelte IEntityComponent gehst
    // da kannst du dann auch direkt implementieren lassen, musst die aber dann anpassen, dass sie wie unten aussehen!
    class SomeComponent : IEntityComponent
    {
        // Schnittstellenmember (Properties) die jede Component hat //
        public ISystem System { get; set; }                         //
        public GameObject GameObject { get; set; }                  //
        //////////////////////////////////////////////////////////////
        

        // Beispiel für eigene Property einer neuen Component ////////    
        public int Health { get; set; }                             //
        //////////////////////////////////////////////////////////////


        // Jede Componente hat einen Konstruktor
        // Als Parameter wird zumindest das übergeordnete System übergeben
        public SomeComponent(ISystem system)                            //
        {                                                               //
            System = system;                                            //
        }                                                               //
        //////////////////////////////////////////////////////////////////
    }
}
