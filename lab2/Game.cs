using System;
using System.IO;
using System.Collections.Generic;

namespace CatAndMouseGame
{
    public class Game
    {
        private int boardSize;
        private Player cat;
        private Player mouse;
        private bool gameOver;
        private List<string> logEntries;

        public Game(int boardSize)
        {
            this.boardSize = boardSize;
            cat = new Player("Cat", boardSize);
            mouse = new Player("Mouse", boardSize);
            gameOver = false;
            logEntries = new List<string>();
        }

        public void ProcessCommand(string line)
        {
            if (gameOver) return;

            string[] parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            char command = parts[0][0];

            if (command == 'P')
            {
                RecordState();
            }
            else if (command == 'M' || command == 'C')
            {
                if (parts.Length >= 2 && int.TryParse(parts[1], out int value))
                {
                    Player player = (command == 'M') ? mouse : cat;

                    if (!player.IsInGame)
                    {
                        player.SetPosition(value);
                    }
                    else
                    {
                        player.Move(value);
                    }
                    CheckCatch();
                }
            }
        }

        private void CheckCatch()
        {
            if (cat.IsInGame && mouse.IsInGame && cat.Position == mouse.Position)
            {
                gameOver = true;
                cat.SetWinner();
                mouse.SetLoser();
            }
        }

        private void RecordState()
        {
            string catStr = cat.IsInGame ? cat.Position.ToString() : "??";
            string mouseStr = mouse.IsInGame ? mouse.Position.ToString() : "??";
            string distStr = (cat.IsInGame && mouse.IsInGame)
                ? Math.Abs(cat.Position - mouse.Position).ToString()
                : "??";

            logEntries.Add($"{catStr.PadRight(10)}{mouseStr.PadRight(10)}{distStr}");
        }

        public void Run(string inputFile, string outputFile)
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Ошибка: Файл '{inputFile}' не найден!");
                Console.WriteLine($"Текущая папка: {Directory.GetCurrentDirectory()}");
                return;
            }

            using (StreamReader sr = new StreamReader(inputFile))
            {
                string? firstLine = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(firstLine)) return;

                if (int.TryParse(firstLine.Trim(), out int n))
                {
                    this.boardSize = n;
                    this.cat = new Player("Cat", n);
                    this.mouse = new Player("Mouse", n);
                }

                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    ProcessCommand(line);

                    if (gameOver) break;
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.WriteLine("Cat and Mouse");
                sw.WriteLine();

                sw.WriteLine($"{"Cat".PadRight(10)}{"Mouse".PadRight(10)}{"Distance"}");
                sw.WriteLine("-----------------------------");

                foreach (var entry in logEntries)
                {
                    sw.WriteLine(entry);
                }

                sw.WriteLine("-----------------------------");
                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine($"Cat distance traveled: {cat.TotalDistance}");
                sw.WriteLine($"Mouse distance traveled: {mouse.TotalDistance}");
                sw.WriteLine();

                if (gameOver && cat.State == PlayerState.Winner)
                {
                    sw.WriteLine($"Мышь поймана в клетке: {cat.Position} - Mouse caught at: {cat.Position}");
                }
                else
                {
                    sw.WriteLine("Мышь ускользнула от кота - Mouse evaded Cat");
                }
            }

            Console.WriteLine($"Симуляция завершена. Результаты записаны в {outputFile}");
        }
    }
}