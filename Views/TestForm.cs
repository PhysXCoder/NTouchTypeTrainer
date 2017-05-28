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

        private KeyboardViewModel _keyboardViewModel;
        private TextExerciseViewModel _textExerciseViewModel;

        private KeyboardView _keyboardView;
        private TextExerciseView _textExerciseView;

        public TestForm()
        {
            ClientSize = new Size(600, 400);
            Title = "NTouchTypingTrainer test form";

            _eventAggregator = new EventAggregator();
            _graphicsProvider = new GraphicsProvider();
            _dialogProvider = new DialogProvider();
            _stringFileReaderWriter = new StringFileReaderWriter(new FileStreamProvider());

            Shown += TestForm_Shown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            var modifiers = keyEventArgs.Modifiers;
            var key = keyEventArgs.Key;
            var x = keyEventArgs.IsChar ? (char?)keyEventArgs.KeyChar : null;   // Don't use this, that's wrong

            // Problem: eto.forms ignores keys like "ö" that aren't accounted for in its key enum...

            keyEventArgs.Handled = true;
        }

        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private void TestForm_Shown(object sender, EventArgs e)
        {
            if (!_firstShown)
            {
                const bool showDialogs = false;
                const bool highlightAll = false;

                _keyboardViewModel = LoadKeyboardViewModel(showDialogs);
                _textExerciseViewModel = LoadExerciseViewModel(true);
                SetContent();

                HighlightKeys(highlightAll);

                _firstShown = true;
            }
        }

        private void SetContent()
        {
            var verticalStack = new StackLayout
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };

            _keyboardView = new KeyboardView(_eventAggregator, _graphicsProvider)
            {
                DataContext = _keyboardViewModel
            };
            verticalStack.Items.Add(_keyboardView);

            _textExerciseView = new TextExerciseView
            {
                Width = 900,
                Height = 400,
                DataContext = _textExerciseViewModel
            };
            verticalStack.Items.Add(_textExerciseView);

            Content = verticalStack;
        }

        private void HighlightKeys(bool highlightAll)
        {
            if (highlightAll)
#pragma warning disable 162
            {
                foreach (var key in _keyboardViewModel.AllKeysViewModel.KeysByPosition.Values)
                {
                    key.IsHighlighted = true;
                }
            }
            else
            {
                _keyboardViewModel.AllKeysViewModel[new MappedCharacter('y')].IsHighlighted = true;
                _keyboardViewModel.AllKeysViewModel[new MappedCharacter('<')].IsHighlighted = true;
                _keyboardViewModel.AllKeysViewModel[new MappedCharacter('#')].IsHighlighted = true;
                _keyboardViewModel.AllKeysViewModel[new MappedCharacter('\\')].IsHighlighted = true;
                _keyboardViewModel.AllKeysViewModel[new MappedCharacter('E')].IsHighlighted = true;
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

        private TextExerciseViewModel LoadExerciseViewModel(bool showDialogs)
        {
            var exercise = LoadExercise(showDialogs);

            var viewModel = new TextExerciseViewModel();
            viewModel.SetExercise(exercise);

            return viewModel;
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