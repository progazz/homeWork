using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Размеры массивов для тестирования
        int[] sizes = { 100000, 1000000, 10000000 };

        foreach (int size in sizes)
        {
            Console.WriteLine($"Размер массива: {size}");

            // Создание и заполнение массива случайными числами
            int[] array = new int[size];
            Random rand = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next(100);
            }

            // Измерение времени для последовательного вычисления
            Stopwatch stopwatch = Stopwatch.StartNew();
            long sumSequential = CalculateSumSequential(array);
            stopwatch.Stop();
            TimeSpan sequentialTime = stopwatch.Elapsed;
            Console.WriteLine($"Последовательное вычисление: {sequentialTime.TotalMilliseconds} мс");

            // Измерение времени для параллельного вычисления
            stopwatch.Restart();
            long sumParallel = CalculateSumParallel(array);
            stopwatch.Stop();
            TimeSpan parallelTime = stopwatch.Elapsed;
            Console.WriteLine($"Параллельное вычисление: {parallelTime.TotalMilliseconds} мс");

            // Измерение времени для вычисления с помощью LINQ
            stopwatch.Restart();
            long sumLinq = CalculateSumLinq(array);
            stopwatch.Stop();
            TimeSpan linqTime = stopwatch.Elapsed;
            Console.WriteLine($"Вычисление с помощью LINQ: {linqTime.TotalMilliseconds} мс");

            // Проверка корректности результатов
            if (sumSequential != sumParallel || sumSequential != sumLinq)
            {
                Console.WriteLine("Ошибка: Результаты различаются!");
            }

            Console.WriteLine();
        }

        Console.ReadLine();
    }

    // Последовательное вычисление суммы элементов массива
    static long CalculateSumSequential(int[] array)
    {
        long sum = 0;
        foreach (int num in array)
        {
            sum += num;
        }
        return sum;
    }

    // Параллельное вычисление суммы элементов массива
    static long CalculateSumParallel(int[] array)
    {
        long sum = 0;
        Parallel.ForEach(array, num =>
        {
            sum += num;
        });
        return sum;
    }

    // Вычисление суммы элементов массива с использованием LINQ
    static long CalculateSumLinq(int[] array)
    {
        return array.AsParallel().Sum();
    }
}
