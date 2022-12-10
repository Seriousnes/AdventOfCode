//using AdventOfCode2022.Day.Seven;

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Execution._2022;

public class Day7 : AdventOfCodeExecutionBase
{
    public Day7(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"$ cd /
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
7214296 k", 95437)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var home = GetDirectoryStructure(input.Split(Environment.NewLine));
        GetDirsWithMaxSize(home, 100000).Sum(x => x.Size).Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"$ cd /
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
7214296 k", 24933642)]
    public void Part2_Validation(string input, int expectedValue)
    {
        var lines = input.Split(Environment.NewLine);
        var diskspace = 70000000;
        const int minSpaceRequired = 30000000;

        var home = GetDirectoryStructure(lines);
        var allDirs = Flattern(home);
        allDirs.Remove(home);

        diskspace -= home.Size;

        var dirToDelete = allDirs.Where(x => diskspace + x.Size > minSpaceRequired).OrderBy(x => x.Size).First();
        dirToDelete.Size.Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var home = GetDirectoryStructure(lines);
            return GetDirsWithMaxSize(home, 100000).Sum(x => x.Size);
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var diskspace = 70000000;
            const int minSpaceRequired = 30000000;

            var home = GetDirectoryStructure(lines);
            var allDirs = Flattern(home);
            allDirs.Remove(home);

            diskspace -= home.Size;

            var dirToDelete = allDirs.Where(x => diskspace + x.Size > minSpaceRequired).OrderBy(x => x.Size).First();
            return dirToDelete.Size;
        });
    }    

    private Dir GetDirectoryStructure(string[] input)
    {
        var home = new Dir { Name = @"/" };
        Dir currentDir = null;
        string[] cmd = null;
        foreach (var line in input)
        {
            if (line.StartsWith("$")) // command
            {
                cmd = line.Substring(2).Split(' ');
            }

            if (cmd is not null)
            {
                switch (cmd[0])
                {
                    case "cd":
                        currentDir = cmd[1] switch
                        {
                            ".." => currentDir.Parent,
                            "/" => home,
                            _ => currentDir.Contents.OfType<Dir>().SingleOrDefault(x => x.Name == cmd[1]) ?? new Dir { Name = cmd[1], Parent = currentDir }
                        };
                        cmd = null;
                        break;
                    case "ls":
                        if (line.StartsWith("$")) continue;
                        var lsInfo = line.Split(' ');

                        if (lsInfo[0] == "dir")
                        {
                            currentDir.Contents.Add(new Dir { Name = lsInfo[1], Parent = currentDir });
                        }
                        else
                        {
                            currentDir.Contents.Add(new Item { Name = lsInfo[1], Parent = currentDir, Size = int.Parse(lsInfo[0]) });
                        }

                        break;
                }
            }
        }

        return home;
    }

    private List<Dir> GetDirsWithMaxSize(Dir initial, int maxSize)
    {
        var result = new List<Dir>();
        foreach (var dir in initial.Contents.OfType<Dir>())
        {
            if (dir.Size < maxSize)                
                result.Add(dir);
            result.AddRange(GetDirsWithMaxSize(dir, maxSize));
        }

        return result;
    }

    private List<Dir> Flattern(Dir initial)
    {
        var result = new List<Dir>(new[] { initial });
        foreach (var dir in initial.Contents.OfType<Dir>())
            result.AddRange(Flattern(dir));
        return result.Distinct().ToList();
    }
}


public interface IDirItem
{
    string Name { get; }
    Dir Parent { get; }
    int Size { get; }
}

[DebuggerDisplay("{Name} (file, size={Size})")]
public class Item : IDirItem
{
    public required string Name { get; set; }
    public required Dir Parent { get; set; }
    public required int Size { get; set; }
}

[DebuggerDisplay("{Name} (dir, size={Size})")]
public class Dir : IDirItem
{
    public string Name { get; set; }
    public Dir Parent { get; set; }
    public List<IDirItem> Contents { get; set; } = new();
    public int Size => Contents.Sum(x => x.Size);
}