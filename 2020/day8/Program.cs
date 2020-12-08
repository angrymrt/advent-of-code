﻿using System;
using System.Collections.Generic;

namespace day8
{
    public class Instruction {
        public Instruction(string raw){
            Operation = raw.Substring(0, 3);
            Argument = int.Parse(raw.Substring(4));
        }

        public string Operation { get; }
        public int Argument { get; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var accumulator = 0;
            var cursor = 0;
            var bootCode = input.Split(Environment.NewLine);
            var executedInstructions = new HashSet<int>();
            while(executedInstructions.Add(cursor)){
                var currentInstruction = new Instruction(bootCode[cursor]);
                switch(currentInstruction.Operation){
                    case "acc":
                        cursor++;
                        accumulator = accumulator + currentInstruction.Argument;
                        break;
                    case "nop":
                        cursor++;
                        break;
                    case "jmp":
                        cursor = cursor + currentInstruction.Argument;
                        break;
                }
            }

            Console.WriteLine($"Answer for day 8, part 1: {accumulator}");


            // part two..
            var endOfFile = bootCode.Length;
            for(int i = 0; i < endOfFile; i++)
            {
                accumulator = 0;
                cursor = 0;
                executedInstructions = new HashSet<int>();
                while(executedInstructions.Add(cursor) && cursor != endOfFile){
                    var rawInstruction = bootCode[cursor];
                    if(i == cursor && rawInstruction.StartsWith("nop")){
                        rawInstruction = rawInstruction.Replace("nop", "jmp");
                    }
                    else if (i == cursor && rawInstruction.StartsWith("jmp"))
                    {
                        rawInstruction = rawInstruction.Replace("jmp", "nop");
                    }
                    var currentInstruction = new Instruction(rawInstruction);
                    switch(currentInstruction.Operation){
                        case "acc":
                            cursor++;
                            accumulator = accumulator + currentInstruction.Argument;
                            break;
                        case "nop":
                            cursor++;
                            break;
                        case "jmp":
                            cursor = cursor + currentInstruction.Argument;
                            break;
                    }
                }
                if(cursor == endOfFile){
                    Console.WriteLine($"Answer for day 8, part 2: {accumulator}");
                    return;
                }
            }
        }

