using Caliburn.Micro;
using Eto.Drawing;
using Eto.Forms;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Common.Graphics;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Interfaces.Common.Files;
using NTouchTypeTrainer.Interfaces.Domain;
using NTouchTypeTrainer.Interfaces.Views;
using NTouchTypeTrainer.Serialization;
using NTouchTypeTrainer.ViewModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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
        private void TestForm_Shown(object sender, EventArgs e)
        {
            if (!_firstShown)
            {
                const bool showDialogs = false;
                const bool highlightAll = false;

                var keyboardViewModel = LoadKeyboardViewModel(showDialogs);
                SetDataContext(keyboardViewModel);

                HighlightKeys(highlightAll, keyboardViewModel);

                var exercise = LoadExercise(showDialogs);
                var sequence = exercise.Sequence;

                _firstShown = true;
            }
        }

        private void SetDataContext(KeyboardViewModel keyboardViewModel)
        {
            var keyboardView = new KeyboardView(_eventAggregator, _graphicsProvider)
            {
                DataContext = keyboardViewModel
            };
            Content = keyboardView;
        }

        private static void HighlightKeys(bool highlightAll, KeyboardViewModel keyboardViewModel)
        {
            if (highlightAll)
#pragma warning disable 162
            {
                foreach (var key in keyboardViewModel.AllKeysViewModel.KeysByPosition.Values)
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
        }

        private KeyboardViewModel LoadKeyboardViewModel(bool showDialogs)
        {
            var fingerColors = LoadFingerColors(showDialogs);
            var fingerPos = LoadFingerPositions(showDialogs);
            var mechanicalLayout = LoadMechanicalLayout(showDialogs);
            var visualLayout = LoadVisualLayout(showDialogs);

            var keyboardViewModel = new KeyboardViewModel();
            keyboardViewModel.LoadFingerPositions(fingerPos);
            keyboardViewModel.LoadFingerColors(fingerColors);
            keyboardViewModel.LoadMechanicalLayout(mechanicalLayout);
            keyboardViewModel.LoadVisualKeyboardLayout(visualLayout);
            return keyboardViewModel;
        }

        private static DirectoryInfo DocumentsDirectory
            => new DirectoryInfo(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    + Path.DirectorySeparatorChar);

        private IFingerColors LoadFingerColors(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile("Select FingerColors file", "FingerColors file", "*.t3c", this)
                : new FileInfo(DocumentsDirectory + "DefaultFingerColors.t3c");

            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return FingerColorsImporter.Import(exportedString);
            }

            return null;
        }

        private FingerPositions LoadFingerPositions(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile("Select FingerPositions file", "FingerPositions file", "*.t3p", this)
                : new FileInfo(DocumentsDirectory + "DefaultFingerPositions.t3p");

            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return FingerPositionsImporter.Import(exportedString);
            }

            return null;
        }

        private MechanicalKeyboardLayout LoadMechanicalLayout(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile("Select mechanical layout file", "Mechanical layout file", "*.t3m", this)
                : new FileInfo(DocumentsDirectory + "DefaultMechanicalLayout.t3m");

            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return MechanicalKeyboardLayoutImporter.Import(exportedString);
            }

            return null;
        }

        private VisualKeyboardLayout LoadVisualLayout(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile("Select visual layout file", "Visual layout file", "*.t3v", this)
                : new FileInfo(DocumentsDirectory + "GermanVisualLayout.t3v");

            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return VisualKeyboardLayoutImporter.Import(exportedString);
            }

            return null;
        }

        private IExercise LoadExercise(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile("Select exercise file", "Exercise file", "*.txt", this)
                : new FileInfo(DocumentsDirectory + ((new Random().Next(2) == 1) ? "FixedExercise.txt" : "RandomizedExercise.txt"));

            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return ExerciseImporter.Import(exportedString);
            }

            return null;
        }
    }
}