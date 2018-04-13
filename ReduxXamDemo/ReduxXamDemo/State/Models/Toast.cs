using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.State.Models
{
  public class Toast
  {
    public Toast(int id, string message)
    {
      Id = id;
      Message = message;
    }

    public int Id { get; }
    public string Message { get; }
  }
}
