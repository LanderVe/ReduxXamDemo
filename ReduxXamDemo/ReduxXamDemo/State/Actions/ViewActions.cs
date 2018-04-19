using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Actions
{
  class SearchPizzasAction
  {
    public SearchPizzasAction(string searchTerm)
    {
      SearchTerm = searchTerm;
    }

    public string SearchTerm { get; }
  }
}
