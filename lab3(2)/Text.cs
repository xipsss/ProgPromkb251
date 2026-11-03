using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

/// <summary>
/// Запись в конкордансе: количество вхождений и упорядоченный набор номеров строк.
/// </summary>
public class ConcordanceEntry
{
    public int Count { get; set; }
    public SortedSet<int> LineNumbers { get; set; } = new SortedSet<int>();
}

/// <summary>
/// Представляет весь текст — коллекцию предложений.
/// </summary>
[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();
}

/// <summary>
/// Представляет предложение — коллекцию слов и знаков препинания.
/// Наследует SentenceItem (так как согласно заданию предложение тоже является токеном).
/// </summary>
public class Sentence : SentenceItem
{
    // В памяти храним все токены (слова и знаки)
    [XmlIgnore]
    public List<SentenceItem> Items { get; set; } = new List<SentenceItem>();

    // Для сериализации в XML строго по образцу из задания:
    // внутри <sentence> только теги <word>...</word>, упорядоченные по алфавиту
    [XmlElement("word")]
    public List<string> WordsXml
    {
        get => Items.OfType<Word>()
                    .Select(w => w.Value)
                    .OrderBy(w => w, StringComparer.OrdinalIgnoreCase)
                    .ToList();
        set
        {
            Items = value?.Select(v => (SentenceItem)new Word { Value = v }).ToList() 
                    ?? new List<SentenceItem>();
        }
    }

    public override string ToString()
    {
        return string.Join("", Items.Select((item, index) =>
            (item is Word && index > 0 && !(Items[index - 1] is Punctuation p && (p.Value == "-" || p.Value == "—")) ? " " : "") + item.Value)).Trim();
    }
}

/// <summary>
/// Базовый класс для всех элементов предложения (токенов).
/// </summary>
public abstract class SentenceItem
{
    [XmlIgnore]
    public string Value { get; set; } = "";

    [XmlIgnore]
    public int LineNumber { get; set; } = 1;
}

public class Word : SentenceItem { }

public class Punctuation : SentenceItem { }