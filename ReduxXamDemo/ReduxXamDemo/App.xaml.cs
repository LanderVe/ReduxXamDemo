using ReduxXamDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

using Xamarin.Forms;
using Autofac.Core;
using System.Reflection;
using ReduxXamDemo.State.Shape;
using ReduxLib;
using ReduxXamDemo.State.Reducers;
using ReduxXamDemo.Services;
using Plugin.Connectivity;
using System.Reactive.Linq;
using Plugin.Connectivity.Abstractions;
using ReduxXamDemo.State.Actions;

namespace ReduxXamDemo
{
  public partial class App : Application
  {
    private NavigationService navigationService;
    private Store<ApplicationState> store;

    public static IContainer Container { get; private set; }

    public App(IModule[] platformSpecificModules)
    {
      InitializeComponent();

      PrepareContainer(platformSpecificModules);

      var nav = new NavigationPage(new MainView());
      MainPage = nav;

      //configure navigationService
      navigationService.SetNavigationPage(nav);

      //get store, setup toasts
      SetUpToasts(store);
      CheckConnectivity(store);
    }

    private void SetUpToasts(Store<ApplicationState> store)
    {
      var toastService = Container.Resolve<IToastService>();

      store.Grab(state => state.UI.Toasts).Subscribe(toasts =>
      {

        toastService.DismissPermanentNotify();

        var last = toasts.LastOrDefault();
        if (last != null)
        {
          toastService.NotifyPermanent(last.Message);
        }
      });
    }

    private void CheckConnectivity(Store<ApplicationState> store)
    {
      if (!CrossConnectivity.Current.IsConnected)
      {
        store.Dispatch(new LostInternetAction());
      }

      CrossConnectivity.Current.ConnectivityChanged += (s, e) =>
      {
        if (CrossConnectivity.Current.IsConnected)
          store.Dispatch(new GotInternetAction());
        else
          store.Dispatch(new LostInternetAction());
      };
    }

    #region Dependency Injection

    private void PrepareContainer(IModule[] platformSpecificModules)
    {
      var builder = new ContainerBuilder();

      RegisterPlatformSpecificModules(platformSpecificModules, builder);

      var assembly = Assembly.GetExecutingAssembly();

      //redux store
      var reducer = new ApplicationReducer();
      store = new Store<ApplicationState>(reducer);
      builder.RegisterInstance(store);

      //register viewmodels
      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Namespace == "ReduxXamDemo.ViewModels")
             .AsSelf()
             .InstancePerDependency();

      //navigation
      navigationService = new NavigationService(store);
      builder.RegisterInstance(navigationService).As<INavigationService>();

      Container = builder.Build();
    }

    private static void RegisterPlatformSpecificModules(IModule[] platformSpecificModules, ContainerBuilder containerBuilder)
    {
      foreach (var platformSpecificModule in platformSpecificModules)
      {
        containerBuilder.RegisterModule(platformSpecificModule);
      }
    }

    #endregion

    #region Life Cycle
    protected override void OnStart()
    {
      // Handle when your app starts
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
    #endregion
  }
}
