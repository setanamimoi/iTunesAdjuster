using System;
using System.Windows;

namespace iTunesAdjuster
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            new Application().Run(new MainWindow());
        }
    }
}
