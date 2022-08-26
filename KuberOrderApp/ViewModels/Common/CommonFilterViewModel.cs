using System;
using System.Collections.Generic;
using KuberOrderApp.Models.StaticModels;
using KuberOrderApp.ViewModels.Base;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Common
{
    public class CommonFilterViewModel : BaseViewModel
    {
        #region Field Section
        private bool _isFromGroup;
        private bool _isFromCategory;
        private bool _isFromType;
        private bool _isFromMaster;
        private List<string> _filterList;
        private string _selectedFilterList;
        private string _title;
        #endregion

        #region Properties
        public bool IsFromGroup
        {
            get { return _isFromGroup; }
            set { SetProperty(ref _isFromGroup, value); }
        }
        public bool IsFromCategory
        {
            get { return _isFromCategory; }
            set { SetProperty(ref _isFromCategory, value); }
        }
        public bool IsFromType
        {
            get { return _isFromType; }
            set { SetProperty(ref _isFromType, value); }
        }
        public bool IsFromMaster
        {
            get { return _isFromMaster; }
            set { SetProperty(ref _isFromMaster, value); }
        }
        public List<string> FilterList
        {
            get { return _filterList; }
            set { SetProperty(ref _filterList, value); }
        }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public string SelectedFilterList
        {
            get { return _selectedFilterList; }
            set { SetProperty(ref _selectedFilterList, value); }
        }
        #endregion


        #region Constructor
        public CommonFilterViewModel()
        {
            SelectionCommand = new Command(() => OnSelected());
        }
        #endregion

        #region Public Methods
        public void SetData()
        {
            if (IsFromGroup)
                Title = "Select Group";
            else if (IsFromCategory)
                Title = "Select Category";
            else if (IsFromType)
                Title = "Select Type";
            else if (IsFromMaster)
                Title = "Select Master";
        }
        #endregion

        #region Private Methods
        async private void OnSelected()
        {
            if (SelectedFilterList == null)
                return;

            if (IsFromGroup)
                MessagingCenter.Send<string, string>("True", "GroupFilter", SelectedFilterList);
            else if (IsFromCategory)
                MessagingCenter.Send<string, string>("True", "CategoryFilter", SelectedFilterList);
            else if (IsFromType)
                MessagingCenter.Send<string, string>("True", "TypeFilter", SelectedFilterList);
            else if (IsFromMaster)
                MessagingCenter.Send<string, string>("True", "MasterFilter", SelectedFilterList);

            await App.Current.MainPage.Navigation.PopAsync();
            SelectedFilterList = null;
        }
        #endregion
    }
}
