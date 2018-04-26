using ReduxXamDemo.State.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReduxXamDemo.Services
{
  public class RESTService : IRESTService
  {
    public async Task<(Order, ImmutableList<OrderDetail>)> OrderAsync(Order order, ImmutableList<OrderDetail> orderDetails)
    {
      await Task.Delay(TimeSpan.FromSeconds(1));

      //emulate server that sets the IDs
      var rnd = new Random();
      var orderId = rnd.Next();
      var resultOrder = new Order(orderId, order.Comments);


      var resultOrderDetails = orderDetails
        .Select(od => od.ToBuilder().WithId(rnd.Next()).WithOrderId(orderId).ToImmutable())
        .ToImmutableList();

      return (resultOrder, resultOrderDetails);
    }
  }
}
