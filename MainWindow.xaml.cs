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
using System.IO;

namespace GraphEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point();
        WriteableBitmap bmp = BitmapFactory.New(650, 350);

        bool draw = false;
        int x, y = 0;
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        int _figure = 0;

        // WriteableBitmap map = new WriteableBitmap();

        public MainWindow()
        {
            InitializeComponent();
            bmp.Clear(Colors.White);
            canvas.Source = bmp;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            bmp.Clear(Colors.White);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var canv = sender as Image;
                currentPoint = e.GetPosition(canv);
                x = (int)currentPoint.X;
                y = (int)currentPoint.Y;

                draw = true;
            }
            else { }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //if (draw) {
            //    FileStream file = new FileStream("\\test.tga", FileMode.Create, FileAccess.ReadWrite);
            //    bmp.WriteTga(file); }
            draw = false;
        }

        //[Obsolete]
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                var canv = sender as Image;
                currentPoint = e.GetPosition(canv);

                x1 = x < (int)currentPoint.X ? x : (int)currentPoint.X;
                x2 = x > (int)currentPoint.X ? x : (int)currentPoint.X;
                y1 = y < (int)currentPoint.Y ? y : (int)currentPoint.Y;
                y2 = y > (int)currentPoint.Y ? y : (int)currentPoint.Y;

                bmp.Clear(Colors.White);
             //   bmp = BitmapFactory.New(650, 350).FromResource("\\test.tga");
                Drawing(_figure);

            }
            else { }
        }

        private void DrawTool(object sender, RoutedEventArgs e)
        {   
            switch ((sender as Button).Content)
            {
                case "Rectangle":
                    _figure = 1;
                    break;
                case "Ellipse":
                    _figure = 2;
                    break;
                case "Cut line":
                    _figure = 3;
                    break;
                case "Broken line":
                    _figure = 4;
                    break;
            }
        }   

        private void Drawing(int _firure)
        {
            switch (_figure)
            {
                case 1:
                    bmp.DrawRectangle(x1, y1, x2, y2, Colors.Black);
                    break;
                case 2:
                    bmp.DrawEllipse(x1, y1, x2, y2, Colors.Black);
                    break;
                case 3:
                    // bmp.DrawEllipse(x1, y1, x2, y2, Colors.Black);
                    bmp.DrawLine(x1, y1, x2, y2, Colors.Black);
                    break;
                case 4:
                    // bmp.DrawEllipse(x1, y1, x2, y2, Colors.Black);
                    break;
                default:
                    break;
            }
        }
    }
}
