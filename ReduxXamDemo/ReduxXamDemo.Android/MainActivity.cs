using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using ReduxXamDemo.Services;
using ReduxXamDemo.Droid.Services;
using Autofac;
using Autofac.Core;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ReduxXamDemo.Droid
{
  [Activity(Label = "ReduxXamDemo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      base.OnCreate(bundle);

      global::Xamarin.Forms.Forms.Init(this, bundle);
#if DEBUG
      TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
      AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentOnUnhandledException;
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif

      LoadApplication(new App(new IModule[] { new AndroidDIModule(this) }));
    }

#if DEBUG
    private void AndroidEnvironmentOnUnhandledException(object sender, RaiseThrowableEventArgs e)
    {
      System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {e.Exception.Message}");
      if (Debugger.IsAttached)
        Debugger.Break();
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
      System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {e.Exception.Message}");
      if (Debugger.IsAttached)
        Debugger.Break();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {e.ExceptionObject}");
      if (Debugger.IsAttached)
        Debugger.Break();
    }
#endif
  }

  class AndroidDIModule : Module
  {
    private readonly Activity context;

    public AndroidDIModule(Activity context)
    {
      this.context = context;
    }

    protected override void Load(ContainerBuilder builder)
    {
      base.Load(builder);
      builder.RegisterInstance(new ToastService(context)).As<IToastService>();
    }
  }
}

