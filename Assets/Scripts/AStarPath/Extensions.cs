using System.Collections.Generic;

namespace AStarPath {
    public static class Extensions {
        private static readonly System.Random Rng = new();

        public static void Shuffle<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = Rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static bool IsWithinBounds(this MapLocation location, int width, int depth) {
            return location.X > 0 && location.X < width && location.Z > 0 && location.Z < depth;
        }
        
        public static bool IsWithinMaze(this MapLocation location, Maze maze) {
            return location.X > 0 && location.X < maze.width && location.Z > 0 && location.Z < maze.depth;
        }
    }
}