﻿using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Immutable;
using System.Linq;
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

    public IObservable<IEnumerable<OrderDetailViewModel>> OrderDetailsStream { get; }
    public IObservable<string> TotalPriceStream { get; }
    public IObservable<bool> CanOrder { get; }
    public Command NewOrderDetailCommand { get; }
    public Command OrderCommand { get; }
    public Command DeleteOrderDetailCommand { get; }

    public MainViewModel(Store<ApplicationState> store)
    {
      this.store = store;

      //create new Order
      store.Dispatch(new CreateOrderAction());

      //bindable properties
      CanOrder = store.Grab(state => !state.CurrentOrder.OrderDetails.IsEmpty);

      var orderDetails = store.Grab(state => state.CurrentOrder.OrderDetails);
      var pizzas = store.Grab(state => state.Data.Pizzas);
      var sizes = store.Grab(state => state.Data.Sizes);
      var toppings = store.Grab(state => state.Data.Toppings);

      OrderDetailsStream = Observable.CombineLatest(orderDetails, pizzas, sizes, toppings, MapToOrderDetailViewModels);

      TotalPriceStream = OrderDetailsStream.Select(vms =>
      {
        var total = vms.Sum(vm => vm.Price);
        return string.Format("Total: {0:C}", total);
      });

      //Commands
      NewOrderDetailCommand = new Command(GoToPizzaSelection);
      DeleteOrderDetailCommand = new Command<OrderDetailViewModel>(OnDeleteOrderDetail);
      OrderCommand = new Command(MakeOrder);
    }

    private List<OrderDetailViewModel> MapToOrderDetailViewModels(ImmutableList<OrderDetail> orderDetails,
      ImmutableSortedDictionary<int, Pizza> pizzas,
      ImmutableSortedDictionary<int, State.Models.Size> sizes,
      ImmutableSortedDictionary<int, Topping> toppings)
    {
      return orderDetails
        .Where(od => od.PizzaId.HasValue && od.SizeId.HasValue)
        .Select((od, index) =>
        {
          var pizza = pizzas[od.PizzaId.Value];
          var size = sizes[od.SizeId.Value];
          var selectedToppings = od.ToppingIds.Select(tid => toppings[tid]).ToList();

          return MapToOrderDetailViewModel(od, pizza, size, selectedToppings, index);
        }).ToList();
    }

    private OrderDetailViewModel MapToOrderDetailViewModel(OrderDetail orderDetail, Pizza pizza,
      State.Models.Size size, IEnumerable<Topping> toppings, int index)
    {
      var price = pizza.BasePrice * size.PriceMultiplier;
      foreach (var topping in toppings)
        price += topping.Price;

      return new OrderDetailViewModel
      {
        OrderDetailId = orderDetail.Id,
        Description = $"{pizza.Type} - {size.Name}",
        Price = price,
        Index = index
      };
    }

    public void GoToPizzaSelection()
    {
      //make new order detail
      store.Dispatch(new CreateOrderDetailAction());

      //navigate
      store.Dispatch(new NavigateToAction(nameof(SelectPizzaViewModel)));
    }
    private void OnDeleteOrderDetail(OrderDetailViewModel vm)
    {
      store.Dispatch(new RemoveOrderDetailAction(vm.Index));
    }

    private void MakeOrder()
    {
      //TODO
      Debug.WriteLine("Order!");
    }


  }

  public class OrderDetailViewModel
  {
    public int Index { get; set; }
    public int OrderDetailId { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
  }
}
