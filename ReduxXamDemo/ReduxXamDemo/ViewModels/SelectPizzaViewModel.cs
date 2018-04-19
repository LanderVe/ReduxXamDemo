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
using System.Reactive.Subjects;
using ReduxXamDemo.Utils;
using System.Diagnostics;

namespace ReduxXamDemo.ViewModels
{
  public class SelectPizzaViewModel : ViewModelBase
  {
    private readonly Store<ApplicationState> store;
    private List<IDisposable> subscriptions = new List<IDisposable>();

    public Command SelectPizza { get; }
    public IObservable<ImmutableList<Pizza>> PizzasStream { get; }
    public Subject<string> SearchTerm { get; } = new Subject<string>();

    public SelectPizzaViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //bindable observables
      var allPizzaStream = store.Select(state => state.Data.Pizzas.Values.OrderBy(s => s.BasePrice));

      var searchStream = store.Select(state => state.View.SelectPizza);

      PizzasStream = allPizzaStream.CombineLatest(searchStream, GetFilteredPizzas);
      //commands
      SelectPizza = new Command<Pizza>(OnSelectPizza);
    }

    private ImmutableList<Pizza> GetFilteredPizzas(IOrderedEnumerable<Pizza> allPizzas, SelectPizzaState searchState)
    {
      if (String.IsNullOrEmpty(searchState.SearchTerm))
        return allPizzas.ToImmutableList();
      else
        return allPizzas.Where(p => p.Type.ToLower().Contains(searchState.SearchTerm.ToLower())).ToImmutableList();
    }

    public override void OnLoaded()
    {
      //subscriptions
      store.Select(state => state.View.SelectPizza.SearchTerm)
        .Subscribe(SearchTerm, subscriptions);

      //dispatches new search when term value has been changed
      SearchTerm.Subscribe(term => store.Dispatch(new SearchPizzasAction(term)), subscriptions);
    }

    public override void OnUnloaded()
    {
      foreach (var subscription in subscriptions)
      {
        subscription.Dispose();
      }
    }

    private void OnSelectPizza(Pizza selectedPizza)
    {
      store.Dispatch(new SetPizzaAction(selectedPizza.Id));
      store.Dispatch(new NavigateToAction(nameof(SelectSizeViewModel)));
    }

  }
}
