using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        static List<Command> ReadCommands(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            List<Command> commands = new List<Command>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('\t');
                Command command;
                command.name = parts[0].Trim();
                command.parameter1 = parts.Length > 1 ? parts[1].Trim() : "";
                command.parameter2 = parts.Length > 2 ? parts[2].Trim() : "";
                commands.Add(command);
            }
            reader.Close();
            return commands;
        }

        static List<Protein> ReadData(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            List<Protein> data = new List<Protein>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('\t');
                if (parts.Length >= 3)
                {
                    Protein protein;
                    protein.name = parts[0].Trim();
                    protein.organism = parts[1].Trim();
                    protein.amino_acids = RLEDecoding(parts[2].Trim());
                    data.Add(protein);
                }
            }
            reader.Close();
            return data;
        }

        static string RLEEncoding(string amino_acids)
        {
            if (string.IsNullOrEmpty(amino_acids)) return amino_acids;

            string encoded = "";
            for (int i = 0; i < amino_acids.Length; i++)
            {
                char ch = amino_acids[i];
                int count = 1;
                while (i < amino_acids.Length - 1 && amino_acids[i + 1] == ch)
                {
                    count++;
                    i++;
                }
                if (count > 2) encoded = encoded + count + ch;
                else if (count == 1) encoded = encoded + ch;
                else if (count == 2) encoded = encoded + ch + ch;
            }
            return encoded;
        }

        static string RLEDecoding(string amino_acids)
        {
            if (string.IsNullOrEmpty(amino_acids)) return amino_acids;

            string decoded = "";
            for (int i = 0; i < amino_acids.Length; i++)
            {
                if (char.IsDigit(amino_acids[i]))
                {
                    string countStr = "";
                    while (i < amino_acids.Length && char.IsDigit(amino_acids[i]))
                    {
                        countStr += amino_acids[i];
                        i++;
                    }

                    if (i < amino_acids.Length)
                    {
                        char letter = amino_acids[i];
                        int count = int.Parse(countStr);
                        for (int j = 0; j < count; j++)
                        {
                            decoded += letter;
                        }
                    }
                }
                else
                {
                    decoded += amino_acids[i];
                }
            }
            return decoded;
        }

        static void ExecuteSearch(List<Protein> proteins, string searchSequence, List<string> output)
        {
            output.Add($"   search   {searchSequence}");
            output.Add($"organism\t\t\tprotein ");

            bool found = false;

            foreach (var protein in proteins)
            {
                if (protein.amino_acids.Contains(searchSequence))
                {
                    output.Add($"{protein.organism}\t\t{protein.name}");
                    found = true;
                }
            }

            if (!found)
            {
                output.Add($"NOT FOUND");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== ГЕНЕТИЧЕСКИЙ ПОИСК ===\n");
            Console.Write("Введите номер набора (0, 1, 2): ");
            string choice = Console.ReadLine();

            string sequencesFile = $"sequences.{choice}.txt";
            string commandsFile = $"commands.{choice}.txt";

            List<Protein> data = ReadData(sequencesFile);
            List<Command> commands = ReadCommands(commandsFile);

            Console.WriteLine($"Загружено белков: {data.Count}");
            Console.WriteLine($"Загружено команд: {commands.Count}");

       
            List<string> testOutput = new List<string>();
            ExecuteSearch(data, "SIIK", testOutput);
            foreach (var line in testOutput)
            {
                Console.WriteLine(line);
            }

            Console.ReadKey();
        }
    }
}