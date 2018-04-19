using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Actions
{
  class SaveOrderAction
  {
    public SaveOrderAction(CurrentOrderState currentOrder)
    {
      CurrentOrder = currentOrder;
    }

    public CurrentOrderState CurrentOrder { get; }
  }
}
