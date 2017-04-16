using Caliburn.Micro;
using Eto.Forms;
using NTouchTypeTrainer.Common.GuiExtensions;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Views.Common;
using NTouchTypeTrainer.Views.Controls;

namespace NTouchTypeTrainer.Views
{
    public class KeyboardView : Panel
    {
        private readonly ISharedSizeGroup _regularKeySharedSizeGroup;

        private StackLayout _allRowsStackLayout;

        private StackLayout _digitRowStackLayout;
        private StackLayout _upperCharRowStackLayout;
        private StackLayout _middleCharRowStackLayout;
        private StackLayout _lowerCharRowStackLayout;
        private StackLayout _controlKeyRowStackLayout;

        private SharedSizeHardwareKeyControl _keyGrave;
        private SharedSizeHardwareKeyControl _keyD1;
        private SharedSizeHardwareKeyControl _keyD2;
        private SharedSizeHardwareKeyControl _keyD3;
        private SharedSizeHardwareKeyControl _keyD4;
        private SharedSizeHardwareKeyControl _keyD5;
        private SharedSizeHardwareKeyControl _keyD6;
        private SharedSizeHardwareKeyControl _keyD7;
        private SharedSizeHardwareKeyControl _keyD8;
        private SharedSizeHardwareKeyControl _keyD9;
        private SharedSizeHardwareKeyControl _keyD0;
        private SharedSizeHardwareKeyControl _keyMinus;
        private SharedSizeHardwareKeyControl _keyEqual;
        private ProportionalSizeHardwareKeyControl _keyBackspace;

        private ProportionalSizeHardwareKeyControl _keyTab;
        private SharedSizeHardwareKeyControl _keyQ;
        private SharedSizeHardwareKeyControl _keyW;
        private SharedSizeHardwareKeyControl _keyE;
        private SharedSizeHardwareKeyControl _keyR;
        private SharedSizeHardwareKeyControl _keyT;
        private SharedSizeHardwareKeyControl _keyY;
        private SharedSizeHardwareKeyControl _keyU;
        private SharedSizeHardwareKeyControl _keyI;
        private SharedSizeHardwareKeyControl _keyO;
        private SharedSizeHardwareKeyControl _keyP;
        private SharedSizeHardwareKeyControl _keyBracketOpen;
        private SharedSizeHardwareKeyControl _keyBracketClose;
        private ProportionalSizeHardwareKeyControl _keyEnter;

        private ProportionalSizeHardwareKeyControl _keyCapsLock;
        private SharedSizeHardwareKeyControl _keyA;
        private SharedSizeHardwareKeyControl _keyS;
        private SharedSizeHardwareKeyControl _keyD;
        private SharedSizeHardwareKeyControl _keyF;
        private SharedSizeHardwareKeyControl _keyG;
        private SharedSizeHardwareKeyControl _keyH;
        private SharedSizeHardwareKeyControl _keyJ;
        private SharedSizeHardwareKeyControl _keyK;
        private SharedSizeHardwareKeyControl _keyL;
        private SharedSizeHardwareKeyControl _keySemicolon;
        private SharedSizeHardwareKeyControl _keyApostrophe;
        private SharedSizeHardwareKeyControl _keyNumberSign;

        private ProportionalSizeHardwareKeyControl _keyShiftLeft;
        private SharedSizeHardwareKeyControl _keyBackslash;
        private SharedSizeHardwareKeyControl _keyZ;
        private SharedSizeHardwareKeyControl _keyX;
        private SharedSizeHardwareKeyControl _keyC;
        private SharedSizeHardwareKeyControl _keyV;
        private SharedSizeHardwareKeyControl _keyB;
        private SharedSizeHardwareKeyControl _keyN;
        private SharedSizeHardwareKeyControl _keyM;
        private SharedSizeHardwareKeyControl _keyComma;
        private SharedSizeHardwareKeyControl _keyDot;
        private SharedSizeHardwareKeyControl _keySlash;
        private ProportionalSizeHardwareKeyControl _keyShiftRight;

        private ProportionalSizeHardwareKeyControl _keyControlLeft;
        private ProportionalSizeHardwareKeyControl _keySuperLeft;
        private ProportionalSizeHardwareKeyControl _keyAlt;
        private ProportionalSizeHardwareKeyControl _keySpace;
        private ProportionalSizeHardwareKeyControl _keyAltGr;
        private ProportionalSizeHardwareKeyControl _keySuperRight;
        private ProportionalSizeHardwareKeyControl _keyMenu;
        private ProportionalSizeHardwareKeyControl _keyControlRight;

