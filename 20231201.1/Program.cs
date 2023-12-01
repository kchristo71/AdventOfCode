using System.Runtime.CompilerServices;

namespace _20231201._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Dictionary<string, int> calibrationCodes = new Dictionary<string, int>();
#endif
            long total = 0;
            using (var puzzleInputStream = typeof(Program).Assembly.GetManifestResourceStream($"{typeof(Program).Namespace}.PuzzleInput.txt"))
            using (var inputReader = new StreamReader(puzzleInputStream))
            {
                var currentLine = inputReader.ReadLine();
                while (currentLine != null)
                {
                    var currentCode = -1;
                    currentLine.ToList().ForEach(character =>
                    {
                        if (Char.IsNumber(character))
                        {
                            if (currentCode == -1)
                            {
                                currentCode = (int)Char.GetNumericValue(character) * 10;
                            }
                            currentCode = (currentCode - currentCode % 10) + (int)Char.GetNumericValue(character);
                        }
                    });
#if DEBUG
                    calibrationCodes.Add(currentLine, currentCode);
#endif
                    total += currentCode;
                    currentLine = inputReader.ReadLine();
                }
            }
            Console.WriteLine(total);
#if DEBUG
            Console.WriteLine(string.Join(Environment.NewLine, calibrationCodes.Select(code => $"{code.Value}: {code.Key}")));
#endif
        }
    }
}