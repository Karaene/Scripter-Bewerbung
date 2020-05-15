using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class AStarPathfinding : ISystem
    {
        private AStarPathfinding() { }
        private static AStarPathfinding instance = null;
        public static AStarPathfinding Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AStarPathfinding();
                }
                return instance;
            }
        }

        //List<Node> nodes = new List<Node>();
        Dictionary<Vector2D, Node> nodes = new Dictionary<Vector2D, Node>();
        

        public Node CreateNodeComponent(GameObject tile)
        {
            Node node = new Node(this);
            node.IsWalkable = tile.GetComponent<TileComponent>().walkable;
            nodes.Add(tile.Transform.Position, node);
            return node;
        }

        public void ShowAllNodes()
        {
            foreach (var node in nodes)
            {
                Console.WriteLine(node.Value.GameObject.Id + " (" + node.Key.X + " , " + node.Key.Y + ")");
            }

            Console.WriteLine("Adjacent Location Test:");
            List<Vector2D> locations = GetAdjacentLocations(new Vector2D(-170, -450));

            foreach (var location in locations)
            {
                Console.WriteLine("Node: " + location);
            }
        }


        private bool Search(Node currentNode)
        {
            /*
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.GameObject.Transform.Position == this.endNode.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                        return true;
                }
            }
            */
            return false;
        }
        /*
        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Vector2D> nextLocations = GetAdjacentLocations(fromNode.GameObject.Transform.Position);

            foreach (var location in nextLocations)
            {
                int x = (int)location.X;
                int y = (int)location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= LevelManager.LEVEL_WIDTH || y < 0 || y >= LevelManager.LEVEL_HEIGHT)
                    continue;

                Node node = nodes[new Vector2D(x,y)];
                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.GameObject.Transform.Position, node.ParentNode.GameObject.Transform.Position);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }
        */
        private List<Vector2D> GetAdjacentLocations(Vector2D location)
        {
            List<Vector2D> locations = new List<Vector2D>();

            Vector2D adjVector = location + new Vector2D(-60, -60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(0, -60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(0, -60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(+60, -60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(-60, 0);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(+60, 0);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(-60, +60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(0, +60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            adjVector = location + new Vector2D(+60, +60);
            if (nodes.ContainsKey(adjVector) && nodes[adjVector].IsWalkable)
                locations.Add(nodes[adjVector].GameObject.Transform.Position);

            return locations;
        }
        
    }
    
}
