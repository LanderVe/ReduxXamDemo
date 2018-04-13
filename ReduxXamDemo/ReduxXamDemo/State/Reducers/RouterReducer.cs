using ReduxLib;
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
      stack: ImmutableList<RouterStackElement>.Empty
    );

    public RouterState Reduce(RouterState state = null, object action = null)
    {
      if (state == null)
      {
        state = initialValue;
      }

      switch (action)
      {
        default:
          return state;
      }
    }

  }
}
