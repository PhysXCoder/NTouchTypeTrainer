using Caliburn.Micro;
using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Common.Graphics;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Interfaces.Common.Files;
using NTouchTypeTrainer.Interfaces.Views;
using NTouchTypeTrainer.Serialization;
using NTouchTypeTrainer.ViewModels;
using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private void TestForm_Shown(object sender, System.EventArgs e)
        {
            if (!_firstShown)
            {
                var fingerColors = LoadFingerColors();
                var fingerPos = LoadFingerPositions();
                var mechanicalLayout = LoadMechanicalLayout();
                var visualLayout = LoadVisualLayout();

                var keyboardViewModel = new KeyboardViewModel();
                keyboardViewModel.LoadFingerPositions(fingerPos);
                keyboardViewModel.LoadFingerColors(fingerColors);
                keyboardViewModel.LoadMechanicalLayout(mechanicalLayout);
                keyboardViewModel.LoadVisualKeyboardLayout(visualLayout);

                const bool highlightAll = false;
                if (highlightAll)
#pragma warning disable 162
                {
                    foreach (var key in keyboardViewModel.AllKeysViewModel.Keys.Values)
                    {
                        key.IsHighlighted = true;
                    }
                }
                else
                {
                    keyboardViewModel.AllKeysViewModel[new MappedCharacter('y')].IsHighlighted = true;
                    keyboardViewModel.AllKeysViewModel[new MappedCharacter('<')].IsHighlighted = true;
                    keyboardViewModel.AllKeysViewModel[new MappedCharacter('#')].IsHighlighted = true;
                    keyboardViewModel.AllKeysViewModel[new MappedCharacter('\\')].IsHighlighted = true;
                    keyboardViewModel.AllKeysViewModel[new MappedCharacter('E')].IsHighlighted = true;
                }
#pragma warning restore 162

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

        private VisualKeyboardLayout LoadVisualLayout()
        {
            var fileToOpen = _dialogProvider.OpenFile("Select visual layout file", "Visual layout file", "*.t3v", this);
            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return VisualKeyboardLayoutImporter.Import(exportedString);
            }

            return null;
        }
    }
}