using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Graphs
{
    public sealed class Edge
    {
        private Velocity traversalVelocity;

        public Edge(Node start, Node end, Velocity traversalVelocity)
        {
            this.Start = start;
            this.End = end;

            this.Distance = Distance.BeweenPositions(start.Position, end.Position);
            this.TraversalVelocity = traversalVelocity;
        }

        public Velocity TraversalVelocity
        {
            get => this.traversalVelocity;
            set
            {
                this.traversalVelocity = value;
                this.TraversalDuration = this.Distance / value;
            }
        }

        public Duration TraversalDuration { get; private set; }

        public Distance Distance { get; }

        public Node Start { get; }
        public Node End { get; }

        public override string ToString() => $"{this.Start} -> {this.End} @ {this.TraversalVelocity}";
    }
}
