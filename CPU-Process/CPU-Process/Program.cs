using System;
using System.Diagnostics;

public class CPUProcess
{
    static void Main(string[] args)
    {
        // Checks that only one argument is provided - number of iterations
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: CPU-Process.exe <number of iterations>");
            return;
        }

        int iterations;
        if (!int.TryParse(args[0], out iterations))
        {
            Console.WriteLine("Error: Invalid number of iterations provided.");
            return;
        }

        Console.WriteLine("Running intensive calculations...");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        IntensiveCalculation(iterations);
        stopwatch.Stop();

        Console.WriteLine($"Time for {iterations} iterations: {stopwatch.ElapsedMilliseconds:0.00} ms");
    }

    static double IntensiveCalculation(int iterations)
    {
        double result = 0;
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < 10000; j++)
            {
                result += Math.Pow(i, 2) * Math.Log(i + 1) * Math.Sin(i) * Math.Cos(i);
            }
        }
        return result;
    }
}
