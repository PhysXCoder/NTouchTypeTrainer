using Eto.Drawing;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.ViewModels
{
    public class KeyboardFingerPositionsViewModel : BaseViewModel
    {
        private KeyViewModel _accentGraveKeyViewModel;
        private KeyViewModel _d1KeyViewModel;
        private KeyViewModel _d2KeyViewModel;
        private KeyViewModel _d3KeyViewModel;
        private KeyViewModel _d4KeyViewModel;
        private KeyViewModel _d5KeyViewModel;
        private KeyViewModel _d6KeyViewModel;
        private KeyViewModel _d7KeyViewModel;
        private KeyViewModel _d8KeyViewModel;
        private KeyViewModel _d9KeyViewModel;
        private KeyViewModel _d0KeyViewModel;
        private KeyViewModel _minusKeyViewModel;
        private KeyViewModel _equalKeyViewModel;
        private KeyViewModel _backspaceKeyViewModel;

        private KeyViewModel _tabKeyViewModel;
        private KeyViewModel _qKeyViewModel;
        private KeyViewModel _wKeyViewModel;
        private KeyViewModel _eKeyViewModel;
        private KeyViewModel _rKeyViewModel;
        private KeyViewModel _tKeyViewModel;
        private KeyViewModel _yKeyViewModel;
        private KeyViewModel _uKeyViewModel;
        private KeyViewModel _iKeyViewModel;
        private KeyViewModel _oKeyViewModel;
        private KeyViewModel _pKeyViewModel;
        private KeyViewModel _bracketOpenKeyViewModel;
        private KeyViewModel _bracketCloseKeyViewModel;
        private KeyViewModel _enterKeyViewModel;

        private KeyViewModel _capsLockKeyViewModel;
        private KeyViewModel _aKeyViewModel;
        private KeyViewModel _sKeyViewModel;
        private KeyViewModel _dKeyViewModel;
        private KeyViewModel _fKeyViewModel;
        private KeyViewModel _gKeyViewModel;
        private KeyViewModel _hKeyViewModel;
        private KeyViewModel _jKeyViewModel;
        private KeyViewModel _kKeyViewModel;
        private KeyViewModel _lKeyViewModel;
        private KeyViewModel _semicolonKeyViewModel;
        private KeyViewModel _apostropheKeyViewModel;
        private KeyViewModel _numberSignKeyViewModel;

        private KeyViewModel _shiftLeftKeyViewModel;
        private KeyViewModel _backslashKeyViewModel;
        private KeyViewModel _zKeyViewModel;
        private KeyViewModel _xKeyViewModel;
        private KeyViewModel _cKeyViewModel;
        private KeyViewModel _vKeyViewModel;
        private KeyViewModel _bKeyViewModel;
        private KeyViewModel _nKeyViewModel;
        private KeyViewModel _mKeyViewModel;
        private KeyViewModel _commaKeyViewModel;
        private KeyViewModel _dotKeyViewModel;
        private KeyViewModel _slashKeyViewModel;
        private KeyViewModel _shiftRightKeyViewModel;

        private KeyViewModel _controlLeftKeyViewModel;
        private KeyViewModel _superLeftKeyViewModel;
        private KeyViewModel _altKeyViewModel;
        private KeyViewModel _spaceKeyViewModel;
        private KeyViewModel _altGrKeyViewModel;
        private KeyViewModel _superRightKeyViewModel;
        private KeyViewModel _menuKeyViewModel;
        private KeyViewModel _controlRightViewModel;

        private IFingerPositions _fingerPositions;
        private IFingerColors _fingerColors;

        public KeyViewModel AccentGraveKeyViewModel
        {
            get => _accentGraveKeyViewModel;
            set => SetProperty(ref _accentGraveKeyViewModel, value);
        }

        public KeyViewModel D1KeyViewModel
        {
            get => _d1KeyViewModel;
            set => SetProperty(ref _d1KeyViewModel, value);
        }

        public KeyViewModel D2KeyViewModel
        {
            get => _d2KeyViewModel;
            set => SetProperty(ref _d2KeyViewModel, value);
        }

        public KeyViewModel D3KeyViewModel
        {
            get => _d3KeyViewModel;
            set => SetProperty(ref _d3KeyViewModel, value);
        }

        public KeyViewModel D4KeyViewModel
        {
            get => _d4KeyViewModel;
            set => SetProperty(ref _d4KeyViewModel, value);
        }

        public KeyViewModel D5KeyViewModel
        {
            get => _d5KeyViewModel;
            set => SetProperty(ref _d5KeyViewModel, value);
        }

        public KeyViewModel D6KeyViewModel
        {
            get => _d6KeyViewModel;
            set => SetProperty(ref _d6KeyViewModel, value);
        }

        public KeyViewModel D7KeyViewModel
        {
            get => _d7KeyViewModel;
            set => SetProperty(ref _d7KeyViewModel, value);
        }

        public KeyViewModel D8KeyViewModel
        {
            get => _d8KeyViewModel;
            set => SetProperty(ref _d8KeyViewModel, value);
        }

        public KeyViewModel D9KeyViewModel
        {
            get => _d9KeyViewModel;
            set => SetProperty(ref _d9KeyViewModel, value);
        }

        public KeyViewModel D0KeyViewModel
        {
            get => _d0KeyViewModel;
            set => SetProperty(ref _d0KeyViewModel, value);
        }

        public KeyViewModel MinusKeyViewModel
        {
            get => _minusKeyViewModel;
            set => SetProperty(ref _minusKeyViewModel, value);
        }

        public KeyViewModel EqualKeyViewModel
        {
            get => _equalKeyViewModel;
            set => SetProperty(ref _equalKeyViewModel, value);
        }

        public KeyViewModel BackspaceKeyViewModel
        {
            get => _backspaceKeyViewModel;
            set => SetProperty(ref _backspaceKeyViewModel, value);
        }

        public KeyViewModel TabKeyViewModel
        {
            get => _tabKeyViewModel;
            set => SetProperty(ref _tabKeyViewModel, value);
        }

        public KeyViewModel QKeyViewModel
        {
            get => _qKeyViewModel;
            set => SetProperty(ref _qKeyViewModel, value);
        }

        public KeyViewModel WKeyViewModel
        {
            get => _wKeyViewModel;
            set => SetProperty(ref _wKeyViewModel, value);
        }

        public KeyViewModel EKeyViewModel
        {
            get => _eKeyViewModel;
            set => SetProperty(ref _eKeyViewModel, value);
        }

        public KeyViewModel RKeyViewModel
        {
            get => _rKeyViewModel;
            set => SetProperty(ref _rKeyViewModel, value);
        }

        // ReSharper disable once InconsistentNaming
        public KeyViewModel TKeyViewModel
        {
            get => _tKeyViewModel;
            set => SetProperty(ref _tKeyViewModel, value);
        }

        public KeyViewModel YKeyViewModel
        {
            get => _yKeyViewModel;
            set => SetProperty(ref _yKeyViewModel, value);
        }

        public KeyViewModel UKeyViewModel
        {
            get => _uKeyViewModel;
            set => SetProperty(ref _uKeyViewModel, value);
        }

        // ReSharper disable once InconsistentNaming
        public KeyViewModel IKeyViewModel
        {
            get => _iKeyViewModel;
            set => SetProperty(ref _iKeyViewModel, value);
        }

        public KeyViewModel OKeyViewModel
        {
            get => _oKeyViewModel;
            set => SetProperty(ref _oKeyViewModel, value);
        }

        public KeyViewModel PKeyViewModel
        {
            get => _pKeyViewModel;
            set => SetProperty(ref _pKeyViewModel, value);
        }

        public KeyViewModel BracketOpenKeyViewModel
        {
            get => _bracketOpenKeyViewModel;
            set => SetProperty(ref _bracketOpenKeyViewModel, value);
        }

        public KeyViewModel BracketCloseKeyViewModel
        {
            get => _bracketCloseKeyViewModel;
            set => SetProperty(ref _bracketCloseKeyViewModel, value);
        }

        public KeyViewModel EnterKeyViewModel
        {
            get => _enterKeyViewModel;
            set => SetProperty(ref _enterKeyViewModel, value);
        }

        public KeyViewModel CapsLockKeyViewModel
        {
            get => _capsLockKeyViewModel;
            set => SetProperty(ref _capsLockKeyViewModel, value);
        }

        public KeyViewModel AKeyViewModel
        {
            get => _aKeyViewModel;
            set => SetProperty(ref _aKeyViewModel, value);
        }

        public KeyViewModel SKeyViewModel
        {
            get => _sKeyViewModel;
            set => SetProperty(ref _sKeyViewModel, value);
        }

        public KeyViewModel DKeyViewModel
        {
            get => _dKeyViewModel;
            set => SetProperty(ref _dKeyViewModel, value);
        }

        public KeyViewModel FKeyViewModel
        {
            get => _fKeyViewModel;
            set => SetProperty(ref _fKeyViewModel, value);
        }

        public KeyViewModel GKeyViewModel
        {
            get => _gKeyViewModel;
            set => SetProperty(ref _gKeyViewModel, value);
        }

        public KeyViewModel HKeyViewModel
        {
            get => _hKeyViewModel;
            set => SetProperty(ref _hKeyViewModel, value);
        }

        public KeyViewModel JKeyViewModel
        {
            get => _jKeyViewModel;
            set => SetProperty(ref _jKeyViewModel, value);
        }

        public KeyViewModel KKeyViewModel
        {
            get => _kKeyViewModel;
            set => SetProperty(ref _kKeyViewModel, value);
        }

        public KeyViewModel LKeyViewModel
        {
            get => _lKeyViewModel;
            set => SetProperty(ref _lKeyViewModel, value);
        }

        public KeyViewModel SemicolonKeyViewModel
        {
            get => _semicolonKeyViewModel;
            set => SetProperty(ref _semicolonKeyViewModel, value);
        }

        public KeyViewModel ApostropheKeyViewModel
        {
            get => _apostropheKeyViewModel;
            set => SetProperty(ref _apostropheKeyViewModel, value);
        }

        public KeyViewModel NumberSignKeyViewModel
        {
            get => _numberSignKeyViewModel;
            set => SetProperty(ref _numberSignKeyViewModel, value);
        }


        public KeyViewModel ShiftLeftKeyViewModel
        {
            get => _shiftLeftKeyViewModel;
            set => SetProperty(ref _shiftLeftKeyViewModel, value);
        }

        public KeyViewModel BackslashKeyViewModel
        {
            get => _backslashKeyViewModel;
            set => SetProperty(ref _backslashKeyViewModel, value);
        }

        public KeyViewModel ZKeyViewModel
        {
            get => _zKeyViewModel;
            set => SetProperty(ref _zKeyViewModel, value);
        }

        public KeyViewModel XKeyViewModel
        {
            get => _xKeyViewModel;
            set => SetProperty(ref _xKeyViewModel, value);
        }

        public KeyViewModel CKeyViewModel
        {
            get => _cKeyViewModel;
            set => SetProperty(ref _cKeyViewModel, value);
        }

        public KeyViewModel VKeyViewModel
        {
            get => _vKeyViewModel;
            set => SetProperty(ref _vKeyViewModel, value);
        }

        public KeyViewModel BKeyViewModel
        {
            get => _bKeyViewModel;
            set => SetProperty(ref _bKeyViewModel, value);
        }

        public KeyViewModel NKeyViewModel
        {
            get => _nKeyViewModel;
            set => SetProperty(ref _nKeyViewModel, value);
        }

        public KeyViewModel MKeyViewModel
        {
            get => _mKeyViewModel;
            set => SetProperty(ref _mKeyViewModel, value);
        }

        public KeyViewModel CommaKeyViewModel
        {
            get => _commaKeyViewModel;
            set => SetProperty(ref _commaKeyViewModel, value);
        }

        public KeyViewModel DotKeyViewModel
        {
            get => _dotKeyViewModel;
            set => SetProperty(ref _dotKeyViewModel, value);
        }

        public KeyViewModel SlashKeyViewModel
        {
            get => _slashKeyViewModel;
            set => SetProperty(ref _slashKeyViewModel, value);
        }

        public KeyViewModel ShiftRightKeyViewModel
        {
            get => _shiftRightKeyViewModel;
            set => SetProperty(ref _shiftRightKeyViewModel, value);
        }

        public KeyViewModel ControlLeftKeyViewModel
        {
            get => _controlLeftKeyViewModel;
            set => SetProperty(ref _controlLeftKeyViewModel, value);
        }

        public KeyViewModel SuperLeftKeyViewModel
        {
            get => _superLeftKeyViewModel;
            set => SetProperty(ref _superLeftKeyViewModel, value);
        }

        public KeyViewModel AltKeyViewModel
        {
            get => _altKeyViewModel;
            set => SetProperty(ref _altKeyViewModel, value);
        }

        public KeyViewModel SpaceKeyViewModel
        {
            get => _spaceKeyViewModel;
            set => SetProperty(ref _spaceKeyViewModel, value);
        }

        public KeyViewModel AltGrKeyViewModel
        {
            get => _altGrKeyViewModel;
            set => SetProperty(ref _altGrKeyViewModel, value);
        }

        public KeyViewModel SuperRightKeyViewModel
        {
            get => _superRightKeyViewModel;
            set => SetProperty(ref _superRightKeyViewModel, value);
        }

        public KeyViewModel MenuKeyViewModel
        {
            get => _menuKeyViewModel;
            set => SetProperty(ref _menuKeyViewModel, value);
        }

        public KeyViewModel ControlRightViewModel
        {
            get => _controlRightViewModel;
            set => SetProperty(ref _controlRightViewModel, value);
        }

        public KeyboardFingerPositionsViewModel()
        {
            _fingerColors = new FingerColors();
            CreateViewModels();
        }

        public void LoadFingerColors(IFingerColors fingerColors)
        {
            _fingerColors = fingerColors;
            UpdateAllKeys();
        }

        public void LoadFingerPositions(IFingerPositions fingerPositions)
        {
            _fingerPositions = fingerPositions;
            UpdateAllKeys();
        }

        private void UpdateAllKeys()
        {
            UpdateKey(HardwareKey.AccentGrave, AccentGraveKeyViewModel);
            UpdateKey(HardwareKey.D1, D1KeyViewModel);
            UpdateKey(HardwareKey.D2, D2KeyViewModel);
            UpdateKey(HardwareKey.D3, D3KeyViewModel);
            UpdateKey(HardwareKey.D4, D4KeyViewModel);
            UpdateKey(HardwareKey.D5, D5KeyViewModel);
            UpdateKey(HardwareKey.D6, D6KeyViewModel);
            UpdateKey(HardwareKey.D7, D7KeyViewModel);
            UpdateKey(HardwareKey.D8, D8KeyViewModel);
            UpdateKey(HardwareKey.D9, D9KeyViewModel);
            UpdateKey(HardwareKey.D0, D0KeyViewModel);
            UpdateKey(HardwareKey.Minus, MinusKeyViewModel);
            UpdateKey(HardwareKey.Equal, EqualKeyViewModel);
            UpdateKey(HardwareKey.Backspace, BackspaceKeyViewModel);

            UpdateKey(HardwareKey.Tab, TabKeyViewModel);
            UpdateKey(HardwareKey.Q, QKeyViewModel);
            UpdateKey(HardwareKey.W, WKeyViewModel);
            UpdateKey(HardwareKey.E, EKeyViewModel);
            UpdateKey(HardwareKey.R, RKeyViewModel);
            UpdateKey(HardwareKey.T, TKeyViewModel);
            UpdateKey(HardwareKey.Y, YKeyViewModel);
            UpdateKey(HardwareKey.U, UKeyViewModel);
            UpdateKey(HardwareKey.I, IKeyViewModel);
            UpdateKey(HardwareKey.O, OKeyViewModel);
            UpdateKey(HardwareKey.P, PKeyViewModel);
            UpdateKey(HardwareKey.BracketOpen, BracketOpenKeyViewModel);
            UpdateKey(HardwareKey.BracketClose, BracketCloseKeyViewModel);
            UpdateKey(HardwareKey.Enter, EnterKeyViewModel);

            UpdateKey(HardwareKey.CapsLock, CapsLockKeyViewModel);
            UpdateKey(HardwareKey.A, AKeyViewModel);
            UpdateKey(HardwareKey.S, SKeyViewModel);
            UpdateKey(HardwareKey.D, DKeyViewModel);
            UpdateKey(HardwareKey.F, FKeyViewModel);
            UpdateKey(HardwareKey.G, GKeyViewModel);
            UpdateKey(HardwareKey.H, HKeyViewModel);
            UpdateKey(HardwareKey.J, JKeyViewModel);
            UpdateKey(HardwareKey.K, KKeyViewModel);
            UpdateKey(HardwareKey.L, LKeyViewModel);
            UpdateKey(HardwareKey.Semicolon, SemicolonKeyViewModel);
            UpdateKey(HardwareKey.Apostrophe, ApostropheKeyViewModel);
            UpdateKey(HardwareKey.NumberSign, NumberSignKeyViewModel);

            UpdateKey(HardwareKey.ShiftLeft, ShiftLeftKeyViewModel);
            UpdateKey(HardwareKey.Backslash, BackslashKeyViewModel);
            UpdateKey(HardwareKey.Z, ZKeyViewModel);
            UpdateKey(HardwareKey.X, XKeyViewModel);
            UpdateKey(HardwareKey.C, CKeyViewModel);
            UpdateKey(HardwareKey.V, VKeyViewModel);
            UpdateKey(HardwareKey.B, BKeyViewModel);
            UpdateKey(HardwareKey.N, NKeyViewModel);
            UpdateKey(HardwareKey.M, MKeyViewModel);
            UpdateKey(HardwareKey.Comma, CommaKeyViewModel);
            UpdateKey(HardwareKey.Dot, DotKeyViewModel);
            UpdateKey(HardwareKey.Slash, SlashKeyViewModel);
            UpdateKey(HardwareKey.ShiftRight, ShiftRightKeyViewModel);

            UpdateKey(HardwareKey.ControlLeft, ControlLeftKeyViewModel);
            UpdateKey(HardwareKey.SuperLeft, SuperLeftKeyViewModel);
            UpdateKey(HardwareKey.Alt, AltKeyViewModel);
            UpdateKey(HardwareKey.Space, SpaceKeyViewModel);
            UpdateKey(HardwareKey.AltGr, AltGrKeyViewModel);
            UpdateKey(HardwareKey.SuperRight, SuperRightKeyViewModel);
            UpdateKey(HardwareKey.Menu, MenuKeyViewModel);
            UpdateKey(HardwareKey.ControlRight, ControlRightViewModel);
        }

        private void CreateViewModels()
        {
            AccentGraveKeyViewModel = new KeyViewModel();
            D1KeyViewModel = new KeyViewModel();
            D2KeyViewModel = new KeyViewModel();
            D3KeyViewModel = new KeyViewModel();
            D4KeyViewModel = new KeyViewModel();
            D5KeyViewModel = new KeyViewModel();
            D6KeyViewModel = new KeyViewModel();
            D7KeyViewModel = new KeyViewModel();
            D8KeyViewModel = new KeyViewModel();
            D9KeyViewModel = new KeyViewModel();
            D0KeyViewModel = new KeyViewModel();
            MinusKeyViewModel = new KeyViewModel();
            EqualKeyViewModel = new KeyViewModel();
            BackspaceKeyViewModel = new KeyViewModel();

            TabKeyViewModel = new KeyViewModel();
            QKeyViewModel = new KeyViewModel();
            WKeyViewModel = new KeyViewModel();
            EKeyViewModel = new KeyViewModel();
            RKeyViewModel = new KeyViewModel();
            TKeyViewModel = new KeyViewModel();
            YKeyViewModel = new KeyViewModel();
            UKeyViewModel = new KeyViewModel();
            IKeyViewModel = new KeyViewModel();
            OKeyViewModel = new KeyViewModel();
            PKeyViewModel = new KeyViewModel();
            BracketOpenKeyViewModel = new KeyViewModel();
            BracketCloseKeyViewModel = new KeyViewModel();
            EnterKeyViewModel = new KeyViewModel();

            CapsLockKeyViewModel = new KeyViewModel();
            AKeyViewModel = new KeyViewModel();
            SKeyViewModel = new KeyViewModel();
            DKeyViewModel = new KeyViewModel();
            FKeyViewModel = new KeyViewModel();
            GKeyViewModel = new KeyViewModel();
            HKeyViewModel = new KeyViewModel();
            JKeyViewModel = new KeyViewModel();
            KKeyViewModel = new KeyViewModel();
            LKeyViewModel = new KeyViewModel();
            SemicolonKeyViewModel = new KeyViewModel();
            ApostropheKeyViewModel = new KeyViewModel();
            NumberSignKeyViewModel = new KeyViewModel();

            ShiftLeftKeyViewModel = new KeyViewModel();
            BackslashKeyViewModel = new KeyViewModel();
            ZKeyViewModel = new KeyViewModel();
            XKeyViewModel = new KeyViewModel();
            CKeyViewModel = new KeyViewModel();
            VKeyViewModel = new KeyViewModel();
            BKeyViewModel = new KeyViewModel();
            NKeyViewModel = new KeyViewModel();
            MKeyViewModel = new KeyViewModel();
            CommaKeyViewModel = new KeyViewModel();
            DotKeyViewModel = new KeyViewModel();
            SlashKeyViewModel = new KeyViewModel();
            ShiftRightKeyViewModel = new KeyViewModel();

            ControlLeftKeyViewModel = new KeyViewModel();
            SuperLeftKeyViewModel = new KeyViewModel();
            AltKeyViewModel = new KeyViewModel();
            SpaceKeyViewModel = new KeyViewModel();
            AltGrKeyViewModel = new KeyViewModel();
            SuperRightKeyViewModel = new KeyViewModel();
            MenuKeyViewModel = new KeyViewModel();
            ControlRightViewModel = new KeyViewModel();
        }

        private void UpdateKey(HardwareKey key, KeyViewModel keyViewModel)
        {
            var finger = LoadKey(key);

            keyViewModel.Name = key.GetDefaultText();
            keyViewModel.HighlightedColor = GetColor(finger);
        }

        private Finger? LoadKey(HardwareKey key)
        {
            return _fingerPositions.ContainsKey(key) ? (Finger?)_fingerPositions[key] : null;
        }

        private Color GetColor(Finger? finger)
        {
            return _fingerColors[finger];
        }
    }
}