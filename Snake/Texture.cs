using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake
{
    public static class Texture
    {

        public readonly static ImageSource Empty = Loadimage(" Empty.png");
        public readonly static ImageSource Body = Loadimage("Body.png");
        public readonly static ImageSource Head = Loadimage("Head.png");
        public readonly static ImageSource Food = Loadimage("Food.png");
        public readonly static ImageSource DeadHead = Loadimage("Deadhead.png");
        public readonly static ImageSource DeadBody = Loadimage("Deadbody.png");

            private static ImageSource Loadimage(string filename)
        {
            return new BitmapImage(new Uri($"assets/{filename}", UriKind.Relative));
        }
    }
}
