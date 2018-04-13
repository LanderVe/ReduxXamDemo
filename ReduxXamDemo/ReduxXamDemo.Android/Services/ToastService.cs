using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using ReduxXamDemo.Droid.Services;
using ReduxXamDemo.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]

namespace ReduxXamDemo.Droid.Services
{
  public class ToastService : IToastService
  {
    private Snackbar currentSnackBar;
    private readonly Activity context;
    private readonly Android.Views.View view;

    public ToastService(Activity context)
    {
      this.context = context;
      view = context.FindViewById(global::Android.Resource.Id.Content);
    }

    //the real work
    private Snackbar MakeAndShowSnackbar(string message, int duration)
    {
      var snack = Snackbar.Make(view, message, duration);

      snack.Show();
      return snack;
    }

    public void Notify(string message, int duration)
      => MakeAndShowSnackbar(message, duration);

    public void NotifyPermanent(string message)
    {
      //dismiss previous
      currentSnackBar?.Dismiss();

      //create new
      var snack = MakeAndShowSnackbar(message, int.MaxValue);
      currentSnackBar = snack;
    }

    public void DismissPermanentNotify()
    {
      currentSnackBar?.Dismiss();
      currentSnackBar = null;
    }

  }
}