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
                    protein.amino_acids = parts[2].Trim();
                    data.Add(protein);
                }
            }
            reader.Close();
            return data;
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
            Console.ReadKey();
        }
    }
}