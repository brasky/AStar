using System.Collections.Generic;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Graphs
{
    public class Node 
    {
        public Node(Position position)
        {
            this.Incoming = new List<Edge>(0);
            this.Outgoing = new List<Edge>(0);

            this.Position = position;
        }

        public IList<Edge> Incoming { get; }
        public IList<Edge> Outgoing { get; }

        public Position Position { get; }

        public float CostSoFar { get; set; } = 0f;
        public bool Visited { get; set; } = false;

        public void Connect(Node node, Velocity traversalVelocity)
        {
            var edge = new Edge(this, node, traversalVelocity);
            this.Outgoing.Add(edge);
            node.Incoming.Add(edge);
        }

        public void Disconnect(Node node)
        {
            for (var i = this.Outgoing.Count - 1; i >= 0; i--)
            {
                var edge = this.Outgoing[i];
                if (edge.End == node)
                {
                    this.Outgoing.Remove(edge);
                    node.Incoming.Remove(edge);
                }
            }
        }

        public override string ToString() => this.Position.ToString();
    }
}
