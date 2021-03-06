﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day11
{
    class Layout{
        public Layout(string raw) {
            Raw = raw;
            Rows = new LinkedList<string>(raw.Split(Environment.NewLine));
        }

        public Layout ApplyRules1() {
            var newLayout = new StringBuilder();
            var currentRow = Rows.First;
            while(currentRow != null) {
                if(currentRow.Previous != null) {
                    newLayout.AppendLine();
                }
                for(int i = 0; i < currentRow.Value.Length; i++){
                    var currentPos = currentRow.Value[i];
                    switch (currentPos){
                        case '.':
                            newLayout.Append('.');
                            break;
                        case 'L':
                            newLayout.Append(CountOccupiedAdjacentSeats1(currentRow, i) == 0 ? '#' : 'L');
                            break;
                        case '#':
                            newLayout.Append(CountOccupiedAdjacentSeats1(currentRow, i) > 3 ? 'L' : '#');
                            break;
                    }
                }
                currentRow = currentRow.Next;
            }
            return new Layout(newLayout.ToString());
        }
        public Layout ApplyRules2() {
            var newLayout = new StringBuilder();
            var currentRow = Rows.First;
            while(currentRow != null) {
                if(currentRow.Previous != null) {
                    newLayout.AppendLine();
                }
                for(int i = 0; i < currentRow.Value.Length; i++){
                    var currentPos = currentRow.Value[i];
                    switch (currentPos){
                        case '.':
                            newLayout.Append('.');
                            break;
                        case 'L':
                            newLayout.Append(CountOccupiedAdjacentSeats2(currentRow, i) == 0 ? '#' : 'L');
                            break;
                        case '#':
                            newLayout.Append(CountOccupiedAdjacentSeats2(currentRow, i) > 4 ? 'L' : '#');
                            break;
                    }
                }
                currentRow = currentRow.Next;
            }
            return new Layout(newLayout.ToString());
        }
        
        public int CountOccupiedAdjacentSeats1(LinkedListNode<string> currentRow, int currentIndex) {
            int result = 0;
            string previousRow = currentRow.Previous?.Value;
            string nextRow = currentRow.Next?.Value;
            int rowLength = currentRow.Value.Length;
            if(previousRow != null) {
                var skip = currentIndex == 0 ? 0 : currentIndex - 1;
                var take = currentIndex == 0 || currentIndex == rowLength - 1 ? 2 : 3;
                result += previousRow.Skip(skip).Take(take).Count(x => x == '#');
            }
            if(currentIndex > 0 && currentRow.Value[currentIndex - 1] == '#') {
                result++;
            }
            if(currentIndex < rowLength - 1 && currentRow.Value[currentIndex + 1] == '#') {
                result++;
            }
            if(nextRow != null) {
                var skip = currentIndex == 0 ? 0 : currentIndex - 1;
                var take = currentIndex == 0 || currentIndex == rowLength - 1 ? 2 : 3;
                result += nextRow.Skip(skip).Take(take).Count(x => x == '#');
            }
            return result;
        }
        public int CountOccupiedAdjacentSeats2(LinkedListNode<string> currentRow, int currentIndex) {
            int result = 0;
            int rowLength = currentRow.Value.Length;

            // Get north seat..
            var row = currentRow.Previous;
            while(row != null) {
                if(row.Value[currentIndex] == '.'){
                    row = row.Previous;
                }else{
                    result += row.Value[currentIndex] == '#' ? 1 : 0;
                    break;
                }
            }
            // Get north west seat..
            row = currentRow.Previous;
            var col = currentIndex - 1;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    row = row.Previous;
                    col--;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }
            // Get north east seat..
            row = currentRow.Previous;
            col = currentIndex + 1;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    row = row.Previous;
                    col++;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }

            // Get west seat..
            row = currentRow;
            col = currentIndex - 1;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    col--;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }
            // Get east seat..
            row = currentRow;
            col = currentIndex + 1;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    col++;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }

            // Get south seat..
            row = currentRow.Next;
            col = currentIndex;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    row = row.Next;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }
            // Get south west seat..
            row = currentRow.Next;
            col = currentIndex - 1;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    row = row.Next;
                    col--;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }
            // Get south east seat..
            row = currentRow.Next;
            col = currentIndex + 1;
            while(row != null && col >= 0 && col < rowLength) {
                if(row.Value[col] == '.'){
                    row = row.Next;
                    col++;
                }else{
                    result += row.Value[col] == '#' ? 1 : 0;
                    break;
                }
            }

            return result;
        }
        public int CountOccupied(){
            return Raw.Count(x => x == '#');
        }
        public string Raw { get; }
        public LinkedList<string> Rows { get; }
    }
    class Program
    {
        public static Layout Solve(string input) {
            var layout = new Layout(input);
            var newLayout = (Layout)null;
            while(true) {
                //Console.WriteLine();
                //Console.WriteLine(layout.Raw);
                newLayout = layout.ApplyRules1();
                if(newLayout.Raw == layout.Raw) {
                    break;
                }
                layout = newLayout;
            }
            return newLayout;
        }
        public static Layout Solve2(string input) {
            var layout = new Layout(input);
            var newLayout = (Layout)null;
            while(true) {
                //Console.WriteLine();
                //Console.WriteLine(layout.Raw);
                newLayout = layout.ApplyRules2();
                if(newLayout.Raw == layout.Raw) {
                    break;
                }
                layout = newLayout;
            }
            return newLayout;
        }
        static void Main(string[] args)
        {
            var answer1 = -1;

            var layout = Solve(testInput);
            answer1 = layout.CountOccupied();
            Console.WriteLine($"Answer for day 11, test input, part 1: {answer1}.");

            layout = Solve2(testInput);
            var answer2 = layout.CountOccupied();
            Console.WriteLine($"Answer for day 11, test input, part 2: {answer2}.");
            
            layout = Solve(input);
            answer1 = layout.CountOccupied();
            Console.WriteLine($"Answer for day 11, part 1: {answer1}.");
            
            layout = Solve2(input);
            answer2 = layout.CountOccupied();
            Console.WriteLine($"Answer for day 11, part 2: {answer2}.");
        }

        private static string testInput = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
        private static string input = @"LLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLLLLL.LLLL..LLLLLLL..LLLLLL.LLLLLLLL.L.LLLLLLLLLLL
LLLLL.LL.LLLLLLL.LLLLL.LLLLL.LLLLLLL..LLLLLLLLLLLLLLLL.LLLLLLLLLL.LL.L.LLLLL.LLL.LLLL..LLLLLLLLLLL.
LLLLLLLL.LLLLLLLLL.LLL.LLLL..LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLL.LL.LLLL.LLLLL.L.LLL.LLLLLLLLLLL.LLLL.L
L.LLLLL..LLLLLLL..LLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLLLLLLL
LLLLLLLLLLLLLLL...LLLLLLLLL..L.LLLLLLLLLLLLLLLL.LLL.LL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLL.LLLLLL.LLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLL.LLLLLLLL..LLLLL
LL.LLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLLLL.LLLL.LL.LLLLLL.LLL.LLL.LLL.LLLLL.LLL.LLLLLLLL..L.LLLLLLLL.L
LLLLLLLL.LLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLL..LLL.LL.LLLLLLL.LLLLLLLL..LLLLLLLLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLL.LL.LLLLL.LLLLLLLLL.LLLLLL.LLL.L.LLLLLLLLLLLLLLL.LLLLLLLL.LLLLLL.LLLLL.
.L.LL..L.L...L.LL....L...LL......LL..L.L...L.L.LLLLLL..L...LLL..LL..L..............L.L..L.......LL.
LLLLLLLL..LLLLLL.LLL.L.L.LLL.LLL.L.LLLLL.LLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLLLL
LLLLLLLL.LLL.LLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLL..LLLLL.LLLLL.L.LLLLLL.LLLLLLLLL.LLLLLL..LLLLL
LLLLLLLL.LLL.LLL.L.LLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLL
.LLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLLLLL.L.LLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLL
LLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLL.LLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLLL.LLLLLL
LLL.LLLL.LLLLLLL.LLLLLLLLLLL.LLLLLL.L.LLLLLLLLL..LLLLL.LLLLL.LLLL.LL..LL.LLL.LLLLLLLLLLLLLLLLLL.LL.
........LL...L..L....L.LLLLL..L..LL........L.L.L.LL.L.LL..L.LLL.L....LL........L..LLLL.LLL....L....
LLLLLLLL.LLLLLLL.LLLLLLLLLLL.LL.LLLLL.LLLLLLLLL.LLLLLLLLLLLL.LLLLL.L.LLLLLLL.LLLLLLLLLLLLLLL.LLLLLL
LLLLLLLL.LL.LLLLLLLLLLLLLLLL.LLLLLLLL.L.LLL.LLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.L.LLLLLLLL.LLLLLL.LLLLLL
L.LLLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLLL..LLLLLL.L.LLL.LL.LLLLL.LLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLL
LLLLLLLLL..L.LLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL..LLLLLLLL.LLLLLL.LLLLL.
LLLLLLLLLLLLLL.L.LLLLLLLLLLL.LLLLLLL..LLLLLLL.L.LLLLLLLLLLLL.LLLLLL..LLLLLLLL.LLLLLLL.LLLLLL.LLLLLL
LLLLLLLL.L.L.LLL.LLLLL.LLLLL.LLLLLLLL.LLLLLL.LL.LLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL
.........L.LL...LLL.....LL..LL.....L.L.L.......L..L......LLLL.L...L.L...L.....LL.........L.LL.L..LL
L.LLLLLL.LLLLLLL..LLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLL.LLLLLL..LLLLLL.LLLLLLLL.LLLL.LLLLLLLL
LLLLLLLLLLLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLLL.LLL.LLLL.L.LLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLL.LLLLLLLLL
LLLLLLLL.LLLLLLL..LLLL.LLLLLLLLLLL.LL.LLLLLLLLL.LLLLLL.LLLLL.LLLLLLLLLLLLLL..LLLLLLLL.LLLLLL.LLLLLL
LLLLLLLL.LLLLLL..LLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.L.LLLLL.LLLL.LLLLLLL.L.LLLLL..LLLLLLL.LLLL.L...LLLL
L.LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLL.LLL.LLLLLLL.L.LLLLLLLLLL.LLLLLLL..L.LLLL.LLLLLLL.LLLLLLL.LLLLLL
LLLLL.LL.LLLLLLL..L.LL.LLLLL.LL.LLLLL.LLL.LLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLL.LLLLLL.LLLLLL
LL.LLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLL.LLLL..LLLLLLL.LLLLLLLLLLLLL.LL.LLLLLLLLLLLLL
..L.......L...........LL...L.L....L.........LL.L.LLL.L.........L..LL....L.L....LL.L......LL....L.L.
LLLLLLLLLLLLLLLL.LLLLL.LLLLL.LLLLLLLL.LL.LLLL.L.LL..LLLLLL.LL.LLLLLL.LLLLLL..LLLLLL.L.LLLLLL.LLLLLL
LL.LL.LL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLL.L.LLL.LLLLLLL.LLLLLLL.LLLLLLLL.LLLLLL.LLLLLL
LLLLLLLL.L.LLLLL.LLLLL.LLLLLLLLLL.LLL..LLLL.LLL.LLLLLL.LLLLL.LLLLLLL.L.LLLLL.LLLLLLLL.LLLLLLLLLLLLL
LLLLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLL.LLLL.LLLLLL
LL.LLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLL.LLLLLL.LLLLLL..LLLLLLL.LLLLLLLL.LLLLLLLLLL.LL
L........L.L.L...LL.LLL...L........LL..L...L..LL..LL.L.L....L..L..L.L....L.L.....LLL.L.LL...L...L.L
LLLLLLLL.L.LLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLL..LL.LLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLL.LLLL.L.LLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLL.LLLLLL.LLLLLLLL.LL.LLLLLLLLLLLLLLLLLLLL.LLL.LLL.LLLLL..LLLLLLLLLLLLL.L
LLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLL.L.LLLLL.LLLLLLLLLLLLLL.LLLL.LL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLL
.LLLLLLL.LLLLLLLLLLLLLLLLLLL.LLL.LLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLLLL..LLLLLLL.LLLLLLLL.LLLL
LLLLLLLLLLLL..LL.LLLLLLLL.LL.LLLLLLLL..LLLLLLLL.LLLLLL.LLLLLLLLL.LLL.LLLLLLL.LLLLLLLL.LLLLLL.LLLLLL
.L..L......L.L.LL..L...LL.L...L....L.L.....LLLL...L.L...LL.....L......L..L.L.L.....L.L......L....LL
LLLLLLLLLLLLLLLLL.LLLL.LLL.L.LLLLLLLL..LL.LLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLL.LLLLLL.LLLLLL
LLLLLLLLLLLLLLLLLLLLLLLLL.LL.LLLLLLLLLLLLL.LLLL.LLLLLL.LLLL..LLLLLLLLLLLLLLL.LLLLLLLLLLLLLLL.LLLLLL
.LLLLLLL.L.LLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLL.L.LLLLLLLLLLLLL
LLLLLLLLLLL.LLLLLLLLLL.LLLLL.LLLLLLLL..LLLLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLLLLL.LLLLLL
LLLLLLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLL.LL.LLLLLL.LLLLLL.LLLLL.LLLLLLL.L.LLLLLLLLLLLLLL.LLLLLL.LLLLLL
LLLLLLLL.LLLLLLLLLLLLL..LLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLL
LLLLLLLLLLLLL.LL.LLLLLLLLLLL.LLLLL.LL.LLLLL.LLL.LLLLLL.LLLLLLLLLLL.LLLLLLLLLLLL.LLLLL.LLLLLLLLLLLL.
LLLL.LLL.LLLLLLL.LLLLL.LLL.LLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLL..LLLLLLLL..LLLLLLLLLLLL
LLLLLLLL.LLLL.LL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLL..LLLLL.L.LLL.LLLLLLL.LLLLLL.LLLLLLLLLL.LLLLL.LLLLLL
.....LL.L..L..L..LL.............LL...L..L.LLLLL.L.L...L.....L...L.....L....LL.L..L.....LLL.........
LLLLLLLLLLLL.LLL.LLLLLLL.LLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLL.LL.LL.LL.LLLL.L.LLLLLL
LLLLLLLLLL.LLL.L.LLLLLLLLLLL.LLLLLLLL.LLL..LLLL.LLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLL.LLL.LLLLLLLL.LLLL
LLL.LLLL.LLL.LLL.LLLLL.LLLL..LLLLL.LL.LL.LLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLL
LLLLLLLL.LLLLLL..L.LLLLLLL.L.LLLLLL.L.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL.LLLLLL
LLLL.LLLLLLLLLLL.LLLLL.LLLLL.LLLLLLLL.LLLLLL.LL.LLLLLL.LLLLL.L.LLLLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL
LLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLL.L.LLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLL.L.LLLLLL.LLLLLL
LLLLLLLL.LLLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLL.L.LL.LLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLL.LL
LLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLL.LL.LLLLL.LLLLLLLLLLLLL
LLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLL.LLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLL.LLL.LLLLLLLLLLL.LLLLLL
..LL..LL..LL......L....L....L....L....L..L...........L.L...L.L.L...LL.L..L..........LLLL.L..L.L....
LLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLL.L..LLL.LLL.LLLLLLLL..LLLLL.LLLLLL
LLLLLLLLLLLLLLLL.LLLLL.LLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLL..LLLLLL..LLLLL
LLLLLLLL.LLLLLLLLLLLLLLLLLLL.LLL.LLLL.LL.LLLLLL.LLL.LL.LLLLLL.LLLLLL.LLLLLLL.LLLLLLLL.LLLLLLLLLLL.L
LLLL.LLL.LLLLLLL.LLLLLL.LLL..L.LLLLLL.LL.LLLLLLLLL.LLLLLLLLL.L.LLLLLLLL.LLLL.LL.L.LLL.LLLLLL.LLLLLL
LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLL.LLL.LLLL.L.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLL
L..LLL.L....L...LL..LLLLL..L...LL.L..L.L...............L.LL.L.....LLL.LL...L..L.LL..L..L...L.......
LLLLLLLL.LL.LLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLL.LLLLLLLLL.LLL
LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLL..LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL.L.L.LLLLLLL.LLLL....LLLLL
LLLLLLLLLLLLLLLL.LLLLL.L.LLLLL.LLLLL..LLLLLLLLL..LL.LL.LLLLL.LLLLL.LL.LLLLLLLLLLLLLLLLLLLLL..LLLL.L
LLLLLLLL.LLLLLLL.LLLL..LLLLL.LLLLLLLLLLLLLLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLL.LLLLL
L.......L....L....L.LL..L.LL...L.L.LL........L......L.......LL.L.L...L...L...LL.L.L.L..............
LLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLL..LLLL.LL.LLLLLLL.L.LLLL.L.L.LLLL.LLLLLL
LLLLLLLLLL.LLLLL.LLLLL.L.LLL..LLLL.LLLL.LLLLL.LLLLLLLL.L.LLL.LLLLLLL.LLLLLLLLLLLL.LLL.LLLLLLLLLLLLL
LLLLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLL.L.LLLLLLLLL.LLLLLL.L.LLLLLLLLLLL.LLLLLLL..LLLLLLL.LLLLLLLLLLLLL
LLLLLLLL.L.LLLL..LLLLL.LLLLL.LLLLLLLLLLLLLLLLL..LLLLLL.LLLLL.LL.LLLL.LLLLLLL.LLLLLLLLLLLLLLL.LLLLLL
LLLLLLLL..LLLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLL.LLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLL.LLLL.L..LLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLL.LLLLLLL.L.LLLLLLLLL.LLLL.LLLLLL.LLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLL.L.LLLLLLLLLL.L.L..LLLL.LLLLLLL.LLLLLLL.LLLLL.LLLLLLLLL.LLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLL..LLLLLLLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLL.LLLLLLLLLLLLLLL.LLLLLL
..LL.....L....L...L.LL........LL.L........L....L....L.L....LLL.L....LLLLLL.L.L.L..LLL.L.L.L..LL..LL
LLLL.LLL.LLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLL..LLLLLLLL.LLLLLL.LL.L.LLLL.LLLLL.LLL.LLL.LLLLLLL.LLLLL.LLLLLLLLL.LLLLL.
LLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLLLLLLL.LLLL.LLLL.LLLLLL.LLLLL..LLLLLL.LLLLLLL.LLLLL.LL.LLLLLL.LLLLLL
LLLLLLLL.LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLL.LLL.LL.L.LLLLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLL
LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLL.LLLL.LLLLLL.LLLLL.LLLLLLL.LLLLLLLLLL.LLLLL.LLLLLL.LLLLLL
LLL.LLLLLLLLLLLL.LLLLL.LLL.LLLLLLLLLLLLLLLLLLLLLLL.LLL.LLLLL.LLLLLLL.LLLLLLL.LLLL.LLL.LLLLLL.LLLLLL";
    }
}
