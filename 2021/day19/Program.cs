﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day19
{
    public static class Converter
    {
        public static int Offset = 0;
        public static Beacon[] All(Beacon x)
        {
            return AllConversions.Select(f => f(x)).ToArray();
        }
        public static Beacon NoConversion(Beacon x)
        {
            return new Beacon(x.X, x.Y, x.Z, x.Scanner);
        }
        public static Beacon X90(Beacon x)
        {
            return new Beacon(x.X, x.Z, 0 - x.Y, x.Scanner);
        }
        public static Beacon X180(Beacon x)
        {
            return new Beacon(x.X, 0 - x.Y, 0 - x.Z, x.Scanner);
        }
        public static Beacon X270(Beacon x)
        {
            return new Beacon(x.X, 0 - x.Z, x.Y, x.Scanner);
        }
        public static Beacon Y90(Beacon x)
        {
            return new Beacon(0 - x.Z, x.Y, x.X, x.Scanner);
        }
        public static Beacon Y180(Beacon x)
        {
            return new Beacon(0 - x.X, x.Y, 0 - x.Z, x.Scanner);
        }
        public static Beacon Y270(Beacon x)
        {
            return new Beacon(x.Z, x.Y, 0 - x.X, x.Scanner);
        }
        public static Beacon Z90(Beacon x)
        {
            return new Beacon(0 - x.Y, x.X, x.Z, x.Scanner);
        }
        public static Beacon Z180(Beacon x)
        {
            return new Beacon(0 - x.X, 0 - x.Y, x.Z, x.Scanner);
        }
        public static Beacon Z270(Beacon x)
        {
            return new Beacon(x.Y, 0 - x.X, x.Z, x.Scanner);
        }
        public static Beacon X90Y90(Beacon x)
        {
            return new Beacon(0 - x.Z, x.X, 0 - x.Y, x.Scanner);
        }
        public static Beacon X90Y180(Beacon x)
        {
            return new Beacon(0 - x.X, 0 - x.Z, 0 - x.Y, x.Scanner);
        }
        public static Beacon X90Y270(Beacon x)
        {
            return new Beacon(x.Z, 0 - x.X, 0 - x.Y, x.Scanner);
        }
        public static Beacon X90Z90(Beacon x)
        {
            return new Beacon(0 - x.Y, x.Z, 0 - x.X, x.Scanner);
        }
        public static Beacon X90Z180(Beacon x)
        {
            return new Beacon(0 - x.X, x.Z, x.Y, x.Scanner);
        }
        public static Beacon X90Z270(Beacon x)
        {
            return new Beacon(x.Y, x.Z, x.X, x.Scanner);
        }
        public static Beacon X180Y90(Beacon x)
        {
            return new Beacon(0 - x.Z, 0 - x.Y, 0 - x.X, x.Scanner);
        }
        public static Beacon X180Y270(Beacon x)
        {
            return new Beacon(x.Z, 0 - x.Y, x.X, x.Scanner);
        }
        public static Beacon X180Z90(Beacon x)
        {
            return new Beacon(0 - x.Y, 0 - x.X, 0 - x.Z, x.Scanner);
        }
        public static Beacon X180Z270(Beacon x)
        {
            return new Beacon(x.Y, x.X, 0 - x.Z, x.Scanner);
        }
        public static Beacon X270Y90(Beacon x)
        {
            return new Beacon(0 - x.Z, 0 - x.X, x.Y, x.Scanner);
        }
        public static Beacon X270Y270(Beacon x)
        {
            return new Beacon(x.Z, x.X, x.Y, x.Scanner);
        }
        public static Beacon X270Z90(Beacon x)
        {
            return new Beacon(0 - x.Y, 0 - x.Z, x.X, x.Scanner);
        }
        public static Beacon X270Z270(Beacon x)
        {
            return new Beacon(x.Y, 0 - x.Z, 0 - x.X, x.Scanner);
        }
        public static Func<Beacon, Beacon>[] AllConversions
        {
            get
            {
                var result = new Func<Beacon, Beacon>[] {
                    NoConversion,
                    X90,
                    X180,
                    X270,
                    Y90,
                    Y180,
                    Y270,
                    Z90,
                    Z180,
                    Z270,
                    X90Y90,
                    X90Y180,
                    X90Y270,
                    X90Z90,
                    X90Z180,
                    X90Z270,
                    X180Y90,
                    X180Y270,
                    X180Z90,
                    X180Z270,
                    X270Y90,
                    X270Y270,
                    X270Z90,
                    X270Z270,
                };
                var part1 = result.Skip(Offset).Take(result.Length - Offset);
                var part2 = result.Skip(0).Take(Offset);
                return part1.Union(part2).ToArray();
            }
        }
    }
    public class Beacon : IEquatable<Beacon>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public Scanner Scanner { get; }

        public Beacon(int x, int y, int z, Scanner scanner) : this(scanner)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Beacon(string input, Scanner scanner) : this(scanner)
        {
            var split = input.Split(',');
            X = int.Parse(split[0]);
            Y = int.Parse(split[1]);
            Z = int.Parse(split[2]);
        }
        private Beacon(Scanner scanner)
        {
            Scanner = scanner;
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public Beacon Subtract(Beacon other)
        {
            return new Beacon(X - other.X, Y - other.Y, Z - other.Z, Scanner);
        }
        public Beacon Add(Beacon other)
        {
            return new Beacon(X + other.X, Y + other.Y, Z + other.Z, Scanner);
        }
        public bool Equals(Beacon other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
    public class BeaconPair : IEquatable<BeaconPair>
    {
        public Beacon First { get; }
        public Beacon Second { get; }

        public int DeltaX => Second.X - First.X;
        public int DeltaY => Second.Y - First.Y;
        public int DeltaZ => Second.Z - First.Z;
        public int Delta => (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ);
        public Beacon Offset => Second.Subtract(First);
        public double Distance => Math.Sqrt(Delta);

        public BeaconPair(Beacon first, Beacon second)
        {
            First = first;
            Second = second;
        }

        public override string ToString()
        {
            return $"{Delta}:([{Second}]-[{First}])";
        }
        public override int GetHashCode()
        {
            var beaconOrderedByHash = new Beacon[] { First, Second }.OrderBy(x => x.GetHashCode());
            return $"{beaconOrderedByHash.First().GetHashCode()}{beaconOrderedByHash.Last().GetHashCode()}".GetHashCode();
        }

        public bool Equals(BeaconPair other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }
    public class Scanner : IEquatable<Scanner>
    {
        public int ID { get; }
        public Beacon[] Beacons { get; }
        public BeaconPair[] Pairs { get; }
        public Beacon Offset { get; private set; }

        public Scanner(string input)
        {
            ID = int.Parse(input.Split(Environment.NewLine).First().Replace("--- scanner ", "").Replace(" ---", ""));
            if (ID == 0)
            {
                Offset = new Beacon(0, 0, 0, this);
            }
            Beacons = input
                .Split(Environment.NewLine)
                .Skip(1)
                .Select(x => new Beacon(x, this))
                .ToArray();
            Pairs = Beacons.SelectMany(x => Beacons.Where(y => !y.Equals(x)), (x, y) => new BeaconPair(x, y)).Distinct().ToArray();
        }
        public Scanner(int id, IEnumerable<Beacon> beacons, Beacon offset)
        {
            ID = id;
            Beacons = beacons.ToArray();
            Pairs = Beacons.SelectMany(x => Beacons.Where(y => !y.Equals(x)), (x, y) => new BeaconPair(x, y)).Distinct().ToArray();
            Offset = offset;
        }

        public BeaconPair[] GetOverlappingBeacons(Scanner other)
        {
            var result = new List<BeaconPair>();
            var matchingPairs = Pairs.SelectMany(
                x => other.Pairs.Where(y => x.Delta == y.Delta),
                (x, y) => new Tuple<BeaconPair, BeaconPair>(x, y))
                .ToArray();

            foreach (var matchGroup in matchingPairs.GroupBy(x => x.Item1.First))
            {
                var allOthers = matchGroup.SelectMany(x => new Beacon[] { x.Item2.First, x.Item2.Second });
                var distinctOthers = allOthers.Distinct();
                var counts = distinctOthers
                    .Select(x => new { Key = x, Count = allOthers.Count(y => y == x) })
                    .OrderByDescending(x => x.Count)
                    .ToArray();
                if (counts[0].Count == counts.Length - 1)
                {
                    //Console.WriteLine($"Counts.Length: {counts.Length}, {counts[0].Count} > {counts[1].Count}");
                    result.Add(new BeaconPair(matchGroup.Key, counts[0].Key));
                }
            }
            foreach (var matchGroup in matchingPairs.GroupBy(x => x.Item1.Second)) //.Where(x => !result.Exists(y => x.Key == y.First)))
            {
                var allOthers = matchGroup.SelectMany(x => new Beacon[] { x.Item2.First, x.Item2.Second });
                var distinctOthers = allOthers.Distinct();
                var counts = distinctOthers
                    .Select(x => new { Key = x, Count = allOthers.Count(y => y == x) })
                    .OrderByDescending(x => x.Count)
                    .ToArray();
                if (counts[0].Count == counts.Length - 1)
                {
                    //Console.WriteLine($"Counts.Length: {counts.Length}, {counts[0].Count} > {counts[1].Count}");
                    result.Add(new BeaconPair(matchGroup.Key, counts[0].Key));
                }
            }

            return result.Distinct().ToArray();
        }

        internal Scanner TryConversion(Scanner scanner)
        {
            var beacons = GetOverlappingBeacons(scanner);
            if (beacons.Length > 11)
            {
                // we should be able to convert this..
                for (var j = 0; j < beacons.Length; j++)
                {
                    var converted = Converter.All(beacons[j].Second);
                    var offsetsJ = converted.Select(x => beacons[j].First.Subtract(x)).ToArray();
                    for (var i = 0; i < beacons.Length; i++)
                    {
                        if (i != j)
                        {
                            converted = Converter.All(beacons[i].Second);
                            var offsetsI = converted.Select(x => beacons[i].First.Subtract(x));
                            var matchingOffsets = offsetsJ.Where(x => offsetsI.Contains(x)).ToArray();
                            if (matchingOffsets.Length == 1)
                            {
                                var converterIndex = Array.IndexOf(offsetsJ, matchingOffsets[0]);
                                Console.WriteLine($"Converting scanner {scanner.ID}, offset: {matchingOffsets[0]}, conversion: {converterIndex}");
                                var conversionFunction = Converter.AllConversions[converterIndex];
                                var result = new Scanner(scanner.ID, scanner.Beacons.Select(x => matchingOffsets[0].Add(conversionFunction(x))), matchingOffsets[0]);
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public bool Equals(Scanner other)
        {
            return ID == other.ID;
        }
    }
    public class ScannerDetector
    {
        public Scanner[] Scanners { get; }

        public ScannerDetector(Scanner[] scanners)
        {
            Scanners = scanners;
        }

        public Scanner[] Detect()
        {
            var convertedScanners = new List<Scanner>();
            convertedScanners.Add(Scanners[0]);
            var normalScanners = new List<Scanner>(Scanners.Skip(1));
            var conversionCount = convertedScanners.Count();
            while (normalScanners.Count() > 0)
            {
                var toLoop = convertedScanners.ToArray();
                foreach (var currentScanner in toLoop)
                {
                    var newConversions = normalScanners
                        .Select(x => currentScanner.TryConversion(x))
                        .Where(x => x != null)
                        .ToArray();
                    if (newConversions.Length > 0)
                    {
                        convertedScanners.AddRange(newConversions);
                        foreach (var scanner in newConversions)
                        {
                            normalScanners.Remove(scanner);
                        }
                    }
                }
                if (conversionCount == convertedScanners.Count())
                {
                    Console.WriteLine("Breaking out of while loop, because we're not adding any more conversions..");
                    break;
                }
                conversionCount = convertedScanners.Count();
            }

            // Write some debug output..
            Console.Write($"Converted {convertedScanners.Count()} of {Scanners.Length}: ");
            foreach (var s in convertedScanners)
            {
                Console.Write($"{s.ID},");
            }
            Console.WriteLine();
            Console.Write("In order: ");
            foreach (var s in convertedScanners.OrderBy(x => x.ID))
            {
                Console.Write($"{s.ID},");
            }
            Console.WriteLine();

            return convertedScanners.ToArray();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 19!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = false;
            var scanners = (useTestInput ? testInput : input)
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x => new Scanner(x))
                .ToArray();

            // var beacon = new Beacon(1,2,3, scanners[0]);
            // var conversions = Converter.All(beacon);
            // foreach(var b in conversions.OrderBy(x => x.Z)) {
            //     Console.WriteLine(b);
            // }

            // Start part 1.
            sw.Start();
            var detector = new ScannerDetector(scanners);
            var convertedScanners = detector.Detect();
            var distinctBeacons = convertedScanners.SelectMany(x => x.Beacons).Distinct().OrderBy(x => x.X);
            // foreach (var beacon in distinctBeacons)
            // {
            //     Console.WriteLine(beacon);
            // }
            answer = distinctBeacons.Count();
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            var best = convertedScanners
                .SelectMany(x => convertedScanners, (x, y) => new { ScannerX = x, ScannerY = y, Offset = x.Offset.Subtract(y.Offset) })
                .Where(x => x.ScannerX.Offset != x.ScannerY.Offset)
                .Select(x => new { Result = x, ManhattanDist = x.Offset.X + x.Offset.Y + x.Offset.Z })
                .OrderByDescending(x => x.ManhattanDist)
                .First();
            answer = best.ManhattanDist;
            Console.WriteLine($"{best.Result.ScannerX.ID}:{best.Result.ScannerX.Offset} - {best.Result.ScannerY.ID}:{best.Result.ScannerY.Offset} = {best.Result.Offset}");
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14";
        public static string input = @"--- scanner 0 ---
629,879,599
537,-422,-909
336,-542,718
360,-562,806
-719,403,-777
-563,628,363
-694,-559,376
574,-351,-962
-490,-830,-917
593,806,503
708,801,463
-850,370,-818
662,960,-958
607,-317,-887
-731,-740,327
619,867,-879
492,905,-947
-862,443,-676
-680,555,402
-54,104,-16
-782,-604,301
-492,-664,-797
-553,-795,-758
-635,693,486
295,-655,741

--- scanner 1 ---
-750,-642,-499
-409,831,-474
-37,-42,57
406,741,863
-415,846,-315
373,-289,-807
-623,-467,549
-351,548,630
-356,671,494
620,-669,373
419,-350,-826
-400,734,646
-629,-715,-409
-749,-583,-407
801,471,-547
511,676,927
458,-460,-831
-399,888,-513
605,-726,578
780,516,-567
880,520,-669
626,-805,422
-613,-531,474
-474,-478,464
80,108,159
420,719,879

--- scanner 2 ---
-824,703,-749
505,-479,-428
-639,562,564
461,-492,-415
512,598,719
312,-733,801
-872,-470,-467
816,674,-715
742,650,-634
352,-805,825
567,-479,-257
-789,586,-704
-55,14,96
-593,-526,635
-610,535,432
-860,-302,-479
728,677,-551
-833,-526,-498
417,-597,826
-733,-541,683
-806,447,-750
-604,-613,629
505,644,731
-772,518,456
601,695,683

--- scanner 3 ---
557,-643,-775
-839,320,319
-911,297,279
-537,495,-490
-941,-665,733
-902,-636,798
798,792,362
798,647,335
616,-675,-736
673,748,338
-526,-454,-903
-551,-423,-693
-778,474,-510
528,-889,292
588,712,-812
-5,1,-52
-848,306,427
-909,-627,748
-716,433,-516
408,-730,308
547,650,-773
612,764,-748
510,-788,363
579,-821,-693
-88,-149,-156
-671,-444,-807

--- scanner 4 ---
535,-675,-665
-935,505,689
-624,-808,670
-567,-796,778
379,-784,357
-536,663,-398
-119,0,-98
477,-780,470
-502,520,-479
230,445,392
-495,-784,615
483,678,-435
438,-639,-576
365,-779,455
-970,346,752
-505,587,-350
-804,466,750
303,587,380
489,-687,-672
-574,-703,-419
-635,-663,-351
456,595,-449
-535,-782,-364
459,759,-361
263,486,523
17,-61,7

--- scanner 5 ---
761,362,-452
-630,-789,370
-516,-858,428
736,341,508
763,355,591
-373,536,-358
916,-474,599
804,-656,-479
956,-617,700
-567,-946,425
-1,-42,0
-403,-990,-423
-406,622,727
101,-181,-82
-481,-930,-463
-375,527,-474
-393,578,714
943,369,-450
666,-701,-563
989,-486,741
885,360,-503
-461,-932,-404
-326,427,-449
-393,516,885
731,-773,-440
760,267,427

--- scanner 6 ---
744,755,-365
548,661,738
-495,663,745
545,-426,673
-412,636,759
117,54,-98
-342,572,-772
679,-667,-890
634,-652,-658
742,615,-439
454,-473,564
144,-87,57
-789,-493,584
-618,-493,522
-548,-540,-816
747,-701,-790
-631,-506,478
-297,689,-807
599,785,740
-331,660,833
774,658,-456
-296,433,-789
546,843,731
-567,-316,-843
-579,-484,-855
570,-475,573

--- scanner 7 ---
-559,-808,659
662,516,-637
430,552,581
684,426,-570
-632,-603,-649
437,641,726
369,-560,-469
388,-635,-384
760,-590,453
-490,475,-887
732,-704,361
-779,467,331
-640,-753,765
-716,-889,697
-688,-569,-676
892,-704,403
34,-113,14
706,421,-644
-666,619,344
-569,320,-890
-435,292,-830
358,-714,-509
449,747,593
-710,-716,-600
-749,570,442

--- scanner 8 ---
-686,416,-453
57,-22,-25
387,571,-553
-468,-675,-759
-319,431,466
-430,-698,759
373,796,-585
-420,-832,734
-538,-633,-690
396,617,-490
724,-495,-473
553,-863,359
638,-528,-590
745,618,841
-739,325,-527
385,-937,340
816,630,700
-342,437,660
743,-524,-701
-417,-604,719
650,626,737
387,-818,401
-728,271,-449
-100,6,91
-360,399,475
-675,-653,-816

--- scanner 9 ---
597,783,-817
-448,-947,591
-808,761,717
755,-448,607
782,-825,-523
14,-162,-122
-659,321,-639
677,251,517
621,364,473
-425,-818,542
646,-749,-455
-817,665,813
483,651,-767
-854,340,-627
-726,-775,-876
597,623,-859
812,-429,722
-677,-858,-778
-574,-888,-889
732,231,433
701,-668,-561
715,-564,692
-748,328,-494
-740,655,676
-508,-760,588
-52,-57,30

--- scanner 10 ---
684,869,-808
606,-623,-681
486,-451,235
-425,483,-974
-503,472,-920
-769,310,568
693,-617,-670
13,54,-3
-632,327,681
414,-427,275
499,851,-846
-380,-333,-687
534,838,391
-362,-410,-863
-632,-521,454
-429,525,-829
490,892,-814
678,-569,-810
-750,-480,574
-298,-321,-892
537,862,420
-738,312,591
-659,-588,658
622,-426,340
466,865,589

--- scanner 11 ---
-448,639,594
1,109,-128
693,603,609
-434,647,693
503,-301,368
-301,-383,241
-390,-412,305
763,645,678
-403,466,-532
-549,-465,-735
814,574,-800
-398,659,477
-411,556,-534
801,668,-840
811,802,-815
556,623,735
545,-446,372
-597,-605,-750
-603,-594,-663
912,-461,-547
871,-429,-556
889,-467,-741
-460,553,-392
492,-355,446
-313,-588,323

--- scanner 12 ---
723,-419,508
466,890,908
-417,-569,762
-785,731,-544
763,-504,392
-440,-520,891
629,-638,-420
-500,-523,704
-27,70,-5
820,-389,373
-682,488,539
643,730,-426
685,-594,-307
-800,-394,-817
-562,738,-554
-749,692,566
740,-678,-287
-732,-435,-711
-637,768,-526
687,809,-589
-790,572,448
-809,-298,-721
594,943,798
726,691,-547
442,943,898

--- scanner 13 ---
420,-360,641
508,-363,590
665,628,-592
710,544,-730
880,-676,-915
113,48,37
-670,562,679
892,-534,-842
376,488,477
577,-305,704
-678,605,-484
-632,-816,-325
436,528,555
-743,-616,326
-599,741,-406
-563,645,-479
749,-635,-882
-462,-805,-380
593,505,-657
434,469,535
-594,-661,392
-753,671,729
-589,-737,-418
-541,-604,328
-636,633,750

--- scanner 14 ---
-597,-824,306
651,808,-522
786,-484,-914
-686,-727,-490
-605,588,-824
345,-770,677
651,766,-362
-626,-863,371
-691,486,796
537,716,-494
359,-761,785
-588,-831,356
-754,-768,-384
-749,588,741
-800,576,-805
-850,606,812
731,-568,-894
787,751,766
686,876,769
-694,443,-817
336,-833,825
831,876,742
12,24,39
-763,-864,-480
675,-524,-781

--- scanner 15 ---
466,-754,577
-488,-741,-626
-637,372,427
-557,-799,-512
561,-500,-888
-817,-865,816
794,521,357
360,-699,496
683,624,387
-702,-808,797
720,359,-850
428,-835,420
-787,-768,681
781,673,470
614,-443,-824
-400,-744,-575
763,-488,-890
494,330,-851
-590,645,-377
-599,685,-545
-683,455,485
-54,71,-58
-542,784,-487
473,338,-872
-686,395,298

--- scanner 16 ---
-28,84,-31
439,554,547
-795,651,248
-679,-690,-539
-554,588,-675
426,441,395
457,-806,-614
-792,730,403
-782,-369,367
726,-767,204
747,-788,212
500,-766,233
754,661,-880
-799,-720,-449
-735,785,220
-823,-506,358
606,-740,-569
-728,-730,-504
-581,532,-641
659,-804,-626
792,504,-961
537,471,498
860,580,-825
156,22,-96
-803,-306,372
-505,510,-788

--- scanner 17 ---
-508,-676,508
746,-629,-405
-589,-782,563
783,-378,612
439,367,681
938,663,-516
943,-420,612
-635,-300,-507
712,-616,-655
-618,538,-586
-581,632,713
813,610,-481
811,-470,726
-665,-714,427
-563,607,-641
-647,-298,-622
-637,659,747
124,-43,155
-682,-298,-672
842,632,-596
409,492,602
7,6,-5
-595,715,-517
-659,583,600
420,452,756
765,-662,-603

--- scanner 18 ---
519,658,858
-555,847,-730
-829,-550,-297
837,-629,-709
705,674,809
802,-446,800
778,-737,-800
859,-485,810
-612,-379,460
-784,-617,-357
826,-789,-662
-496,577,586
860,910,-403
-822,-452,-390
-507,492,735
878,910,-333
-671,-471,459
817,-287,794
-367,865,-753
-475,937,-647
642,522,853
8,101,145
-744,-381,580
-560,502,596
846,847,-403

--- scanner 19 ---
-805,869,529
-783,-464,477
244,-283,522
603,-738,-602
-662,601,-567
234,-336,546
-470,-355,-763
-734,730,-621
242,812,-869
-35,7,17
694,-654,-710
324,607,-857
-749,830,560
479,765,625
-749,590,-573
-973,857,541
306,783,-780
-796,-570,319
-678,-420,-764
344,-283,674
-796,-508,490
529,605,610
-160,106,98
576,-534,-669
449,665,622
-542,-315,-739

--- scanner 20 ---
640,-478,634
786,661,-349
529,-687,-564
-694,768,607
-761,-255,896
518,718,512
-595,744,596
-658,-334,866
-678,514,-543
469,550,576
-745,619,-536
-657,-825,-434
-549,-727,-438
681,-481,673
762,681,-261
-625,841,667
476,-451,735
-759,-324,882
493,-760,-468
-576,-836,-351
447,539,471
46,123,115
827,774,-326
-71,-7,28
-760,496,-497
414,-682,-604

--- scanner 21 ---
-213,-501,-417
-568,540,465
447,-790,-337
843,455,-367
825,871,682
995,-746,666
860,871,732
-587,507,663
-633,699,-666
-765,749,-552
-399,-896,464
813,869,567
582,-760,-434
-233,-585,-472
-310,-899,316
967,-792,577
-534,553,715
466,-675,-394
185,2,-23
901,-853,704
767,387,-503
-308,-880,327
71,-123,51
-346,-541,-528
-605,791,-552
686,493,-448

--- scanner 22 ---
639,545,-456
422,716,301
-630,468,-404
-658,-342,804
-667,-479,845
-792,-739,-478
776,-771,651
-527,498,-515
-777,678,817
744,-326,-731
884,-699,757
-478,581,-417
-622,757,814
696,-681,734
151,162,-147
-653,-279,825
507,547,-576
467,783,361
755,-411,-582
-805,812,757
691,-336,-592
580,637,-455
-813,-761,-725
429,618,394
52,55,40
-821,-740,-704

--- scanner 23 ---
-574,762,520
-312,521,-529
-625,765,432
99,-10,113
396,320,-450
434,388,-577
-61,24,-14
898,490,761
559,-880,-719
-655,-664,631
-310,509,-731
341,309,-620
-349,-309,-462
-328,-317,-441
782,-780,927
931,449,892
-823,-624,622
540,-857,-623
858,591,904
-688,820,589
-713,-763,574
684,-776,814
575,-671,-660
-350,-338,-688
796,-775,829
-376,464,-582

--- scanner 24 ---
615,353,-654
550,309,-485
-453,-431,711
-500,582,-541
433,471,483
857,-778,376
-523,513,-718
-483,503,297
781,-888,-809
-459,-604,-586
504,400,449
791,-823,525
-48,-116,20
454,509,366
-529,-567,779
741,-853,-760
-610,-512,673
577,219,-625
-436,668,295
-555,637,-610
855,-766,578
771,-943,-880
-537,-717,-635
-481,-751,-662
-471,618,295

--- scanner 25 ---
-638,430,749
361,-836,-416
527,-716,-418
-26,2,127
-704,404,819
-484,-506,489
629,887,434
451,408,-657
-544,322,802
-513,-556,-705
587,382,-708
649,799,410
840,-881,648
769,-828,611
-488,-569,-810
632,457,-620
396,-677,-479
150,84,39
-586,-427,490
536,822,369
-520,753,-820
874,-806,681
-592,-526,559
-407,-460,-766
57,-83,-55
-552,746,-796
-468,677,-830

--- scanner 26 ---
252,-554,-711
36,158,8
646,-559,236
-945,558,220
-733,649,-433
642,-451,334
729,-478,225
-883,-382,300
-866,601,313
386,-583,-673
-826,-354,314
342,-414,-738
542,437,-834
504,554,-855
-855,-738,-507
433,520,-784
-100,63,-181
-745,447,-431
-934,728,275
612,591,301
-790,596,-497
-820,-689,-547
-910,-809,-517
476,512,379
516,627,374
-868,-566,310

--- scanner 27 ---
40,65,-13
-713,781,-416
-942,467,337
694,-735,-920
644,-646,598
-909,508,468
-734,-617,-631
-637,-653,-720
-869,609,325
702,-745,655
563,780,709
-683,-665,-727
766,-759,-880
677,838,640
812,-606,607
-695,-758,415
440,323,-665
453,481,-771
-689,700,-490
-807,-757,400
558,-760,-815
614,783,797
-793,-762,302
440,494,-727
-138,-51,-140
-716,617,-504

--- scanner 28 ---
-877,-921,789
-397,533,642
799,-507,573
-877,-929,707
774,-402,648
-453,-857,-368
-458,600,668
66,-63,95
468,745,787
750,499,-426
759,-621,-523
-759,682,-391
-850,790,-450
-53,-1,-82
-453,-794,-382
-889,-914,834
-332,704,692
-383,-857,-350
458,614,797
-712,842,-402
808,-605,-709
453,762,720
744,340,-411
803,-469,-586
760,538,-369
763,-407,463

--- scanner 29 ---
-691,-723,815
674,564,688
-872,-760,864
-563,624,-452
-731,575,-388
-592,-711,-673
-787,266,450
-674,307,473
423,-709,789
438,617,-611
-552,646,-347
468,412,-549
-53,-52,106
-736,-811,890
747,600,592
-567,-682,-557
478,-701,-265
561,-664,705
496,-592,828
669,679,728
-560,-752,-731
465,453,-510
-694,331,385
565,-608,-350
393,-627,-370

--- scanner 30 ---
-590,-563,-456
37,43,-8
437,-555,648
-793,823,812
611,447,626
555,-426,-879
483,-402,-839
-563,352,-433
-238,-805,552
406,504,611
455,368,605
535,890,-525
691,-401,-774
547,-416,696
-700,335,-546
-425,-745,555
-798,713,773
-681,-455,-540
-563,-496,-660
603,863,-723
606,887,-694
-774,884,858
-367,-828,601
-669,337,-523
605,-515,639

--- scanner 31 ---
-642,-798,-543
-726,764,-505
-680,577,615
-476,644,588
406,526,-768
838,-803,404
286,-793,-279
-699,-779,-338
-453,-271,615
581,909,811
447,594,-662
532,886,940
792,-742,385
-756,689,-477
-719,541,-495
348,-789,-232
-566,-254,478
-101,18,112
-543,-256,509
753,-632,380
463,-707,-310
-635,-641,-427
-560,536,497
310,517,-672
535,826,753

--- scanner 32 ---
-684,731,-473
-494,344,414
-639,440,404
-111,46,124
-904,-911,415
453,-523,706
-505,757,-535
-602,-634,-535
648,511,635
579,474,-690
-519,554,383
555,-603,817
644,499,-804
-378,-677,-564
589,472,603
611,554,781
448,-708,768
619,-791,-820
669,-787,-703
620,-783,-595
-860,-754,480
620,435,-752
-746,737,-514
-794,-914,444
-425,-576,-539
32,-2,-36

--- scanner 33 ---
-404,545,-644
637,656,698
657,-408,504
566,686,795
-112,197,48
762,-386,493
-455,-469,711
-702,-617,-631
-412,561,-793
-817,762,468
-916,825,461
337,822,-301
-487,656,-678
-570,-335,668
-804,-644,-612
721,-457,672
560,-620,-527
-15,54,-88
351,820,-370
498,-415,-497
-760,922,445
626,755,722
599,-579,-524
-728,-728,-486
-524,-328,716
367,719,-380

--- scanner 34 ---
480,-685,-389
549,-592,714
871,582,284
755,522,286
522,-518,591
-578,395,-892
837,857,-436
-629,756,521
65,16,-68
-682,666,646
-645,440,-832
-674,-832,-436
510,-404,763
-747,-674,470
417,-680,-368
-754,632,513
909,749,-542
-540,-804,-376
-480,402,-799
617,-694,-348
883,591,355
819,669,-459
-601,-675,342
-619,-649,-379
-523,-645,476

--- scanner 35 ---
574,952,-661
-810,-374,-944
586,954,-863
-663,530,-756
266,-733,602
-794,-204,673
209,-752,717
-592,601,-790
-860,-339,-944
306,-617,687
-705,-232,719
-558,725,316
239,-356,-528
375,-432,-460
-40,186,-94
333,690,388
-695,-386,-942
-192,24,-96
275,576,457
631,888,-714
320,-513,-566
330,639,300
-832,-298,780
-499,745,314
-518,803,256
-701,551,-873

--- scanner 36 ---
-731,524,801
535,-990,-859
530,528,779
-29,-170,-162
512,-988,-828
881,403,-628
422,-973,-722
-637,-878,452
509,484,650
-784,-612,-957
629,477,632
683,-438,447
712,-462,444
-622,-821,247
-362,368,-795
-823,-502,-952
-836,589,737
-822,567,753
884,368,-687
-869,-441,-933
-462,364,-693
-616,-957,296
-441,319,-673
-35,-16,-11
514,-435,376
870,333,-635

--- scanner 37 ---
-406,-916,-666
-470,696,-443
725,504,671
495,-635,588
732,399,-456
-513,739,-531
-358,-752,-699
-746,-530,717
635,-582,-533
-353,774,608
1,-120,37
-844,-526,609
606,-651,-433
-343,790,507
749,515,572
-473,-904,-728
589,-490,-432
600,-628,521
791,425,-561
-274,804,505
-401,822,-524
477,-589,506
812,539,-500
-855,-443,690
822,575,641
119,59,92

--- scanner 38 ---
568,-575,297
595,795,-575
511,-560,502
777,-413,-907
759,519,388
-463,833,-519
-777,772,801
-798,525,823
-491,-538,328
-512,735,-436
-741,-634,-347
432,-581,346
654,794,-483
706,658,398
-352,-490,438
-451,-547,496
-615,846,-406
887,-374,-902
114,-71,80
522,674,-485
948,-328,-883
16,-86,-109
770,668,473
-681,-631,-534
-764,-585,-476
190,47,-42
-806,546,802

--- scanner 39 ---
-597,-493,-298
944,-665,-729
692,655,-769
561,-840,850
-768,-408,-322
-415,715,-630
-498,256,844
629,-799,744
621,371,417
-707,-454,-303
141,-8,8
48,-177,69
921,-714,-858
515,504,429
-754,-898,314
-434,332,829
795,766,-799
-672,-783,392
643,753,-686
861,-711,-671
-342,594,-720
-554,390,876
-758,-960,361
710,516,410
552,-936,754
-474,714,-722";
    }
}
