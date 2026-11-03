using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string inputFilePath = "input.txt";

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Файл '{inputFilePath}' не найден! Создаю пример из задания...");
            string defaultContent =
                "Peter Piper picked a peck of pickled peppers. A peck of pickled\r\n" +
                "peppers Peter Piper picked. If Peter Piper picked a peck of\r\n" +
                "pickled peppers, where is the peck that Peter Piper picked?";
            File.WriteAllText(inputFilePath, defaultContent, Encoding.UTF8);
        }

        string rawText = File.ReadAllText(inputFilePath, Encoding.UTF8);

        Console.WriteLine("================ ИСХОДНЫЙ ТЕКСТ ================");
        Console.WriteLine(rawText);
        Console.WriteLine("================================================\n");

        // 1. Парсим текст для базовых операций (не изменяющих структуру текста)
        Text text = TextParser.Parse(rawText);
        var processor = new TextProcessor(text);

        // Задание 1: Вывести предложения по возрастанию количества слов
        processor.PrintSentencesByWordCount();

        // Задание 2: Вывести предложения по возрастанию длины
        processor.PrintSentencesByLength();

        // Задание 3: Слова заданной длины в вопросительных предложениях (без дублей)
        int targetQuestionWordLength = 4;
        Console.WriteLine($"\n--- 3. Слова длины {targetQuestionWordLength} в вопросительных предложениях (без повторов) ---");
        var questionWords = processor.FindWordsInInterrogative(targetQuestionWordLength);
        foreach (var w in questionWords)
        {
            Console.WriteLine($"  • {w}");
        }

        // Задание 7: Экспорт исходного текста в XML (System.Xml.Serialization)
        processor.ExportToXml("result.xml");
        Console.WriteLine("\n--- 7. Экспорт в XML ---");
        Console.WriteLine("Текст успешно экспортирован в файл result.xml (в формате задания).");

        // Project 04: Построение конкорданса
        processor.PrintConcordance();

        // ---------------------------------------------------------------------------------
        // Для демонстрации модифицирующих операций (4, 5, 6) используем свежие копии текста,
        // чтобы одно действие не искажало результат другого:
        // ---------------------------------------------------------------------------------

        // Задание 4: Удалить слова заданной длины, начинающиеся с согласной
        int consonantLength = 4;
        Text textForConsonants = TextParser.Parse(rawText);
        var procConsonants = new TextProcessor(textForConsonants);
        procConsonants.RemoveWordsStartingWithConsonant(consonantLength);
        Console.WriteLine($"\n--- 4. Текст после удаления слов длины {consonantLength} на согласную ---");
        foreach (var sentence in procConsonants.TextData.Sentences)
        {
            Console.WriteLine(sentence);
        }

        // Задание 5: Замена слов заданной длины в указанном предложении
        int sentenceIdx = 0;
        int replaceLength = 4;
        string replacement = "[ЗАМЕНА]";
        Text textForReplace = TextParser.Parse(rawText);
        var procReplace = new TextProcessor(textForReplace);
        procReplace.ReplaceWordsInSentence(sentenceIdx, replaceLength, replacement);
        Console.WriteLine($"\n--- 5. Предложение #{sentenceIdx + 1} после замены слов длины {replaceLength} на \"{replacement}\" ---");
        Console.WriteLine(procReplace.TextData.Sentences[sentenceIdx]);

        // Задание 6: Загрузка стоп-слов из файлов и удаление их из текста
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        string[] stopWordFiles = { "stopwords_ru.txt", "stopwords_ru (1).txt", "stopwords_en.txt", "stopwords_en (1).txt" };
        foreach (var file in stopWordFiles)
        {
            if (File.Exists(file))
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    string word = line.Trim();
                    if (!string.IsNullOrEmpty(word))
                        stopWords.Add(word);
                }
            }
        }

        Text textForStopWords = TextParser.Parse(rawText);
        var procStopWords = new TextProcessor(textForStopWords);
        procStopWords.RemoveStopWords(stopWords);
        Console.WriteLine($"\n--- 6. Текст после удаления стоп-слов (загружено из файлов: {stopWords.Count} слов) ---");
        foreach (var sentence in procStopWords.TextData.Sentences)
        {
            Console.WriteLine(sentence);
        }

        Console.WriteLine("\nВсе пункты задания успешно выполнены!");
    }
}