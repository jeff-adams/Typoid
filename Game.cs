using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace Typoid
{
    class Game
    {
        private Timer timer;
        private string word;
        private List<char> input = new List<char>();
        private List<string> completedWords = new List<string>();
        private double elapsedTime = 0.0; 
        private string filePath = "ext/words_alpha.txt";

        public void Run()
        {
            var words = CreateWordList();
            SetTimer();

            using (timer)
            {
                var isPlaying = true;
                var completed = 0;
                var endGoal = 10;
                var rng = new Random();

                while(isPlaying)
                {
                    word = words[rng.Next(0, words.Length -1)];
                    completedWords.Add(word);
                    input.Clear();
                    var isTyping = true;

                    while (isTyping)
                    {
                        var key = Console.ReadKey(true);
                        if (word.StartsWith(key.KeyChar))
                        {
                            word = word.TrimStart(key.KeyChar);
                            input.Add(key.KeyChar);
                        }
                        isTyping = !string.IsNullOrEmpty(word);
                    }; 

                    completed++;
                    isPlaying = completed < endGoal;
                    Console.Clear();   
                };   
            }

            PrintResults();
            Console.ReadKey();
        }

        private Task Refresh()
        {
            elapsedTime += 0.25;
            PrintGameScreen();

            return new Task(()=> Refresh());
        } 

        private string[] CreateWordList()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllLines(filePath);  
            }

            throw new DirectoryNotFoundException($"{filePath} not found");
        }

        private void SetTimer()
        {
            timer = new Timer(250);
            timer.Elapsed += async (sender, e) => await Refresh();
            timer.AutoReset = true;
            timer.Enabled = true;
        } 

        private void PrintGameScreen()
        {
            var timeXPosition = (Console.WindowWidth / 2) - (elapsedTime.ToString("000.00").Length / 2);
            var timeYPosition = Console.WindowHeight / 2 - 4;
            Console.SetCursorPosition(timeXPosition, timeYPosition);
            Console.Write(elapsedTime.ToString("000.00"));
            
            var wordXPosition = (Console.WindowWidth / 2 ) - ((input.Count + word.Length) / 2);
            var wordYPosition = Console.WindowHeight / 2 + 4;
            Console.SetCursorPosition(wordXPosition, wordYPosition);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            foreach (var letter in input)
            {
                Console.Write(letter);
            }
            Console.ResetColor();
            Console.Write(word);
        }

        private void PrintResults()
        {
            var result = $"You typed {completedWords.Count} words in {elapsedTime.ToString("000.00")}";
            var resultXPosition = (Console.WindowWidth / 2) - (result.Length / 2);
            var resultYPosition = Console.WindowHeight / 2;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(resultXPosition, resultYPosition);
            Console.Write(result);
            Console.ResetColor();

            for (var i = 0; i < completedWords.Count; i++)
            {
                var wordXPosition = Console.WindowWidth / 2 - (completedWords[i].Length / 2);
                var wordYPosition = (Console.WindowHeight / 2) + 2;
                Console.SetCursorPosition(wordXPosition, wordYPosition + i);
                Console.Write(completedWords[i]);
            }
        }
    }
}