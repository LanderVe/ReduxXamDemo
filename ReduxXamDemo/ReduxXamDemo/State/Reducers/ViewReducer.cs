using ReduxLib;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Reducers
{
  class ViewReducer : IReducer<ViewState>
  {
    private static readonly ViewState initialValue = new ViewState (
        selectPizza: new SelectPizzaState(
          searchTerm: null,
          pizzaIds: ImmutableList<int>.Empty
        )
      );

    public ViewState Reduce(ViewState state = null, object action = null)
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
