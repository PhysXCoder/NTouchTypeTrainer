using System.IO;
using Caliburn.Micro;
using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Common.Graphics;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Serialization;
using NTouchTypeTrainer.ViewModels;

namespace NTouchTypeTrainer.Views
{
    public class TestForm : Form
    {
        private bool _firstShown;
        private readonly IEventAggregator _eventAggregator;
        private readonly GraphicsProvider _graphicsProvider;

        public TestForm()
        {
            ClientSize = new Size(600, 400);
            Title = "NTouchTypingTrainer test form";

            _eventAggregator = new EventAggregator();
            _graphicsProvider = new GraphicsProvider();

            Shown += TestForm_Shown;
        }

        private void TestForm_Shown(object sender, System.EventArgs e)
        {
            if (!_firstShown)
            {
                var fingerColors = LoadFingerColors();
                var fingerPos = LoadFingerPositions();

                var fingerPosViewModel = new KeyboardFingerPositionsViewModel();
                fingerPosViewModel.LoadFingerPositions(fingerPos);
                fingerPosViewModel.LoadFingerColors(fingerColors);

                fingerPosViewModel.GKeyViewModel.IsHighlighted = true;

                var keyboardView = new KeyboardView(_eventAggregator, _graphicsProvider)
                {
                    DataContext = fingerPosViewModel
                };
                Content = keyboardView;
            }

            _firstShown = true;
        }

        private FingerColors LoadFingerColors()
        {
            var fingerColors = new FingerColors();

            var ofd = new OpenFileDialog()
            {
                Title = "Select FingerColors file"
            };
            if (ofd.ShowDialog(this) == DialogResult.Ok)
            {
                using (var reader = new StreamReader(ofd.FileName))
                {
                    var exportedString = reader.ReadToEnd();
                    fingerColors = fingerColors.Import(exportedString);
                }

                return fingerColors;
            }

            return null;
        }

        private FingerPositions LoadFingerPositions()
        {
            var ofd = new OpenFileDialog()
            {
                Title = "Select FingerPositions file"
            };

            if (ofd.ShowDialog(this) == DialogResult.Ok)
            {
                using (var reader = new StreamReader(ofd.FileName))
                {
                    var exportedString = reader.ReadToEnd();
                    return (new FingerPositionsImporter()).Import(exportedString);
                }
            }

            return null;
        }
    }
}