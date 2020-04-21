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
        int x1 = 0, y1 = 0, x2 = 1, y2 = 1;
        int size = 1;
        Color clr = Colors.Black;

        List<Tool> tools = new List<Tool>();
        List<int> points = new List<int>();
        
        public MainWindow()
        {
            InitializeComponent();
            bmp.Clear(Colors.White);
            canvas.Source = bmp;

            //    Button button = new Button() // Добавление кнопок, может понадобиться для слоёв и истории
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
                if (tools[tools.Count - 1] == null)
                {
                }
                else if (tools[tools.Count - 1].tool_name == "Broken line" && x1 == x2 && y1 == y2)
                {
                    tools.Add(null);
                }
                else
                {
                    var canv = sender as Image;
                    currentPoint = e.GetPosition(canv);

                    if (tools[tools.Count - 1].tool_name == "Pencil" || tools[tools.Count - 1].tool_name == "Brush")
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
            }
            else { }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (tools[tools.Count - 1] != null && tools[tools.Count - 1].tool_name == "Broken line")
            {
                x1 = x2;
                y1 = y2;
                x2 = (int)currentPoint.X;
                y2 = (int)currentPoint.Y;
                Drawing(tools[tools.Count - 1].tool_name);
            }
            else
            {
                draw = false;
            }
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

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (draw && tools.Count != 0)
            {
                var canv = sender as Image;
                currentPoint = e.GetPosition(canv);



                if (tools[tools.Count - 1].tool_name == "Rectangle" || tools[tools.Count - 1].tool_name == "Ellipse")
                {
                    x1 = x < (int)currentPoint.X ? x : (int)currentPoint.X;
                    x2 = x > (int)currentPoint.X ? x : (int)currentPoint.X;
                    y1 = y < (int)currentPoint.Y ? y : (int)currentPoint.Y;
                    y2 = y > (int)currentPoint.Y ? y : (int)currentPoint.Y;
                }
                else if (tools[tools.Count - 1].tool_name == "Pencil" || tools[tools.Count - 1].tool_name == "Brush")
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

                if (tools[tools.Count - 1].tool_name == "Pencil" || tools[tools.Count - 1].tool_name == "Brush") { } else { Load(); }
                //   bmp = BitmapFactory.New(650, 350).FromResource("\\test.tga");
                // у конкретной фигуры должен быть уже прописанный конкретный способ рисования, вызывать последний созданный объект и рисовать на основании его названия?

                Drawing(tools[tools.Count - 1].tool_name); // Можно вместо 1 оставить перемнную, как счётчик для истории // Узнать про прозрачность цвета
            }
            else { }
        }

        private void Drawing(string tool_name) // enum для этого
        {
            switch (tool_name)
            {
                case "Rectangle":
                    bmp.FillRectangle(x1, y1, x2, y2, clr); // Сделать выпакдающую менюшку на 2 типа
                    break;
                case "Ellipse":
                    bmp.DrawEllipse(x1, y1, x2, y2, clr);
                    break;
                case "Cut line":
                    bmp.DrawLine(x1, y1, x2, y2, clr);
                    break;
                case "Broken line":
                    bmp.DrawLine(x1, y1, x2, y2, clr);
                    break;
                case "Pencil":
                    bmp.DrawLine(x1, y1, x2, y2, clr); x1 = x2; y1 = y2;
                    break;
                case "Brush":
                    bmp.DrawLineAa(x1, y1, x2, y2, clr, size); x1 = x2; y1 = y2;
                    break;
                default:
                    break;
            }
        }

        private void DrawTool(object sender, RoutedEventArgs e)
        {
            string name = (string)(sender as Button).Content;
            switch (name)
            {
                case "Rectangle":
                    Tool Rectangle = new Tool(points, clr, name);
                    tools.Add(Rectangle);
                    break;
                case "Ellipse":
                    Tool Ellipse = new Tool(points, clr, name);
                    tools.Add(Ellipse);
                    break;
                case "Cut line":
                    Tool Cut_Line = new Tool(points, clr, name);
                    tools.Add(Cut_Line);
                    break;
                case "Broken line":
                    Tool Broken_Line = new Tool(points, size, clr, name);
                    tools.Add(Broken_Line);
                    break;
                case "Pencil":
                    Tool Pencil = new Tool(points, clr, name);
                    tools.Add(Pencil);
                    break;
                case "Brush":
                    Tool Brush = new Tool(points, size, clr, name);
                    tools.Add(Brush);
                    break;
                default:
                    break;
            }
        }
    }
}

// Починить размер кисти
// Сделать историю и слои
// Добавить иконку цвета, который в данный момент используется
// Разобраться с оформлением
// Сделать привязку канваса к окну, привязать bmp к размеру канваса
//
//
