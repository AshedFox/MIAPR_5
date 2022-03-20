using System.Drawing;

namespace AlgorithmLib;

public class Algorithm
{
    public Function CalculatesSepFunction(Point[][] points)
    {
        var sepFunction = new Function();
        var correction = 1;
        var shouldContinue = true;

        while (shouldContinue)
        {
            for (var i = 0; i < points.Length; i++)
            {
                for (var j = 0; j < points[i].Length; j++)
                {
                    var potentialFunction = CalculatePotentialFunction(points[i][j]);
                    var newSepFunction = SumFunctions(sepFunction, MultiplyFunction(potentialFunction, correction));

                    correction = CalculateCorrection(newSepFunction, points, i, j);

                    if (correction == 0)
                    {
                        shouldContinue = false;
                    }
                    
                    sepFunction = newSepFunction;
                }
            }
        }

        return sepFunction;
    }

    private int CalculateCorrection(Function sepFunction, Point[][] points, int i, int j)
    {
        var nextJ = (j + 1) % points[i].Length;
        var nextI = nextJ == 0 ? (i + 1) % points.Length : i;

        var value = sepFunction.Calculate(points[nextI][nextJ].X, points[nextI][nextJ].Y);

        if (nextI == 0 && value <= 0)
        {
            return 1;
        } 
        else if (nextI == 1 && value > 0)
        {
            return -1;
        }

        return 0;
    }

    private Function SumFunctions(Function func1, Function func2) => new()
    {
        FreeCoef = func1.FreeCoef + func2.FreeCoef,
        X1Coef = func1.X1Coef + func2.X1Coef,
        X2Coef = func1.X2Coef + func2.X2Coef,
        X1X2Coef = func1.X1X2Coef + func2.X1X2Coef
    };
    
    private Function MultiplyFunction(Function func, int multiplier) => new()
    {
        FreeCoef = func.FreeCoef * multiplier,
        X1Coef = func.X1Coef * multiplier,
        X2Coef = func.X2Coef * multiplier,
        X1X2Coef = func.X1X2Coef * multiplier
    };

    private Function CalculatePotentialFunction(Point point) => new ()
    {
        FreeCoef = 1,
        X1Coef = point.X * 4,
        X2Coef = point.Y * 4,
        X1X2Coef = point.X * point.Y * 16
    };
}