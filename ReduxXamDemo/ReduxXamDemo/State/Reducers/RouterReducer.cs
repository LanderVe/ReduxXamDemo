using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Reducers
{
  class RouterReducer : IReducer<RouterState>
  {
    private static readonly RouterState initialValue = new RouterState(
      stack: new List<RouterStackElement> { new RouterStackElement("MainView") }.ToImmutableList()
    );

    public RouterState Reduce(RouterState state = null, object action = null)
    {
      if (state == null)
      {
        state = initialValue;
      }

      switch (action)
      {
        case NavigateToAction a:
          {
            return new RouterState(state.Stack.Add(new RouterStackElement(a.ViewModelName)));
          }

        default:
          return state;
      }
    }

  }
}
