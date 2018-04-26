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
      toasts: ImmutableList<Toast>.Empty,
      showSpinner: false
    );

    private static readonly Toast NO_INTERNET_TOAST = new Toast(1, "No Internet");
    private static readonly Toast ORDER_FAILED = new Toast(2, "Order failed, please try again");

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
              toasts: state.Toasts.MoveItemAtIndexToFront(NO_INTERNET_TOAST),
              showSpinner: state.ShowSpinner
            );
          }
          else
          {
            return new UIState(
              toasts: state.Toasts.Insert(0, NO_INTERNET_TOAST),
              showSpinner: state.ShowSpinner
            );
          }
        case GotInternetAction a:
          if (state.Toasts.Contains(NO_INTERNET_TOAST))
          {
            return new UIState(
              toasts: state.Toasts.Remove(NO_INTERNET_TOAST),
              showSpinner: state.ShowSpinner
            );
          }
          else
          {
            return state;
          }
        case StartOrderAction a:
          {
            return new UIState(
              toasts: state.Toasts,
              showSpinner: true
            );
          }
        case OrderFailAction a:
          {
            var newToasts = state.Toasts;
            if (state.Toasts.Contains(ORDER_FAILED))
            {
              newToasts = state.Toasts.MoveItemAtIndexToFront(ORDER_FAILED);
            }
            else
            {
              newToasts = state.Toasts.Insert(0, ORDER_FAILED);
            }

            return new UIState(
              toasts: newToasts,
              showSpinner: false
            );
          }
        case OrderSuccessAction a:
          {
            var newToasts = state.Toasts;
            if (state.Toasts.Contains(ORDER_FAILED))
            {
              newToasts = state.Toasts.Remove(ORDER_FAILED);
            }

            return new UIState(
              toasts: newToasts,
              showSpinner: false
            );
          }
        default:
          return state;
      }

    }
  }
}