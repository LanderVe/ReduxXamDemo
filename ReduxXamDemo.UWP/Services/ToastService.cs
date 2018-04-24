using ReduxXamDemo.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReduxXamDemo.UWP.Services
{
  class ToastService : IToastService
  {
    public void DismissPermanentNotify()
    {
      Debug.WriteLine($"Dismiss Permanent");

    }

    public void Notify(string message, int duration = 3000)
    {
      Debug.WriteLine($"Notify {message}");
    }

    public void NotifyPermanent(string message)
    {
      Debug.WriteLine($"Notify Permanent {message}");
    }
  }
}
