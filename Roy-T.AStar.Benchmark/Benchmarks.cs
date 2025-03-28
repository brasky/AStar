using BenchmarkDotNet.Attributes;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Benchmark
{
    /// <summary>
    /// For more thorough explanation, and benchmark history, see BenchmarkHistory.md
    /// </summary>
    
    [MemoryDiagnoser]
    public class AStarBenchmark
    {
        private static readonly Velocity MaxSpeed = Velocity.FromMetersPerSecond(1);

        private PathFinder PathFinder;

        private Grid Grid;
        private Grid GridWithGradient;
        private Grid GridWithHole;
        private Grid GridWithRandomLimits;
        private Grid GridWithRandomHoles;
        private Grid GridWithUnreachableTarget;

        public AStarBenchmark()
        {

        }

        [GlobalSetup]
        public void Setup()
        {
            this.PathFinder = new PathFinder();

            var gridSize = new GridSize(100, 100);
            var cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));

            this.Grid = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);

            this.GridWithGradient = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.SetGradientLimits(this.GridWithGradient);

            this.GridWithHole = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.DisconnectDiagonallyExceptForOneNode(this.GridWithHole);

            this.GridWithRandomLimits = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.SetRandomTraversalVelocities(this.GridWithRandomLimits);

            this.GridWithRandomHoles = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.DisconnectRandomNodes(this.GridWithRandomHoles);

            this.GridWithUnreachableTarget = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
            GridBuilder.DisconnectRightHalf(this.GridWithUnreachableTarget);
        }

        [Benchmark]
        public Grid CreatingGrid()
        {
            var gridSize = new GridSize(100, 100);
            var cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
            return Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, MaxSpeed);
        }

        [Benchmark]
        public Path GridBench()
        {
            return this.PathFinder.FindPath(
                this.Grid.GetNode(GridPosition.Zero),
                this.Grid.GetNode(new GridPosition(this.Grid.Columns - 1, this.Grid.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public Path GridWithHoleBench()
        {
            return this.PathFinder.FindPath(
                this.GridWithHole.GetNode(GridPosition.Zero),
                this.GridWithHole.GetNode(new GridPosition(this.GridWithHole.Columns - 1, this.GridWithHole.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public Path GridWithRandomHolesBench()
        {
            return this.PathFinder.FindPath(
                this.GridWithRandomHoles.GetNode(GridPosition.Zero),
                this.GridWithRandomHoles.GetNode(new GridPosition(this.GridWithRandomHoles.Columns - 1, this.GridWithRandomHoles.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public Path GridWithRandomLimitsBench()
        {
            return this.PathFinder.FindPath(
                this.GridWithRandomLimits.GetNode(GridPosition.Zero),
                this.GridWithRandomLimits.GetNode(new GridPosition(this.GridWithRandomLimits.Columns - 1, this.GridWithRandomLimits.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public Path GridWithUnreachableTargetBench()
        {
            return this.PathFinder.FindPath(
                this.GridWithUnreachableTarget.GetNode(GridPosition.Zero),
                this.GridWithUnreachableTarget.GetNode(new GridPosition(this.GridWithUnreachableTarget.Columns - 1, this.GridWithUnreachableTarget.Rows - 1)),
                MaxSpeed);
        }

        [Benchmark]
        public Path GridWithGradientBench()
        {
            var maxSpeed = Velocity.FromKilometersPerHour((this.GridWithGradient.Rows * this.GridWithGradient.Columns) + 1);
            return this.PathFinder.FindPath(
                this.GridWithGradient.GetNode(GridPosition.Zero),
                this.GridWithGradient.GetNode(new GridPosition(this.GridWithGradient.Columns - 1, this.GridWithGradient.Rows - 1)),
                maxSpeed);
        }
    }
}
