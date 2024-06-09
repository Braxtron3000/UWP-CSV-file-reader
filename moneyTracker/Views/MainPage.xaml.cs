using System;

using moneyTracker.ViewModels;

using Windows.UI.Xaml.Controls;

namespace moneyTracker.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
