using Eto.Forms;
using NTouchTypeTrainer.Views;
using System;

namespace NTouchTypeTrainer
{
    internal class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();

            app.Run(new TestForm());
        }
    }
}