        private const float BackspaceFactor = 2.7f;
        private const float TabFactor = 1.7f;
        private const float EnterFactor = 2.0f;
        private const float CapsLockFactor = 2.2f;
        private const float ShiftLeftFactor = 1.5f;
        private const float ShiftRightFactor = 3.4f;
        private const float SmallKeyFactor = 1.5f;
        private const float SpaceKeyFactor = 6.2f;

        public KeyboardView(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _regularKeySharedSizeGroup = new SharedSizeGroup(eventAggregator, "RegularKeys");

            InitRows(eventAggregator, graphicsProvider);
            Content = _allRowsStackLayout;
        }

        private void InitRows(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            InitDigitRow(eventAggregator, graphicsProvider);
            InitUpperCharRow(eventAggregator, graphicsProvider);
            InitMiddleCharRow(eventAggregator, graphicsProvider);
            InitLowerCharRow(eventAggregator, graphicsProvider);
            InitControlKeyRow(eventAggregator, graphicsProvider);

            _allRowsStackLayout = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Top,
                Padding = 10,
                Spacing = 5,
                Items =
                {
                    _digitRowStackLayout,
                    _upperCharRowStackLayout,
                    _middleCharRowStackLayout,
                    _lowerCharRowStackLayout,
                    _controlKeyRowStackLayout,
                }
            };
        }

