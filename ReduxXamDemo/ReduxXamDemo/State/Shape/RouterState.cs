using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.State.Shape
{
  public class RouterState
  {
    public RouterState(ImmutableList<RouterStackElement> stack)
    {
      Stack = stack;
    }

    public ImmutableList<RouterStackElement> Stack { get; }
  }

  public class RouterStackElement
  {
    public RouterStackElement(string viewModelName)
    {
      ViewModelName = viewModelName;
    }

    public string ViewModelName { get; }
  }

  public enum HistoryBehavior
  {
    Default,
    ClearHistory
  }
}
