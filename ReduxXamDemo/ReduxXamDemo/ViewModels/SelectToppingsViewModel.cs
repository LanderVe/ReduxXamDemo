using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Forms;

namespace ReduxXamDemo.ViewModels
{
  public class SelectToppingsViewModel : ViewModelBase
  {
    private readonly Store<ApplicationState> store;

    public IObservable<List<ToppingViewModel>> ToppingsStream { get; }
    public Command FinishCommand { get; }


    public SelectToppingsViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //bindable observables
      var toppings = store.Grab(state => state.Data.Toppings)
        .Select(ts => ts.Values.OrderBy(t => t.Name));

      var selectedToppings = store.Grab(state => state.CurrentOrder)
        .Where(co => co.CurrentOrderDetailIndex.HasValue)
        .Select(co => co.OrderDetails[co.CurrentOrderDetailIndex.Value].ToppingIds);

      ToppingsStream = toppings
        .CombineLatest(selectedToppings, MapToppingsToViewModels)
        //register for change events
        .Scan(new List<ToppingViewModel>(), (oldToppings, newToppings) =>
        {
          // remove old event listeners
          foreach (var tvm in oldToppings)
            tvm.PropertyChanged -= ToppingSelectionChangd;

          // add new event listeners
          foreach (var tvm in newToppings)
            tvm.PropertyChanged += ToppingSelectionChangd;

          return newToppings;
        });

      //Commands
      FinishCommand = new Command(OnFinish);
    }

    private List<ToppingViewModel> MapToppingsToViewModels(IEnumerable<Topping> toppings, ImmutableList<int> selectedIds) 
      => toppings.Select(t => MapToppingToViewModel(t, selectedIds.Contains(t.Id))).ToList();

    private ToppingViewModel MapToppingToViewModel(Topping topping, bool isSelected)
    {
      return new ToppingViewModel
      {
        Id = topping.Id,
        Description = $"{topping.Name} ({string.Format("{0:C}", topping.Price)})",
        IsSelected = isSelected
      };
    }

    private void ToppingSelectionChangd(object sender, PropertyChangedEventArgs e)
    {
      var topping = (ToppingViewModel)sender;
      if (topping.IsSelected)
        store.Dispatch(new AddToppingAction(topping.Id));
      else
        store.Dispatch(new RemoveToppingAction(topping.Id));
    }

    private void OnFinish()
    {
      store.Dispatch(new NavigateBackToAction(nameof(MainViewModel)));
    }
  }

  public class ToppingViewModel : INotifyPropertyChanged
  {
    public int Id { get; set; }
    public string Description { get; set; }

    private bool _IsSelected;
    public bool IsSelected
    {
      get { return _IsSelected; }
      set
      {
        if (!Object.Equals(value, _IsSelected))
        {
          _IsSelected = value;
          OnPropertyChanged(nameof(IsSelected));
        }
      }
    }


    #region Notify Property Changed Members
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }

}
