using Caliburn.Micro;
using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Common.Graphics;

namespace NTouchTypeTrainer.Views
{
    public class TestForm : Form
    {
        public TestForm()
        {
            ClientSize = new Size(600, 400);
            Title = "NTouchTypingTrainer test form";

            var eventAggregator = new EventAggregator();
            var graphicsProvider = new GraphicsProvider();

            var keyboardView = new KeyboardView(eventAggregator, graphicsProvider);
            Content = keyboardView;
        }
    }
}