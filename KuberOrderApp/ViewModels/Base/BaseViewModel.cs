using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KuberOrderApp.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Field Section
        private bool _isBusy;
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        #endregion

        #region Commands
        public ICommand PrintPDFCommand { protected set; get; }
        public ICommand ShareCommand { protected set; get; }
        public ICommand SelectionCommand { protected set; get; }
        public ICommand LoadMoreCommand { protected set; get; }
        #endregion

        #region Constructor
        public BaseViewModel()
        {
        }
        #endregion

        #region Binding event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Methods
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}