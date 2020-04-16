using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace GraphEditor
{
     class ToolPanel
    {
        public string tool_name;
        public Color clr;
        public List<int> points = new List<int>();
    }

    class Tool : ToolPanel
    {
        public int size;

        public Tool(List<int> points, Color clr, string figure)
        {
            tool_name = figure;
            this.clr = clr;
            this.points = points;
        }

        public Tool(List<int> points, int size, Color clr, string brush)
        {
            tool_name = brush;
            this.clr = clr;
            this.points = points;
            this.size = size;
        }

        // Записывать полученный цвет, чтобы можно было его менять у отдельной фигуры при обращении к ней
        // Запоминать координаты после рисования этой фигуры (обращаться к текущему эл. в листе и перепрописывать 4 координаты)
        // Что-нибудь придумать с рисованием // Или ничего не придумывать
        // Лист листов, чтобы запоминать массив точек конкретной фигуры
    }
}
