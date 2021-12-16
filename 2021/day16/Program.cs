using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace day15
{
    class Packet
    {
        public int V { get; }
        public int T { get; }
        public long L { get; }
        public List<Packet> SubPackets { get; } = new List<Packet>();
        public string I { get; }
        public int NextStartIndex { get; }

        public Packet(StringBuilder bin, int startIndex)
        {
            V = Convert.ToInt32(bin.ToString(startIndex, 3), 2);
            T = Convert.ToInt32(bin.ToString(startIndex + 3, 3), 2);
            var i = startIndex + 6;
            if (T == 4)
            {
                // Literal packet..
                var literal = "";
                var prefix = "1";
                while (prefix == "1")
                {
                    prefix = bin.ToString(i, 1);
                    literal += bin.ToString(i + 1, 4);
                    i += 5;
                }
                L = Convert.ToInt64(literal, 2);
            }
            else
            {
                // Operator packet..
                I = bin.ToString(i, 1);
                i += 1;
                if (I == "0")
                {
                    L = Convert.ToInt32(bin.ToString(i, 15), 2);
                    i += 15;
                    var end = i + L;
                    // the next L bits have sub-packets..
                    while (i < end)
                    {
                        SubPackets.Add(new Packet(bin, i));
                        i = SubPackets.Last().NextStartIndex;
                    }
                }
                else
                {
                    L = Convert.ToInt32(bin.ToString(i, 11), 2);
                    i += 11;
                    // the next L packets are sub-packets..
                    while (SubPackets.Count < L)
                    {
                        SubPackets.Add(new Packet(bin, i));
                        i = SubPackets.Last().NextStartIndex;
                    }
                }
            }
            NextStartIndex = i;
        }

        public int SumOfVersions()
        {
            return V + SubPackets.Sum(x => x.SumOfVersions());
        }
        public long ExecuteOperation()
        {
            switch (T)
            {
                case 0:
                    return SubPackets.Sum(x => x.ExecuteOperation());
                case 1:
                    return SubPackets
                        .Select(x => x.ExecuteOperation())
                        .Aggregate((x, y) => x * y);
                case 2:
                    return SubPackets
                        .Min(x => x.ExecuteOperation());
                case 3:
                    return SubPackets
                        .Max(x => x.ExecuteOperation());
                default:
                case 4: 
                    return L;
                case 5:
                    return SubPackets[0].ExecuteOperation() > SubPackets[1].ExecuteOperation() ? 1: 0;
                case 6:
                    return SubPackets[0].ExecuteOperation() < SubPackets[1].ExecuteOperation() ? 1: 0;
                case 7:
                    return SubPackets[0].ExecuteOperation() == SubPackets[1].ExecuteOperation() ? 1: 0;
            }
        }
    }
    class Transmission
    {
        public Packet Packet { get; }
        private Dictionary<char, string> Translation = new Dictionary<char, string> {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'A', "1010" },
            { 'B', "1011" },
            { 'C', "1100" },
            { 'D', "1101" },
            { 'E', "1110" },
            { 'F', "1111" },
        };
        public Transmission(string hexInput)
        {
            var bin = new StringBuilder();
            foreach (var c in hexInput)
            {
                bin.Append(Translation[c]);
            }
            Packet = new Packet(bin, 0);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 16!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var hex = (useTestInput ? testInput : input);

            // Start part 1.
            sw.Start();
            var transmission = new Transmission(hex);
            answer = transmission.Packet.SumOfVersions();
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            answer = transmission.Packet.ExecuteOperation();
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        //public static string testInput = @"38006F45291200";
        //public static string testInput = @"EE00D40C823060";
        //public static string testInput = @"8A004A801A8002F478"; // answer = 16
        //public static string testInput = @"620080001611562C8802118E34"; // answer = 12
        //public static string testInput = @"C0015000016115A2E0802F182340"; // answer = 23
        public static string testInput = @"A0016C880162017C3686B18A3D4780"; // answer = 31

        public static string input = @"020D78804D397973DB5B934D9280CC9F43080286957D9F60923592619D3230047C0109763976295356007365B37539ADE687F333EA8469200B666F5DC84E80232FC2C91B8490041332EB4006C4759775933530052C0119FAA7CB6ED57B9BBFBDC153004B0024299B490E537AFE3DA069EC507800370980F96F924A4F1E0495F691259198031C95AEF587B85B254F49C27AA2640082490F4B0F9802B2CFDA0094D5FB5D626E32B16D300565398DC6AFF600A080371BA12C1900042A37C398490F67BDDB131802928F5A009080351DA1FC441006A3C46C82020084FC1BE07CEA298029A008CCF08E5ED4689FD73BAA4510C009981C20056E2E4FAACA36000A10600D45A8750CC8010989716A299002171E634439200B47001009C749C7591BD7D0431002A4A73029866200F1277D7D8570043123A976AD72FFBD9CC80501A00AE677F5A43D8DB54D5FDECB7C8DEB0C77F8683005FC0109FCE7C89252E72693370545007A29C5B832E017CFF3E6B262126E7298FA1CC4A072E0054F5FBECC06671FE7D2C802359B56A0040245924585400F40313580B9B10031C00A500354009100300081D50028C00C1002C005BA300204008200FB50033F70028001FE60053A7E93957E1D09940209B7195A56BCC75AE7F18D46E273882402CCD006A600084C1D8ED0E8401D8A90BE12CCF2F4C4ADA602013BC401B8C11360880021B1361E4511007609C7B8CA8002DC32200F3AC01698EE2FF8A2C95B42F2DBAEB48A401BC5802737F8460C537F8460CF3D953100625C5A7D766E9CB7A39D8820082F29A9C9C244D6529C589F8C693EA5CD0218043382126492AD732924022CE006AE200DC248471D00010986D17A3547F200CA340149EDC4F67B71399BAEF2A64024B78028200FC778311CC40188AF0DA194CF743CC014E4D5A5AFBB4A4F30C9AC435004E662BB3EF0";
    }
}
