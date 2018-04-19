using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Actions
{
  class CreateOrderAction { }

  class CreateOrderDetailAction { }

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


}
