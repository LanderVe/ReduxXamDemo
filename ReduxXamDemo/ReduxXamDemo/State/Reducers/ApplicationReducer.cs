using ReduxLib;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Reducers
{
  class ApplicationReducer : IReducer<ApplicationState>
  {
    private DataReducer dataReducer = new DataReducer();
    private UIReducer uiReducer = new UIReducer();
    private ViewReducer viewReducer = new ViewReducer();
    private RouterReducer routerReducer = new RouterReducer();

    public ApplicationState Reduce(ApplicationState state = null, object action = null)
    {
      var newState = new ApplicationState(
        data: dataReducer.Reduce(state?.Data, action),
        ui: uiReducer.Reduce(state?.UI, action),
        view: viewReducer.Reduce(state?.View, action),
        router: routerReducer.Reduce(state?.Router, action)
      );

      var hasChanged = state == null
                       || state.Data != newState.Data
                       || state.UI != newState.UI
                       || state.View != newState.View
                       || state.Router != newState.Router;

      return hasChanged ? newState : state;
    }

  }
}
