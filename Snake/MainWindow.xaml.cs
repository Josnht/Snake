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

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Map, ImageSource> gridToIMG = new()
        {
            {Map.Empty, Texture.Empty },
            {Map.Food, Texture.Food },
            {Map.Snake, Texture.Body }
        };

        private readonly Dictionary<Direction, int> dirRotation = new()
        {
            {Direction.Up, 0},
            {Direction.Down, 180 },
            {Direction.Left,  270},
            { Direction.Right, 90}
        };

        private readonly int rows =15, cols =15;
        private readonly Image[,] gridImages;
        private State state;
        private bool Running;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = GridSetUp();
            state = new State (rows, cols);
        }
    private async Task Loading ()
        {
            Load();
            await Countdown();
            Over1ay.Visibility = Visibility.Hidden;
            await Loop();
            await GameOver();
            state = new State (rows, cols);
        }

    private async void Windown_Previewkeydown (object sender, KeyEventArgs e)
        {
            if(Over1ay.Visibility == Visibility.Visible)
            {
                e.Handled= true;
            }
            if(!Running)
            {
                Running=true;
                await Loading();
                Running=false;
            }
        }
    private void Windown_Keydown (object sender, KeyEventArgs e)
        {
            if (state.gameover)
            {
                return;
            } switch (e.Key)
            {
                case Key.Left:
                    state.ChangeDirection(Direction.Left); 
                    break;       
                case Key.Right:
                    state.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    state.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    state.ChangeDirection(Direction.Down);
                    break;
            }
        }

    private async Task Loop ()
        {
            while (!state.gameover)
            {
                await Task.Delay(100);
                state.Move();
                Load();
            }
        }

    private Image[,] GridSetUp()
        {
            Image[,] images = new Image[rows, cols];        
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            for (int r= 0; r < rows; r++)
            {
                for(int c= 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Texture.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

    private void Load()
    {
            DrawGrid();
            SnakeHead();
            ScoreText.Text= $"SCORE {state.score}";
    }
    private void DrawGrid()
    {
        for(int r= 0; r < rows; ++r)
        {
            for (int c= 0;c < cols; c++)
            {
                    Map Gridval = state.Grid[r, c];
                gridImages[r, c].Source = gridToIMG[Gridval];
                gridImages[r, c].RenderTransform = Transform.Identity;
            }
        }
    }

    private void SnakeHead()
        {
            Position headpos = state.HeadPosition();
            Image image = gridImages[headpos.Row, headpos.Col];
            image.Source = Texture.Head;

            int Rotation = dirRotation[state.Dir];
            image.RenderTransform = new RotateTransform(Rotation);
        }
    private async Task SnakeHeadDead()
        {
            List<Position> positions = new List<Position>(state.SnakePosition());
            for (int i = 0; i < positions.Count; ++i) {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Texture.DeadHead : Texture.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
                    }
        }

    private async Task Countdown()
        {
            for (int i=0; i < 3; ++i)
            {
                Over1ayText.Text=i.ToString();
                await Task.Delay(1000);
            }
        }
     private async Task GameOver()
        {
            await SnakeHeadDead();
            await Task.Delay(1000);
            Over1ay.Visibility = Visibility.Visible;
            Over1ayText.Text = "PRESS ANY KEY TO START";
        }  
    }
}
