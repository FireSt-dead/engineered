using Geometry;
using UserInterface;
using UserInterface.Controls;

namespace DirectXTestApp
{
    public class ButtonTemplate : Tempalte
    {
        protected override Visual Create()
        {
            return new MyButton() { Margin = new Rect(3, 3, 3, 3) };
        }
    }
}
