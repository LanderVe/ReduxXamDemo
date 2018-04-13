using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Models
{
  class Pizza
  {
    public Pizza(int id, string type, decimal basePrice)
    {
      Id = id;
      Type = type;
      BasePrice = basePrice;
    }

    public int Id { get; }
    public string Type { get; }
    public decimal BasePrice { get; }
  }
}
