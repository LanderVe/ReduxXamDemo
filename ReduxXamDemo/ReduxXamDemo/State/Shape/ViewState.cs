using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  public class ViewState
  {
    public ViewState(SelectPizzaState selectPizza)
    {
      SelectPizza = selectPizza;
    }

    public SelectPizzaState SelectPizza { get; }
  }



  /// <summary>
  /// State associated with Select Pizza View
  /// </summary>
  public class SelectPizzaState
  {
    public SelectPizzaState(string searchTerm, ImmutableList<int> pizzaIds)
    {
      SearchTerm = searchTerm;
      PizzaIds = pizzaIds;
    }

    public string SearchTerm { get; }
    public ImmutableList<int> PizzaIds { get; }
  }

  
}
