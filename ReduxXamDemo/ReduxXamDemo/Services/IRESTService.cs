using System.Collections.Immutable;
using System.Threading.Tasks;
using ReduxXamDemo.State.Models;

namespace ReduxXamDemo.Services
{
  public interface IRESTService
  {
    Task<(Order, ImmutableList<OrderDetail>)> OrderAsync(Order order, ImmutableList<OrderDetail> orderDetails);
  }
}