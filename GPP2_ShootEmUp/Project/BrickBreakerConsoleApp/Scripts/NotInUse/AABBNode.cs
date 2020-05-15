using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class AABBNode
    {
        AABB aabb;

        const uint AABB_NULL_NODE = 0xffffffff;

        uint parentNodeIndex;
        uint leftNodeIndex;
        uint rightNodeIndex;

        uint nextNodeIndex;

        public bool IsLeaf() { return leftNodeIndex == AABB_NULL_NODE; }

        public AABBNode()
        {
            parentNodeIndex = AABB_NULL_NODE;
            leftNodeIndex = AABB_NULL_NODE;
            rightNodeIndex = AABB_NULL_NODE;
            nextNodeIndex = AABB_NULL_NODE;
        }
    }

    class AABBTree
    {
    }
}
