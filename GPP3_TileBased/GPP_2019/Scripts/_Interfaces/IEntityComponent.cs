using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    public interface IEntityComponent
    {
        ISystem System { get; }
        GameObject GameObject { get; set; }

    }
}
