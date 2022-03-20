using System;

namespace Lab5Client;

public static class RandomExtension
{
    public static double GenerateDouble(this Random random, double min, double max) => random.NextDouble() * (max - min) + min;
}