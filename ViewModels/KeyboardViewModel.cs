using NTouchTypeTrainer.Contracts.Domain;

namespace NTouchTypeTrainer.ViewModels
{
    public class KeyboardViewModel : BaseViewModel
    {
        private AllKeysViewModel _allKeysViewModel;
        private MechanicalKeyboardLayoutViewModel _mechanicalKeyboardLayoutViewModel;

        public AllKeysViewModel AllKeysViewModel
        {
            get => _allKeysViewModel;
            set => SetProperty(ref _allKeysViewModel, value);
        }

        public MechanicalKeyboardLayoutViewModel MechanicalKeyboardLayoutViewModel
        {
            get => _mechanicalKeyboardLayoutViewModel;
            set
            {
                _mechanicalKeyboardLayoutViewModel = value;
                OnPropertyChanged();
            }
        }

        public KeyboardViewModel()
        {
            _allKeysViewModel = new AllKeysViewModel();
            _mechanicalKeyboardLayoutViewModel = new MechanicalKeyboardLayoutViewModel();
        }

        public void LoadFingerColors(IFingerColors fingerColors)
            => AllKeysViewModel.LoadFingerColors(fingerColors);

        public void LoadFingerPositions(IFingerPositions fingerPositions)
            => AllKeysViewModel.LoadFingerPositions(fingerPositions);

        public void LoadMechanicalLayout(IMechanicalKeyboardLayout mechanicalLayout)
        {
            MechanicalKeyboardLayoutViewModel.LoadMechanicalLayout(mechanicalLayout);
            AllKeysViewModel.LoadMechanicalKeyboardLayout(mechanicalLayout);
        }
    }
}