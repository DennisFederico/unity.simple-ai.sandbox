namespace AStarPath {
    public class Recursive : Maze {
        public override void Generate() {
            Generate(5, 5);
        }

        private void Generate(int x, int z) {
            if (CountSquareNeighbours(x, z) >= 2) return;
            Map[x, z] = 0;

            Directions.Shuffle();

            Generate(x + Directions[0].X, z + Directions[0].Z);
            Generate(x + Directions[1].X, z + Directions[1].Z);
            Generate(x + Directions[2].X, z + Directions[2].Z);
            Generate(x + Directions[3].X, z + Directions[3].Z);
        }
    }
}