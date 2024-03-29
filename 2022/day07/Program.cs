﻿internal class FileSystem
{
    public Directory Root
    {
        get;
        private set;
    }

    public FileSystem()
    {
        Root = new Directory("/", null);
    }

    public static FileSystem Parse(string terminalOutput)
    {
        var result = new FileSystem();
        var currentDirectory = result.Root;
        var lines = terminalOutput.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            currentDirectory = parseLine(line, result, currentDirectory);
        }
        return result;
    }

    private static Directory parseLine(string line, FileSystem fileSystem, Directory currentDirectory)
    {
        if (line.StartsWith('$'))
        {
            return parseCommand(line.Substring(2), fileSystem, currentDirectory);
        }
        else if (line.StartsWith("dir"))
        {
            return parseDirectory(line.Substring(4), fileSystem, currentDirectory);
        }
        return parseFile(line, fileSystem, currentDirectory);
    }

    private static Directory parseFile(string line, FileSystem fileSystem, Directory currentDirectory)
    {
        var split = line.Split(' ');
        var size = int.Parse(split[0]);
        var name = split[1];
        currentDirectory.Files.TryAdd(name, new File(name, size));
        return currentDirectory;
    }

    private static Directory parseDirectory(string name, FileSystem fileSystem, Directory currentDirectory)
    {
        currentDirectory.Directories.TryAdd(name, new Directory(name, currentDirectory));
        return currentDirectory;
    }

    private static Directory parseCommand(string command, FileSystem fileSystem, Directory currentDirectory)
    {
        if (command.StartsWith("cd"))
        {
            var argument = command.Substring(3);
            return parseChangeDirectoryCommand(argument, fileSystem, currentDirectory);
        }
        // ignore ls command, as it doesn't really affect anything..
        return currentDirectory;
    }

    private static Directory parseChangeDirectoryCommand(string argument, FileSystem fileSystem, Directory currentDirectory)
    {
        switch (argument)
        {
            case "/":
                return fileSystem.Root;
            case "..":
                return currentDirectory.Parent;
            default:
                return currentDirectory.Directories[argument];
        }
    }
}

public class Directory
{
    public string Name { get; private set; }
    public Directory Parent { get; private set; }
    public Dictionary<string, Directory> Directories { get; } = new Dictionary<string, Directory>();
    public Dictionary<string, File> Files { get; } = new Dictionary<string, File>();

    public Directory(string name, Directory parent)
    {
        Name = name;
        Parent = parent;
    }

    public int GetSize()
    {
        return Directories.Sum(x => x.Value.GetSize()) + Files.Sum(x => x.Value.Size);
    }

    public IEnumerable<Directory> Descendants => RecursiveFlatten(Directories.Values); 

    private IEnumerable<Directory> RecursiveFlatten(IEnumerable<Directory> directories)
    {
        return directories.SelectMany(s => s.Directories.Any() ? RecursiveFlatten(s.Directories.Values).Union(new Directory[] { s }) : new Directory[] { s });
    }
}
public class File
{
    public string Name { get; private set; }
    public int Size { get; private set; }

    public File(string name, int size)
    {
        Name = name;
        Size = size;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2022, day 7");

        var fileSystem = FileSystem.Parse(testInput);
        var testAnswerPart1 = fileSystem.Root.Descendants
            .Where(x => x.GetSize() < 100000)
            .Sum(x => x.GetSize());
        Console.WriteLine($"Test answer part 1: {testAnswerPart1}");

        fileSystem = FileSystem.Parse(input);
        var answerPart1 = fileSystem.Root.Descendants
            .Where(x => x.GetSize() < 100000)
            .Sum(x => x.GetSize());
        Console.WriteLine($"Answer part 1: {answerPart1}");

        var totalDiskSpace = 70000000;
        var freeSpace = totalDiskSpace - fileSystem.Root.GetSize();
        var neededSpace = 30000000;
        var minSizeToDelete = neededSpace - freeSpace;
        var answerPart2 = fileSystem.Root.Descendants
            .Where(x => x.GetSize() > minSizeToDelete)
            .Min(x => x.GetSize());
        Console.WriteLine($"Answer part 2: {answerPart2}");
    }

