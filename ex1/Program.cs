using System;
using System.Collections.Generic;//para ocupar tablash hash
using System.Diagnostics;
using System.IO;//para habilitar la lectura del text 
using System.Linq;
using System.Runtime.InteropServices;
using static System.Console;

namespace ex1
{
    internal class Program
    {
        private class ListedWord
        {
            public string Word { get; }
            public int Count { get; set; } = 1;

            public ListedWord(string newword)
            {
                Word = newword;
            }
        }

        private class WordList
        {
            private readonly Dictionary<string, ListedWord> _words = new Dictionary<string, ListedWord>();
            private static int MaxWords { get; set; }
            public static List<int> WordFrequencies { get; set; }
            private readonly char _separator;

            public WordList(char separator = ',')
            {
                _separator = separator;
                WordFrequencies = new List<int>();
                for (var i = 0; i < 30; i++)
                {
                    WordFrequencies.Add(0);
                }
            }

            public int Count => _words.Count;

            private void AddWord(string word)
            {
                if (!_words.ContainsKey(word))
                {
                    var lword = new ListedWord(word); // add new word to list
                    _words.Add(word, lword);
                    return;
                }
                _words[word].Count++;
            }

            internal void DumpWords(StreamWriter sw)
            {
                var index = 0;
                foreach (var word in _words.Values.OrderByDescending(x => x.Count).ToList())
                {
                    {
                        sw.Write(index);
                        sw.Write(',');
                        //sw.Write(word.Count);
                        //sw.Write(',');
                    }
                    sw.WriteLine(word.Word);
                    index++;
                }
            }

            // lee entre la longitud en busca de alpanumericos
            internal void ScanLine(string line)
            {
                var wordCount = 0;

                line = line.ToLower();
                var parts = line.Split(_separator).ToList();

                foreach (var part in parts)
                {
                    if (part.Length == 0) continue;

                    var splitwords = part.Split(' ');
                    foreach (var sw in splitwords)
                    {
                        var sw2 = sw.Trim();
                        if (sw2 != "")
                        {
                            AddWord(sw2);
                            WordFrequencies[sw2.Length]++;
                            wordCount++;
                        }
                    }
                }
                if (wordCount > MaxWords)
                {
                    MaxWords = wordCount;
                }
            }
        }

        private static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"palabra.txt");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var words = new WordList(',');
            var total = 0;
            var lineCount = 0;
            //imprimir el texto del tzt

            Console.WriteLine("-------------------------------------------");
            foreach (var line in lines)
            {
                {
                    total += line.Length;
                    lineCount++;
                    words.ScanLine(line);
                }
            }
            
            StreamReader imprimir = new StreamReader(@"palabra.txt");
            Console.WriteLine("-------------------------------------------");
            while (!imprimir.EndOfStream)
            {
                string res = imprimir.ReadLine();
                Console.WriteLine(res);
            }
            Console.WriteLine("-------------------------------------------");
            stopWatch.Stop();
            WriteLine($"tiempo de execucion : {stopWatch.ElapsedMilliseconds/1000.0} secs");
            WriteLine($"tamaño de la longitud ={total/lineCount}");
            Console.WriteLine("-------------------------------------------");
            using (var sw = new StreamWriter(@"tabla.txt"))
            {
                words.DumpWords(sw);
                sw.Close();
            }
            if (words.Count > 0)
            {
                WriteLine($"{words.Count}numero de palabras copiadas de palabra.txt a tabla.txt ");
            }
            var totalWords = 0;
            for (var i = 0; i < WordList.WordFrequencies.Count; i++)
            {
                if (WordList.WordFrequencies[i] > 0)
                {
                    WriteLine($"la palabra numero  {i}  = {WordList.WordFrequencies[i]}");
                    totalWords += WordList.WordFrequencies[i];
                }
            }
            Console.WriteLine("-------------------------------------------");
            StreamReader salida = new StreamReader(@"tabla.txt");
            while (!salida.EndOfStream)
            {
                string ola = salida.ReadLine();
                Console.WriteLine(ola);
            }
            Console.WriteLine("-------------------------------------------");
            WriteLine($"Total #palabras ={totalWords}");
            
            Console.ReadKey();
        }
    }
}
