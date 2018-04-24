using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Actions
{
  class NavigateToAction
  {
    public NavigateToAction(string viewModelName)
    {
      ViewModelName = viewModelName;
    }

    public string ViewModelName { get; }
  }

  class NavigateBackToAction
  {
    public NavigateBackToAction(string viewModelName)
    {
      ViewModelName = viewModelName;
    }

    public string ViewModelName { get; }
  }

  class PopAction { }
  class PopToRootAction { }



}
