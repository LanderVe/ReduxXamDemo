﻿using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Actions
{
  class CreateOrderAction { }

  class CreateOrderDetailAction { }

  class SetCurrentOrderDetailAction {
    public SetCurrentOrderDetailAction(int? currentOrderDetailIndex)
    {
      CurrentOrderDetailIndex = currentOrderDetailIndex;
    }

    public int? CurrentOrderDetailIndex { get; }
  }

  class RemoveOrderDetailAction
  {
    public RemoveOrderDetailAction(int index)
    {
      Index = index;
    }

    public int Index { get; }
  }

  class SetPizzaAction
  {
    public SetPizzaAction(int pizzaId)
    {
      PizzaId = pizzaId;
    }

    public int PizzaId { get; }
  }

  class SetSizeAction
  {
    public SetSizeAction(int sizeId)
    {
      SizeId = sizeId;
    }

    public int SizeId { get; }
  }

  class SetToppingsAction
  {
    public SetToppingsAction(ImmutableList<int> toppingIds)
    {
      ToppingIds = toppingIds;
    }

    public ImmutableList<int> ToppingIds { get;}
  }

  class AddToppingAction
  {
    public AddToppingAction(int toppingId)
    {
      ToppingId = toppingId;
    }

    public int ToppingId { get; }
  }

  class RemoveToppingAction
  {
    public RemoveToppingAction(int toppingId)
    {
      ToppingId = toppingId;
    }

    public int ToppingId { get; }
  }

  class SetCommentsAction
  {
    public SetCommentsAction(string comments)
    {
      Comments = comments;
    }

    public string Comments { get; }
  }

  class StartOrderAction { }
  class OrderSuccessAction {
    public OrderSuccessAction(Order order, ImmutableList<OrderDetail> orderDetails)
    {
      Order = order;
      OrderDetails = orderDetails;
    }

    public Order Order { get; }
    public ImmutableList<OrderDetail> OrderDetails { get; }
  }
  class OrderFailAction { }
}
