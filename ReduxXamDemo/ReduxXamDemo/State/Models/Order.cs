using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ReduxXamDemo.State.Models
{
  public class Order
  {
    public Order(int id, string comments)
    {
      Id = id;
      Comments = comments;
    }

    public int Id { get; }
    public string Comments { get; }
    //public decimal TotalPrice => Items.Sum(od => od.DetailPrice);
  }
}
