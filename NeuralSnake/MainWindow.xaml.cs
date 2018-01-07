using NeuralSnake.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace NeuralSnake {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private const int GRID_SIZE = 25;
        private const int FIELD_HEIGHT = 1000;
        private const int FIELD_WIDTH = 1000;

        private SolidColorBrush bodyColor;
        private SolidColorBrush appleColor;

        public MainWindow() {
            InitializeComponent();

            bodyColor = new SolidColorBrush();
            appleColor = new SolidColorBrush();

            appleColor.Color = Color.FromRgb(255, 0, 0);
            bodyColor.Color = Color.FromRgb(0, 0, 0);

            // Create a StackPanel to contain the shape.
            Canvas canvas = new Canvas();

            canvas.Width = FIELD_WIDTH;
            canvas.Height = FIELD_HEIGHT;

            this.Content = canvas;

            var player = new KeyBoardPlayer();

            var game = new Game(player, 40, 40, 8);

            game.OnUpdate += (s, a) => {
                RunOnUiThread(() => DrawGame(a.State, canvas));
            };

            game.Start();
        }

        public object RunOnUiThread(Action method) {
            return Dispatcher.Invoke(DispatcherPriority.Normal, method);
        }

        private void DrawGame(GameState state, Canvas canvas) {
            canvas.Children.Clear();

            foreach (var part in state.Snake) {
                var rectangle = new Rectangle();

                rectangle.Width = GRID_SIZE;
                rectangle.Height = GRID_SIZE;

                rectangle.Fill = bodyColor;

                canvas.Children.Add(rectangle);
                Canvas.SetTop(rectangle, 1000 - part.Location.Y * GRID_SIZE + 1);
                Canvas.SetLeft(rectangle, part.Location.X * GRID_SIZE + 1);
            }

            // Draw apple
            var apple = new Rectangle();

            apple.Width = GRID_SIZE;
            apple.Height = GRID_SIZE;

            apple.Fill = appleColor;

            canvas.Children.Add(apple);
            Canvas.SetTop(apple, 1000 - state.Apple.y * GRID_SIZE + 1);
            Canvas.SetLeft(apple, state.Apple.x * GRID_SIZE + 1);

            var outline = new Rectangle();
            outline.Width = 990;
            outline.Height = 990;

            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 255, 0, 0);

            outline.StrokeThickness = 5;
            outline.Stroke = bodyColor;
            outline.StrokeDashArray = new DoubleCollection(new double[] { 5.0, 5.0 });

            canvas.Children.Add(outline);
        }
    }
}