    private static string testInput = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
    private static string input = @"$ cd /
$ ls
233998 glh.fcb
184686 jzn
dir qcznqph
dir qtbprrq
299692 rbssdzm.ccn
dir vtb
$ cd qcznqph
$ ls
32148 lhsrj.fnr
dir lnj
dir mtr
dir mznnlph
dir pdtpt
24836 rsjcg.lrh
dir vrj
dir wrqcfl
$ cd lnj
$ ls
12592 tlh
$ cd ..
$ cd mtr
$ ls
118870 twdhlmp.gbw
$ cd ..
$ cd mznnlph
$ ls
240977 fmmhnhtf
dir gbhcnts
dir gsbjrrd
dir pmwcs
dir qtbprrq
286007 rhnjndsq.gst
dir twdhlmp
283716 twdhlmp.rpr
$ cd gbhcnts
$ ls
dir fctrnwb
dir gbhcnts
46017 gft.hvm
234925 gjsnzbtw.ncd
dir nvnwh
dir srslsjp
dir swtlfsv
66115 tgpmsb
64086 tqnvb
308270 tqwfpnbn.btp
$ cd fctrnwb
$ ls
112643 qhcdd
$ cd ..
$ cd gbhcnts
$ ls
26196 cmttgsmm.bdn
317410 fthqln
dir lwshph
32809 tdmfc
dir tqcllnv
dir twdhlmp
$ cd lwshph
$ ls
214023 ctqvrzs.jvr
104432 gbch
dir gpqgrw
105909 qshbtd.nml
dir rhhsfbdd
dir svvqh
161439 tqnvb
60152 twdhlmp.qzw
$ cd gpqgrw
$ ls
dir mbsgrlld
dir nhb
dir qtbprrq
$ cd mbsgrlld
$ ls
13247 tsztmlfg
dir twdhlmp
$ cd twdhlmp
$ ls
236804 mcrd
$ cd ..
$ cd ..
$ cd nhb
$ ls
86570 gtvnbsv.zbr
$ cd ..
$ cd qtbprrq
$ ls
111178 npg.qph
110775 tlh
$ cd ..
$ cd ..
$ cd rhhsfbdd
$ ls
37729 fmmhnhtf
263415 ljvwzj.btm
$ cd ..
$ cd svvqh
$ ls
185682 wlcl.fhs
$ cd ..
$ cd ..
$ cd tqcllnv
$ ls
dir cbdj
dir ccsfm
55264 tqnvb
267792 wlcl.fhs
$ cd cbdj
$ ls
128247 fmmhnhtf
dir mtnbs
240520 ngmw.clj
30569 qbqltr.lbw
188801 zwdpp
$ cd mtnbs
$ ls
dir bsfbrmh
dir ftmnrwm
$ cd bsfbrmh
$ ls
dir tltvzp
$ cd tltvzp
$ ls
312469 dnst.sbm
$ cd ..
$ cd ..
$ cd ftmnrwm
$ ls
278974 nlztftc.zhb
$ cd ..
$ cd ..
$ cd ..
$ cd ccsfm
$ ls
4017 wlcl.fhs
$ cd ..
$ cd ..
$ cd twdhlmp
$ ls
dir qtbprrq
$ cd qtbprrq
$ ls
dir tdpz
$ cd tdpz
$ ls
210400 fmmhnhtf
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd nvnwh
$ ls
dir jlpbbds
dir pphv
285452 qtbprrq
$ cd jlpbbds
$ ls
7058 vmrcqz
$ cd ..
$ cd pphv
$ ls
290310 msz.swz
$ cd ..
$ cd ..
$ cd srslsjp
$ ls
dir nnz
192902 twdhlmp.vgp
$ cd nnz
$ ls
215711 tlh
$ cd ..
$ cd ..
$ cd swtlfsv
$ ls
274236 frwncp.gff
$ cd ..
$ cd ..
$ cd gsbjrrd
$ ls
dir dnst
dir gbhcnts
61000 gqdf
175813 jvz
dir ldqjzrtp
$ cd dnst
$ ls
124352 dnst
220618 mzsqzbfz.qfd
134211 qmrvh
dir qqlm
dir qtbprrq
223840 tlh
dir twdhlmp
24794 wfb.rtf
$ cd qqlm
$ ls
113976 wlcl.fhs
$ cd ..
$ cd qtbprrq
$ ls
212775 qtbprrq.ngs
$ cd ..
$ cd twdhlmp
$ ls
308083 fzhd
63311 wlcl.fhs
$ cd ..
$ cd ..
$ cd gbhcnts
$ ls
dir dlvhzdbg
$ cd dlvhzdbg
$ ls
305798 twdhlmp
$ cd ..
$ cd ..
$ cd ldqjzrtp
$ ls
93085 dcvfpz.bjl
264488 zssvm.wdp
$ cd ..
$ cd ..
$ cd pmwcs
$ ls
125444 qtbprrq.tgl
$ cd ..
$ cd qtbprrq
$ ls
dir bjnctfv
133127 fmmhnhtf
dir gztmrrff
dir qtbprrq
$ cd bjnctfv
$ ls
dir cpwrcf
dir fdjzsfc
1223 gbhcnts.qvf
272526 gbhcnts.sgs
dir qnsdl
dir snq
dir tmjnvcbl
dir vdjqsbr
271339 wslnqh.rgr
134589 zzqrbr.fcz
$ cd cpwrcf
$ ls
143124 pdr
$ cd ..
$ cd fdjzsfc
$ ls
dir gbhcnts
dir nqpbzvpq
$ cd gbhcnts
$ ls
151265 jrdvt.fcg
11872 tlh
$ cd ..
$ cd nqpbzvpq
$ ls
dir hpwhslq
27858 ljvwzj.prq
dir nzcnb
$ cd hpwhslq
$ ls
136646 bqgj.wvw
252823 ngmw.clj
137072 tqnvb
$ cd ..
$ cd nzcnb
$ ls
99882 twdhlmp.grg
$ cd ..
$ cd ..
$ cd ..
$ cd qnsdl
$ ls
8925 fmmhnhtf
dir mnzqwfnh
206990 vqgrhqgc
$ cd mnzqwfnh
$ ls
271442 bmztfjlc.lzr
$ cd ..
$ cd ..
$ cd snq
$ ls
25995 tqnvb
$ cd ..
$ cd tmjnvcbl
$ ls
dir gclzbvt
$ cd gclzbvt
$ ls
dir jtfddbs
$ cd jtfddbs
$ ls
10564 pdf.tsj
32415 tlh
$ cd ..
$ cd ..
$ cd ..
$ cd vdjqsbr
$ ls
256668 cwbd
265036 fmmhnhtf
$ cd ..
$ cd ..
$ cd gztmrrff
$ ls
52260 bdqcl.bdw
dir lsss
120102 tlh
$ cd lsss
$ ls
13729 wlcl.fhs
$ cd ..
$ cd ..
$ cd qtbprrq
$ ls
dir bttpq
dir lcvgwpt
$ cd bttpq
$ ls
216247 nnlv.dgl
138688 wlcl.fhs
$ cd ..
$ cd lcvgwpt
$ ls
dir dth
198570 tsqgm.zht
dir zbcstsb
$ cd dth
$ ls
dir cqmbtj
120437 hdqp.vhq
dir vpzn
$ cd cqmbtj
$ ls
11882 sdngnzb
$ cd ..
$ cd vpzn
$ ls
dir jqbz
271714 plcq.bfg
$ cd jqbz
$ ls
dir qqhnfglj
136307 stncbrm
177843 tlh
168253 tqnvb
297085 wcn
$ cd qqhnfglj
$ ls
197471 twdhlmp
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd zbcstsb
$ ls
298115 bvljmpc.gss
308872 ljr.lzl
201657 ngmw.clj
170617 ppln
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd twdhlmp
$ ls
dir dbb
277215 ngmw.clj
310263 twdhlmp.wvs
dir vsfrqsnl
$ cd dbb
$ ls
258300 tqnvb
$ cd ..
$ cd vsfrqsnl
$ ls
dir gbhcnts
12285 tlh
$ cd gbhcnts
$ ls
248251 dnst.bcs
91471 gbhcnts.ntr
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd pdtpt
$ ls
164477 flcgj.zwr
dir ljvwzj
51483 ljvwzj.htl
dir pbtr
dir qtbprrq
dir rrhcsn
$ cd ljvwzj
$ ls
dir nsq
133318 qtbprrq.gqq
166365 rnfbl.ljh
130617 tlh
16112 vbw
$ cd nsq
$ ls
dir fwfcmfbz
$ cd fwfcmfbz
$ ls
71451 zcc.ngn
$ cd ..
$ cd ..
$ cd ..
$ cd pbtr
$ ls
dir qtbprrq
$ cd qtbprrq
$ ls
117780 gjqbnrv.sdl
$ cd ..
$ cd ..
$ cd qtbprrq
$ ls
269746 dld
dir fcmbv
42544 mlzvd.vcw
165396 nbtlfm.vzq
dir sbtl
dir twdhlmp
$ cd fcmbv
$ ls
202047 wdzcrg.mcg
$ cd ..
$ cd sbtl
$ ls
dir dbcdf
dir fbz
dir lvz
dir ncnwbsdh
dir rft
23523 zphlfqf.phv
$ cd dbcdf
$ ls
dir dhdw
dir dvtjfhvm
182513 lclmdwr
63921 ngmw.clj
dir qqmddq
318020 tlh
dir twdwfj
83108 vmwlfdlf
121901 wlcl.fhs
$ cd dhdw
$ ls
dir qtbprrq
dir twdhlmp
dir wbllhmd
$ cd qtbprrq
$ ls
111984 fhc.tzm
$ cd ..
$ cd twdhlmp
$ ls
277414 fwfqbb.dpj
$ cd ..
$ cd wbllhmd
$ ls
dir dnst
dir jqz
dir lbdclnfb
dir ljvwzj
dir mzfdg
96340 ngmw.clj
dir twdhlmp
dir wmcfzznt
147877 zwgvvd
$ cd dnst
$ ls
310179 fmmhnhtf
243908 twdhlmp
$ cd ..
$ cd jqz
$ ls
94739 twdhlmp
$ cd ..
$ cd lbdclnfb
$ ls
112509 ljvwzj
$ cd ..
$ cd ljvwzj
$ ls
28274 bshlmj.lzc
84072 tlh
283462 twdhlmp.ccd
$ cd ..
$ cd mzfdg
$ ls
282099 hbbrjc.jff
63535 tlh
$ cd ..
$ cd twdhlmp
$ ls
283817 jltvl.tgl
$ cd ..
$ cd wmcfzznt
$ ls
294565 fmmhnhtf
$ cd ..
$ cd ..
$ cd ..
$ cd dvtjfhvm
$ ls
292813 qgmvm.fsg
$ cd ..
$ cd qqmddq
$ ls
11670 dnst.btd
241275 fmmhnhtf
196615 fpnmptm
dir nnzscbvw
dir qnrr
$ cd nnzscbvw
$ ls
250962 dflhdfz
$ cd ..
$ cd qnrr
$ ls
dir trzj
$ cd trzj
$ ls
36993 gbhcnts.rdh
273052 tlh
$ cd ..
$ cd ..
$ cd ..
$ cd twdwfj
$ ls
162470 hfdhmbcq.hwz
dir qtbprrq
dir scjzbdsz
2609 wlcl.fhs
$ cd qtbprrq
$ ls
dir cfmglhwj
103703 cscftrsr.jbs
71160 dnst.rbw
dir nrmp
311716 qtbprrq
$ cd cfmglhwj
$ ls
dir fmcmjfg
$ cd fmcmjfg
$ ls
82998 ljvwzj.qbd
8407 nhmmwwzl
dir qtbprrq
261949 tlh
$ cd qtbprrq
$ ls
314421 hwqtl
92593 zcdvf
$ cd ..
$ cd ..
$ cd ..
$ cd nrmp
$ ls
94387 fmmhnhtf
$ cd ..
$ cd ..
$ cd scjzbdsz
$ ls
6861 dgzhldd.dhs
dir gbhcnts
dir qtbprrq
dir sfdl
$ cd gbhcnts
$ ls
dir qdsrs
$ cd qdsrs
$ ls
25165 ngmw.clj
$ cd ..
$ cd ..
$ cd qtbprrq
$ ls
151403 tswd.hpf
$ cd ..
$ cd sfdl
$ ls
308622 jcmsnj
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd fbz
$ ls
dir dgjf
dir qtbprrq
$ cd dgjf
$ ls
254198 rvf.hfq
$ cd ..
$ cd qtbprrq
$ ls
dir frlj
231222 njjfqgt.bph
dir rjsw
dir vjhzc
$ cd frlj
$ ls
dir ljvwzj
$ cd ljvwzj
$ ls
57572 ljvwzj.bvh
$ cd ..
$ cd ..
$ cd rjsw
$ ls
131875 lbcq.rlc
272908 mnfs
$ cd ..
$ cd vjhzc
$ ls
279363 fmmhnhtf
238051 zdzbb.rfj
$ cd ..
$ cd ..
$ cd ..
$ cd lvz
$ ls
289192 tqnvb
dir twdhlmp
$ cd twdhlmp
$ ls
dir wqtgwzdn
$ cd wqtgwzdn
$ ls
283475 ghvpfl
$ cd ..
$ cd ..
$ cd ..
$ cd ncnwbsdh
$ ls
dir dfrdwfgm
dir ljvwzj
dir vgh
$ cd dfrdwfgm
$ ls
279286 mrbwmws.nzd
197337 nqgq.fhf
248096 tqs.jfb
35181 wlcl.fhs
$ cd ..
$ cd ljvwzj
$ ls
250455 gmph.scm
147449 ljvwzj
100189 qfr
$ cd ..
$ cd vgh
$ ls
244540 bzwrldnz.ldt
235508 dzm
dir gbhcnts
dir qtv
dir tvtwlt
262356 wlcl.fhs
$ cd gbhcnts
$ ls
160689 srvpbf.szt
191895 tqnvb
$ cd ..
$ cd qtv
$ ls
9491 dnst.szf
268602 ngmw.clj
dir pbcrfzz
39049 rzgqqvlt.nsm
dir tfpl
79589 wwcrv.ncv
$ cd pbcrfzz
$ ls
dir stt
256685 wlcl.fhs
$ cd stt
$ ls
12650 jbdfwj
$ cd ..
$ cd ..
$ cd tfpl
$ ls
92079 dfhj
$ cd ..
$ cd ..
$ cd tvtwlt
$ ls
dir cqv
$ cd cqv
$ ls
dir vdv
$ cd vdv
$ ls
119483 fmmhnhtf
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd rft
$ ls
24341 bjhzvzp.flg
dir glwdmdt
$ cd glwdmdt
$ ls
288082 jdtlwrzh.wcj
$ cd ..
$ cd ..
$ cd ..
$ cd twdhlmp
$ ls
dir gbhcnts
154240 wlcl.fhs
$ cd gbhcnts
$ ls
217462 ddzp
$ cd ..
$ cd ..
$ cd ..
$ cd rrhcsn
$ ls
308440 dzbfl.vcg
dir jbhcpdh
238941 rnqdz
dir szljjhc
$ cd jbhcpdh
$ ls
dir bmg
dir mdqplln
dir twdhlmp
dir zbt
$ cd bmg
$ ls
dir djwfl
dir gbhcnts
dir ljvwzj
142159 mwl.psh
110681 rzmdgbng
dir zqjbb
$ cd djwfl
$ ls
dir dpfcrjl
dir rqtz
$ cd dpfcrjl
$ ls
206939 tlh
$ cd ..
$ cd rqtz
$ ls
232264 tlh
$ cd ..
$ cd ..
$ cd gbhcnts
$ ls
186364 ngmw.clj
248882 twdhlmp
306411 wjqvlzp
$ cd ..
$ cd ljvwzj
$ ls
dir dgqw
$ cd dgqw
$ ls
dir mpczlcrz
dir qtbprrq
dir twdhlmp
dir zjsltthh
$ cd mpczlcrz
$ ls
142906 gvd.nnz
$ cd ..
$ cd qtbprrq
$ ls
179566 fmmhnhtf
309800 jhwwppc.vcp
$ cd ..
$ cd twdhlmp
$ ls
dir bhqjhjvp
$ cd bhqjhjvp
$ ls
dir lmj
dir qmcqggbl
$ cd lmj
$ ls
275070 twdhlmp
$ cd ..
$ cd qmcqggbl
$ ls
dir mhgnpm
$ cd mhgnpm
$ ls
dir rnzzqr
$ cd rnzzqr
$ ls
126574 pgnlrjs.czj
7567 tqnvb
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd zjsltthh
$ ls
dir twdhlmp
$ cd twdhlmp
$ ls
198813 dnst.cqc
$ cd ..
$ cd ..
$ cd ..
$ cd ..
$ cd zqjbb
$ ls
dir czdvd
94020 dnst
46041 qtbprrq.pzm
dir rcfvq
dir rwj
118305 vbcpcz
48725 wlcl.fhs
$ cd czdvd
$ ls
302317 tlf
$ cd ..
$ cd rcfvq
$ ls
dir cjws
$ cd cjws
$ ls
dir dsgf
dir fvqbhq
203941 hgcbcvb
9562 qqjh.mfh
32161 qtbprrq.tgn
225251 sbmpn
dir sdhvcj
$ cd dsgf
$ ls
dir cbwzg
141466 ctpszzvn.qrq
277153 ngmw.clj
100681 vmdwgrp
$ cd cbwzg
$ ls
dir nblvrbv
$ cd nblvrbv
$ ls
129474 dlcbng.sgf
$ cd ..
$ cd ..
$ cd ..
$ cd fvqbhq
$ ls
75755 fmmhnhtf
229463 tlh
$ cd ..
$ cd sdhvcj
$ ls
306751 tqnvb
$ cd ..
$ cd ..
$ cd ..
$ cd rwj
$ ls
130415 cjbz
283701 rgsdtn
$ cd ..
$ cd ..
$ cd ..
$ cd mdqplln
$ ls
169404 dvss.mvd
105385 fmmhnhtf
222834 jhzpwscp.sqg
164293 jsqlprqn.vnp
57167 pwpjfq.bmb
dir qtbprrq
$ cd qtbprrq
$ ls
62823 ljvwzj.flm
252940 tlh
$ cd ..
$ cd ..
$ cd twdhlmp
$ ls
dir dhvgfhc
dir qrlq
$ cd dhvgfhc
$ ls
dir vpldlp
$ cd vpldlp
$ ls
279067 dnst.jfs
9050 fmmhnhtf
88586 mfbj.fgs
$ cd ..
$ cd ..
$ cd qrlq
$ ls
dir qwwftl
$ cd qwwftl
$ ls
103153 tnczww
$ cd ..
$ cd ..
$ cd ..
$ cd zbt
$ ls
99657 fsq.rzj
158138 gbfjfctj.bgg
260423 tqnvb
161379 trg
$ cd ..
$ cd ..
$ cd szljjhc
$ ls
287080 stnp.lgp
173682 wjzvglm.lfm
$ cd ..
$ cd ..
$ cd ..
$ cd vrj
$ ls
129084 ngmw.clj
250696 pdpzzbs
$ cd ..
$ cd wrqcfl
$ ls
dir bjlwb
105899 gsvm
dir jdnjpg
178665 znnmmhqt.hth
$ cd bjlwb
$ ls
207939 gbhcnts
$ cd ..
$ cd jdnjpg
$ ls
260418 tqnvb
302144 twdhlmp.ghg
$ cd ..
$ cd ..
$ cd ..
$ cd qtbprrq
$ ls
95562 fmmhnhtf
dir plf
dir qtbprrq
306396 rqqmm.wvw
dir wpfj
$ cd plf
$ ls
dir fmftrbn
20347 twb.zjd
$ cd fmftrbn
$ ls
dir rfznrm
$ cd rfznrm
$ ls
283327 rlzjcg
$ cd ..
$ cd ..
$ cd ..
$ cd qtbprrq
$ ls
313931 ztmhrjc
$ cd ..
$ cd wpfj
$ ls
3969 wrbhb.jll
$ cd ..
$ cd ..
$ cd vtb
$ ls
14260 fmmhnhtf
dir gbhcnts
dir lwcznw
dir mhp
dir pqcddzsf
272267 qgh
301727 rsjrn.wjg
101787 vqscjb
dir zvn
$ cd gbhcnts
$ ls
7627 tqnvb
$ cd ..
$ cd lwcznw
$ ls
98498 dnst.tds
dir gfh
dir jdg
dir llnl
161511 mtmrr.hvb
dir ppzwbgnz
210908 qtbprrq
dir tvhz
$ cd gfh
$ ls
169547 mjjvvlqj.jmv
$ cd ..
$ cd jdg
$ ls
dir cthptwcf
dir ljvwzj
dir vnlndl
$ cd cthptwcf
$ ls
98711 qwzwz.qct
$ cd ..
$ cd ljvwzj
$ ls
245473 zhptcmcr.fts
$ cd ..
$ cd vnlndl
$ ls
151466 ljvwzj
285091 twdhlmp.mzv
59067 vcdpbg.nmp
$ cd ..
$ cd ..
$ cd llnl
$ ls
141508 phtmjj.qzl
dir qtbprrq
105151 tlh
$ cd qtbprrq
$ ls
62020 hdzljht.fvq
$ cd ..
$ cd ..
$ cd ppzwbgnz
$ ls
298940 pzdqzrn.zlz
$ cd ..
$ cd tvhz
$ ls
96628 hrzr
$ cd ..
$ cd ..
$ cd mhp
$ ls
226604 mbdn.tbq
dir ndgqtvhg
$ cd ndgqtvhg
$ ls
55244 dnst
dir sljbrmhb
$ cd sljbrmhb
$ ls
32711 dnst
$ cd ..
$ cd ..
$ cd ..
$ cd pqcddzsf
$ ls
dir shwrrq
$ cd shwrrq
$ ls
dir dplcwvhg
dir pvtpf
dir qpsmgfjl
247965 rrw.wwv
dir vmrwpt
$ cd dplcwvhg
$ ls
242534 fmmhnhtf
202367 fzmt.qrw
197586 ljvwzj.qgm
dir stp
dir zpz
$ cd stp
$ ls
12921 mlcqtthb.jtd
$ cd ..
$ cd zpz
$ ls
235965 ngmw.clj
$ cd ..
$ cd ..
$ cd pvtpf
$ ls
319563 rdspj.slv
279577 vqpjzrdl.hhj
$ cd ..
$ cd qpsmgfjl
$ ls
131841 cqhrgc.cqz
105373 fbnp
$ cd ..
$ cd vmrwpt
$ ls
176373 phgsdlnj.ggq
$ cd ..
$ cd ..
$ cd ..
$ cd zvn
$ ls
dir gbhcnts
dir gfh
dir ppqjzln
dir qtbprrq
$ cd gbhcnts
$ ls
156292 wlcl.fhs
$ cd ..
$ cd gfh
$ ls
189836 ljvwzj.wpt
10416 zbnhzjvw.jct
$ cd ..
$ cd ppqjzln
$ ls
95088 sszd
$ cd ..
$ cd qtbprrq
$ ls
295187 hnnl
292421 qtbprrq.ppg
220281 wlcl.fhs";
}