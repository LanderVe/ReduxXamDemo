using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Reducers
{
  class ViewReducer : IReducer<ViewState>
  {
    private static readonly ViewState initialValue = new ViewState(
        selectPizza: new SelectPizzaState(
          searchTerm: null
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
        case SearchPizzasAction a:
          {
            if (state.SelectPizza.SearchTerm != a.SearchTerm)
              return new ViewState(new SelectPizzaState(a.SearchTerm));
            else
              return state;
          }

        default:
          return state;
      }

    }
  }
}
