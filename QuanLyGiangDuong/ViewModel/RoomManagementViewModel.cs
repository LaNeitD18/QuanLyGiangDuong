using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGiangDuong.ViewModel
{
    class RoomManagementViewModel : BaseViewModel
    {
        #region Variable
        private ObservableCollection<ROOM> _ListRoom;

        public ObservableCollection<ROOM> ListRoom
        {
            get { return _ListRoom; }
            set { _ListRoom = value; OnPropertyChanged(); }
        }

        private string _RoomID;

        public string RoomID
        {
            get { return _RoomID; }
            set { _RoomID = value; OnPropertyChanged(); }
        }

        private string _Capacity;

        public string Capacity
        {
            get { return _Capacity; }
            set { _Capacity = value; OnPropertyChanged(); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; OnPropertyChanged(); }
        }

        private bool _IsEnabledInput;
        public bool IsEnabledInput
        {
            get { return _IsEnabledInput; }
            set { _IsEnabledInput = value; OnPropertyChanged(); }
        }

        private bool _IsAddingEnabled;
        public bool IsAddingEnabled
        {
            get { return _IsAddingEnabled; }
            set { _IsAddingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsEditingEnabled;
        public bool IsEditingEnabled
        {
            get { return _IsEditingEnabled; }
            set { _IsEditingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsDeletingEnabled;
        public bool IsDeletingEnabled
        {
            get { return _IsDeletingEnabled; }
            set { _IsDeletingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsIDEnabled;
        public bool IsIDEnabled
        {
            get { return _IsIDEnabled; }
            set { _IsIDEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsCapacityEnabled;
        public bool IsCapacityEnabled
        {
            get { return _IsCapacityEnabled; }
            set { _IsCapacityEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsDescriptionEnabled;
        public bool IsDescriptionEnabled
        {
            get { return _IsDescriptionEnabled; }
            set { _IsDescriptionEnabled = value; OnPropertyChanged(); }
        }

        private string _RoomIDForSearching;
        public string RoomIDForSearching
        {
            get { return _RoomIDForSearching; }
            set { _RoomIDForSearching = value; OnPropertyChanged(); }
        }

        private ROOM _SelectedRoom;
        public ROOM SelectedRoom
        {
            get { return _SelectedRoom; }
            set { _SelectedRoom = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommand
        public ICommand AddRoomCommand { get; set; }
        public ICommand EditRoomCommand { get; set; }
        public ICommand DeleteRoomCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchRoomCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        #endregion

        #region Function
        private void LoadData()
        {
            var listRoom = DataProvider.Ins.DB.ROOMs;
            ListRoom = new ObservableCollection<ROOM>(listRoom);

            IsEnabledInput = false;
        }
        private void AddRoom()
        {
            // add room to database
            var room = new ROOM()
            {
                RoomID = RoomID,
                Capacity = Int32.Parse(Capacity),
                Description_ = Description
            };
            DataProvider.Ins.DB.ROOMs.Add(room);
            DataProvider.Ins.DB.SaveChanges();

            // add room to listroom for displaying
            ListRoom.Add(room);
        }
        // disable 3 buttons: add, edit, delete
        private void DisableButtons() {
            IsAddingEnabled = false;
            IsEditingEnabled = false;
            IsDeletingEnabled = false;
        }
        private void EnableOnlyAdding() {
            DisableButtons();
            IsAddingEnabled = true;
        }
        private void EnableOnlyEditing() {
            DisableButtons();
            IsEditingEnabled = true;
        }
        private void EnableOnlyDeleting() {
            DisableButtons();
            IsDeletingEnabled = true;
        }
        private void EnableTextboxes() {
            IsIDEnabled = true;
            IsCapacityEnabled = true;
            IsDescriptionEnabled = true;
        }
        private void DisableTextboxes()
        {
            IsIDEnabled = false;
            IsCapacityEnabled = false;
            IsDescriptionEnabled = false;
        }
        private void ResetTextbox() {
            RoomID = null;
            Capacity = null;
            Description = null;
        }
        private void ResetAll() {
            ResetTextbox();
            EnableOnlyAdding();

            SelectedRoom = null;
        }
        private void SetValueForTextbox() {
            RoomID = SelectedRoom.RoomID;
            Capacity = SelectedRoom.Capacity.ToString();
            Description = SelectedRoom.Description_;
        }
        private bool IsValidForAdding(ref bool flag) {
            if (string.IsNullOrWhiteSpace(RoomID)) {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(Capacity)) {
                flag = false;
            }
            return flag;
        }
        private bool IsNotDuplicatedForAdding(ref bool flag) {
            var roomID = DataProvider.Ins.DB.ROOMs.Where(x => x.RoomID == RoomID);

            if(roomID.Count() != 0) {
                MessageBox.Show("Duplicated");
                flag = false;
            }

            return flag;
        }
        private bool CheckValidData() {
            bool flag = true;

            IsValidForAdding(ref flag);
            IsNotDuplicatedForAdding(ref flag);

            return flag;
        }
        #endregion

        public RoomManagementViewModel()
        {
            // init values
            LoadData();
            EnableOnlyAdding();
            DisableTextboxes();

            // ICommands
            AddRoomCommand = new RelayCommand((p) => {
                //if(CheckValidData()) {
                //    AddRoom();
                //}
                //ResetAll();

                EnableOnlyAdding(); // buttons

                EnableTextboxes();
                ResetTextbox();
            });

            EditRoomCommand = new RelayCommand((p) => {
                EnableOnlyEditing(); // buttons

                EnableTextboxes();
                IsIDEnabled = false;
            });

            DeleteRoomCommand = new RelayCommand((p) => {
                EnableOnlyDeleting(); // buttons

                DisableTextboxes();
            });

            SearchRoomCommand = new RelayCommand((p) => {
                var room = DataProvider.Ins.DB.ROOMs.Find(RoomIDForSearching);

                if(room != null) {
                    SelectedRoom = room;

                    SetValueForTextbox();
                }
            });

            CancelCommand = new RelayCommand((p) => {
                ResetAll();
            });

            SelectionChangedCommand = new RelayCommand((p) =>
            {
                if(SelectedRoom != null) {
                    SetValueForTextbox();

                    IsEditingEnabled = IsDeletingEnabled = true;
                }
            });
        }
    }
}
