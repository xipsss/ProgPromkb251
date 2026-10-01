using System;
using System.IO;

namespace CatAndMouseGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile;
            string outputFile;

            if (args.Length > 0 && int.TryParse(args[0], out int testNum))
            {
                inputFile = $"{testNum}.ChaseData.txt";
                outputFile = $"{testNum}.PursuitLog_test.txt";
            }
            else
            {
                Console.WriteLine("Выберите тестовый файл:");
                Console.WriteLine("1 - Тест №1 (1.ChaseData.txt)");
                Console.WriteLine("2 - Тест №2 (2.ChaseData.txt)");
                Console.WriteLine("3 - Тест №3 (3.ChaseData.txt)");
                Console.Write("Введите номер (1-3): ");

                string? choice = Console.ReadLine();
                if (!int.TryParse(choice, out testNum) || testNum < 1 || testNum > 3)
                {
                    Console.WriteLine("Неверный выбор. Завершение работы.");
                    return;
                }

                inputFile = $"{testNum}.ChaseData.txt";
                outputFile = $"{testNum}.PursuitLog_test.txt";
            }

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Ошибка: Файл '{inputFile}' не найден в папке {Directory.GetCurrentDirectory()}");
                return;
            }

            Console.WriteLine($"Запуск симуляции для файла: {inputFile}");

            Game game = new Game(10000);
            game.Run(inputFile, outputFile);
        }
    }
}