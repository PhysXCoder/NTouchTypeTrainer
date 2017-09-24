using Caliburn.Micro;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Common.Primitives;
using NTouchTypeTrainer.Domain.Enums;
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var pressedKey = keyEventArgs.Key;  // To be more precise, that's the key that has been pressed and is now released
            var otherKeysPressed = EnumExtensions.GetValues<Key>()
                .Where(key => key != Key.None)
                .Where(key => (keyboardDevice.GetKeyStates(key) & KeyStates.Down) != 0)
                .ToList();

            var modifierKeys = otherKeysPressed.Where(_modifierKeys.ContainsKey).ToList();

            // Only valid if key is known!
            var valid = _keyMapping.ContainsKey(pressedKey);
            if (!valid) return;

            // Ignore other regular keys, but take into account modifiers
            var modifiers = modifierKeys
                .Select(key => _modifierKeys[key])
                .Aggregate(Modifier.None, (mod, acc) => mod | acc);
            var pressedConvertedKey = new HardwareKeyMappingTarget(_keyMapping[pressedKey], modifiers);

            _textExerciseViewModel.EvaluateInput(pressedConvertedKey);
        }

        private readonly Dictionary<Key, Modifier> _modifierKeys = new Dictionary<Key, Modifier>()
        {
            {Key.LeftCtrl, Modifier.Ctrl},
            {Key.RightCtrl, Modifier.Ctrl},
            {Key.LeftShift, Modifier.Shift},
            {Key.RightShift, Modifier.Shift},
            {Key.LeftAlt, Modifier.Alt},
            {Key.RightAlt, Modifier.AltGr},
        };

        private readonly Dictionary<Key, HardwareKey> _keyMapping = new Dictionary<Key, HardwareKey>()
        {
            {Key.None, HardwareKey.None},

            {Key.Oem5, HardwareKey.AccentGrave},        // German keyboard: ^
            {Key.D1, HardwareKey.D1},
            {Key.D2, HardwareKey.D2},
            {Key.D3, HardwareKey.D3},
            {Key.D4, HardwareKey.D4},
            {Key.D5, HardwareKey.D5},
            {Key.D6, HardwareKey.D6},
            {Key.D7, HardwareKey.D7},
            {Key.D8, HardwareKey.D8},
            {Key.D9, HardwareKey.D9},
            {Key.D0, HardwareKey.D0},
            {Key.OemOpenBrackets, HardwareKey.Minus},   // German keyboard: ß
            {Key.Oem6, HardwareKey.Equal},              // German keyboard: `
            {Key.Back, HardwareKey.Backspace},

            {Key.A, HardwareKey.A},
            {Key.B, HardwareKey.B},
            {Key.C, HardwareKey.C},
            {Key.D, HardwareKey.D},
            {Key.E, HardwareKey.E},
            {Key.F, HardwareKey.F},
            {Key.G, HardwareKey.G},
            {Key.H, HardwareKey.H},
            {Key.I, HardwareKey.I},
            {Key.J, HardwareKey.J},
            {Key.K, HardwareKey.K},
            {Key.L, HardwareKey.L},
            {Key.M, HardwareKey.M},
            {Key.N, HardwareKey.N},
            {Key.O, HardwareKey.O},
            {Key.P, HardwareKey.P},
            {Key.Q, HardwareKey.Q},
            {Key.R, HardwareKey.R},
            {Key.S, HardwareKey.S},
            {Key.T, HardwareKey.T},
            {Key.U, HardwareKey.U},
            {Key.V, HardwareKey.V},
            {Key.W, HardwareKey.W},
            {Key.X, HardwareKey.X},
            {Key.Y, HardwareKey.Y},
            {Key.Z, HardwareKey.Z},

            {Key.Tab, HardwareKey.Tab},
            {Key.Oem1, HardwareKey.BracketOpen},        // German keyboard: ü
            {Key.OemPlus, HardwareKey.BracketClose},    // German keyboard: +
            {Key.Return, HardwareKey.Enter},

            {Key.CapsLock, HardwareKey.CapsLock},
            {Key.Oem3, HardwareKey.Semicolon},          // German keyboard: ö
            {Key.OemQuotes, HardwareKey.Apostrophe},    // German keyboard: ä
            {Key.OemQuestion, HardwareKey.NumberSign},  // German keyboard: #

            {Key.LeftShift, HardwareKey.ShiftLeft},
            {Key.OemBackslash, HardwareKey.Backslash},  // German keyboard: <
            {Key.OemComma, HardwareKey.Comma},          // German keyboard: ,
            {Key.OemPeriod, HardwareKey.Dot},           // German keyboard: ,
            {Key.OemMinus, HardwareKey.Slash},          // German keyboard: -
            {Key.RightShift, HardwareKey.ShiftRight},

            {Key.LeftCtrl, HardwareKey.ControlLeft},
            {Key.LWin, HardwareKey.SuperLeft},
            {Key.LeftAlt, HardwareKey.Alt},
            {Key.Space, HardwareKey.Space},
            {Key.RightAlt, HardwareKey.AltGr},
            {Key.RWin, HardwareKey.SuperRight},
            {Key.Apps, HardwareKey.Menu},
            {Key.RightCtrl, HardwareKey.ControlRight},
        };

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