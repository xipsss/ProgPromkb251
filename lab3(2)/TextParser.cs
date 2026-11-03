using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class TextParser
{
    public static Text Parse(string rawText)
    {
        var textObject = new Text();
        if (string.IsNullOrWhiteSpace(rawText))
            return textObject;

        // Разбиваем текст на фрагменты предложений по завершающим знакам (. ! ?)
        var sentenceMatches = Regex.Matches(rawText, @"[^.!?]+[.!?]*");

        int currentLine = 1;
        int lastScannedIndex = 0;

        foreach (Match sentenceMatch in sentenceMatches)
        {
            var sentence = new Sentence();

            // Токены: либо слова (буквы, цифры, дефисы/апострофы), либо знаки препинания
            var tokenMatches = Regex.Matches(sentenceMatch.Value, @"[a-zA-Zа-яА-ЯёЁ0-9'-]+|[^\w\s]+");

            foreach (Match tokenMatch in tokenMatches)
            {
                int absoluteIndex = sentenceMatch.Index + tokenMatch.Index;

                // Быстро и эффективно считаем перевод строки без лишних Substring (O(N)):
                while (lastScannedIndex < absoluteIndex)
                {
                    if (rawText[lastScannedIndex] == '\n')
                        currentLine++;
                    lastScannedIndex++;
                }

                // Если токен начинается с буквы или цифры — это слово
                if (char.IsLetterOrDigit(tokenMatch.Value[0]))
                {
                    sentence.Items.Add(new Word { Value = tokenMatch.Value, LineNumber = currentLine });
                }
                else
                {
                    // Иначе это знак препинания (включая тире, точки, запятые)
                    sentence.Items.Add(new Punctuation { Value = tokenMatch.Value, LineNumber = currentLine });
                }
            }

            if (sentence.Items.Count > 0)
                textObject.Sentences.Add(sentence);
        }

        return textObject;
    }
}