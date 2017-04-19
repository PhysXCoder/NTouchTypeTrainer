using System;
using Caliburn.Micro;
using Eto.Forms;
using NTouchTypeTrainer.Common.DataBinding;
using NTouchTypeTrainer.Common.GuiExtensions;
using NTouchTypeTrainer.Contracts.Common.Graphics;
using NTouchTypeTrainer.Contracts.Views;
using NTouchTypeTrainer.ViewModels;
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

        private SharedSizeHardwareKeyControl _keyAccentGrave;
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

        private readonly IEventAggregator _eventAggregator;
        private readonly IGraphicsProvider _graphicsProvider;

        private const float BackspaceFactor = 2.7f;
        private const float TabFactor = 1.7f;
        private const float EnterFactor = 2.0f;
        private const float CapsLockFactor = 2.2f;
        private const float ShiftLeftFactor = 1.5f;
        private const float ShiftRightFactor = 3.4f;
        private const float ControlKeyFactor = 1.5f;
        private const float SpaceKeyFactor = 6.2f;

        public KeyboardView(IEventAggregator eventAggregator, IGraphicsProvider graphicsProvider)
        {
            _eventAggregator = eventAggregator;
            _graphicsProvider = graphicsProvider;

            _regularKeySharedSizeGroup = new SharedSizeGroup(eventAggregator, "RegularKeys");

            DataContextChanged += KeyboardView_DataContextChanged;

            InitRows();

            Content = _allRowsStackLayout;
        }

        private void KeyboardView_DataContextChanged(object sender, EventArgs e)
        {
            var dataCtx = DataContext as KeyboardFingerPositionsViewModel;
            _keyAccentGrave.DataContext = dataCtx?.AccentGraveKeyViewModel;
            _keyD1.DataContext = dataCtx?.D1KeyViewModel;
            _keyD2.DataContext = dataCtx?.D2KeyViewModel;
            _keyD3.DataContext = dataCtx?.D3KeyViewModel;
            _keyD4.DataContext = dataCtx?.D4KeyViewModel;
            _keyD5.DataContext = dataCtx?.D5KeyViewModel;
            _keyD6.DataContext = dataCtx?.D6KeyViewModel;
            _keyD7.DataContext = dataCtx?.D7KeyViewModel;
            _keyD8.DataContext = dataCtx?.D8KeyViewModel;
            _keyD9.DataContext = dataCtx?.D9KeyViewModel;
            _keyD0.DataContext = dataCtx?.D0KeyViewModel;
            _keyMinus.DataContext = dataCtx?.MinusKeyViewModel;
            _keyEqual.DataContext = dataCtx?.EqualKeyViewModel;
            _keyBackspace.DataContext = dataCtx?.BackspaceKeyViewModel;

            _keyTab.DataContext = dataCtx?.TabKeyViewModel;
            _keyQ.DataContext = dataCtx?.QKeyViewModel;
            _keyW.DataContext = dataCtx?.WKeyViewModel;
            _keyE.DataContext = dataCtx?.EKeyViewModel;
            _keyR.DataContext = dataCtx?.RKeyViewModel;
            _keyT.DataContext = dataCtx?.TKeyViewModel;
            _keyY.DataContext = dataCtx?.YKeyViewModel;
            _keyU.DataContext = dataCtx?.UKeyViewModel;
            _keyI.DataContext = dataCtx?.IKeyViewModel;
            _keyO.DataContext = dataCtx?.OKeyViewModel;
            _keyP.DataContext = dataCtx?.PKeyViewModel;
            _keyBracketOpen.DataContext = dataCtx?.BracketOpenKeyViewModel;
            _keyBracketClose.DataContext = dataCtx?.BracketCloseKeyViewModel;
            _keyEnter.DataContext = dataCtx?.EnterKeyViewModel;

            _keyCapsLock.DataContext = dataCtx?.CapsLockKeyViewModel;
            _keyA.DataContext = dataCtx?.AKeyViewModel;
            _keyS.DataContext = dataCtx?.SKeyViewModel;
            _keyD.DataContext = dataCtx?.DKeyViewModel;
            _keyF.DataContext = dataCtx?.FKeyViewModel;
            _keyG.DataContext = dataCtx?.GKeyViewModel;
            _keyH.DataContext = dataCtx?.HKeyViewModel;
            _keyJ.DataContext = dataCtx?.JKeyViewModel;
            _keyK.DataContext = dataCtx?.KKeyViewModel;
            _keyL.DataContext = dataCtx?.LKeyViewModel;
            _keySemicolon.DataContext = dataCtx?.SemicolonKeyViewModel;
            _keyApostrophe.DataContext = dataCtx?.ApostropheKeyViewModel;
            _keyNumberSign.DataContext = dataCtx?.NumberSignKeyViewModel;

            _keyShiftLeft.DataContext = dataCtx?.ShiftLeftKeyViewModel;
            _keyBackslash.DataContext = dataCtx?.BackslashKeyViewModel;
            _keyZ.DataContext = dataCtx?.ZKeyViewModel;
            _keyX.DataContext = dataCtx?.XKeyViewModel;
            _keyC.DataContext = dataCtx?.CKeyViewModel;
            _keyV.DataContext = dataCtx?.VKeyViewModel;
            _keyB.DataContext = dataCtx?.BKeyViewModel;
            _keyN.DataContext = dataCtx?.NKeyViewModel;
            _keyM.DataContext = dataCtx?.MKeyViewModel;
            _keyComma.DataContext = dataCtx?.CommaKeyViewModel;
            _keyDot.DataContext = dataCtx?.DotKeyViewModel;
            _keySlash.DataContext = dataCtx?.SlashKeyViewModel;
            _keyShiftRight.DataContext = dataCtx?.ShiftRightKeyViewModel;

            _keyControlLeft.DataContext = dataCtx?.ControlLeftKeyViewModel;
            _keySuperLeft.DataContext = dataCtx?.SuperLeftKeyViewModel;
            _keyAlt.DataContext = dataCtx?.AltKeyViewModel;
            _keySpace.DataContext = dataCtx?.SpaceKeyViewModel;
            _keyAltGr.DataContext = dataCtx?.AltGrKeyViewModel;
            _keySuperRight.DataContext = dataCtx?.SuperRightKeyViewModel;
            _keyMenu.DataContext = dataCtx?.MenuKeyViewModel;
            _keyControlRight.DataContext = dataCtx?.ControlRightViewModel;
        }

        private void InitRows()
        {
            InitDigitRow();
            InitUpperCharRow();
            InitMiddleCharRow();
            InitLowerCharRow();
            InitControlKeyRow();

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

        private void InitDigitRow()
        {
            _keyAccentGrave = CreateRegularSharedSizeKeyControl();
            _keyD1 = CreateRegularSharedSizeKeyControl();
            _keyD2 = CreateRegularSharedSizeKeyControl();
            _keyD3 = CreateRegularSharedSizeKeyControl();
            _keyD4 = CreateRegularSharedSizeKeyControl();
            _keyD5 = CreateRegularSharedSizeKeyControl();
            _keyD6 = CreateRegularSharedSizeKeyControl();
            _keyD7 = CreateRegularSharedSizeKeyControl();
            _keyD8 = CreateRegularSharedSizeKeyControl();
            _keyD9 = CreateRegularSharedSizeKeyControl();
            _keyD0 = CreateRegularSharedSizeKeyControl();
            _keyMinus = CreateRegularSharedSizeKeyControl();
            _keyEqual = CreateRegularSharedSizeKeyControl();
            _keyBackspace = CreateProportionalSizeKeyControl(BackspaceFactor);
            _keyBackspace.Font = _keyBackspace.Font.Inflate(2.0f);

            _digitRowStackLayout = new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Padding = 0,
                Spacing = 5,
                Items =
                {
                    _keyAccentGrave,
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

        private void InitUpperCharRow()
        {
            _keyTab = CreateProportionalSizeKeyControl(TabFactor);
            _keyQ = CreateRegularSharedSizeKeyControl();
            _keyW = CreateRegularSharedSizeKeyControl();
            _keyE = CreateRegularSharedSizeKeyControl();
            _keyR = CreateRegularSharedSizeKeyControl();
            _keyT = CreateRegularSharedSizeKeyControl();
            _keyY = CreateRegularSharedSizeKeyControl();
            _keyU = CreateRegularSharedSizeKeyControl();
            _keyI = CreateRegularSharedSizeKeyControl();
            _keyO = CreateRegularSharedSizeKeyControl();
            _keyP = CreateRegularSharedSizeKeyControl();
            _keyBracketOpen = CreateRegularSharedSizeKeyControl();
            _keyBracketClose = CreateRegularSharedSizeKeyControl();
            _keyEnter = CreateProportionalSizeKeyControl(EnterFactor);
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

        private void InitMiddleCharRow()
        {
            _keyCapsLock = CreateProportionalSizeKeyControl(CapsLockFactor);
            _keyCapsLock.Font = _keyCapsLock.Font.Inflate(2.0f);
            _keyA = CreateRegularSharedSizeKeyControl();
            _keyS = CreateRegularSharedSizeKeyControl();
            _keyD = CreateRegularSharedSizeKeyControl();
            _keyF = CreateRegularSharedSizeKeyControl();
            _keyG = CreateRegularSharedSizeKeyControl();
            _keyH = CreateRegularSharedSizeKeyControl();
            _keyJ = CreateRegularSharedSizeKeyControl();
            _keyK = CreateRegularSharedSizeKeyControl();
            _keyL = CreateRegularSharedSizeKeyControl();
            _keySemicolon = CreateRegularSharedSizeKeyControl();
            _keyApostrophe = CreateRegularSharedSizeKeyControl();
            _keyNumberSign = CreateRegularSharedSizeKeyControl();

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

        private void InitLowerCharRow()
        {
            _keyShiftLeft = CreateProportionalSizeKeyControl(ShiftLeftFactor);
            _keyShiftLeft.Font = _keyShiftLeft.Font.Inflate(3.0f);
            _keyBackslash = CreateRegularSharedSizeKeyControl();
            _keyZ = CreateRegularSharedSizeKeyControl();
            _keyX = CreateRegularSharedSizeKeyControl();
            _keyC = CreateRegularSharedSizeKeyControl();
            _keyV = CreateRegularSharedSizeKeyControl();
            _keyB = CreateRegularSharedSizeKeyControl();
            _keyN = CreateRegularSharedSizeKeyControl();
            _keyM = CreateRegularSharedSizeKeyControl();
            _keyComma = CreateRegularSharedSizeKeyControl();
            _keyDot = CreateRegularSharedSizeKeyControl();
            _keySlash = CreateRegularSharedSizeKeyControl();
            _keyShiftRight = CreateProportionalSizeKeyControl(ShiftRightFactor);
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

        private void InitControlKeyRow()
        {
            _keyControlLeft = CreateProportionalSizeKeyControl(ControlKeyFactor);
            _keySuperLeft = CreateProportionalSizeKeyControl(ControlKeyFactor);
            _keyAlt = CreateProportionalSizeKeyControl(ControlKeyFactor);
            _keySpace = CreateProportionalSizeKeyControl(SpaceKeyFactor);
            _keyAltGr = CreateProportionalSizeKeyControl(ControlKeyFactor);
            _keySuperRight = CreateProportionalSizeKeyControl(ControlKeyFactor);
            _keyMenu = CreateProportionalSizeKeyControl(ControlKeyFactor);
            _keyControlRight = CreateProportionalSizeKeyControl(ControlKeyFactor);

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

        private SharedSizeHardwareKeyControl CreateRegularSharedSizeKeyControl()
        {
            var keyControl = new SharedSizeHardwareKeyControl(_regularKeySharedSizeGroup, _eventAggregator, _graphicsProvider);

            keyControl.BindToKeyViewModelDataContext();

            return keyControl;
        }

        private ProportionalSizeHardwareKeyControl CreateProportionalSizeKeyControl(float widthFactor)
        {
            var keyControl = new ProportionalSizeHardwareKeyControl(
                widthFactor,
                _regularKeySharedSizeGroup,
                _eventAggregator,
                _graphicsProvider);

            keyControl.BindToKeyViewModelDataContext();

            return keyControl;
        }
    }
}