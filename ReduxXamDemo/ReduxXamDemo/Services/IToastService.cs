using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxXamDemo.Services
{
  public interface IToastService
  {
    void Notify(string message, int duration = 3000);
    void NotifyPermanent(string message);
    void DismissPermanentNotify();
  }
}
