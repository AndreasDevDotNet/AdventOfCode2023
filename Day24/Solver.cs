using AoCToolbox;
using Microsoft.Z3;

namespace Day24
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 24th (God Jul) *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Never Tell Me The Odds";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
            var hailStones = parseHailStones(GetInputLines());

            var value = part switch
            {
                1 => RunPart1(hailStones),
                2 => RunPart2(hailStones)
            };

            return part switch
            {
                1 => $" Part #1\n  Number of intersecting hailstones within the test area: {value}",
                2 => $" Part #2\n  Sum of the coordinates the stone needs to have to hit every hailstone (using Z3 lib): {value}",
            };
        }

        private List<HailStone> parseHailStones(string[] input)
        {
            var hailStones = new List<HailStone>();

            foreach (var line in input)
            {
                var split = line.Split('@', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var position = split[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(double.Parse).ToArray();
                var velocity = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(double.Parse).ToArray();

                var hail = new HailStone(
                    new HailStonePosition(position[0], position[1], position[2]),
                    new HailStonePosition(velocity[0], velocity[1], velocity[2]));

                var next = hail.Move();

                // We calculate the line equation for the hail's path and store it in the hail object.
                var slope = (next.Position.Y - hail.Position.Y) / (next.Position.X - hail.Position.X);
                var intercept = next.Position.Y - slope * next.Position.X;

                hailStones.Add(hail with { Slope = slope, Intersect = intercept });
            }

            return hailStones;
        }

        private object RunPart1(List<HailStone> hailStones)
        {
            var min = 200000000000000.0;
            var max = 400000000000000.0;

            return findNumberOfIntersections(hailStones, min, max);
        }

        private object RunPart2(List<HailStone> hailStones)
        {
            return solveStoneCoordinates(hailStones);
        }
        private long findNumberOfIntersections(List<HailStone> hailStones, double min, double max)
        {
            var numberOfIntersections = 0L;
            var visited = new HashSet<HailStone>();
            const int b = -1;
            foreach (var first in hailStones)
            {
                visited.Add(first);
                var a1 = first.Slope;
                var c1 = first.Intersect;
                var x1 = first.Position.X;
                var y1 = first.Position.Y;
                var vx1 = first.Velocity.X;
                var vy1 = first.Velocity.Y;

                foreach (var second in hailStones.Where(h => !visited.Contains(h)))
                {
                    var a2 = second.Slope;
                    var c2 = second.Intersect;
                    var x2 = second.Position.X;
                    var y2 = second.Position.Y;
                    var vx2 = second.Velocity.X;
                    var vy2 = second.Velocity.Y;

                    if (a1 == a2)
                    {
                        continue; // Parallel
                    }

                    var x = (b * c2 - b * c1) / (a1 * b - a2 * b);
                    var y = (a2 * c1 - a1 * c2) / (a1 * b - a2 * b);

                    if (x - x1 < 0 != vx1 < 0 ||
                        y - y1 < 0 != vy1 < 0)
                    {
                        continue; // Opposite to first's direction
                    }

                    if (x - x2 < 0 != vx2 < 0 ||
                        y - y2 < 0 != vy2 < 0)
                    {
                        continue; // Opposite to second's direction
                    }

                    if (new HailStonePosition(x, y).InBounds(min, max))
                    {
                        numberOfIntersections++;
                    }
                }
            }

            return numberOfIntersections;
        }
        private long solveStoneCoordinates(List<HailStone> hailStones)
        {
            var ctx = new Context();
            var solver = ctx.MkSolver();

            // Coordinates of the stone
            var x = ctx.MkIntConst("x");
            var y = ctx.MkIntConst("y");
            var z = ctx.MkIntConst("z");

            // Velocity of the stone
            var vx = ctx.MkIntConst("vx");
            var vy = ctx.MkIntConst("vy");
            var vz = ctx.MkIntConst("vz");

            // For each iteration, we will add 3 new equations and one new condition to the solver.
            // We want to find 9 variables (x, y, z, vx, vy, vz, t0, t1, t2) that satisfy all the equations, so a system of 9 equations is enough.
            for (var i = 0; i < 3; i++)
            {
                var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
                var hail = hailStones[i];

                var px = ctx.MkInt(Convert.ToInt64(hail.Position.X));
                var py = ctx.MkInt(Convert.ToInt64(hail.Position.Y));
                var pz = ctx.MkInt(Convert.ToInt64(hail.Position.Z));

                var pvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
                var pvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
                var pvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

                var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
                var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
                var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz

                var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
                var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
                var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz

                solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
                solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
                solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
                solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz
            }

            solver.Check();
            var model = solver.Model;

            var rx = model.Eval(x);
            var ry = model.Eval(y);
            var rz = model.Eval(z);

            return Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());
        }

        record HailStonePosition(double X, double Y, double Z = 0)
        {
            public HailStonePosition Move(double x, double y, double z) => new(X + x, Y + y, Z + z);
            public bool InBounds(double min, double max) => X >= min && X <= max && Y >= min && Y <= max;
        }

        record HailStone(HailStonePosition Position, HailStonePosition Velocity, double Slope = 0, double Intersect = 0)
        {
            public HailStone Move() => this with { Position = Position.Move(Velocity.X, Velocity.Y, Velocity.Z) };
        }
    }
}
