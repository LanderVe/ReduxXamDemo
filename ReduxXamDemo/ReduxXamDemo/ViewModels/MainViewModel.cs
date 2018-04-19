using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReduxXamDemo.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private readonly Store<ApplicationState> store;

    public IObservable<bool> IsOrderButtonVisible { get; }
    public Command NewOrderDetailCommand { get; }
    public Command OrderCommand { get; }

    public MainViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //create new Order
      store.Dispatch(new CreateOrderAction());

      //bindable properties
      IsOrderButtonVisible = store.Select(state => !state.CurrentOrder.OrderDetails.IsEmpty);

      //Commands
      NewOrderDetailCommand = new Command(GoToPizzaSelection);
      OrderCommand = new Command(MakeOrder);
    }
    public void GoToPizzaSelection()
    {
      //make new order detail
      store.Dispatch(new CreateOrderDetailAction());

      //navigate
      store.Dispatch(new NavigateToAction(nameof(SelectPizzaViewModel)));
    }

    private void MakeOrder()
    {
      Debug.WriteLine("Order!");
    }

  }
}
