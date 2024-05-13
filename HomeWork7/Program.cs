using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Пример для чтения трех файлов параллельно
        string[] filePaths = { "file1.txt", "file2.txt", "file3.txt" };
        ParallelReadFiles(filePaths);

        // Пример для чтения всех файлов в папке параллельно
        string folderPath = @"D:\tmp";
        Stopwatch stopwatch = Stopwatch.StartNew();
        int totalSpaces = ParallelReadFilesInFolder(folderPath);
        stopwatch.Stop();

        Console.WriteLine($"Общее количество пробелов в файлах: {totalSpaces}");
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");

        Console.ReadLine();
    }

    static void ParallelReadFiles(string[] filePaths)
    {
        // Используем Task.WhenAll для параллельного выполнения задач
        var tasks = new Task<int>[filePaths.Length - 1];
        for (int i = 0; i < filePaths.Length - 1; i++)
        {
            tasks[i] = Task.Run(() => CountSpacesInFile(filePaths[i]));
        }

        Task.WhenAll(tasks).ContinueWith((result) =>
        {
            Console.WriteLine("Количество пробелов в каждом файле:");
            for (int i = 0; i < tasks.Length - 1; i++)
            {
                Console.WriteLine($"{filePaths[i]}: {tasks[i].Result}");
            }
        });
    }

    static int ParallelReadFilesInFolder(string folderPath)
    {
        string[] filePaths = Directory.GetFiles(folderPath);

        // Используем Task.WhenAll для параллельного выполнения задач
        var tasks = new Task<int>[filePaths.Length - 1];
        for (int i = 0; i < filePaths.Length - 1; i++)
        {
            tasks[i] = Task.Run(() => CountSpacesInFile(filePaths[i]));
        }

        Task.WaitAll(tasks);

        // Суммируем результаты всех задач для общего количества пробелов
        int totalSpaces = tasks.Sum(task => task.Result);

        return totalSpaces;
    }

    static int CountSpacesInFile(string filePath)
    {
        try
        {
            string content = File.ReadAllText(filePath);
            int spaceCount = content.Count(char.IsWhiteSpace);
            Console.WriteLine($"{filePath}: {spaceCount} пробелов");
            return spaceCount;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла {filePath}: {ex.Message}");
            return 0;
        }
    }
}
