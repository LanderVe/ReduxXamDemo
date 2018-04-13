using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  public class ApplicationState
  {
    public ApplicationState(DataState data, UIState ui, ViewState view, RouterState router)
    {
      Data = data;
      UI = ui;
      View = view;
      Router = router;
    }

    public DataState Data { get; }
    public UIState UI { get; }
    public ViewState View { get; }
    public RouterState Router { get; }
  }
}
