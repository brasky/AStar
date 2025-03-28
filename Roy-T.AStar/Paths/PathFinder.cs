using System.Collections.Generic;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Paths
{
    public sealed class PathFinder
    {
        private readonly PriorityQueue<Node, float> InterestingNodes;
        private readonly Dictionary<Node, Edge> CameFrom;

        

        public PathFinder()
        {
            this.InterestingNodes = new PriorityQueue<Node, float>();
            this.CameFrom = new Dictionary<Node, Edge>();
        }

        public Path FindPath(GridPosition start, GridPosition end, Grid grid)
        {
            var startNode = grid.GetNode(start);
            var endNode = grid.GetNode(end);
            var maximumVelocity = grid.GetMaxTraversalVelocity();
            return this.FindPath(startNode, endNode, maximumVelocity);
        }

        public Path FindPath(GridPosition start, GridPosition end, Grid grid, Velocity maximumVelocity)
        {
            var startNode = grid.GetNode(start);
            var endNode = grid.GetNode(end);

            return this.FindPath(startNode, endNode, maximumVelocity);
        }

        public Path FindPath(Node start, Node goal, Velocity maximumVelocity)
        {
            InterestingNodes.Clear();
            this.CameFrom.Clear();
            Node NodeClosestToGoal = start;
            Duration closestNodeExpectedTime = ExpectedDuration(start, goal, maximumVelocity);

            InterestingNodes.Enqueue(start, ExpectedDuration(start, goal, maximumVelocity).Seconds);
            start.Visited = true;

            while (this.InterestingNodes.TryDequeue(out var current, out var totalCost))
            {
                if (current == goal)
                {
                    return ConstructPathTo(current, goal);
                }

                if (ExpectedDuration(current, goal, maximumVelocity) < closestNodeExpectedTime)
                {
                    NodeClosestToGoal = current;
                    closestNodeExpectedTime = ExpectedDuration(current, goal, maximumVelocity);
                }

                foreach (var edge in current.Outgoing)
                {
                    var nextNode = edge.End;
                    var costSoFar = current.CostSoFar + edge.TraversalDuration.Seconds;
                    if (nextNode.Visited)
                    {
                        if (nextNode.CostSoFar > costSoFar)
                        {
                            InterestingNodes.Remove(nextNode, out _, out _);
                            this.CameFrom[nextNode] = edge;
                            nextNode.CostSoFar = costSoFar;
                            InterestingNodes.Enqueue(nextNode, costSoFar + ExpectedDuration(nextNode, goal, maximumVelocity).Seconds);
                            nextNode.Visited = true;
                        }
                    }
                    else
                    {
                        this.CameFrom[nextNode] = edge;
                        nextNode.CostSoFar += costSoFar;
                        InterestingNodes.Enqueue(nextNode, costSoFar + ExpectedDuration(nextNode, goal, maximumVelocity).Seconds);
                        nextNode.Visited = true;
                    }
                }
            }

            return ConstructPathTo(NodeClosestToGoal, goal);
        }

        private Path ConstructPathTo(Node node, Node goal)
        {
            var current = node;
            var edges = new List<Edge>();

            while (this.CameFrom.TryGetValue(current, out var via))
            {
                edges.Add(via);
                current = via.Start;
            }

            edges.Reverse();

            var type = node == goal ? PathType.Complete : PathType.ClosestApproach;
            return new Path(type, edges);
        }

        public static Duration ExpectedDuration(Node a, Node b, Velocity maximumVelocity)
            => Distance.BeweenPositions(a.Position, b.Position) / maximumVelocity;
    }
}
