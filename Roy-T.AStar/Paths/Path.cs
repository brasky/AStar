using System.Collections.Generic;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Paths
{
    public struct Path
    {
        public Path(PathType type, IReadOnlyList<Edge> edges)
        {
            this.Type = type;
            this.Edges = edges;

            for (var i = 0; i < this.Edges.Count; i++)
            {
                this.Duration += this.Edges[i].TraversalDuration;
                this.Distance += this.Edges[i].Distance;
            }
        }

        public PathType Type { get; }

        public Duration Duration { get; }

        public IReadOnlyList<Edge> Edges { get; }
        public Distance Distance { get; }
    }
}
