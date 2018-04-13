using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using ReduxXamDemo.Utils;
using System.Collections.Immutable;
using System.Linq;

namespace ReduxXamDemo.State.Reducers
{
  class UIReducer : IReducer<UIState>
  {
    private static readonly UIState initialValue = new UIState(
      order: new OrderState(null, null),
      toasts: ImmutableList<Toast>.Empty
    );

    private static readonly Toast NO_INTERNET_TOAST = new Toast(1, "No Internet");

    public UIState Reduce(UIState state = null, object action = null)
    {
      if (state == null)
      {
        state = initialValue;
      }

      switch (action)
      {
        case LostInternetAction a:
          if (state.Toasts.Contains(NO_INTERNET_TOAST))
          {
            return new UIState(
              order: state.Order,
              toasts: state.Toasts.MoveItemAtIndexToFront(NO_INTERNET_TOAST)
            );
          }
          else
          {
            return new UIState(
              order: state.Order,
              toasts: state.Toasts.Insert(0, NO_INTERNET_TOAST)
            );
          }
        case GotInternetAction a:
          if (state.Toasts.Contains(NO_INTERNET_TOAST))
          {
            return new UIState(
              order: state.Order,
              toasts: state.Toasts.Remove(NO_INTERNET_TOAST)
            );
          }
          else
          {
            return state;
          }
        default:
          return state;
      }

    }
  }
}