using System.Drawing;

namespace AlgorithmLib;

public class Function
{
    public int X1Coef { get; set; }
    public int X2Coef { get; set; }
    public int X1X2Coef { get; set; }
    public int FreeCoef { get; set; }

    public double Calculate(double x1, double x2) => FreeCoef + X1Coef * x1 + X2Coef * x2 + X1X2Coef * x1 * x2;

    public double CalculateX2(double x1) => -(X1Coef * x1 + FreeCoef) / (X1X2Coef * x1 + X2Coef); 

    public override string ToString()
    {
        var x1 = X1Coef == 0 ? "" : X1Coef > 0 ? $" + {X1Coef}x1" : $" - {Math.Abs(X1Coef)}x1";
        var x2 = X2Coef == 0 ? "" : X2Coef >= 0 ? $" + {X2Coef}x2" : $" - {Math.Abs(X2Coef)}x2";
        var x1x2 = X1X2Coef == 0 ? "" : X1X2Coef >= 0 ? $" + {X1X2Coef}x1x2" : $" - {Math.Abs(X1X2Coef)}x1x2";

        return $"{FreeCoef}{x1}{x2}{x1x2}";
    }
}