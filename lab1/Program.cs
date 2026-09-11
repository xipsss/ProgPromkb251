using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneticSearch
{
    class Program
    {
        struct Protein
        {
            public string name;
            public string organism;
            public string amino_acids;
        }

        struct Command
        {
            public string name;
            public string parameter1;
            public string parameter2;
        }

        static int operationCounter = 0;

        static List<Command> ReadCommands(string filename)
        {
            var commands = new List<Command>();
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('\t');
                    commands.Add(new Command
                    {
                        name = parts[0].Trim(),
                        parameter1 = parts.Length > 1 ? parts[1].Trim() : "",
                        parameter2 = parts.Length > 2 ? parts[2].Trim() : ""
                    });
                }
            }
            return commands;
        }

        static List<Protein> ReadData(string filename)
        {
            var data = new List<Protein>();
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('\t');
                    if (parts.Length >= 3)
                    {
                        data.Add(new Protein
                        {
                            name = parts[0].Trim(),
                            organism = parts[1].Trim(),
                            amino_acids = RLEDecoding(parts[2].Trim())
                        });
                    }
                }
            }
            return data;
        }

        static string RLEEncoding(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            string result = "";
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                int count = 1;
                while (i < s.Length - 1 && s[i + 1] == ch) { count++; i++; }

                if (count > 2) result += count + ch.ToString();
                else if (count == 1) result += ch;
                else result += ch.ToString() + ch;
            }
            return result;
        }

        static string RLEDecoding(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            string result = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    string num = "";
                    while (i < s.Length && char.IsDigit(s[i])) { num += s[i]; i++; }

                    if (i < s.Length)
                    {
                        int count = int.Parse(num);
                        for (int j = 0; j < count; j++) result += s[i];
                    }
                }
                else result += s[i];
            }
            return result;
        }

        static void HandleSearch(List<Protein> proteins, Command cmd, List<string> output)
        {
            ExecuteSearch(proteins, RLEDecoding(cmd.parameter1), output);
        }

        static void HandleDiff(List<Protein> proteins, Command cmd, List<string> output)
        {
            ExecuteDiff(proteins, cmd.parameter1, cmd.parameter2, output);
        }

        static void HandleMode(List<Protein> proteins, Command cmd, List<string> output)
        {
            ExecuteMode(proteins, cmd.parameter1, output);
        }

        static void ExecuteSearch(List<Protein> proteins, string searchSeq, List<string> output)
        {
            output.Add($"   search   {searchSeq}");
            output.Add("organism\t\t\tprotein ");

            bool found = false;
            foreach (var p in proteins)
            {
                if (p.amino_acids.Contains(searchSeq))
                {
                    output.Add($"{p.organism}\t\t{p.name}");
                    found = true;
                }
            }
            if (!found) output.Add("NOT FOUND");
        }

        static void ExecuteDiff(List<Protein> proteins, string name1, string name2, List<string> output)
        {
            output.Add($"   diff   {name1}   {name2}");

            Protein? p1 = null, p2 = null;
            foreach (var p in proteins)
            {
                if (p.name == name1) p1 = p;
                if (p.name == name2) p2 = p;
            }

            output.Add("amino-acids difference:");
            if (p1 == null || p2 == null)
            {
                string missing = "";
                if (p1 == null) missing += name1 + " ";
                if (p2 == null) missing += name2 + " ";
                output.Add($"MISSING: {missing.Trim()}");
                return;
            }

            string s1 = p1.Value.amino_acids, s2 = p2.Value.amino_acids;
            int maxLen = Math.Max(s1.Length, s2.Length);
            int diff = 0;

            for (int i = 0; i < maxLen; i++)
            {
                char c1 = i < s1.Length ? s1[i] : '\0';
                char c2 = i < s2.Length ? s2[i] : '\0';
                if (c1 != c2) diff++;
            }

            output.Add($"{diff} ");
        }

        static void ExecuteMode(List<Protein> proteins, string name, List<string> output)
        {
            output.Add($"   mode   {name} ");

            Protein? target = null;
            foreach (var p in proteins)
                if (p.name == name) { target = p; break; }

            output.Add("amino-acid occurs:");
            if (target == null)
            {
                output.Add($"MISSING: {name}");
                return;
            }

            var freq = new Dictionary<char, int>();
            foreach (char c in target.Value.amino_acids)
                freq[c] = freq.ContainsKey(c) ? freq[c] + 1 : 1;

            int max = 0;
            char best = '\0';
            bool first = true;

            foreach (var kv in freq)
            {
                if (kv.Value > max || (kv.Value == max && (first || kv.Key < best)))
                {
                    max = kv.Value;
                    best = kv.Key;
                    first = false;
                }
            }

            output.Add($"{best}          {max}");
        }

        static void DispatchCommand(List<Protein> proteins, Command cmd, List<string> output)
        {
            switch (cmd.name.ToLower())
            {
                case "search": HandleSearch(proteins, cmd, output); break;
                case "diff":   HandleDiff(proteins, cmd, output);   break;
                case "mode":   HandleMode(proteins, cmd, output);   break;
                default:       output.Add($"Неизвестная операция: {cmd.name}"); break;
            }
        }

        static string FormatCommandLine(int number, Command cmd)
        {
            string line = $"{number:D3}   {cmd.name.ToLower()}   {cmd.parameter1}";
            if (!string.IsNullOrEmpty(cmd.parameter2)) line += $"   {cmd.parameter2}";
            return line;
        }

        static void ProcessCommands(List<Protein> proteins, List<Command> commands, string outputFile)
        {
            var outputLines = new List<string>
            {
                "Dwight Barnette",
                "Genetic Searching",
                "--------------------------------------------------------------------------"
            };

            operationCounter = 0;
            foreach (var cmd in commands)
            {
                operationCounter++;
                outputLines.Add(FormatCommandLine(operationCounter, cmd));
                DispatchCommand(proteins, cmd, outputLines);
                outputLines.Add("--------------------------------------------------------------------------");
            }

            if (File.Exists(outputFile))
            {
                Console.Write($"\nФайл {outputFile} уже существует. Перезаписать? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                {
                    Console.WriteLine("Запись отменена.");
                    return;
                }
            }

            File.WriteAllLines(outputFile, outputLines, Encoding.UTF8);
            Console.WriteLine($"\nЗаписано в {outputFile}. Обработано {operationCounter} команд");
        }

        static string ReadChoice(string label)
        {
            while (true)
            {
                Console.WriteLine($"\nВыберите номер для {label}:");
                Console.WriteLine($"  0 - {label.ToLower()}.0.txt");
                Console.WriteLine($"  1 - {label.ToLower()}.1.txt");
                Console.WriteLine($"  2 - {label.ToLower()}.2.txt");
                Console.Write("Введите номер (0, 1, 2): ");

                string input = Console.ReadLine();
                if (input == "0" || input == "1" || input == "2") return input;

                Console.WriteLine("Неверный ввод!");
            }
        }

        static bool CheckFile(string filename)
        {
            if (File.Exists(filename)) return true;
            Console.WriteLine($"Файл {filename} не найден!");
            Console.WriteLine($"   Текущая папка: {Directory.GetCurrentDirectory()}");
            return false;
        }

        static void PrintProteins(List<Protein> data)
        {
            Console.WriteLine($"Загружено {data.Count} белков:");
            foreach (var p in data)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Организм: {p.organism}");
                Console.WriteLine($"Белок:    {p.name}");
                int len = Math.Min(40, p.amino_acids.Length);
                Console.WriteLine($"Цепочка:  {p.amino_acids.Substring(0, len)}...");
            }
            Console.WriteLine("----------------------------------------");
        }

        static bool AskRestart()
        {
            while (true)
            {
                Console.Write("\nЗапустить снова? (y/n): ");
                string answer = Console.ReadLine()?.ToLower().Trim();

                if (answer == "y" || answer == "yes" || answer == "д" || answer == "да") return true;
                if (answer == "n" || answer == "no" || answer == "н" || answer == "нет") return false;

                Console.WriteLine("Введите y или n.");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== ГЕНЕТИЧЕСКИЙ ПОИСК ===");

            bool restart = true;
            while (restart)
            {
                try
                {
                    string seqChoice = ReadChoice("SEQUENCES");
                    string cmdChoice = ReadChoice("COMMANDS");

                    string sequencesFile = $"sequences.{seqChoice}.txt";
                    string commandsFile = $"commands.{cmdChoice}.txt";
                    string outputFile = $"genedata.{seqChoice}_{cmdChoice}.txt";

                    if (!CheckFile(sequencesFile) || !CheckFile(commandsFile))
                    {
                        restart = AskRestart();
                        continue;
                    }

                    Console.WriteLine($"\nЧтение файла: {sequencesFile}");
                    var data = ReadData(sequencesFile);
                    PrintProteins(data);

                    Console.WriteLine($"\nЧтение файла: {commandsFile}");
                    var commands = ReadCommands(commandsFile);
                    Console.WriteLine($"Загружено {commands.Count} команд");

                    Console.WriteLine("\n=== ОБРАБОТКА КОМАНД ===");
                    ProcessCommands(data, commands, outputFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nОШИБКА: {ex.Message}");
                }

                restart = AskRestart();
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}