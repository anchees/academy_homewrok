using System;
using System.IO;

namespace ChildApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Дочерний процесс запущен");

            if (args.Length == 3)
            {
                int a = Convert.ToInt32(args[0]);
                int b = Convert.ToInt32(args[1]);
                string operation = args[2];

                Console.WriteLine("Первое число: " + a);
                Console.WriteLine("Второе число: " + b);
                Console.WriteLine("Операция: " + operation);

                int result = 0;

                if (operation == "+")
                    result = a + b;
                else if (operation == "-")
                    result = a - b;
                else if (operation == "*")
                    result = a * b;
                else if (operation == "/")
                    result = a / b;

                Console.WriteLine("Результат: " + result);
            }
            else if (args.Length == 2)
            {
                string filePath = args[0];
                string word = args[1];

                string text = File.ReadAllText(filePath);

                string[] words = text.Split(
                    new char[] { ' ', '.', ',', '!', '?', ';', ':', '\n', '\r', '\t' },
                    StringSplitOptions.RemoveEmptyEntries
                );

                int count = 0;

                foreach (string item in words)
                {
                    if (item.ToLower() == word.ToLower())
                        count++;
                }

                Console.WriteLine("Файл: " + filePath);
                Console.WriteLine("Слово: " + word);
                Console.WriteLine("Количество вхождений: " + count);
            }
            else
            {
                Console.WriteLine("Аргументы не переданы");
            }

            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}