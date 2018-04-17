using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReduxXamDemo.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private readonly Store<ApplicationState> store;

    public Command NewOrderCommand { get; private set; }

    public MainViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //Commands
      NewOrderCommand = new Command(GoToPizzaSelection);
    }

    public void GoToPizzaSelection()
      => store.Dispatch(new NavigateToAction(nameof(SelectPizzaViewModel)));

  }
}
