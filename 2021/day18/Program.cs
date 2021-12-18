using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day18
{
    public class SnailfishNmbr
    {
        public int Value { get; private set; }
        public SnailfishNmbr Left
        {
            get => _left;
            private set
            {
                _left = value;
                if (_left != null)
                {
                    _left.Parent = this;
                }
            }
        }
        private SnailfishNmbr _left;
        public SnailfishNmbr Right
        {
            get => _right;
            private set
            {
                _right = value;
                if (_right != null)
                {
                    _right.Parent = this;
                }
            }
        }
        private SnailfishNmbr _right;
        public bool IsLiteral => Left == null;
        public SnailfishNmbr Parent { get; private set; }
        public SnailfishNmbr Root => Parent == null ? this : Parent.Root;
        public IEnumerable<SnailfishNmbr> Parents
        {
            get
            {
                if (Parent != null)
                {
                    yield return Parent;
                    foreach (var nmbr in Parent.Parents)
                    {
                        yield return nmbr;
                    }
                }
            }
        }
        public int Depth => Parent == null ? 0 : Parent.Depth + 1;

        private SnailfishNmbr(SnailfishNmbr parent)
        {
            Parent = parent;
        }
        private SnailfishNmbr(SnailfishNmbr parent, int value) : this(parent)
        {
            Value = value;
        }
        private SnailfishNmbr(SnailfishNmbr parent, SnailfishNmbr left, SnailfishNmbr right) : this(parent)
        {
            Left = left;
            Right = right;
        }

        public static SnailfishNmbr Create(string input)
        {
            var stack = new Stack<SnailfishNmbr>();
            var result = new SnailfishNmbr(null);
            stack.Push(result);
            for (var i = 1; i < input.Length - 1; i++)
            {
                var c = input[i];
                var isRight = i > 0 && input[i - 1] == ',';
                var parent = stack.Peek();
                switch (c)
                {
                    case '[':
                        stack.Push(new SnailfishNmbr(parent));
                        if (isRight)
                        {
                            parent.Right = stack.Peek();
                        }
                        else
                        {
                            parent.Left = stack.Peek();
                        }
                        break;
                    case ']':
                        stack.Pop();
                        break;
                    case ',':
                        // do nothing..
                        break;
                    default: // number..
                        if (isRight)
                        {
                            parent.Right = new SnailfishNmbr(parent, int.Parse(c.ToString()));
                        }
                        else
                        {
                            parent.Left = new SnailfishNmbr(parent, int.Parse(c.ToString()));
                        }
                        break;
                }
            }
            return result;
        }

        public SnailfishNmbr Add(SnailfishNmbr nmbr)
        {
            var result = new SnailfishNmbr(null, this, nmbr);
            result.Reduce();
            return result;
        }

        private SnailfishNmbr Reduce()
        {
            var result = this;
            var nested4Deep = (SnailfishNmbr)null;
            do
            {
                nested4Deep = result.AllLtr().FirstOrDefault(x => !x.IsLiteral && x.Depth > 3);
                if (nested4Deep != null)
                {
                    nested4Deep.Explode();
                }
            } while (nested4Deep != null);

            var toSplit = result.AllLtr().FirstOrDefault(x => x.IsLiteral && x.Value > 9);
            if (toSplit != null)
            {
                toSplit.Split();
                result.Reduce();
            }
            return result;
        }

        private void Split()
        {
            Left = new SnailfishNmbr(this, Value / 2);
            Right = new SnailfishNmbr(this, Value / 2 + (Value % 2));
        }

        private void Explode()
        {
            var left = Left;
            Left = null;
            var firstLeftLiteral = GetFirstLeftLiteral();
            if (firstLeftLiteral != null)
            {
                firstLeftLiteral.Value += left.Value;
            }
            var right = Right;
            Right = null;
            var firstRightLiteral = GetFirstRightLiteral();
            if (firstRightLiteral != null)
            {
                firstRightLiteral.Value += right.Value;
            }
            Value = 0;
        }
        private SnailfishNmbr GetFirstLeftLiteral()
        {
            var current = this;
            while (current?.Parent != null)
            {
                if (current.Parent.Left != current)
                {
                    // get first literal..
                    return current.Parent.Left.AllRtl().First(x => x.IsLiteral);
                }
                current = current.Parent;
            }
            return null;
        }
        private SnailfishNmbr GetFirstRightLiteral()
        {
            var current = this;
            while (current?.Parent != null)
            {
                if (current.Parent.Right != current)
                {
                    // get first literal..
                    return current.Parent.Right.AllLtr().First(x => x.IsLiteral);
                }
                current = current.Parent;
            }
            return null;
        }
        public IEnumerable<SnailfishNmbr> AllLtr()
        {
            yield return this;
            if (Left != null)
            {
                foreach (var nmbr in Left.AllLtr())
                {
                    yield return nmbr;
                }
            }
            if (Right != null)
            {
                foreach (var nmbr in Right.AllLtr())
                {
                    yield return nmbr;
                }
            }
        }
        public IEnumerable<SnailfishNmbr> AllRtl()
        {
            yield return this;
            if (Right != null)
            {
                foreach (var nmbr in Right.AllRtl())
                {
                    yield return nmbr;
                }
            }
            if (Left != null)
            {
                foreach (var nmbr in Left.AllRtl())
                {
                    yield return nmbr;
                }
            }
        }
        public long GetMagnitude()
        {
            if (IsLiteral)
            {
                return Value;
            }
            var leftValue = Left.GetMagnitude() * 3;
            var rightValue = Right.GetMagnitude() * 2;
            return leftValue + rightValue;
        }
        public override string ToString()
        {
            if (IsLiteral)
            {
                return Value.ToString();
            }
            else
            {
                return $"[{Left},{Right}]";
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 18!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var lines = (useTestInput ? testInput : input)
                .Split(Environment.NewLine)
                .Select(x => SnailfishNmbr.Create(x))
                .ToArray();


            // Start part 1.
            sw.Start();
            var sum = lines[0];
            for (var i = 1; i < lines.Length; i++)
            {
                if (useTestInput)
                {
                    Console.WriteLine($"  {sum}");
                    Console.WriteLine($"+ {lines[i]}");
                }
                sum = sum.Add(lines[i]);
                if (useTestInput)
                {
                    Console.WriteLine($"= {sum}");
                    Console.WriteLine();
                }
            }
            answer = sum.GetMagnitude();
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            lines = (useTestInput ? testInput : input)
                .Split(Environment.NewLine)
                .Select(x => SnailfishNmbr.Create(x))
                .ToArray();
            
            var magnitudes = lines.SelectMany((x) => lines, (y, z) => SnailfishNmbr.Create(y.ToString()).Add(SnailfishNmbr.Create(z.ToString())).GetMagnitude());
            answer = magnitudes.Max();
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
        public static string input = @"[[3,[[6,3],[9,6]]],[6,[[0,9],[9,7]]]]
[[[3,9],[[0,8],[7,6]]],[[[7,9],1],[1,3]]]
[8,[[[9,6],[8,4]],4]]
[5,[[1,2],[3,7]]]
[[[[7,7],5],[[3,5],8]],4]
[[[5,[0,7]],3],[[5,[5,3]],[1,[9,4]]]]
[[[[3,5],[7,1]],6],[[[3,6],[5,6]],[[3,2],5]]]
[[[[2,0],[3,0]],[5,7]],[[4,4],[[9,9],[9,3]]]]
[[[[8,0],7],[[7,1],9]],[[3,[8,6]],8]]
[[6,[7,5]],[[6,8],9]]
[[[9,[1,8]],2],[[[4,0],[9,3]],1]]
[[7,[1,[3,8]]],[[4,7],[8,1]]]
[[[5,5],[[4,5],[2,9]]],[[[7,7],0],8]]
[[[[4,7],3],5],[[[4,3],[3,8]],[[6,5],5]]]
[[[[3,8],2],[1,7]],[[[3,1],4],9]]
[[[[2,1],4],[[9,5],[1,4]]],[[3,5],[[9,1],9]]]
[[[6,[1,8]],[0,0]],[9,[0,3]]]
[[[[2,2],[3,3]],[[4,8],4]],[[[6,8],4],5]]
[4,[[[7,8],[3,4]],[[3,2],9]]]
[[[9,0],3],[[[7,1],4],7]]
[[[1,4],8],[[7,5],[[8,0],[0,7]]]]
[9,[[4,6],[[2,9],1]]]
[[[[1,8],8],6],[[[2,0],6],[0,5]]]
[[[5,5],[6,4]],[[3,8],[9,[7,6]]]]
[[0,[8,[1,4]]],2]
[[[[9,5],0],5],[9,[7,5]]]
[[9,[4,8]],[[8,1],[[8,6],[7,1]]]]
[4,[[[9,6],5],9]]
[[[[3,7],6],0],[[7,7],[[2,7],[9,3]]]]
[[[6,[3,7]],[[8,3],2]],[8,[6,[8,5]]]]
[[[5,[2,7]],[[6,7],3]],[5,[[4,4],1]]]
[[1,0],[[2,8],[[0,4],9]]]
[[[1,4],6],[[[9,8],[1,0]],1]]
[[3,4],[[1,[8,4]],8]]
[[[[9,4],[0,7]],[[5,4],[8,2]]],2]
[5,[[[8,7],[3,4]],[2,4]]]
[[[[1,3],[8,6]],[[3,4],6]],[[8,5],[[9,3],[5,7]]]]
[[0,[[0,9],[7,8]]],[3,9]]
[0,[[8,[2,3]],[[3,5],[4,9]]]]
[[[4,3],[[1,9],[1,5]]],[4,[[9,1],1]]]
[[[[3,6],[2,5]],3],[[8,[8,0]],[[6,9],[5,8]]]]
[7,[[3,[3,6]],[[6,9],[2,7]]]]
[[[[8,3],[6,5]],[[3,9],2]],[6,1]]
[[[2,0],[2,3]],8]
[[1,[[8,7],2]],[[[9,4],8],[4,[9,0]]]]
[[[6,7],[[5,2],3]],[[0,5],[[9,4],[2,6]]]]
[[[9,[5,8]],[[9,3],[6,9]]],5]
[[[5,[4,6]],[5,[3,2]]],[2,[9,[5,4]]]]
[8,6]
[[[4,8],[3,1]],[1,[[7,8],[7,5]]]]
[[4,[[8,8],4]],[5,[8,[3,9]]]]
[[[4,[9,0]],[[0,3],5]],[[5,[3,0]],[6,[2,3]]]]
[[[4,0],8],[[[4,0],7],[[9,6],3]]]
[[8,[[7,8],5]],[[[6,2],8],[1,[0,4]]]]
[[1,[[3,4],[0,8]]],[[6,5],3]]
[[5,2],[[8,6],[1,[9,7]]]]
[5,[6,[[1,3],[1,0]]]]
[[0,[[1,9],[5,6]]],[[[6,2],[5,1]],[[1,2],[1,0]]]]
[[[7,1],4],[[[0,3],3],[[4,8],1]]]
[[3,[9,[3,4]]],[1,[[0,0],[1,4]]]]
[1,[7,[1,[3,7]]]]
[[[0,[5,6]],[[7,4],[5,7]]],[[[6,8],[4,6]],9]]
[[[9,8],[7,[1,3]]],3]
[[[4,[0,3]],[[3,0],6]],[[2,[9,2]],1]]
[[[[1,9],[3,3]],[8,1]],5]
[[7,[5,2]],[[4,[0,1]],[3,3]]]
[[[6,6],[0,6]],[[3,[5,9]],[[4,2],[4,3]]]]
[[[7,[5,4]],[7,1]],9]
[[6,[5,2]],[[7,[0,5]],4]]
[[[8,1],[[7,6],[4,1]]],2]
[[[[4,3],[1,4]],[9,6]],[3,[[2,5],3]]]
[[[[9,3],[5,0]],1],[1,[[9,7],9]]]
[[[8,5],[5,9]],[2,[4,[0,0]]]]
[[[[7,9],2],[[8,8],[6,3]]],[7,[0,9]]]
[[[[6,6],[0,2]],[2,[9,0]]],[[0,9],[9,9]]]
[[[9,[1,3]],[6,5]],[[[1,1],8],[9,[7,2]]]]
[[8,[[8,4],6]],[[4,[5,9]],0]]
[[8,[5,[6,7]]],[[[1,9],9],[0,[0,9]]]]
[[9,[9,[7,3]]],[4,[4,7]]]
[[[[9,3],7],5],[[5,[8,5]],[0,[8,0]]]]
[[[5,[9,0]],[[7,4],[5,3]]],[3,[[1,1],[1,8]]]]
[[1,[[1,4],[5,9]]],[[[9,1],[6,5]],[9,[0,7]]]]
[[[[9,4],9],[5,3]],[[[4,2],[2,2]],[[1,0],0]]]
[[[6,[8,6]],9],[8,[[0,1],[9,7]]]]
[[2,0],[5,[[8,3],4]]]
[[[[0,2],0],8],[8,[[2,5],[8,2]]]]
[[[[7,4],8],[9,[7,5]]],[8,[7,[5,3]]]]
[[2,4],[3,[3,8]]]
[[5,4],[[0,[5,8]],[4,3]]]
[6,[[5,[4,7]],9]]
[[[2,[6,8]],[5,5]],[[[3,0],4],[[6,6],[0,1]]]]
[[[1,[4,2]],[[8,0],8]],[8,[[6,1],[0,0]]]]
[[9,[2,[3,3]]],[[2,6],[[5,2],[5,8]]]]
[[9,[4,4]],[[[8,6],1],2]]
[2,[[[0,7],7],[[7,8],5]]]
[[[4,0],[[1,1],[7,6]]],[[6,7],[[7,2],1]]]
[[[[2,5],0],[[9,5],9]],[6,[7,[6,1]]]]
[[[7,8],1],[[[6,2],0],[[9,7],[3,5]]]]
[[[9,1],0],[3,[[6,1],[6,9]]]]
[[[[9,0],0],[4,[7,0]]],[[6,[4,0]],[8,[4,2]]]]";
    }
}
