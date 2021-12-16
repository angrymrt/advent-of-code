﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace day15
{
    public class Node
    {
        public Path ShortestPath { get; set; }
        public int Value { get; }
        public int X { get; }
        public int Y { get; }
        public Node[][] Map { get; }
        public Node Up => X == 0 ? null : Map[X - 1][Y];
        public Node Down => X == Map.Length - 1 ? null : Map[X + 1][Y];
        public Node Left => Y == 0 ? null : Map[X][Y - 1];
        public Node Right => Y == Map[X].Length - 1 ? null : Map[X][Y + 1];
        public IEnumerable<Node> Neighbours => new Node[] { Up, Down, Left, Right }.Where(x => x != null);
        public int DistanceToBottomRight => (int)Math.Sqrt(Math.Pow(DistanceToBottom, 2) + Math.Pow(DistanceToRight, 2));
        public int DistanceToBottom => (Map.Length - 1) - X;
        public int DistanceToRight => (Map[0].Length - 1) - Y;

        public Node(int value, int x, int y, Node[][] map)
        {
            Value = value;
            X = x;
            Y = y;
            Map = map;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override string ToString()
        {
            return $"[{X},{Y}]:{Value}";
        }
    }
    public class Path
    {
        public LinkedList<Node> Nodes { get; }
        public int Risk { get; private set; }
        public int Score { get; private set; }
        public bool HasReachedEnd { get; private set; }

        public Path(Node start)
        {
            Nodes = new LinkedList<Node>();
            Nodes.AddFirst(start);
        }
        public Path(Path copyFrom)
        {
            Nodes = new LinkedList<Node>(copyFrom.Nodes);
            Risk = copyFrom.Risk;
            Score = copyFrom.Score;
        }

        public int CalculateScore(Node next) {
            return Risk + next.DistanceToBottomRight;
        }
        public void Add(Node next)
        {
            Nodes.AddLast(next);
            Risk += next.Value;
            Score = CalculateScore(next);
            if (next.DistanceToBottomRight == 0)
            {
                HasReachedEnd = true;
            }
        }
    }
    public static class PathFinder
    {
        public static Path GetShortestPath(Node[][] map)
        {
            var paths = new List<Path>();
            paths.Add(new Path(map[0][0]));
            var currentPath = paths.First();
            while (currentPath?.HasReachedEnd == false)
            {
                paths.Remove(currentPath);
                var node = currentPath.Nodes.Last();
                var neighbours = node.Neighbours.ToArray();
                for (var i = 0; i < neighbours.Length; i++)
                {
                    if (neighbours[i].ShortestPath == null
                        || neighbours[i].ShortestPath.Risk > currentPath.Risk + neighbours[i].Value)
                    {
                        neighbours[i].ShortestPath = new Path(currentPath);
                        neighbours[i].ShortestPath.Add(neighbours[i]);
                        paths.Add(neighbours[i].ShortestPath);
                    }
                }
                currentPath = paths.OrderBy(x => x.Risk).First();
            }
            return currentPath;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 15!");
            var sw = new Stopwatch();
            long answer = 0;
            var useTestInput = true;
            var lines =
                (useTestInput ? testInput : input)
                    .Split(Environment.NewLine)
                    .Select(x => x.Select(c => int.Parse(c.ToString())).ToArray())
                    .ToArray();

            // Start part 1.
            sw.Start();
            var map = new Node[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                map[i] = new Node[lines[i].Length];
                for (var j = 0; j < lines[i].Length; j++)
                {
                    map[i][j] = new Node(lines[i][j], i, j, map);
                }
            }
            var shortestPath = PathFinder.GetShortestPath(map);
            if (useTestInput)
            {
                foreach (var node in shortestPath.Nodes)
                {
                    Console.WriteLine(node);
                }
            }
            answer = shortestPath.Risk;
            sw.Stop();

            Console.WriteLine("Answer to part 1: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");


            // Start part 2.
            sw.Restart();
            var height = lines.Length;
            var width = lines[0].Length;
            map = new Node[lines.Length * 5][];
            for (int i = 0; i < height * 5; i++)
            {
                map[i] = new Node[width * 5];
                for (var j = 0; j < width * 5; j++)
                {
                    var iMod = i % height;
                    var jMod = j % width;
                    var addition = (i / height) + (j / width);
                    var value = lines[iMod][jMod] + addition;
                    if (value > 9) value = value - 9;
                    map[i][j] = new Node(value, i, j, map);
                }
            }
            shortestPath = PathFinder.GetShortestPath(map);
            if (useTestInput)
            {
                foreach (var node in shortestPath.Nodes)
                {
                    Console.WriteLine(node);
                }
            }
            answer = shortestPath.Risk;
            sw.Stop();

            Console.WriteLine("Answer to part 2: " + answer + " (" + sw.ElapsedMilliseconds + "ms, " + sw.ElapsedTicks + " ticks)");
        }

        public static string testInput = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";
        public static string input = @"7715611621811483411121929153345417926121654738244558184333228256741518593939121351941172244538819793
5978192582116933121967117233221363821218169348147142177193168221219699236451313112198933914323123492
3523162115513315258921111444182316618172925755923799136271191151813516522171176112647162541858173363
7931931461145231897547234134741682994126523123999252325119362919742719127391146114344331291116971777
3912157144212332298711374454411326743116179131395281613188171189221945922126933817119198199326455346
2219143418191822913131432991534835872531239119117118416185912111192962531111319326176629318111185111
9116687994992138729148521124241932317616396617951124297314235138854797168842416251775729287611967414
7851151594489117347687963146449123171411231791678721232616848894581346355912616826231169619235952732
2851861191919753472141651266114831227654463284483498521612827841418971981411931436113213396426946112
6712388671398963172123321653869646551586461492219377899341274966231951164199917165932561616983929924
3677326111792395522936119111175266111868435913873231478188182951214919133631271356331241911265195215
2411238217381994191274413116991994522732518981599292983991998111631182731112386631322271192212411843
1924296312792281346181932776933796211556871296835132761484192932264311663412971542122254194197225125
5193911629511224247133119192711512118326922142611261829118422127224838641841967498792466123922241611
3518421332332225373982119933189183848164192326315196127257111819966291868392125511182719699513213126
1243423697719296145178212414496848356131291919119337192521512383952691111818938552679591331194252815
1114134841622115582771529171517131623995521325269361158192181119511123926429217441181915118546111129
3413463671141433362959518115315279211752711714921711141946919111126196926192311117361329611368827123
1372787612785741712553922962223999622392124621931191746293231951121499351641599723992929211182183521
1646324114562413462565234121468141622211633146398136728423519196521422661392274137424189885124111979
8593927754728491812319341327322619591487122534343481851326173126244497617119592817617578171431611211
3544996545112122938226413169257256114389524779177813732798495971419433331931141513863257159222211327
1317112599316491522993299211918262655114758519194159564179393912978824213816415221134382876196791131
2992415411131212997526331872111112996344865592792815133565817171814172837534233288248214812739937128
7399281754443127946141431876481961612345132118851715323452891116597132773351188234354812269914136314
1353222121126771342784863118971948121131151269737693294597543471364852531536931711275612918142912339
1231143322424173511217841979211499894524361726733118828862941949323516193769441834412128333517121219
4153113123817117759341379297738315498194824968743168793739217211372842125911549919617297111441613631
9731976338891216952882213198614419772196764362991958541418412247316741111113414519698991177561491619
1549968122497536224817279413173193149141335731431114111191969373537193295361951832648538349971115651
3141199162911153239685453949619182151714537212236218628247835892234915714197151176914515494168817613
2321634214222164696761674371984136262645585522114118771793891255516343661843273111241429184252481112
1323271335941123627134295288123814147892434111122433863111883333942311312165717429336197351684297515
1799383479828119861997991252537621715111112211243118199888781621163257948541164311576771111337441297
3474558728714311424212121852197696651899775852781115664112735924716128287452112718142116441487711179
1212211282414612598188336251113541918828321262911122168152739929958181711329326391542161816879229457
9369141739122111275981911719178226989791269152711118514211312351951888312115211119912146165111417274
1557942397217218772114458897214197121496239971221799298121731791112838411839164943531148113513822329
2116511928331138153519713938955192461891971295394211113118549975869129341915148999142791221429844338
1216615164189449189844291299217862321339427481291811189889121368125354184186711881158278233714798812
2131462662132519141953391411117681836144434719816111765876594111661972132119291718521288742511921812
1352178182113553243431145717611728111429117626322671124512968719613828222757811142112237112915916411
9111861843583342173156761182486951611273911221233459621981815173222733765922411929621144273182167173
6461614968141185513751845166496129581174136991312882746411333574211232145145411259791494747636513831
8395826326831481781816141231151946694412171233118734118443712934188141261953428112498397283833996953
3224325116954176581173265389433772885221125196135137181681253382516579111971122321919261361455573426
5612711314828933927115772517317242878121717862126961211324793885131324871781627863678621349344985162
9219113183181524712318355994194342313255174114168321236494161659235237996744938323416519175154774368
1455219173629652263128124278132571923466188331613694112619643242969214341411691592236981126229531246
9997212872235551519294812463451816576987351769737341319699248495132218138849526591159541452141311227
9439551788914742428219371619927339189298769319132111753411511544151872351486489786185314584942895521
4623921912121114189211698941781144517423493925244616879259454219595112526216227151411539965913122341
9233881249291833812284319142291868129327632421241354425437738985391254261131535339322529281581561495
6621816747917191382495161833567118397773372129721681832591214252274233112632684542595451911924821133
1961985912891945611341231799388123719733816334154627934522422918536573253556769424412919116426182323
8613914491619713822131419117143163659542154519169992171969135131941141557529457324123161951111765294
4268219915611518484714151351482241221275914371738124149239386327831219359136247361161375174224223839
2417628752221923911251535153892648194748229231761133182371314159657511173541128258975114145282143131
1149132123898111199247912341113411962434312877836882119771691759571972141321747138787951211139182122
8589114128711558917336937119159115193482491549524442733618822133531142619542652131192519259679572118
8417549457115646563131638317315161112292939814589243166226251236828614439212129983319595172328298496
7132261193213112392744229915239441239812134161721522541289116993632721981519211498478878212229136195
5284114931928992331114192299645884318993264292231979919528343172899594924111575491982184914671211943
4138427142832314543311113423227271112997816549131893391237171313241261242672582112128835619821371121
4918821233299337972289281128133411678349812271421157479832992298658193248154113222111521996724714323
9174191481421563857184797521172297912449121127431641255195914981149724121258184197136514813734273234
4112626214511388118211446512388876694313187965929168166814111113382224115821767413172111376427638393
3231154459742912972641231118291138185681466529142819182885451441118191771984959111111623683628161113
5841171953222465917336112439651628888517153249964599148991263916813419222189131513441336935294361738
1816982764214991569342131547714487273197791551197149221141426622298772319696223747144351291172292314
9475574461545913358168363336339111314873371711345178219127111194927223149613171931929322893211152758
4858113211241899116852635135769311112114182724528552311872195867129114349887193381564631182288641431
9742923193935298412422519924115919956189443126116429369399671331114348991891115412454184715126299998
9121814433419188117111757994391996912918393911152421324919462957262487176581182129438199211342711241
9152171557731172939156141661228213792961792411377162925187423286231921473559751361928233131516412191
7288111938512228994743624943311448872522112536176928681443419899213241329198212953522514116811911197
9349832181421346375192594111125921597518122235122992135833853191479358257121815429654613383832118393
5787987676213718162591455312614145484267395142381485479118249352234818169546988191111121531915975846
1593328873913943873678371287612191117251139167998756142661344997618573521246115295381218111199353123
7792811294182231635163292331735316144325221519124512326112978344515322371153671145111948211314111961
2318518221984644229891593834523191322527124556185113492672653928784664122218293847186517161551631851
1112829983141931112651721133161412432143348121156512484214145542131639818492551211129611431998577181
3592613729447389151192925815192425191916611234864255478391561193394996337454189464938128832119348811
1271921732441182191447179913999221292183221141274914288646297111212268851116317378213664353119951955
1952724641973599331362393291821311217123755111853629765725951319311313197419922547371189952219569671
9175174929918626863191261492871635282719323944341514814391991254113352393192263965371316169319756114
7796162132715959911612986171614281934119795141614191521216692472181723351815721531573126767456111621
1929441344612524315169915972256329374194651851188791336975193211635164981382125948798143115991218147
3196918124398432195448219783729332721415743182553947677319291263569111194212214141296123149311195918
3158114419662114389969334121776231616687394316562465952127448362341922599962916219531841316418231911
7689421696457392537729119321129879691967715569123191226313319112314943111466159111416157881113611939
7831513935396926282212738725323211942958912226993621996662133721199282226293728546453712751822771285
2123699211953181196938911293262179311281131944194219219814973246548631519123711749121712622686521314
5191342118926287865138111129119915719811398331646995511667311198221618374236169145912891632927912111
8214442493117223193442175221522611659488871176512331552482329112125141241921222999759248511345262843
1914183724394277228911926796911512739125432271261643511196439716723711131491717732211162514393511298
1119745958685726562282127671296828241734389837168591468985891281128335925991724159212218314375287914
9517519684395161899187861919321914215994991181859919191641351832193212127643354718852685876126577315
3342917922251914252338185581691191842316989111841311466776912683629971312998965551711141287138724241
8176221727998313186834559714891971413547985825489911732617146966111238282644811394895278181837711185";
    }
}
