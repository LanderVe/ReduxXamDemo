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
      currentOrderDetailId: 0
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
            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailId];

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithPizzaId(a.PizzaId)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailId);
          }
        case SetSizeAction a:
          {
            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailId];

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithSizeId(a.SizeId)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailId);
          }
        case AddToppingAction a:
          {
            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailId];

            var newToppings = prevOrderDetail.ToppingIds.Add(a.ToppingId);

            var newOrderDetail = prevOrderDetail
              .ToBuilder()
              .WithToppingIds(newToppings)
              .ToImmutable();

            var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailId);
          }
        case RemoveToppingAction a:
          {
            var prevOrderDetail = state.OrderDetails[state.CurrentOrderDetailId];

            if (prevOrderDetail.ToppingIds.Contains(a.ToppingId))
            {
              var newToppings = prevOrderDetail.ToppingIds.Remove(a.ToppingId);

              var newOrderDetail = prevOrderDetail
                .ToBuilder()
                .WithToppingIds(newToppings)
                .ToImmutable();

              var newOrderDetails = state.OrderDetails.Replace(prevOrderDetail, newOrderDetail);

              return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailId);
            }
            else
            {
              return state; //return same object when nothing has changed
            }
          }
        case CreateOrderAction a:
          {
            var order = new Order(0, null);
            return new CurrentOrderState(order, ImmutableList<OrderDetail>.Empty, 0);
          }
        case CreateOrderDetailAction a:
          {
            var orderDetail = new OrderDetail.Builder().WithOrderId(state.Order.Id).ToImmutable();

            var newOrderDetails = state.OrderDetails.Add(orderDetail);

            return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailId);
          }
        case RemoveOrderDetailAction a:
          {
            var orderDetail = state.OrderDetails.ElementAtOrDefault(a.Index);
            if (orderDetail != null)
            {
              var newOrderDetails = state.OrderDetails.Remove(orderDetail);

              return new CurrentOrderState(state.Order, newOrderDetails, state.CurrentOrderDetailId);
            }

            return state; //return same object when nothing has changed
          }
        case SetCommentsAction a:
          {
            var newOrder = new Order(state.Order.Id, a.Comments);

            return new CurrentOrderState(newOrder, state.OrderDetails, state.CurrentOrderDetailId);
          }
        default:
          return state;
      }
    }
  }
}