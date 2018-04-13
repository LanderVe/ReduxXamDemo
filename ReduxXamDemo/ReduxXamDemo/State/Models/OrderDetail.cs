using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ReduxXamDemo.State.Models
{
  public class OrderDetail
  {
    public OrderDetail(int id, int orderId, int? pizzaId, int? sizeId, ImmutableList<int> toppings)
    {
      Id = id;
      OrderId = orderId;
      PizzaId = pizzaId;
      SizeId = sizeId;
      ToppingIds = toppings;
    }

    public int Id { get; }
    public int OrderId { get; }
    public int? PizzaId { get; }
    public int? SizeId { get; }
    public ImmutableList<int> ToppingIds { get; } //Replace with many-to-many?

    //public decimal GetDetailPrice() TODO into selector?
    //{
    //  if (Pizza == null || Size == null) return 0M;

    //  return Pizza.BasePrice * Size.PriceMultiplier + Toppings.Sum(t => t.Price);
    //}

    public Builder ToBuilder() => new Builder(Id, OrderId, PizzaId, SizeId, ToppingIds);


    #region Builder
    public class Builder
    {
      public Builder()
      {
        Id = 0;
        OrderId = 0;
        PizzaId = null;
        SizeId = null;
        ToppingIds = ImmutableList<int>.Empty;
      }

      public Builder(int id, int orderId, int? pizzaId, int? sizeId, ImmutableList<int> toppingIds)
      {
        Id = id;
        OrderId = orderId;
        PizzaId = pizzaId;
        SizeId = sizeId;
        ToppingIds = toppingIds;
      }

      public int Id { get; set; }
      public int OrderId { get; set; }
      public int? PizzaId { get; set; }
      public int? SizeId { get; set; }
      public ImmutableList<int> ToppingIds { get; set; }

      public OrderDetail ToImmutable() => new OrderDetail(Id, OrderId, PizzaId, SizeId, ToppingIds);

      public Builder WithId(int id)
      {
        Id = Id;
        return this;
      }

      public Builder WithOrderId(int orderId)
      {
        OrderId = orderId;
        return this;
      }

      public Builder WithPizzaId(int pizzaId)
      {
        PizzaId = pizzaId;
        return this;
      }

      public Builder WithSizeId(int sizeId)
      {
        SizeId = sizeId;
        return this;
      }

      public Builder WithToppingIds(ImmutableList<int> toppingIds)
      {
        ToppingIds = toppingIds;
        return this;
      }

    }
    #endregion
  }
}
