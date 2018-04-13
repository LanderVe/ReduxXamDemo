using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  class DataState
  {
    public DataState(ImmutableSortedDictionary<int, Pizza> pizzas, ImmutableSortedDictionary<int, Size> sizes,
      ImmutableSortedDictionary<int, Topping> toppings, ImmutableSortedDictionary<int, Order> orders,
      ImmutableSortedDictionary<int, OrderDetail> orderDetails)
    {
      Pizzas = pizzas;
      Sizes = sizes;
      Toppings = toppings;
      Orders = orders;
      OrderDetails = orderDetails;
    }

    public ImmutableSortedDictionary<int, Pizza> Pizzas { get; }
    public ImmutableSortedDictionary<int, Size> Sizes { get; }
    public ImmutableSortedDictionary<int, Topping> Toppings { get; }
    public ImmutableSortedDictionary<int, Order> Orders { get; }
    public ImmutableSortedDictionary<int, OrderDetail> OrderDetails { get; }

    public Builder ToBuilder() => new Builder(Pizzas, Sizes, Toppings, Orders, OrderDetails);

    #region Builder
    internal class Builder
    {

      public Builder()
      {
        Pizzas = ImmutableSortedDictionary.Create<int, Pizza>();
        Sizes = ImmutableSortedDictionary.Create<int, Size>();
        Toppings = ImmutableSortedDictionary.Create<int, Topping>();
        Orders = ImmutableSortedDictionary.Create<int, Order>();
        OrderDetails = ImmutableSortedDictionary.Create<int, OrderDetail>();
      }

      public Builder(ImmutableSortedDictionary<int, Pizza> pizzas, ImmutableSortedDictionary<int, Size> sizes,
        ImmutableSortedDictionary<int, Topping> toppings, ImmutableSortedDictionary<int, Order> orders,
        ImmutableSortedDictionary<int, OrderDetail> orderDetails)
      {
        Pizzas = pizzas;
        Sizes = sizes;
        Toppings = toppings;
        Orders = orders;
        OrderDetails = orderDetails;
      }

      public ImmutableSortedDictionary<int, Pizza> Pizzas { get; set; }
      public ImmutableSortedDictionary<int, Size> Sizes { get; set; }
      public ImmutableSortedDictionary<int, Topping> Toppings { get; set; }
      public ImmutableSortedDictionary<int, Order> Orders { get; set; }
      public ImmutableSortedDictionary<int, OrderDetail> OrderDetails { get; set; }

      public DataState ToImmutable() => new DataState(Pizzas, Sizes, Toppings, Orders, OrderDetails);

      public Builder WithPizzas(ImmutableSortedDictionary<int, Pizza> pizzas) {
        Pizzas = pizzas;
        return this;
      }

      public Builder WithSizes(ImmutableSortedDictionary<int, Size> sizes)
      {
        Sizes = sizes;
        return this;
      }

      public Builder WithToppings(ImmutableSortedDictionary<int, Topping> toppings)
      {
        Toppings = toppings;
        return this;
      }

      public Builder WithOrders(ImmutableSortedDictionary<int, Order> orders)
      {
        Orders = orders;
        return this;
      }

      public Builder WithOrderDetails(ImmutableSortedDictionary<int, OrderDetail> orderDetails)
      {
        OrderDetails = orderDetails;
        return this;
      }

    }
    #endregion

  }
}
