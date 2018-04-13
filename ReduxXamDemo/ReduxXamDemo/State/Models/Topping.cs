using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Models
{
  public class Topping
  {
    public Topping(int id, string name, decimal price)
    {
      Id = id;
      Name = name;
      Price = price;
    }

    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }
  }
}
