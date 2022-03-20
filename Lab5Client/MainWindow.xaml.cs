using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlgorithmLib;
using Point = System.Drawing.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Lab5Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly double _eps = 0.01;
        private double _start = -10;
        private double _end = 10;
        private readonly int _testPointsCount = 100;
        private Function _sepFunction;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            var algorithm = new Algorithm();

            var x1 = Convert.ToInt32(FirstFirstPointXTextBox.Text);
            var x2 = Convert.ToInt32(FirstSecondPointXTextBox.Text);
            var x3 = Convert.ToInt32(SecondFirstPointXTextBox.Text);
            var x4 = Convert.ToInt32(SecondSecondPointXTextBox.Text);
            
            var y1 = Convert.ToInt32(FirstFirstPointYTextBox.Text);
            var y2 = Convert.ToInt32(FirstSecondPointYTextBox.Text);
            var y3 = Convert.ToInt32(SecondFirstPointYTextBox.Text);
            var y4 = Convert.ToInt32(SecondSecondPointYTextBox.Text);

            var maxX = Math.Max(x1, Math.Max(x2, Math.Max(x3, x4)));
            var maxY = Math.Max(y1, Math.Max(y2, Math.Max(y3, y4)));

            _end = Math.Max(maxX, maxY) + 10;
            _start = -_end;
            
            var points = new[]
            {
                new[]
                {
                    new Point(x1, y1),
                    new Point(x2, y2),
                },
                new[]
                {
                    new Point(x3, y3),
                    new Point(x4, y4),
                },
            };

            _sepFunction = algorithm.CalculatesSepFunction(points);
            
            Draw(_sepFunction, ToWindowsPoints(points));
        }

        private System.Windows.Point[] ToWindowsPoints(Point[][] points)
        {
            var windowsPoints = new List<System.Windows.Point>();

            foreach (var pointsArray in points)
            {
                foreach (var point in pointsArray)
                {
                    windowsPoints.Add(new System.Windows.Point()
                    {
                        X = point.X,
                        Y = point.Y
                    });
                }
            }

            return windowsPoints.ToArray();
        }

        private void Draw(Function function, System.Windows.Point[] basePoints)
        {
            Canvas.Children.Clear();
            var scale = Math.Min(Canvas.ActualWidth / 2, Canvas.ActualHeight / 2) / _end;

            DrawAxis();
            DrawChart(function, scale);
            DrawPoints(function, basePoints, scale);
        }

        private void DrawAxis()
        {
            var xAxis = new Line()
            {
                X1 = 0,
                X2 = Canvas.ActualWidth,
                Y1 = Canvas.ActualHeight / 2,
                Y2 = Canvas.ActualHeight / 2,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            var yAxis = new Line()
            {
                X1 = Canvas.ActualWidth / 2,
                X2 = Canvas.ActualWidth / 2,
                Y1 = 0,
                Y2 = Canvas.ActualHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            Canvas.Children.Add(xAxis);
            Canvas.Children.Add(yAxis);
        }
        
        private void DrawChart(Function function, double scale)
        {
            var prevX1 = _start;
            var prevX2 = function.CalculateX2(prevX1);

            for (var i = prevX1; i < _end; i += _eps)
            {
                var x1 = i;
                var x2 = function.CalculateX2(x1);

                var line = new Line()
                {
                    X1 = Canvas.ActualWidth / 2 + prevX1 * scale,
                    Y1 = Canvas.ActualHeight / 2 - prevX2 * scale,
                    X2 = Canvas.ActualWidth / 2 + x1 * scale,
                    Y2 = Canvas.ActualHeight / 2 - x2 * scale,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                };
                Canvas.Children.Add(line);

                prevX1 = x1;
                prevX2 = x2;
            }
        }
        
        private System.Windows.Point[] GenerateTestPoints(int count)
        {
            var points = new System.Windows.Point[count];
            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                points[i].X = random.GenerateDouble(_start, _end);
                points[i].Y = random.GenerateDouble(_start, _end);
            }

            return points;
        }

        private void DrawPoints(Function function, System.Windows.Point[] points, double scale)
        {
            foreach (var point in points)
            {
                var rect = new Rectangle()
                {
                    Fill = function.Calculate(point.X, point.Y) > 0 ? Brushes.Indigo : Brushes.Tomato,
                    Width = 4,
                    Height = 4
                };
                Canvas.Children.Add(rect);
                Canvas.SetLeft(rect, Canvas.ActualWidth / 2 + point.X * scale - 2);
                Canvas.SetBottom(rect, Canvas.ActualHeight / 2 + point.Y * scale - 2);
            }
            
        }

        private void TestPointsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var testPoints = GenerateTestPoints(_testPointsCount);
            var scale = Math.Min(Canvas.ActualWidth / 2, Canvas.ActualHeight / 2) / _end;

            DrawPoints(_sepFunction, testPoints, scale);
        }
    }
}