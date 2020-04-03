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
using ColorPickerWPF;

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
        int size = 1;
        Color clr = Colors.Black;


        public MainWindow()
        {
            InitializeComponent();
            bmp.Clear(Colors.White);
            canvas.Source = bmp;

            //    Button button = new Button()
            //    {
            //        Content = "Григорий линия",
            //    Background = Brushes.Pink
            //  };

            //  button.Click += DrawTool;

            //    sp.Children.Add(button);
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

                if (_figure == 5 || _figure == 6)
                {
                    x1 = (int)currentPoint.X;
                    y1 = (int)currentPoint.Y;
                }
                else
                {
                    x = (int)currentPoint.X;
                    y = (int)currentPoint.Y;
                }

                _buf = bmp.Clone();

                draw = true;
            }
            else { }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //if (draw) {
            //    FileStream file = new FileStream("test.tga", FileMode.Create, FileAccess.ReadWrite);
            //    bmp.WriteTga(file); }
            draw = false;
        }

        WriteableBitmap _buf;

        private void Load()
        {
            bmp = _buf.Clone();
            canvas.Source = bmp;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            size = (int)(sender as Slider).Value;
        }

        private void Pltte(object sender, RoutedEventArgs e)
        {
            Color buf_c;
            bool buf_b = ColorPickerWindow.ShowDialog(out buf_c);
            if (buf_b)
            {
                clr = buf_c;
            }
        }

        //[Obsolete]
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                var canv = sender as Image;
                currentPoint = e.GetPosition(canv);

                if (_figure == 1 || _figure == 2)
                {
                    x1 = x < (int)currentPoint.X ? x : (int)currentPoint.X;
                    x2 = x > (int)currentPoint.X ? x : (int)currentPoint.X;
                    y1 = y < (int)currentPoint.Y ? y : (int)currentPoint.Y;
                    y2 = y > (int)currentPoint.Y ? y : (int)currentPoint.Y;
                }
                else if (_figure == 5 || _figure == 6)
                {
                    x2 = (int)currentPoint.X;
                    y2 = (int)currentPoint.Y;
                }
                else
                {
                    x1 = x;
                    y1 = y;
                    x2 = (int)currentPoint.X;
                    y2 = (int)currentPoint.Y;
                }

                if (_figure == 5 || _figure == 6) { } else { Load(); }
                //   bmp = BitmapFactory.New(650, 350).FromResource("\\test.tga");
                Drawing(_figure);

            }
            else { }
        }

        private void DrawTool(object sender, RoutedEventArgs e)
        {
            string name = (string)(sender as Button).Content;
            if (name == "Rectangle") { _figure = 1; }
            else if (name == "Ellipse") { _figure = 2; }
            else if (name == "Cut line") { _figure = 3; }
            else if (name == "Broken line") { _figure = 4; }
            else if (name == "Pencil") { _figure = 5; }
            else if (name == "Brush") { _figure = 6; }
            else { _figure = 3; }
        }

        private void Drawing(int _firure)
        {
            if (_figure == 1) { bmp.DrawRectangle(x1, y1, x2, y2, clr); }
            else if (_figure == 2) { bmp.DrawEllipse(x1, y1, x2, y2, clr); }
            else if (_figure == 3) { bmp.DrawLine(x1, y1, x2, y2, clr); }
            else if (_figure == 4) { } // bmp.DrawEllipse(x1, y1, x2, y2, Colors.Black);
            else if (_figure == 5) { bmp.DrawLine(x1, y1, x2, y2, clr); x1 = x2; y1 = y2; }
            else if (_figure == 6) { bmp.DrawLineAa(x1, y1, x2, y2, clr, size); x1 = x2; y1 = y2; }
            else { }
        }

    }
}
