using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Models;
using ReduxXamDemo.State.Shape;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using ReduxXamDemo.Utils;

namespace ReduxXamDemo.State.Reducers
{
  class OrderReducer : IReducer<CurrentOrderState>
  {
    private static readonly CurrentOrderState initialValue = new CurrentOrderState(
      order: null,
      orderDetails: ImmutableList<OrderDetail>.Empty,
      currentOrderDetailIndex: null
  );

    public CurrentOrderState Reduce(CurrentOrderState state = null, object action = null)
    {
      if (state == null)
      {
        state = initialValue;
      }

      switch (action)
      {
        case SetPizzaAction a:
          {
            if (!state.CurrentOrderDetailIndex.HasValue) return state;

            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailIndex.Value];

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithPizzaId(a.PizzaId)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailIndex);
          }
        case SetSizeAction a:
          {
            if (!state.CurrentOrderDetailIndex.HasValue) return state;

            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailIndex.Value];

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithSizeId(a.SizeId)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailIndex);
          }
        case SetToppingsAction a:
          {
            if (!state.CurrentOrderDetailIndex.HasValue) return state;

            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailIndex.Value];

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithToppingIds(a.ToppingIds)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailIndex);
          }
        case AddToppingAction a:
          {
            if (!state.CurrentOrderDetailIndex.HasValue) return state;

            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailIndex.Value];

            var newToppings = prevOrderDetail.ToppingIds.Add(a.ToppingId);

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithToppingIds(newToppings)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailIndex);
          }
        case RemoveToppingAction a:
          {
            if (!state.CurrentOrderDetailIndex.HasValue) return state;

            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailIndex.Value];

            if (prevOrderDetail.ToppingIds.Contains(a.ToppingId))
            {
              var newToppings = prevOrderDetail.ToppingIds.Remove(a.ToppingId);

              var newOrderDetail = prevOrderDetail
                .ToBuilder()
                .WithToppingIds(newToppings)
                .ToImmutable();

              var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

              return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailIndex);
            }
            else
            {
              return state; //return same object when nothing has changed
            }
          }
        case CreateOrderAction a:
          {
            var order = new Order(0, null);
            return new CurrentOrderState(order, ImmutableList<OrderDetail>.Empty, null);
          }
        case CreateOrderDetailAction a:
          {
            var orderDetail = new OrderDetail.Builder().WithOrderId(state.Order.Id).ToImmutable();

            var newOrderDetails = state.OrderDetails.Add(orderDetail);
            var newIndex = newOrderDetails.IndexOf(orderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, newIndex);
          }
        case SetCurrentOrderDetailAction a:
          {
            return new CurrentOrderState(state.Order, state.OrderDetails, a.CurrentOrderDetailIndex);
          }
        case RemoveOrderDetailAction a:
          {
            var orderDetail = state.OrderDetails.ElementAtOrDefault(a.Index);
            if (orderDetail != null)
            {
              var newOrderDetails = state.OrderDetails.Remove(orderDetail);

              //update current index
              var currentOrderDetailIndex = state.CurrentOrderDetailIndex;
              if (state.CurrentOrderDetailIndex.HasValue)
              {
                if (state.CurrentOrderDetailIndex == a.Index)
                {
                  currentOrderDetailIndex = null;
                }
                else
                {
                  var currentOrderDetail = state.OrderDetails[state.CurrentOrderDetailIndex.Value];
                  currentOrderDetailIndex = newOrderDetails.IndexOf(currentOrderDetail);
                }
              }

              return new CurrentOrderState(state.Order, newOrderDetails, currentOrderDetailIndex);
            }

            return state; //return same object when nothing has changed
          }
        case SetCommentsAction a:
          {
            var newOrder = new Order(state.Order.Id, a.Comments);

            return new CurrentOrderState(newOrder, state.OrderDetails, state.CurrentOrderDetailIndex);
          }
        default:
          return state;
      }

    }
  }
}