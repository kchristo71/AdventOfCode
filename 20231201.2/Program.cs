namespace _20231201._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> numberNames = new Dictionary<string, string>
            {
                { "one"  ,"1" },
                { "two"  ,"2" },
                { "three","3" },
                { "four" ,"4" },
                { "five" ,"5" },
                { "six"  ,"6" },
                { "seven","7" },
                { "eight","8" },
                { "nine" ,"9" }
            };
#if DEBUG
            Dictionary<string, int> calibrationCodes = new Dictionary<string, int>();
            string originalLine;
#endif
            long total = 0;
            string[] names = numberNames.Keys.ToArray();
            List<char> nameInitials = names.Select(name => name[0]).Distinct().ToList();
            using (var puzzleInputStream = typeof(Program).Assembly.GetManifestResourceStream($"{typeof(Program).Namespace}.PuzzleInput.txt"))
            using (var inputReader = new StreamReader(puzzleInputStream))
            {
                var currentLine = inputReader.ReadLine();
                while (currentLine != null)
                {
                    var currentCode = -1;
                    originalLine = currentLine;
                    for (int characterIndex = 0; characterIndex < currentLine.Length; characterIndex++)
                    {
                        var character = currentLine[characterIndex];
                        if (Char.IsNumber(character))
                        {
                            if (currentCode == -1)
                            {
                                currentCode = (int)Char.GetNumericValue(character) * 10;
                            }
                            currentCode = (currentCode - currentCode % 10) + (int)Char.GetNumericValue(character);
                        }
                        else if (nameInitials.Contains(character))
                        {
                            bool replaced = false;
                            names
                                .Where(name => name[0] == character)
                                .ToList()
                                .ForEach(numberName =>
                                {
                                    if (replaced) return;
                                    if (Enumerable.SequenceEqual(currentLine.Skip(characterIndex).Take(numberName.Length), numberName))
                                    {
                                        if (currentCode == -1)
                                        {
                                            currentCode = (int)Char.GetNumericValue(numberNames[numberName][0]) * 10;
                                        }
                                        currentCode = (currentCode - currentCode % 10) + (int)Char.GetNumericValue(numberNames[numberName][0]);

                                        characterIndex += numberName.Length - 2;
                                        replaced = true;
#if DEBUG
                                        if (originalLine[0] != '*')
                                        {
                                            originalLine = $"*{originalLine}";
                                        }
#endif
                                    }
                                });
                        }
                    }
#if DEBUG
                    calibrationCodes.Add(originalLine, currentCode);
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