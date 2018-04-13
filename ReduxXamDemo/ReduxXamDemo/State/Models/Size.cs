using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Models
{
  class Size
  {
    public Size(int id, string name, decimal priceMultiplier)
    {
      Id = id;
      Name = name;
      PriceMultiplier = priceMultiplier;
    }

    public int Id { get; }
    public string Name { get; }
    public decimal PriceMultiplier { get; }
  }
}
