using MagicDart.Mvvm;

namespace Zg.Core.Windows
{
    public static class ViewModelLocator
    {
        public static T CreateViewModel<T>() where T : ViewModel, new()
        {
            var viewModel = new T();

            if (DesignTime.IsOn)
                viewModel.SetupDesignMode();

            return viewModel;
        }
    }
}
