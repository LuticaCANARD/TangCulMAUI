using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TangCulMAUI.Schema;
using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI.DataGrid
{
    public class PersonDataView : INotifyPropertyChanged
    {
        private List<Person> _people;
        private Person _humanToEdit;
        private Person _selectedItem;
        private bool _isRefreshing;
        private bool _teamColumnVisible = true;
        private bool _wonColumnVisible = true;
        private bool _headerBordersVisible = true;
        private bool _paginationEnabled = true;
        private ushort _teamColumnWidth = 70;

        public PersonDataView() 
        {
            People = [];
            CancelEditCommand = new Command(CmdCancelEdit);
            EditCommand = new Command<Person>(CmdEdit);
            RefreshCommand = new Command(CmdRefresh);
        }
        public Person HumanToEdit
        {
            get => _humanToEdit;
            set
            {
                _humanToEdit = value;
                OnPropertyChanged(nameof(HumanToEdit));
            }
        }

        public List<Person> People
        {
            get => _people;
            set
            {
                _people = value;
                OnPropertyChanged(nameof(People));
            }
        }

        public bool HeaderBordersVisible
        {
            get => _headerBordersVisible;
            set
            {
                _headerBordersVisible = value;
                OnPropertyChanged(nameof(HeaderBordersVisible));
            }
        }

        public bool TeamColumnVisible
        {
            get => _teamColumnVisible;
            set
            {
                _teamColumnVisible = value;
                OnPropertyChanged(nameof(TeamColumnVisible));
            }
        }

        public bool WonColumnVisible
        {
            get => _wonColumnVisible;
            set
            {
                _wonColumnVisible = value;
                OnPropertyChanged(nameof(WonColumnVisible));
            }
        }

        public ushort TeamColumnWidth
        {
            get => _teamColumnWidth;
            set
            {
                _teamColumnWidth = value;
                OnPropertyChanged(nameof(TeamColumnWidth));
            }
        }

        public bool PaginationEnabled
        {
            get => _paginationEnabled;
            set
            {
                _paginationEnabled = value;
                OnPropertyChanged(nameof(PaginationEnabled));
            }
        }

        public Person SelectedPerson
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                Debug.WriteLine("Team Selected : " + value?.Name);
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand CancelEditCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand RefreshCommand { get; }

        private void CmdCancelEdit()
        {
            HumanToEdit = null;
        }

        private void CmdEdit(Person humanToEdit)
        {
            ArgumentNullException.ThrowIfNull(humanToEdit);

            HumanToEdit = humanToEdit;
        }

        private async void CmdRefresh()
        {
            IsRefreshing = true;
            People = [];
            People = AppData.Instance.PersonData;
            IsRefreshing = false;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        #endregion INotifyPropertyChanged implementation

    }
}
