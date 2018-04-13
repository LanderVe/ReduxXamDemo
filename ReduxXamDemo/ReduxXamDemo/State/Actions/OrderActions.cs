using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Actions
{
  class CreateOrderAction { }

  class CreateOrderDetailAction
  {
    public CreateOrderDetailAction(int orderId)
    {
      OrderId = orderId;
    }

    public int OrderId { get; }
  }

  class RemoveOrderDetailAction
  {
    public RemoveOrderDetailAction(int orderDetailId)
    {
      OrderDetailId = orderDetailId;
    }

    public int OrderDetailId { get; }
  }

  class SetPizzaAction
  {
    public SetPizzaAction(int orderDetailId, int pizzaId)
    {
      OrderDetailId = orderDetailId;
      PizzaId = pizzaId;
    }

    public int OrderDetailId { get; }
    public int PizzaId { get; }
  }

  class SetSizeAction
  {
    public SetSizeAction(int orderDetailId, int sizeId)
    {
      OrderDetailId = orderDetailId;
      SizeId = sizeId;
    }

    public int OrderDetailId { get; }
    public int SizeId { get; }
  }

  class AddToppingAction
  {
    public AddToppingAction(int orderDetailId, int toppingId)
    {
      OrderDetailId = orderDetailId;
      ToppingId = toppingId;
    }

    public int OrderDetailId { get; }
    public int ToppingId { get; }
  }

  class RemoveToppingAction
  {
    public RemoveToppingAction(int orderDetailId, int toppingId)
    {
      OrderDetailId = orderDetailId;
      ToppingId = toppingId;
    }

    public int OrderDetailId { get; }
    public int ToppingId { get; }
  }

  class SetCommentsAction
  {
    public SetCommentsAction(int orderId, string comments)
    {
      OrderId = orderId;
      Comments = comments;
    }

    public int OrderId { get; }
    public string Comments { get; }
  }


}
