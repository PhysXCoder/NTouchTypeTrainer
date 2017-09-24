using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NTouchTypeTrainer.ViewModels
{
    public class KeyRowViewModel : BaseViewModel
    {
        private int _rowIndex;
        private ObservableCollection<KeyViewModel> _keys;

        public ObservableCollection<KeyViewModel> Keys
        {
            get => _keys;
            set
            {
                _keys = value;
                NotifyOfPropertyChange();
            }
        }

        public int RowIndex
        {
            get => _rowIndex;
            set
            {
                _rowIndex = value;
                NotifyOfPropertyChange();
            }
        }

        public KeyRowViewModel()
            : this(null, -1)
        { }

        public KeyRowViewModel(IEnumerable<KeyViewModel> keyViewModels, int rowIndex)
        {
            _rowIndex = rowIndex;

            _keys = keyViewModels != null
                ? new ObservableCollection<KeyViewModel>(keyViewModels)
                : new ObservableCollection<KeyViewModel>();
        }
    }
}