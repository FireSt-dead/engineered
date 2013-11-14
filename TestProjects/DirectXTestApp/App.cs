using Geometry;
using UserInterface;
using UserInterface.Controls;
using UserInterface.Layout;
using UserInterface.Primitives;
using WindowsOS;
using WindowsUI;

namespace DirectXTestApp
{
    public class App
    {
        public static int Main(string[] args)
        {
            Window win = new Window();

            var canvas = new VisualContainer();
            win.Visual = canvas;
            canvas.Children.Add(new Rectangle() { RadiusX = 5, RadiusY = 5, Rect = new Rect() { Left = 300, Top = 100, Right = 400, Bottom = 200 } });
            canvas.Children.Add(new Rectangle() { Rect = new Rect() { Left = 400, Top = 250, Right = 500, Bottom = 350 } });

            HorizontalStack hs = new HorizontalStack();
            hs.VerticalAlignment = Alignment.ToEnd;
            //hs.Children.Add(new Border() { Name = "H1", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });
            //hs.Children.Add(new Border() { Name = "H2", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });
            //hs.Children.Add(new Border() { Name = "H3", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });

            MyButton btn = new MyButton();
            hs.Children.Add(btn);
            HorizontalStack btnHStack = new HorizontalStack();
            btn.Content = btnHStack;
            btnHStack.Children.Add(new MyButton() { Name = "H1", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });
            btnHStack.Children.Add(new MyButton() { Name = "H2", Margin = new Rect(1) });
            btnHStack.Children.Add(new TextField() { Text = "T3", Margin = new Rect(3, 3, 3, 3) });
            btnHStack.Children.Add(new MyButton() { Name = "H4", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });

            VerticalStack vs = new VerticalStack();
            hs.Children.Add(vs);
            //vs.Children.Add(new Border() { Name = "V1", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });
            //vs.Children.Add(new Border() { Name = "V2", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });
            //vs.Children.Add(new Border() { Name = "V3", VerticalAlignment = Alignment.Center, Margin = new Rect(1) });

            vs.Children.Add(new MyButton() { Name = "V1", Margin = new Rect(1) });
            vs.Children.Add(new TextField() { Text = "T3", Margin = new Rect(3, 3, 3, 3) });
            vs.Children.Add(new MyButton() { Name = "V2", Margin = new Rect(1) });
            vs.Children.Add(new MyButton() { Name = "V4", Margin = new Rect(1) });

            canvas.Children.Add(hs);

            //hs.Children.Add(new Border() { Name = "L", VerticalAlignment = Alignment.ToEnd, Margin = new Rect(1) });
            hs.Children.Add(new MyButton() { Name = "L", VerticalAlignment = Alignment.ToEnd, Margin = new Rect(1) });

            hs.Children.Add(new TextField() { Text = "Hello World", Margin = new Rect(3, 3, 3, 3) });
            hs.Children.Add(new TextField() { Text = "TB2", Margin = new Rect(3, 3, 3, 3) });
            hs.Children.Add(new TextField() { Text = "Text Block 3", Margin = new Rect(3, 3, 3, 3) });


            //
            VerticalStack verticalButtons = new VerticalStack() { HorizontalAlignment = Alignment.ToStart, VerticalAlignment = Alignment.ToStart };
            var template = new ButtonTemplate();
            verticalButtons.Children.Add(new Control() { Template = template });
            verticalButtons.Children.Add(new Control() { Template = template });
            verticalButtons.Children.Add(new Control() { Template = template });
            canvas.Children.Add(verticalButtons);

            win.Show();

            Dispatcher.Current.Run();

            return 0;
        }
    }
}