        private void InitDigitRow(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _keyGrave = new SharedSizeHardwareKeyControl(HardwareKey.Grave, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD1 = new SharedSizeHardwareKeyControl(HardwareKey.D1, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD2 = new SharedSizeHardwareKeyControl(HardwareKey.D2, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD3 = new SharedSizeHardwareKeyControl(HardwareKey.D3, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD4 = new SharedSizeHardwareKeyControl(HardwareKey.D4, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD5 = new SharedSizeHardwareKeyControl(HardwareKey.D5, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD6 = new SharedSizeHardwareKeyControl(HardwareKey.D6, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD7 = new SharedSizeHardwareKeyControl(HardwareKey.D7, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD8 = new SharedSizeHardwareKeyControl(HardwareKey.D8, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD9 = new SharedSizeHardwareKeyControl(HardwareKey.D9, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD0 = new SharedSizeHardwareKeyControl(HardwareKey.D0, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyMinus = new SharedSizeHardwareKeyControl(HardwareKey.Minus, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyEqual = new SharedSizeHardwareKeyControl(HardwareKey.Equal, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyBackspace = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.Backspace,
                factor: BackspaceFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyBackspace.Font = _keyBackspace.Font.Inflate(2.0f);

            _digitRowStackLayout = new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Padding = 0,
                Spacing = 5,
                Items =
                {
                    _keyGrave,
                    _keyD1,
                    _keyD2,
                    _keyD3,
                    _keyD4,
                    _keyD5,
                    _keyD6,
                    _keyD7,
                    _keyD8,
                    _keyD9,
                    _keyD0,
                    _keyMinus,
                    _keyEqual,
                    _keyBackspace
                }
            };
        }

        private void InitUpperCharRow(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _keyTab = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.Tab,
                factor: TabFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyQ = new SharedSizeHardwareKeyControl(HardwareKey.Q, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyW = new SharedSizeHardwareKeyControl(HardwareKey.W, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyE = new SharedSizeHardwareKeyControl(HardwareKey.E, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyR = new SharedSizeHardwareKeyControl(HardwareKey.R, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyT = new SharedSizeHardwareKeyControl(HardwareKey.T, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyY = new SharedSizeHardwareKeyControl(HardwareKey.Z, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyU = new SharedSizeHardwareKeyControl(HardwareKey.U, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyI = new SharedSizeHardwareKeyControl(HardwareKey.I, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyO = new SharedSizeHardwareKeyControl(HardwareKey.O, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyP = new SharedSizeHardwareKeyControl(HardwareKey.P, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyBracketOpen = new SharedSizeHardwareKeyControl(HardwareKey.BracketOpen, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyBracketClose = new SharedSizeHardwareKeyControl(HardwareKey.BracketClose, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyEnter = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.Enter,
                factor: EnterFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyEnter.Font = _keyEnter.Font.Inflate(2.0f);

            _upperCharRowStackLayout = new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Padding = 0,
                Spacing = 5,
                Items =
                {
                    _keyTab,
                    _keyQ,
                    _keyW,
                    _keyE,
                    _keyR,
                    _keyT,
                    _keyY,
                    _keyU,
                    _keyI,
                    _keyO,
                    _keyP,
                    _keyBracketOpen,
                    _keyBracketClose,
                    _keyEnter,
                }
            };
        }

        private void InitMiddleCharRow(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _keyCapsLock = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.CapsLock,
                factor: CapsLockFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyCapsLock.Font = _keyCapsLock.Font.Inflate(2.0f);
            _keyA = new SharedSizeHardwareKeyControl(HardwareKey.A, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyS = new SharedSizeHardwareKeyControl(HardwareKey.S, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyD = new SharedSizeHardwareKeyControl(HardwareKey.D, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyF = new SharedSizeHardwareKeyControl(HardwareKey.F, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyG = new SharedSizeHardwareKeyControl(HardwareKey.G, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyH = new SharedSizeHardwareKeyControl(HardwareKey.H, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyJ = new SharedSizeHardwareKeyControl(HardwareKey.J, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyK = new SharedSizeHardwareKeyControl(HardwareKey.K, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyL = new SharedSizeHardwareKeyControl(HardwareKey.L, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keySemicolon = new SharedSizeHardwareKeyControl(HardwareKey.Semicolon, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyApostrophe = new SharedSizeHardwareKeyControl(HardwareKey.Apostrophe, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyNumberSign = new SharedSizeHardwareKeyControl(HardwareKey.NumberSign, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);

            _middleCharRowStackLayout = new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Padding = 0,
                Spacing = 5,
                Items =
                {
                    _keyCapsLock,
                    _keyA,
                    _keyS,
                    _keyD,
                    _keyF,
                    _keyG,
                    _keyH,
                    _keyJ,
                    _keyK,
                    _keyL,
                    _keySemicolon,
                    _keyApostrophe,
                    _keyNumberSign,
                }
            };
        }

        private void InitLowerCharRow(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _keyShiftLeft = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.ShiftLeft,
                factor: ShiftLeftFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyShiftLeft.Font = _keyShiftLeft.Font.Inflate(3.0f);
            _keyBackslash = new SharedSizeHardwareKeyControl(HardwareKey.Backslash, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyZ = new SharedSizeHardwareKeyControl(HardwareKey.Z, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyX = new SharedSizeHardwareKeyControl(HardwareKey.X, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyC = new SharedSizeHardwareKeyControl(HardwareKey.C, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyV = new SharedSizeHardwareKeyControl(HardwareKey.V, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyB = new SharedSizeHardwareKeyControl(HardwareKey.B, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyN = new SharedSizeHardwareKeyControl(HardwareKey.N, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyM = new SharedSizeHardwareKeyControl(HardwareKey.M, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyComma = new SharedSizeHardwareKeyControl(HardwareKey.Comma, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyDot = new SharedSizeHardwareKeyControl(HardwareKey.Dot, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keySlash = new SharedSizeHardwareKeyControl(HardwareKey.Slash, _regularKeySharedSizeGroup, eventAggregator, graphicsProvider);
            _keyShiftRight = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.ShiftRight,
                factor: ShiftRightFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyShiftRight.Font = _keyShiftRight.Font.Inflate(3.0f);

            _lowerCharRowStackLayout = new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Padding = 0,
                Spacing = 5,
                Items =
                {
                    _keyShiftLeft,
                    _keyBackslash,
                    _keyZ,
                    _keyX,
                    _keyC,
                    _keyV,
                    _keyB,
                    _keyN,
                    _keyM,
                    _keyComma,
                    _keyDot,
                    _keySlash,
                    _keyShiftRight,
                }
            };
        }

        private void InitControlKeyRow(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _keyControlLeft = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.ControlLeft,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keySuperLeft = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.SuperLeft,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyAlt = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.Alt,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keySpace = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.Space,
                factor: SpaceKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyAltGr = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.AltGr,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keySuperRight = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.SuperRight,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyMenu = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.Menu,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);
            _keyControlRight = new ProportionalSizeHardwareKeyControl(
                key: HardwareKey.ControlRight,
                factor: SmallKeyFactor,
                sourceSizeGroup: _regularKeySharedSizeGroup,
                eventAggregator: eventAggregator,
                graphicsProvider: graphicsProvider);

            _controlKeyRowStackLayout = new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Padding = 0,
                Spacing = 5,
                Items =
                {
                    _keyControlLeft,
                    _keySuperLeft,
                    _keyAlt,
                    _keySpace,
                    _keyAltGr,
                    _keySuperRight,
                    _keyMenu,
                    _keyControlRight,
                }
            };
        }
    }
}