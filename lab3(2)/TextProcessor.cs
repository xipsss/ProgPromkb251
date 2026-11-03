using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

public class TextProcessor
{
    public Text TextData { get; set; }

    public TextProcessor(Text textData)
    {
        TextData = textData;
    }

    // 1. Вывести предложения по возрастанию количества слов
    public void PrintSentencesByWordCount()
    {
        var sorted = TextData.Sentences.OrderBy(s => s.Items.OfType<Word>().Count());
        Console.WriteLine("--- 1. Предложения по возрастанию количества слов ---");
        foreach (var s in sorted)
        {
            Console.WriteLine($"({s.Items.OfType<Word>().Count()} слов) {s}");
        }
    }

    // 2. Вывести предложения по возрастанию длины (количества символов)
    public void PrintSentencesByLength()
    {
        var sorted = TextData.Sentences.OrderBy(s => s.ToString().Length);
        Console.WriteLine("\n--- 2. Предложения по возрастанию длины (в символах) ---");
        foreach (var s in sorted)
        {
            Console.WriteLine($"({s.ToString().Length} симв.) {s}");
        }
    }

    // 3. Во всех вопросительных предложениях найти уникальные слова заданной длины
    public IEnumerable<string> FindWordsInInterrogative(int wordLength)
    {
        return TextData.Sentences
            .Where(s => s.Items.OfType<Punctuation>().Any(p => p.Value.Contains("?")))
            .SelectMany(s => s.Items.OfType<Word>())
            .Where(w => w.Value.Length == wordLength)
            .Select(w => w.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    // 4. Удалить из текста все слова заданной длины, начинающиеся с согласной
    public void RemoveWordsStartingWithConsonant(int length)
    {
        string consonants = "bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщ";
        foreach (var sentence in TextData.Sentences)
        {
            sentence.Items.RemoveAll(item =>
                item is Word word &&
                word.Value.Length == length &&
                consonants.Contains(char.ToLower(word.Value[0])));
        }
    }

    // 5. Замена слов заданной длины в указанном предложении на подстроку
    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string substring)
    {
        if (sentenceIndex >= 0 && sentenceIndex < TextData.Sentences.Count)
        {
            var sentence = TextData.Sentences[sentenceIndex];
            foreach (var item in sentence.Items)
            {
                if (item is Word word && word.Value.Length == wordLength)
                {
                    word.Value = substring;
                }
            }
        }
    }

    // 6. Удаление стоп-слов из текста
    public void RemoveStopWords(HashSet<string> stopWords)
    {
        foreach (var sentence in TextData.Sentences)
        {
            sentence.Items.RemoveAll(item =>
                item is Word word && stopWords.Contains(word.Value.ToLower()));
        }
    }

    // 7. Экспорт в XML по образцу задания (System.Xml.Serialization)
    public void ExportToXml(string filePath)
    {
        var serializer = new XmlSerializer(typeof(Text));
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty); // Убираем технические атрибуты xmlns:xsi и xmlns:xsd

        var settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = false
        };

        using (var writer = XmlWriter.Create(filePath, settings))
        {
            serializer.Serialize(writer, TextData, namespaces);
        }
    }

    // Project 04: Построение конкорданса (Dictionary + SortedSet)
    public void PrintConcordance()
    {
        // Словарь: Слово -> Запись конкорданса (частота + номера строк)
        var concordance = new Dictionary<string, ConcordanceEntry>(StringComparer.OrdinalIgnoreCase);

        foreach (var sentence in TextData.Sentences)
        {
            foreach (var word in sentence.Items.OfType<Word>())
            {
                string key = word.Value.ToLower();

                if (!concordance.ContainsKey(key))
                {
                    concordance[key] = new ConcordanceEntry();
                }

                concordance[key].Count++;
                concordance[key].LineNumbers.Add(word.LineNumber); // SortedSet исключает дубли строк
            }
        }

        // Сортировка слов по алфавиту
        var sortedWords = new SortedSet<string>(concordance.Keys, StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("\n--- Конкорданс текста (Project 04) ---");
        foreach (var word in sortedWords)
        {
            var entry = concordance[word];
            int dotsCount = Math.Max(2, 35 - word.Length);
            string dots = new string('.', dotsCount);
            string lines = string.Join(" ", entry.LineNumbers);

            Console.WriteLine($"{word}{dots}{entry.Count}: {lines}");
        }
    }
}