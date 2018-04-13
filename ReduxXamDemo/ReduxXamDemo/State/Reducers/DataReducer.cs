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
  class DataReducer : IReducer<DataState>
  {
    private static readonly DataState initialValue = new DataState(
      pizzas: new Dictionary<int, Pizza>
      {
        [1] = new Pizza(1, "Margarita", 10),
        [2] = new Pizza(2, "Funghi", 11),
        [3] = new Pizza(3, "Quatro Stagioni", 10),
        [4] = new Pizza(4, "Quatro Formaggio", 10)
      }.ToImmutableSortedDictionary()
      ,
      sizes: new Dictionary<int, Size>
      {
        [1] = new Size(1, "Small", 1M),
        [2] = new Size(2, "Medium", 1.5M),
        [3] = new Size(3, "Large", 2M)
      }.ToImmutableSortedDictionary()
      ,
      toppings: new Dictionary<int, Topping>
      {
        [1] = new Topping(1, "Extra cheese", .5M),
        [2] = new Topping(2, "Extra onion", .5M),
        [3] = new Topping(3, "Extra pepperoni", .5M),
        [4] = new Topping(4, "Extra muchrooms", .5M)
      }.ToImmutableSortedDictionary()
      ,
      orders: ImmutableSortedDictionary.Create<int, Order>(),
      orderDetails: ImmutableSortedDictionary.Create<int, OrderDetail>()
  );

    public DataState Reduce(DataState state = null, object action = null)
    {
      if (state == null)
      {
        state = initialValue;
      }

      switch (action)
      {
        case SetPizzaAction a:
          #region code without builder pattern
          //var prevOrderDetail = state.OrderDetails[a.OrderDetailId];

          //var newOrderDetail = new OrderDetail(
          //    a.OrderDetailId,
          //    prevOrderDetail.OrderId,
          //    a.PizzaId,
          //    prevOrderDetail.SizeId,
          //    prevOrderDetail.ToppingIds
          //  );

          //var builder = state.OrderDetails.ToBuilder();
          //builder.Remove(a.OrderDetailId);
          //builder.Add(a.OrderDetailId, newOrderDetail);

          //return new DataState(
          //  pizzas: state.Pizzas,
          //  sizes: state.Sizes,
          //  toppings: state.Toppings,
          //  orders: state.Orders,
          //  orderDetails: builder.ToImmutable()
          //);
          #endregion
          {
            var prevOrderDetail = state.OrderDetails[a.OrderDetailId];

            var newOrderDetail = prevOrderDetail.ToBuilder().WithPizzaId(a.PizzaId).ToImmutable();

            var orderDetails = state.OrderDetails.Update(a.OrderDetailId, newOrderDetail);

            return state.ToBuilder().WithOrderDetails(orderDetails).ToImmutable();
          }
        case SetSizeAction a:
          {
            var prevOrderDetail = state.OrderDetails[a.OrderDetailId];

            var newOrderDetail = prevOrderDetail.ToBuilder().WithSizeId(a.SizeId).ToImmutable();

            var orderDetails = state.OrderDetails.Update(a.OrderDetailId, newOrderDetail);

            return state.ToBuilder().WithOrderDetails(orderDetails).ToImmutable();
          }
        case AddToppingAction a:
          {
            var prevOrderDetail = state.OrderDetails[a.OrderDetailId];

            var newToppings = prevOrderDetail.ToppingIds.Add(a.ToppingId);

            var newOrderDetail = prevOrderDetail.ToBuilder().WithToppingIds(newToppings).ToImmutable();

            var orderDetails = state.OrderDetails.Update(a.OrderDetailId, newOrderDetail);

            return state.ToBuilder().WithOrderDetails(orderDetails).ToImmutable();
          }
        case RemoveToppingAction a:
          {
            var prevOrderDetail = state.OrderDetails[a.OrderDetailId];

            if (prevOrderDetail.ToppingIds.Contains(a.ToppingId))
            {
              var newToppings = prevOrderDetail.ToppingIds.Remove(a.ToppingId);

              var newOrderDetail = prevOrderDetail.ToBuilder().WithToppingIds(newToppings).ToImmutable();

              var orderDetails = state.OrderDetails.Update(a.OrderDetailId, newOrderDetail);

              return state.ToBuilder().WithOrderDetails(orderDetails).ToImmutable();
            }
            else
            {
              return state; //return same object when nothing has changed
            }
          }
        case CreateOrderDetailAction a:
          {
            var orderDetail = new OrderDetail.Builder().WithOrderId(a.OrderId).ToImmutable();
            var orderDetailId = state.OrderDetails.Keys.Max() + 1;

            var orderDetails = state.OrderDetails.Add(orderDetailId, orderDetail);

            return state.ToBuilder().WithOrderDetails(orderDetails).ToImmutable();
          }
        case RemoveOrderDetailAction a:
          {
            if (state.OrderDetails.ContainsKey(a.OrderDetailId))
            {
              var orderDetails = state.OrderDetails.Remove(a.OrderDetailId);

              return state.ToBuilder().WithOrderDetails(orderDetails).ToImmutable();
            }

            return state; //return same object when nothing has changed
          }
        case SetCommentsAction a:
          {
            var oldOrder = state.Orders[a.OrderId];
            var newOrder = new Order(a.OrderId, a.Comments);
            var orders = state.Orders.Update(a.OrderId, newOrder);

            return state.ToBuilder().WithOrders(orders).ToImmutable();
          }
        default:
          return state;
      }
    }
  }
}