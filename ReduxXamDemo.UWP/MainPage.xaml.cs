using Autofac;
using Autofac.Core;
using ReduxXamDemo.Services;
using ReduxXamDemo.UWP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReduxXamDemo.UWP
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage
  {
    public MainPage()
    {
      this.InitializeComponent();
      LoadApplication(new ReduxXamDemo.App(new IModule[] { new WindowsDIModule() }));
    }
  }

  class WindowsDIModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      base.Load(builder);
      builder.RegisterInstance(new ToastService()).As<IToastService>();
    }
  }
}
