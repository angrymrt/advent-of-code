using System;
using System.Linq;
using System.Text;

namespace day17
{
    class Grid {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
        public Cell[,,] Cells { get; }

        public Grid(int width, int height, int depth){
            Width = width;
            Height = height;
            Depth = depth;
            Cells = new Cell[depth,height,width];
            forEachCell((x,y,z) => {
                Cells[x,y,z] = new Cell(x,y,z);
                return 1;
            });
        }
        public static Grid InitializeFromInput(string input) {
            var lines = input.Split(Environment.NewLine);
            var gridSizeX = lines.First().Length * 10;
            var gridSizeY = lines.Count() * 10;
            var gridSizeZ = gridSizeX > gridSizeY ? gridSizeX : gridSizeY;
            var result = new Grid(gridSizeX, gridSizeY, gridSizeZ);
            var startZ = gridSizeZ / 2;
            var startX = (gridSizeX / 2) - (lines.First().Length / 2);
            var startY = (gridSizeY / 2) - (lines.Count() / 2);
            var endY = startY + lines.Count();
            var endX = startX + lines.First().Length;
            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    result.Cells[x,y,startZ].Alive = lines[y-startY][x-startX] == '#';
                }
            }
            return result;
        }
        public int CountAlive(){
            var result = 0;
            forEachCell((x,y,z) => {
                result += Cells[x,y,z].Alive ? 1 : 0;
                return 0;
            });
            return result;
        }
        public void Run(int cycles = 1){
            for(var i = 0; i < cycles; i++) {
                forEachCell((x,y,z) => {
                    Cells[x,y,z].Neighbours = countNeighbours(x,y,z);
                    return 0;
                });
                forEachCell((x,y,z) => {
                    if(Cells[x,y,z].Alive) {
                        Cells[x,y,z].Alive = Cells[x,y,z].Neighbours == 2 || Cells[x,y,z].Neighbours == 3;
                    } else {
                        Cells[x,y,z].Alive = Cells[x,y,z].Neighbours == 3;
                    }
                    return 0;
                });
            }
        }
        public string Print(int z) {
            var result = new StringBuilder();
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if(Cells[x,y,z].Alive) {
                        result.Append('#');
                    }else{
                        result.Append('.');
                    }
                }
                result.AppendLine();
            }
            return result.ToString();
        }
        private int countNeighbours(int x, int y, int z) {
        int neighbours = 0;
        for (int zi = z - 1; zi <= z + 1; zi++) {
            if (zi < 0 || zi >= Depth) continue;
            for (int yi = y - 1; yi <= y + 1; yi++) {
                if (yi < 0 || yi >= Height) continue;
                for (int xi = x - 1; xi <= x + 1; xi++) {
                    if (xi < 0 || xi >= Width) continue;
                    else if (xi == x && yi == y && zi == z) continue;
                    else if (Cells[xi,yi,zi].Alive) neighbours++;
                }
            }
        }
        return neighbours;
    }
        private int forEachCell(Func<int, int, int, int> runnable) {
            var sum = 0;
            for (int z = 0; z < Depth; z++) {
                for (int y = 0; y < Height; y++) {
                    for (int x = 0; x < Width; x++) {
                        sum += runnable(x, y, z);
                    }
                }
            }
            return sum;
        }
    }

    class Cell
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int Neighbours { get; set; }
        public bool Alive { get; set; }
        public Cell(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            var grid = Grid.InitializeFromInput(@"...#...#
#######.
....###.
.#..#...
#.#.....
.##.....
#.####..
#....##.");
            Console.WriteLine(grid.Print(15));
            grid.Run(6);
            // Console.WriteLine(grid.Print(13));
            // Console.WriteLine(grid.Print(14));
            // Console.WriteLine(grid.Print(15));
            // Console.WriteLine(grid.Print(16));
            // Console.WriteLine(grid.Print(17));
            Console.WriteLine($"Answer day 17, part 1: {grid.CountAlive()}");
        }
    }
}
