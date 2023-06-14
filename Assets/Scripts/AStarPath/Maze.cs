using System.Collections.Generic;
using UnityEngine;

namespace AStarPath {
    public class MapLocation {
        public int X { get; }
        public int Z { get; }

        public MapLocation(int x, int z) {
            this.X = x;
            this.Z = z;
        }
        
        public Vector2 ToVector() {
            return new Vector2(X, Z);
        }

        public static MapLocation operator +(MapLocation a, MapLocation b)
            => new(a.X + b.X, a.Z + b.Z);

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) return false;
            return X == ((MapLocation)obj).X && Z == ((MapLocation)obj).Z;
        }

        public override int GetHashCode() {
            return X.GetHashCode() ^ Z.GetHashCode();
        }
    }

    public class Maze : MonoBehaviour {
        public readonly List<MapLocation> Directions = new List<MapLocation>() {
            new MapLocation(1, 0),
            new MapLocation(0, 1),
            new MapLocation(-1, 0),
            new MapLocation(0, -1)
        };

        public int width = 30; //x length
        public int depth = 30; //z length
        public byte[,] Map;
        public int scale = 6;

        // Start is called before the first frame update
        void Start() {
            InitialiseMap();
            Generate();
            DrawMap();
        }

        void InitialiseMap() {
            Map = new byte[width, depth];
            for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++) {
                Map[x, z] = 1; //1 = wall  0 = corridor
            }
        }

        public virtual void Generate() {
            for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++) {
                if (Random.Range(0, 100) < 50)
                    Map[x, z] = 0; //1 = wall  0 = corridor
            }
        }

        void DrawMap() {
            for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++) {
                if (Map[x, z] == 1) {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
        }

        public int CountSquareNeighbours(int x, int z) {
            int count = 0;
            if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
            if (Map[x - 1, z] == 0) count++;
            if (Map[x + 1, z] == 0) count++;
            if (Map[x, z + 1] == 0) count++;
            if (Map[x, z - 1] == 0) count++;
            return count;
        }

        public int CountDiagonalNeighbours(int x, int z) {
            int count = 0;
            if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
            if (Map[x - 1, z - 1] == 0) count++;
            if (Map[x + 1, z + 1] == 0) count++;
            if (Map[x - 1, z + 1] == 0) count++;
            if (Map[x + 1, z - 1] == 0) count++;
            return count;
        }

        public int CountAllNeighbours(int x, int z) {
            return CountSquareNeighbours(x, z) + CountDiagonalNeighbours(x, z);
        }
    }
}