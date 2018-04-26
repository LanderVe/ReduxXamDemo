using Autofac;
using ReduxXamDemo.Services;
using ReduxXamDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReduxXamDemo.Views
{
  public class ViewPage<TViewModel> : ContentPage where TViewModel : ViewModelBase
  {
    public TViewModel ViewModel { get; }

    public ViewPage()
    {
      ViewModel = App.Container.Resolve<TViewModel>();
      BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
      ViewModel.OnLoaded();
      BindingContext = ViewModel;
    }

    protected override void OnDisappearing()
    {
      ViewModel.OnUnloaded();
      BindingContext = null;
    }

  }
}
