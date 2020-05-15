using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class Node : IEntityComponent
    {
        public bool IsWalkable { get; set; }
        public float G { get; set; }
        public float H { get; set; }
        public float F { get; set; }
        public NodeState State { get; set; }
        public Node ParentNode { get; set; }
        public ISystem System { get; set; }
        public GameObject GameObject { get; set; }

        public Node(ISystem system)
        {
            System = system;
        }

    }
}

public enum NodeState { Untested, Open, Closed }
