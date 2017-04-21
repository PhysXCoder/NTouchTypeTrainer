using Caliburn.Micro;
using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Common.Graphics;
using NTouchTypeTrainer.Contracts.Common.Files;
using NTouchTypeTrainer.Contracts.Views;
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
        private readonly IDialogProvider _dialogProvider;
        private readonly IFileReaderWriter<string> _stringFileReaderWriter;

        public TestForm()
        {
            ClientSize = new Size(600, 400);
            Title = "NTouchTypingTrainer test form";

            _eventAggregator = new EventAggregator();
            _graphicsProvider = new GraphicsProvider();
            _dialogProvider = new DialogProvider();
            _stringFileReaderWriter = new StringFileReaderWriter(new FileStreamProvider());

            Shown += TestForm_Shown;
        }

        private void TestForm_Shown(object sender, System.EventArgs e)
        {
            if (!_firstShown)
            {
                var fingerColors = LoadFingerColors();
                var fingerPos = LoadFingerPositions();
                var mechanicalLayout = LoadMechanicalLayout();

                var keyboardViewModel = new KeyboardViewModel();
                keyboardViewModel.LoadFingerPositions(fingerPos);
                keyboardViewModel.LoadFingerColors(fingerColors);
                keyboardViewModel.LoadMechanicalLayout(mechanicalLayout);

                keyboardViewModel.AllKeysViewModel.Keys[new KeyPosition(3, 6)].IsHighlighted = true;
                keyboardViewModel.AllKeysViewModel.Keys[new KeyPosition(4, 3)].IsHighlighted = true;
                keyboardViewModel.AllKeysViewModel.Keys[new KeyPosition(1, 12)].IsHighlighted = true;

                var keyboardView = new KeyboardView(_eventAggregator, _graphicsProvider)
                {
                    DataContext = keyboardViewModel
                };
                Content = keyboardView;
            }

            _firstShown = true;
        }

        private FingerColors LoadFingerColors()
        {
            var fileToOpen = _dialogProvider.OpenFile("Select FingerColors file", "FingerColors file", "*.t3c", this);
            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return FingerColorsImporter.Import(exportedString);
            }

            return null;
        }

        private FingerPositions LoadFingerPositions()
        {
            var fileToOpen = _dialogProvider.OpenFile("Select FingerPositions file", "FingerPositions file", "*.t3p", this);
            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return FingerPositionsImporter.Import(exportedString);
            }

            return null;
        }

        private MechanicalKeyboardLayout LoadMechanicalLayout()
        {
            var fileToOpen = _dialogProvider.OpenFile("Select mechanical layout file", "Mechanical layout file", "*.t3m", this);
            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return MechanicalKeyboardLayoutImporter.Import(exportedString);
            }

            return null;
        }
    }
}