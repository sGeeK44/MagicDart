using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace MagicDart.Mvvm
{
    public class ViewModel<T> : ViewModel where T : ViewModel, new()
    {
        protected static T CreateViewModel => ViewModelLocator.CreateViewModel<T>();
    }

    public class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            OpenCommand = new Command(OnOpen);
            CloseCommand = new Command(OnClose);
        }

        public ICommand OpenCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        protected virtual void OnOpen() { }

        protected virtual void OnClose() { }

        protected void Set<T>(string propertyName, ref T field, T value)
        {
            VerifyPropertyName(propertyName);

            if (Equals(field, value)) return;
            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetupDesignMode() { }

        public void Dispose()
        {

        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;

            throw new Exception($"Invalid property name. propertyName:{propertyName}.");
        }
    }
}