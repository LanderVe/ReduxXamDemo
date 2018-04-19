using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  public class ApplicationState
  {

    public ApplicationState(DataState data, CurrentOrderState currentOrder, UIState ui, ViewState view, RouterState router)
    {
      Data = data;
      CurrentOrder = currentOrder;
      UI = ui;
      View = view;
      Router = router;
    }

    public DataState Data { get; }
    public CurrentOrderState CurrentOrder { get; }
    public UIState UI { get; }
    public ViewState View { get; }
    public RouterState Router { get; }
  }
}
