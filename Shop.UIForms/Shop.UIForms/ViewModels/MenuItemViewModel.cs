using GalaSoft.MvvmLight.Command;
using Shop.UIForms.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    class MenuItemViewModel : Common.Models.Menu

    {
        public ICommand SelectMenuCommand => new RelayCommand(this.SelectMenu);

        private async void SelectMenu()
        {
            App.Master.IsPresented = false;
            var mainViewModel = MainViewModel.GetInstance();
            Console.WriteLine(this.PageName);
            switch (this.PageName)
            {
                
                case "Dashboard":
                    await App.Navigator.PushAsync(new Dashboard());
                    break;
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;
                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;
                case "ProductsPage":
                    mainViewModel.Products = new ProductsViewModel();
                    await App.Navigator.PushAsync(new ProductsPage());
                    break;


                default:
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        }

    }
}

