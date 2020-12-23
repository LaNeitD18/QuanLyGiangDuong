using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private bool _IsBeingInTask;
        public bool IsBeingInTask
        {
            get { return _IsBeingInTask; }
            set { _IsBeingInTask = value; OnPropertyChanged(); }
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

        private ObservableCollection<ROOM> _ListSelectedRoom;
        public ObservableCollection<ROOM> ListSelectedRoom
        {
            get { return _ListSelectedRoom; }
            set { _ListSelectedRoom = value; OnPropertyChanged(); }
        }
        #endregion

        #region Function
        private void LoadData()
        {
            var listRoom = DataProvider.Ins.DB.ROOMs.Where(x => x.RoomID != "[NULL]");
            ListRoom = new ObservableCollection<ROOM>(listRoom);
            ListSelectedRoom = new ObservableCollection<ROOM>();

            IsEnabledInput = false;
        }
        private void AddRoom()
        {
            // add room to database
            var room = new ROOM()
            {
                RoomID = RoomID,
                Status_ = "Còn sử dụng",
                Capacity = Int32.Parse(Capacity),
                Description_ = Description
            };
            DataProvider.Ins.DB.ROOMs.Add(room);
            DataProvider.Ins.DB.SaveChanges();

            // add room to listroom for displaying
            ListRoom.Add(room);
        }
        private void EditRoom() {
            SelectedRoom.Capacity = Int32.Parse(Capacity);
            SelectedRoom.Description_ = Description;
            DataProvider.Ins.DB.SaveChanges();

            var temp = SelectedRoom;
            int length = ListRoom.Count();
            for (int i = 0; i < length; i++)
            {
                if (ListRoom[i].RoomID == SelectedRoom.RoomID)
                {
                    ListRoom.RemoveAt(i);
                    ListRoom.Insert(i, temp);
                    break;
                }
            }

            SelectedRoom = temp;
        }
        private void DeleteRoom() {
            // check if not used anymore first
            if(IsNotUsing()) {
                SelectedRoom.Status_ = "Không còn sử dụng";
                DataProvider.Ins.DB.SaveChanges();

                var temp = SelectedRoom;
                for (int i = 0; i < ListRoom.Count(); i++)
                {
                    if (ListRoom[i].RoomID == SelectedRoom.RoomID)
                    {
                        ListRoom.RemoveAt(i);
                        ListRoom.Insert(i, temp);
                        break;
                    }
                }
            } else {
                MessageBox.Show("Phong dang duoc su dung");
            }
        }
        private void DeleteRooms() {
            List<ROOM> list = ListSelectedRoom.ToList();
            foreach(var room in list) {
                SelectedRoom = room;
                DeleteRoom();
            }
        }

        // disable 3 buttons: add, edit, delete
        private void DisableButtons() {
            IsAddingEnabled = false;
            IsEditingEnabled = false;
            IsDeletingEnabled = false;
            IsBeingInTask = false;
        }
        private void EnableOnlyAdding() {
            DisableButtons();
            IsAddingEnabled = true;
            IsBeingInTask = true;
        }
        private void EnableOnlyEditing() {
            DisableButtons();
            IsEditingEnabled = true;
            IsBeingInTask = true;
        }
        private void EnableOnlyDeleting() {
            DisableButtons();
            IsDeletingEnabled = true;
            IsBeingInTask = true;
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

            RoomIDForSearching = null;
        }
        private void ResetAll() {
            ResetTextbox();
            DisableTextboxes();
            EnableOnlyAdding();
            IsBeingInTask = false;

            SelectedRoom = null;
        }
        private void SetValueForTextbox() {
            RoomID = SelectedRoom.RoomID;
            Capacity = SelectedRoom.Capacity.ToString();
            Description = SelectedRoom.Description_;
        }
        private bool IsValidInput() {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(RoomID)) {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(Capacity)) {
                flag = false;
            }
            return flag;
        }
        private bool IsNotDuplicatedForAdding() {
            bool flag = true;
            var roomID = DataProvider.Ins.DB.ROOMs.Where(x => x.RoomID == RoomID);

            if(roomID.Count() != 0) {
                MessageBox.Show("Duplicated");
                flag = false;
            }

            return flag;
        }
        private bool IsNotDuplicatedForEditing()
        {
            bool flag = true;
            
            if(Capacity == SelectedRoom.Capacity.ToString()) {
                if (SelectedRoom.Description_ != null && Description == SelectedRoom.Description_) {
                    MessageBox.Show("Trung desc vaf capa");
                    flag = false;
                }
            }

            return flag;
        }
        private bool IsNotUsing() {
            var countUsingClass = DataProvider.Ins.DB.USINGCLASSes.Where(x => x.RoomID == SelectedRoom.RoomID).Count();
            var countUsingEvent = DataProvider.Ins.DB.USINGEVENTs.Where(x => x.RoomID == SelectedRoom.RoomID).Count();
            var countUsingExam = DataProvider.Ins.DB.USINGEXAMs.Where(x => x.RoomID == SelectedRoom.RoomID).Count();

            if(countUsingClass==0 && countUsingEvent==0 && countUsingExam==0) {
                return true;
            }
            return false;
        }
        //private bool CheckValidData() {
        //    bool flag = true;

        //    if(IsIDEnabled) {
        //        IsValidForAdding(ref flag);
        //        IsNotDuplicatedForAdding(ref flag);
        //        return flag;
        //    }

        //    return false;
        //}
        private void GetDataGridSelectedItems(DataGrid dataGrid) {
            ListSelectedRoom.Clear();

            foreach(var item in dataGrid.SelectedItems) {
                ListSelectedRoom.Add((ROOM)item);
            }
        }
        #endregion

        #region ICommand
        public ICommand AddRoomCommand { get; set; }
        public ICommand EditRoomCommand { get; set; }
        public ICommand DeleteRoomCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchRoomCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        #endregion

        public RoomManagementViewModel()
        {
            // init values
            LoadData();

            EnableOnlyAdding();
            IsBeingInTask = false;
            DisableTextboxes();

            // ICommands
            AddRoomCommand = new RelayCommand((p) => {
                EnableOnlyAdding(); // buttons

                EnableTextboxes();
                ResetTextbox();
                SelectedRoom = null;
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

                if (room != null)
                {
                    SelectedRoom = room;

                    SetValueForTextbox();
                }

                //for(int i=0; i<SelectedRoom.Count(); i++)
                //{
                //    MessageBox.Show(ListRoom[i].RoomID);
                //}
            });

            // confirm button is enabled only when adding, editing or deleting
            ConfirmCommand = new RelayCommand((p) => {
                if(IsIDEnabled) {
                    if(IsValidInput() && IsNotDuplicatedForAdding()) {
                        AddRoom();
                        ResetAll();
                    }
                }
                if(IsBeingInTask && IsEditingEnabled) {
                    if(IsValidInput() && IsNotDuplicatedForEditing()) {
                        EditRoom();
                        ResetAll();
                    }
                }
                if(IsBeingInTask && IsDeletingEnabled) {
                    if(ListSelectedRoom.Count() == 1) {
                        DeleteRoom();
                        ResetAll();
                    } else {
                        DeleteRooms();
                        ResetAll();
                    }
                }
            });

            CancelCommand = new RelayCommand((p) => {
                ResetAll();
            });

            SelectionChangedCommand = new RelayCommand((p) =>
            {
                if (!(p is  DataGrid))
                    return;

                var dataGrid = p as DataGrid;
                GetDataGridSelectedItems(dataGrid);

                if(SelectedRoom != null) {
                    SetValueForTextbox();

                    IsEditingEnabled = IsDeletingEnabled = true;
                }
            });
        }
    }
}
