using System.Collections.ObjectModel;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.ViewModels
{
    public class MechanicalKeyboardLayoutViewModel : BaseViewModel
    {
        private ObservableCollection<ObservableCollection<HardwareKey>> _rows;

        public ObservableCollection<ObservableCollection<HardwareKey>> Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged();
            }
        }

        public MechanicalKeyboardLayoutViewModel()
        {
            _rows = new ObservableCollection<ObservableCollection<HardwareKey>>();
        }

        public void LoadMechanicalLayout(IMechanicalKeyboardLayout layout)
        {
            var newRows = new ObservableCollection<ObservableCollection<HardwareKey>>();

            foreach (var keyboardRow in layout.KeyboardRows)
            {
                var currentNewRow = new ObservableCollection<HardwareKey>(keyboardRow);
                newRows.Add(currentNewRow);
            }

            Rows = newRows;
        }
    }
}