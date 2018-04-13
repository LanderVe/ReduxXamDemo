using ReduxLib;
using ReduxXamDemo.Services;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Xamarin.Forms;
using ReduxXamDemo.State.Actions;

namespace ReduxXamDemo.ViewModels
{
  public class SelectPizzaViewModel : ViewModelBase
  {
    private readonly Store<ApplicationState> store;
    private IDisposable subscription;
    public int? currentOrderDetailId;

    public Command SelectPizza { get; }
    public IObservable<IOrderedEnumerable<Pizza>> PizzasStream { get; }

    public SelectPizzaViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //bindable observables
      PizzasStream = store.Select(state => state.Data.Pizzas.Values.OrderBy(s => s.BasePrice));

      //commands
      SelectPizza = new Command<Pizza>(OnSelectPizza);
    }

    public override void OnLoaded()
    {
      //subscriptions
      subscription = store.Select(state => state.UI.Order.CurrentOrderDetailId)
        .Subscribe(cid => currentOrderDetailId = cid);
    }

    public override void OnUnloaded()
    {
      subscription.Dispose();
    }

    private void OnSelectPizza(Pizza selectedPizza)
    {
      if (currentOrderDetailId != null)
      {
        store.Dispatch(new SetPizzaAction(currentOrderDetailId.Value, selectedPizza.Id));
        store.Dispatch(new NavigateToAction(nameof(SelectSizeViewModel)));
      }
    }

  }
}
