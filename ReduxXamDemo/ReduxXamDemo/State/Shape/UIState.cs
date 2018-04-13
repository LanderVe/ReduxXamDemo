using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  class UIState
  {
    public UIState(OrderState order, ImmutableList<Toast> toasts)
    {
      Order = order;
      Toasts = toasts;
    }

    public OrderState Order { get; }
    public ImmutableList<Toast> Toasts { get; }
  }

  /// <summary>
  /// state associated with Order process
  /// </summary>
  class OrderState
  {
    public OrderState(int? currentOrderId, int? currentOrderDetailId)
    {
      CurrentOrderId = currentOrderId;
      CurrentOrderDetailId = currentOrderDetailId;
    }

    public int? CurrentOrderId { get; }
    public int? CurrentOrderDetailId { get; }
  }
}
