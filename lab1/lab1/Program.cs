using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



class Program
{
    static int minNum = -10000,
        maxNum = 10000;
    static void Main(string[] args)
    {
        Console.OutputEncoding = UTF8Encoding.UTF8;
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        while (true)
        {
            int option = SelectChoice([
                "Виберіть дію:",
                "Згенерувати числа у файл",
                "Відсортувати числа у файлі"]);
            switch (option)
            {
                case 0:
                    continue;
                case 1:
                    Console.WriteLine("Введіть назву .txt файлу для його створення на робочому столі з подальшою генерацією чисел");
                    string fileName;
                    if ((fileName = Console.ReadLine()) != null)
                        GenerateNumbersToFile($"{desktopPath}\\{fileName}.txt");
                    break;
                case 2:
                    List<string> txtFiles = Directory.GetFiles(desktopPath, "*.txt").ToList();
                    for (int i = 0; i < txtFiles.Count; i++)
                        txtFiles[i] = Path.GetFileName(txtFiles[i]);
                    txtFiles.Insert(0, "Виберіть .txt файл з робочого столу для сортування:");
                    option = SelectChoice(txtFiles);
                    if (option != 0)
                        SortNumbersInFile($"{desktopPath}\\{txtFiles[option]}");
                    break;
                default:
                    break;
            }
        }
    }

    static int SelectChoice(List<string> choice)
    {
        int option = 1;
        while (true)
        {
            Console.Clear();
            Console.WriteLine(choice[0]);
            for (int i = 1; i < choice.Count; i++)
                Console.WriteLine(((option == i) ? "→ " : "  ") + choice[i]);
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (option == 1)
                        option = choice.Count - 1;
                    else
                        option--;
                    break;
                case ConsoleKey.DownArrow:
                    if (option == choice.Count - 1)
                        option = 1;
                    else
                        option++;
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    return option;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return 0;
                default:
                    continue;
            }
        }
    }
    static void PressToContinue()
    {
        Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
        Console.ReadKey();
    }
    static void GenerateNumbersToFile(string filePath)
    {
        while (true)
        {
            int option = SelectChoice([
                "Виберіть спосіб генерації:",
                "Кількість чисел",
                "Розмір файлу в мегабайтах"]);
            switch (option)
            {
                case 0:
                    return;
                case 1:
                    Console.WriteLine("Скільки чисел згенерувати?");
                    if (int.TryParse(Console.ReadLine(), out int count))
                        GenerateByCount(filePath, count);
                    else
                    {
                        Console.WriteLine("Некоректне число.");
                        PressToContinue();
                    }
                    break;
                case 2:
                    Console.WriteLine("Скільки мегабайтів чисел згенерувати?");
                    if (int.TryParse(Console.ReadLine(), out int megabytes))
                        GenerateBySize(filePath, megabytes);
                    else
                    {
                        Console.WriteLine("Некоректне число.");
                        PressToContinue();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    static void GenerateByCount(string filePath, int count)
    {
        Random rand = new Random();
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            for (int i = 0; i < count; i++)
                writer.WriteLine(rand.Next(minNum, maxNum + 1));
        }
        Console.WriteLine($"Згенеровано {count} чисел у файл '{filePath}'");
        PressToContinue();
    }
    static void GenerateBySize(string filePath, int megabytes)
    {
        Random rand = new Random();
        long targetSize = (megabytes * 1024L * 1024L) / sizeof(int);
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            for (long i = 0; i < targetSize; i++)
                writer.WriteLine(rand.Next(minNum, maxNum + 1));
        }
        Console.WriteLine($"Згенеровано файл розміром приблизно {megabytes} МБ у файл '{filePath}'");
        PressToContinue();
    }



    static void SortNumbersInFile(string filePath)
    {
        Console.WriteLine(filePath);
        Console.ReadKey();
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не знайдено.");
            return;
        }

        try
        {
            var numbers = File.ReadAllLines(filePath).Select(int.Parse).ToList();
            numbers.Sort();
            File.WriteAllLines(filePath, numbers.Select(n => n.ToString()));
            Console.WriteLine($"Числа у файлі '{filePath}' відсортовано.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
    }
}
