using Caliburn.Micro;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Domain.Exercises;
using NTouchTypeTrainer.Domain.Keyboard;
using NTouchTypeTrainer.Domain.Keyboard.Keys.MappingTargets;
using NTouchTypeTrainer.Interfaces.Common.Files;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using NTouchTypeTrainer.Interfaces.Domain.Exercises;
using NTouchTypeTrainer.Interfaces.Domain.Keyboard;
using NTouchTypeTrainer.Interfaces.View;
using NTouchTypeTrainer.Messages;
using NTouchTypeTrainer.Serialization;
using System;
using System.IO;
using System.Windows.Input;

namespace NTouchTypeTrainer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IHandle<ExpectedKeyChangedMsg>
    {
        private readonly IFileReaderWriter<string> _stringFileReaderWriter;
        private readonly IDialogProvider _dialogProvider;

        private TextExerciseViewModel _textExerciseViewModel;
        private KeyboardViewModel _keyboardViewModel;

        public KeyboardViewModel KeyboardViewModel
        {
            get => _keyboardViewModel;
            set
            {
                _keyboardViewModel = value;
                NotifyOfPropertyChange();
            }
        }

        public TextExerciseViewModel TextExerciseViewModel
        {
            get => _textExerciseViewModel;
            set
            {
                _textExerciseViewModel = value;
                NotifyOfPropertyChange();
            }
        }

        // This is only for the WPF Designer
        /*internal MainWindowViewModel()
            : this(new DefaultThemeProvider(), new SharedSizeGroup(new EventAggregator()), new EventAggregator())
        { }*/

        public MainWindowViewModel(
            IEventAggregator eventAggregator,
            IThemeProvider themeProvider,
            IDialogProvider dialogProvider,
            IFileReaderWriter<string> stringFileReaderWriter,
            ISizeGroup sizeGroup)
        {
            _dialogProvider = dialogProvider;
            _stringFileReaderWriter = stringFileReaderWriter;

            _keyboardViewModel = new KeyboardViewModel(themeProvider, sizeGroup, eventAggregator);
            _textExerciseViewModel = new TextExerciseViewModel(themeProvider, eventAggregator);

            eventAggregator.Subscribe(this);   // ToDo: is this necessary after everything ocmes from Dependency injection?

            LoadKeyboardViewModel(false);
            var execise = LoadExerciseViewModel(true);
            var runningExercise = new RunningExercise(execise);
            _textExerciseViewModel.SetRunningExercise(runningExercise);
        }

        public void Handle(ExpectedKeyChangedMsg message)
        {
            if (_textExerciseViewModel.Equals(message.Sender))
            {
                var expectedMappingTarget = message.NewExpectedMappingTarget;

                _keyboardViewModel.HighlightExpectedKeys(expectedMappingTarget);
            }
        }

        public void KeyDown(ActionExecutionContext ctx)
        {
            var keyEventArgs = (KeyEventArgs)ctx.EventArgs;
            if (!(keyEventArgs?.Device is KeyboardDevice keyboardDevice))
            {
                // ToDo: Logging
                return;
            }

            var pressedConvertedKey = WpfKeyConverter.ConvertPressedKey(keyEventArgs, keyboardDevice);

            if (pressedConvertedKey != null)
            {
                _textExerciseViewModel.EvaluateInput(pressedConvertedKey);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void HighlightKeys(bool highlightAll)
        {
            if (highlightAll)
            {
                foreach (var key in _keyboardViewModel)
                {
                    key.IsHighlighted = true;
                }
            }
            else
            {
                _keyboardViewModel[new CharacterMappingTarget('y')].IsHighlighted = true;
                _keyboardViewModel[new CharacterMappingTarget('<')].IsHighlighted = true;
                _keyboardViewModel[new CharacterMappingTarget('#')].IsHighlighted = true;
                _keyboardViewModel[new CharacterMappingTarget('\\')].IsHighlighted = true;
                _keyboardViewModel[new CharacterMappingTarget('E')].IsHighlighted = true;
            }
        }

        private void LoadKeyboardViewModel(bool showDialogs)
        {
            var fingerColors = LoadFingerColors(showDialogs);
            var fingerPos = LoadFingerPositions(showDialogs);
            var mechanicalLayout = LoadMechanicalLayout(showDialogs);
            var visualLayout = LoadVisualLayout(showDialogs);

            _keyboardViewModel.LoadFingerPositions(fingerPos);
            _keyboardViewModel.LoadFingerColors(fingerColors);
            _keyboardViewModel.LoadMechanicalLayout(mechanicalLayout);
            _keyboardViewModel.LoadVisualKeyboardLayout(visualLayout);
        }

        private static DirectoryInfo DocumentsDirectory
        => new DirectoryInfo(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar
            + "NT3" + Path.DirectorySeparatorChar);

        // ToDo: Correct place for loading functions?
        private IFingerColors LoadFingerColors(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile(
                    "Select FingerColors file",
                    new[] { new FileFilter("FingerColors file", "t3c"), FileFilter.AllFiles },
                    DocumentsDirectory)
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
                ? _dialogProvider.OpenFile(
                    "Select FingerPositions file",
                    new[] { new FileFilter("FingerPositions file", "t3p"), FileFilter.AllFiles },
                    DocumentsDirectory)
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
                ? _dialogProvider.OpenFile(
                    "Select mechanical layout file",
                    new[] { new FileFilter("Mechanical layout file", "t3m"), FileFilter.AllFiles },
                    DocumentsDirectory)
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
                ? _dialogProvider.OpenFile(
                    "Select visual layout file",
                    new[] { new FileFilter("Visual layout file", "t3v"), FileFilter.AllFiles },
                    DocumentsDirectory)
                : new FileInfo(DocumentsDirectory + "GermanVisualLayout.t3v");

            if (fileToOpen != null)
            {
                var exportedString = _stringFileReaderWriter.Read(fileToOpen);
                return VisualKeyboardLayoutImporter.Import(exportedString);
            }

            return null;
        }

        private IExercise LoadExerciseViewModel(bool showDialogs)
        {
            var exercise = LoadExercise(showDialogs);
            return exercise;
        }

        private IExercise LoadExercise(bool showDialog = true)
        {
            var fileToOpen = showDialog
                ? _dialogProvider.OpenFile(
                    "Select exercise file",
                    new[] { new FileFilter("Exercise file", "txt"), FileFilter.AllFiles },
                    DocumentsDirectory)
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