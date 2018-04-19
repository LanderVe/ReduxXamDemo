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
        case SaveOrderAction a: //sync save, maybe replace this with a webservice call
          {
            // store order
            var orderId = state.Orders.Keys.Max() + 1;
            var newOrder = new Order(orderId, a.CurrentOrder.Order.Comments);
            var newOrders = state.Orders.Add(orderId, newOrder);

            // store orderdetails
            var orderDetailId = state.OrderDetails.Keys.Max();

            var addedOrderDetails = a.CurrentOrder.OrderDetails
              .Select(orderDetail => orderDetail.ToBuilder().WithId(++orderDetailId).WithOrderId(orderId).ToImmutable())
              .ToDictionary(od => od.Id);

            var newOrderDetails = state.OrderDetails.AddRange(addedOrderDetails); //more efficient than adding one by one

            return state.ToBuilder().WithOrders(newOrders).WithOrderDetails(newOrderDetails).ToImmutable();
          }
        default:
          return state;
      }
    }
  }
}