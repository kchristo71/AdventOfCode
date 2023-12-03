using System.Text.RegularExpressions;

namespace _20231202._1
{
    internal class Program
    {
        private static readonly IDictionary<CubeColor, int> CubeColorAvailability = new Dictionary<CubeColor, int> {
            { CubeColor.Red, 12 },
            { CubeColor.Green, 13 },
            { CubeColor.Blue, 14 }
        };

        private const string GameRegEx = "^Game\\s(?<gameId>[\\d].*):";
        private const string MarbleCountRegEx = "^(?<cubeCount>[\\d]{1,})\\s(?<cubeColor>red|green|blue)$";

        static void Main(string[] args)
        {
            var power = !(args.Any() && (args[1] == "2"));
            long total = 0;
            int gameId = 0;
            using (var puzzleInputStream = typeof(Program).Assembly.GetManifestResourceStream($"{typeof(Program).Namespace}.PuzzleInput.txt"))
            using (var inputReader = new StreamReader(puzzleInputStream))
            {
                var currentLine = inputReader.ReadLine();
                Dictionary<CubeColor, int> GameColorAvailability = new Dictionary<CubeColor, int>()
                {
                    { CubeColor.Red, 0 },
                    { CubeColor.Green, 0 },
                    { CubeColor.Blue, 0 }
                };
                while (currentLine != null)
                {
                    var originalLine = currentLine;
                    var gameMatches = Regex.Matches(currentLine, GameRegEx).FirstOrDefault().Groups.Values;
                    gameId = int.Parse(gameMatches.Last().Value);
                    currentLine = currentLine.Replace(gameMatches.First().Value, "");
                    var cubeCounts = currentLine.Split(',', ';').Select(s => s.Trim());
                    bool success = true;
                    foreach (var cubeCount in cubeCounts)
                    {
                        var cubeMatches = Regex.Matches(cubeCount, MarbleCountRegEx).FirstOrDefault().Groups.Values;
                        var cubeColor = Enum.Parse<CubeColor>(cubeMatches.Last().Value, true);
                        var cubeItems = int.Parse(cubeMatches.Skip(1).First().Value);
                        if (!power)
                        {
                            if (CubeColorAvailability[cubeColor] < cubeItems)
                            {
                                success = false;
                                break;
                            }
                        }
                        else
                        {
                            if (GameColorAvailability.ContainsKey(cubeColor) && GameColorAvailability[cubeColor] < cubeItems)
                            {
                                GameColorAvailability[cubeColor] = cubeItems;
                            }
                        }
                    }
                    if (!power)
                    {
                        total += success ? gameId : 0;
                    }
                    else {
                        total +=
                            (GameColorAvailability.ContainsKey(CubeColor.Red) ? GameColorAvailability[CubeColor.Red] : 1) *
                            (GameColorAvailability.ContainsKey(CubeColor.Blue) ? GameColorAvailability[CubeColor.Blue] : 1) *
                            (GameColorAvailability.ContainsKey(CubeColor.Green) ? GameColorAvailability[CubeColor.Green] : 1);
                    }
                    GameColorAvailability.ToList().ForEach(availability => GameColorAvailability[availability.Key] = 0);
                    currentLine = inputReader.ReadLine();
                }
            }
            Console.WriteLine(total);
        }
    }
}