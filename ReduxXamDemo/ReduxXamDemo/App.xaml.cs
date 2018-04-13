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

namespace ReduxXamDemo
{
  public partial class App : Application
  {
    private NavigationService navigationService;
    private Store<ApplicationState> store;

    public IContainer Container { get; private set; }

    public App(IModule[] platformSpecificModules)
    {
      InitializeComponent();

      PrepareContainer(platformSpecificModules);

      var nav = new NavigationPage(new MainPage());
      MainPage = nav;

      //configure navigationService
      navigationService.SetNavigationPage(nav);

      //get store, setup toasts
      SetUpToasts(store);
    }

    private void SetUpToasts(Store<ApplicationState> store)
    {
      throw new NotImplementedException();
    }

    private void PrepareContainer(IModule[] platformSpecificModules)
    {
      var builder = new ContainerBuilder();

      RegisterPlatformSpecificModules(platformSpecificModules, builder);

      var assembly = Assembly.GetExecutingAssembly(typeof(App));

      //redux store
      var reducer = new ApplicationReducer();
      store = new Store<ApplicationState>(reducer);
      builder.RegisterInstance(store);

      //navigation
      navigationService = new NavigationService();
      builder.RegisterInstance(navigationService).As<INavigationService>();

      //services

      //register viewmodels
      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Namespace == nameof(ReduxXamDemo.ViewModels))
             .AsSelf()
             .InstancePerDependency();

      ////navigation
      //builder.RegisterInstance(BuildNavigationService()).As<INavigationService>();

      ////services
      //builder.RegisterType<RecipeSettings>().As<IRecipeSettings>().SingleInstance();

      ////viewmodels
      //builder.RegisterType<LoginViewModel>().InstancePerDependency();


      Container = builder.Build();
    }

    private static void RegisterPlatformSpecificModules(IModule[] platformSpecificModules, ContainerBuilder containerBuilder)
    {
      foreach (var platformSpecificModule in platformSpecificModules)
      {
        containerBuilder.RegisterModule(platformSpecificModule);
      }
    }

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
