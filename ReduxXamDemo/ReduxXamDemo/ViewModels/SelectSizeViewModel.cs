using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using XF = Xamarin.Forms;

namespace ReduxXamDemo.ViewModels
{
  public class SelectSizeViewModel : ViewModelBase
  {
    private readonly Store<ApplicationState> store;

    public XF.Command SelectSize { get; }
    public IObservable<IEnumerable<SizeOptionViewModel>> SizesStream { get; }

    public SelectSizeViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //bindable observables
      var pizzas = store.Grab(state => state.Data.Pizzas);

      var sizes = store.Grab(state => state.Data.Sizes);

      var selectedPizzaId = store.Grab(state => state.CurrentOrder)
        .Select(co => co.OrderDetails[co.CurrentOrderDetailId].PizzaId).Where(pid => pid.HasValue);

      var selectedPizza = pizzas.CombineLatest(selectedPizzaId, (p, id) => p[id.Value]);

      SizesStream = sizes.CombineLatest(selectedPizza, (s, p) => MapSizeToViewModel(s, p));

      //commands
      SelectSize = new XF.Command<SizeOptionViewModel>(OnSelectSize);
    }

    private IEnumerable<SizeOptionViewModel> MapSizeToViewModel(ImmutableSortedDictionary<int, Size> sizes, Pizza selectedPizza)
    {
      return sizes.Values
        .OrderBy(s => s.PriceMultiplier)
        .Select(s => new SizeOptionViewModel {
            Id = s.Id,
            Name = s.Name,
            FormattedPrice = string.Format("{0:C}", selectedPizza.BasePrice * s.PriceMultiplier)
          });
    }

    private void OnSelectSize(SizeOptionViewModel selectedSize)
    {
      store.Dispatch(new SetSizeAction(selectedSize.Id));
      store.Dispatch(new NavigateToAction(nameof(SelectToppingsViewModel)));
    }
  }

  public class SizeOptionViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string FormattedPrice { get; set; }
  }

}
