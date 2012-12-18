using NUnit.Gui;
using System;
using System.Windows.Forms;

namespace _Tests
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            AppEntry.Main(new string[] { Application.ExecutablePath });
        }
    }
}