        private static string input = @"nop +283
acc +26
acc +37
acc +6
jmp +109
acc +10
jmp +18
acc +5
jmp +327
acc -4
jmp +269
acc -7
acc +27
nop +7
acc +0
jmp +81
acc +42
nop +338
acc -5
jmp +391
nop +276
jmp +354
acc +22
jmp +528
acc +0
acc +20
acc +15
acc -17
jmp +537
acc -15
acc +12
acc -17
acc +17
jmp +34
acc -19
jmp +88
acc +19
acc +35
acc +17
acc +7
jmp +443
acc +22
jmp +584
jmp -2
jmp +408
acc +46
acc +43
acc +4
jmp +532
acc -19
acc -19
acc +38
acc -10
jmp +476
acc +1
acc +3
acc +19
acc +28
jmp +480
jmp +1
acc +32
acc -2
jmp +518
acc +5
acc -19
acc +19
jmp +344
jmp +99
acc +0
acc +30
acc -13
acc -19
jmp +385
acc -18
jmp +157
acc +15
acc +4
jmp +503
acc -6
acc +42
jmp +461
acc -6
jmp +328
acc -9
nop +199
acc +15
jmp +206
jmp +182
acc +35
nop +275
acc +3
jmp +1
jmp -25
nop -20
nop -6
jmp -7
nop +145
acc +4
acc +28
jmp +315
nop -76
nop +12
nop +170
jmp +291
acc -16
acc +5
nop -10
jmp +235
acc +6
acc -1
nop +492
acc +44
jmp +119
jmp +128
jmp +1
jmp +328
acc -7
jmp +126
nop +351
acc +9
acc +4
acc -1
jmp +276
acc +0
nop +133
acc +36
acc +32
jmp +173
acc +41
nop -95
jmp +153
acc +7
acc +13
acc -10
jmp +223
jmp +186
acc +4
jmp +90
acc -7
acc +15
jmp +366
acc +9
acc +27
acc +1
jmp +417
acc -19
jmp +268
acc +38
acc +1
acc +27
jmp +1
jmp +420
acc +13
acc +9
acc +1
jmp +370
acc +25
acc +3
acc -1
jmp +324
nop +352
acc +39
jmp +121
acc +15
jmp +348
jmp +11
acc -12
acc +23
jmp +407
jmp -6
acc +43
jmp -8
acc +48
nop +316
acc +5
jmp +323
acc +3
jmp +1
acc +34
jmp +191
jmp -160
acc -18
acc +33
jmp -79
acc +9
acc +50
acc -15
acc -1
jmp -100
acc -18
acc +49
nop -184
acc +20
jmp +404
nop +280
jmp +294
acc -12
jmp +1
acc +8
jmp +320
nop +387
acc +15
nop +359
acc -7
jmp +182
nop +1
nop -40
acc +3
jmp -38
acc +44
acc -11
nop +297
jmp +174
jmp -140
acc +32
acc +28
acc +8
acc +9
jmp -194
acc -9
acc +32
jmp +291
acc +43
nop +220
acc +9
acc +15
jmp -167
jmp -8
acc -3
acc +12
jmp +195
acc +48
acc +16
nop +137
acc +29
jmp +48
acc +11
acc +46
acc +22
acc -2
jmp -167
jmp +123
jmp +128
acc +24
acc +50
acc -10
jmp -202
acc -17
acc -13
jmp +1
jmp +89
acc -4
acc +41
jmp +111
acc +50
acc +41
jmp +83
acc -2
nop +194
jmp +239
acc +33
acc +25
jmp +347
nop +6
acc +0
acc -16
jmp +73
acc -12
jmp -5
jmp +188
jmp +1
jmp -264
acc +44
acc +6
acc +35
jmp +312
acc +28
acc +8
jmp -15
acc +48
jmp +215
acc -1
jmp -55
acc +22
acc -18
acc +47
jmp -266
jmp +1
acc +18
acc +0
acc -11
jmp +221
acc -10
nop -189
jmp -216
jmp -3
acc -8
acc +22
jmp +253
jmp -168
acc -7
acc +14
nop +315
acc +11
jmp -47
nop -36
acc +40
jmp +95
jmp +13
acc -14
acc -5
acc +48
jmp -85
acc -17
acc +20
acc -5
acc +6
jmp +221
acc +32
acc +7
jmp +12
nop +266
acc -11
acc -8
nop +182
jmp -184
nop -137
acc +48
jmp +155
jmp -124
acc +44
jmp +24
acc +12
jmp -292
jmp +195
jmp -301
acc +45
acc -14
jmp -66
jmp +86
acc +33
jmp -136
jmp -146
acc -3
acc -13
acc +16
jmp -183
acc +4
acc -8
acc +14
jmp -169
acc +35
acc +18
nop -24
jmp -127
jmp -219
jmp +190
acc -4
acc +1
jmp +62
nop +220
acc +18
acc +36
jmp +58
acc +25
jmp +21
nop -24
acc +2
acc +49
jmp -325
acc +24
acc +23
acc +13
jmp +143
jmp -45
nop +212
jmp -29
acc -12
acc -12
jmp -107
nop +126
acc +32
jmp -113
jmp +1
acc -6
jmp -102
nop +57
acc -16
acc +25
jmp -213
acc +19
acc +29
acc +0
jmp -320
acc +42
jmp +94
acc +6
jmp -363
acc -18
jmp -365
acc +39
jmp +13
acc +47
acc +24
acc +9
acc +25
jmp +151
acc +17
jmp +1
jmp -77
jmp +24
acc -13
acc -13
jmp -141
acc +22
acc +9
nop +92
jmp -334
acc +30
acc +11
jmp -304
acc +8
jmp -275
acc +35
jmp -95
jmp +1
acc -18
nop -407
nop -18
jmp +146
acc +37
acc -4
acc +19
jmp -409
acc +28
acc -10
nop +151
acc +17
jmp -418
nop +56
acc +40
acc -13
jmp -301
acc +28
acc -7
acc -6
jmp +62
acc +0
acc +6
acc +25
acc +26
jmp +18
acc -14
jmp +93
acc +43
acc +19
jmp -109
acc +24
acc +0
jmp -328
acc +42
jmp -165
acc -3
acc +18
jmp +153
jmp +1
acc -10
acc -7
jmp -199
acc +30
nop -403
acc -12
jmp -209
jmp -242
acc +38
nop +33
acc -10
acc +22
jmp -419
acc -18
acc +27
acc +22
jmp -57
nop -313
acc +20
acc -7
acc -10
jmp -371
jmp -159
jmp -478
acc +9
acc +7
acc +15
nop +72
jmp -358
jmp -138
acc -17
jmp +9
acc +47
acc -2
jmp -221
nop -331
nop -297
acc +12
acc -13
jmp +3
jmp -198
jmp -150
acc +17
jmp -313
nop -314
jmp +69
acc +0
nop -397
jmp -104
jmp -223
acc -14
jmp +44
jmp -61
acc -7
acc -18
jmp -270
acc -14
acc +32
jmp -177
jmp +84
acc +6
nop +14
jmp +47
acc +37
acc -19
acc -9
jmp -200
acc +11
acc -5
acc +2
acc +37
jmp -488
nop +19
jmp -490
jmp -491
acc +24
acc +30
acc +14
jmp -19
jmp -37
acc +19
jmp -540
acc +48
acc +22
jmp -434
jmp -196
acc +12
acc -9
acc +48
acc -5
jmp -433
acc +23
jmp -245
acc +43
jmp -228
acc +44
jmp -168
nop -221
jmp -102
jmp +1
acc +39
nop -153
jmp -455
acc +48
jmp -75
jmp +31
nop -383
acc -12
jmp -245
acc -2
acc +3
jmp -421
acc +38
jmp -158
acc +39
acc -4
acc -1
acc +0
jmp -186
acc +28
jmp -247
jmp +1
acc -19
acc +31
acc +34
jmp -148
acc +5
nop -417
nop -230
acc +11
jmp -162
jmp +1
acc +32
jmp -303
nop -214
jmp -332
acc -10
acc +33
jmp -142
acc +19
acc +41
acc +12
jmp -495
acc +42
nop -318
acc +36
jmp -524
jmp +1
acc +46
acc -6
jmp -582
acc +28
acc +38
acc -17
acc +2
jmp -432
acc +35
nop -550
acc -6
jmp -394
acc +38
acc +49
nop -99
acc +50
jmp +1";
    }
}
