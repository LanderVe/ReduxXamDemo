using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  public class UIState
  {
    public UIState(ImmutableList<Toast> toasts, bool showSpinner)
    {
      Toasts = toasts;
      ShowSpinner = showSpinner;
    }

    public ImmutableList<Toast> Toasts { get; }
    public bool ShowSpinner { get; }
  }

  ///// <summary>
  ///// state associated with Order process
  ///// </summary>
  //public class OrderState
  //{
  //  public OrderState(int? currentOrderId, int? currentOrderDetailId)
  //  {
  //    CurrentOrderId = currentOrderId;
  //    CurrentOrderDetailId = currentOrderDetailId;
  //  }

  //  public int? CurrentOrderId { get; }
  //  public int? CurrentOrderDetailId { get; }
  //}
}
