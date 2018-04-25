using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  /// <summary>
  /// state associated with Order process, shared over all views
  /// </summary>
  public class CurrentOrderState
  {
    public CurrentOrderState(Order order, ImmutableList<OrderDetail> orderDetails, int? currentOrderDetailIndex)
    {
      Order = order;
      OrderDetails = orderDetails;
      CurrentOrderDetailIndex = currentOrderDetailIndex;
    }

    public Order Order { get; }
    public ImmutableList<OrderDetail> OrderDetails { get; }
    public int? CurrentOrderDetailIndex { get; }
  }
}
