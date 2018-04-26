using ReduxLib;
using ReduxXamDemo.State.Actions;
using ReduxXamDemo.State.Shape;
using ReduxXamDemo.ViewModels;
using ReduxXamDemo.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReduxXamDemo.Services
{
  class NavigationService : INavigationService
  {
    private NavigationPage navigationPage;
    private Dictionary<string, Type> pageMappings;
    private bool navigating = false;
    private readonly Store<ApplicationState> store;

    public NavigationService(Store<ApplicationState> store)
    {
      this.store = store;

      ConfigurePageMappings();

      store.Grab(state => state?.Router).Subscribe(async routerState => await OnNewRouterStateAsync(routerState));
    }

    private void ConfigurePageMappings()
    {
      var q = from t in Assembly.GetExecutingAssembly().GetTypes()
              where t.IsClass && t.Namespace == "ReduxXamDemo.Views" && t.BaseType.IsConstructedGenericType
              select t;

      pageMappings = q.ToDictionary(pageType => pageType.BaseType.GenericTypeArguments[0].Name);
    }

    private async Task OnNewRouterStateAsync(RouterState routerState)
    {
      if (navigationPage == null) return;

      var current = navigationPage.Navigation.NavigationStack.Select(page => page.GetType()).ToList();
      var desired = routerState.Stack.Select(el => pageMappings[el.ViewModelName]).ToList();

      //if match do nothing
      if (current.SequenceEqual(desired)) return;


      var biggestCount = Math.Max(current.Count, desired.Count);

      var pagesToBeRemoved = new List<Page>();
      var pagesToBeAdded = new List<Type>();
      var pagesToBeReplaced = new Dictionary<int, Type>(); //by index

      FindDifference();
      await ApplyDifferenceAsync();

      void FindDifference()
      {
        for (int i = 0; i < biggestCount; i++)
        {
          var currentPageType = current.ElementAtOrDefault(i);
          var desiredPageType = desired.ElementAtOrDefault(i);

          //if same, do nothing
          if (currentPageType == desiredPageType) continue;

          //add page
          else if (currentPageType == null)
          {
            pagesToBeAdded.Add(desiredPageType);
          }

          //remove page
          else if (desiredPageType == null)
          {
            var pageToRemove = navigationPage.Navigation.NavigationStack[i];
            pagesToBeRemoved.Add(pageToRemove);
          }

          //replace page
          else
          {
            pagesToBeReplaced.Add(i, desiredPageType);
          }
        }

      }

      async Task ApplyDifferenceAsync()
      {
        navigating = true;

        //replace
        var lastDesiredIndex = desired.Count - 1;
        foreach (var indexToReplace in pagesToBeReplaced.Keys)
        {
          var pageToRemove = navigationPage.Navigation.NavigationStack[indexToReplace];
          var typeToAdd = pagesToBeReplaced[indexToReplace];
          var pageToAdd = (Page)Activator.CreateInstance(typeToAdd);

          if (indexToReplace == lastDesiredIndex)
          {
            //if matches last desired page => use proper navigation
            navigationPage.Navigation.PushAsync(pageToAdd); //push new
          }
          else
          {
            navigationPage.Navigation.InsertPageBefore(pageToAdd, pageToRemove);
          }

          navigationPage.Navigation.RemovePage(pageToRemove);
        }

        //remove
        //remove middle
        if (pagesToBeRemoved.Count > 0)
        {
          for (int i = 0; i < pagesToBeRemoved.Count - 1; i++)
          {
            var pageToRemove = pagesToBeRemoved[i];
            navigationPage.Navigation.RemovePage(pageToRemove);
          }
          //pop last
          await navigationPage.Navigation.PopAsync();
        }

        //add
        //navigate to last page
        if (pagesToBeAdded.Count > 0)
        {
          var lastTypeToAdd = pagesToBeAdded[pagesToBeAdded.Count - 1];
          var lastPageToAdd = (Page)Activator.CreateInstance(lastTypeToAdd);
          await navigationPage.Navigation.PushAsync(lastPageToAdd);

          //insert before
          for (int i = 0; i < pagesToBeAdded.Count - 1; i++)
          {
            var typeToAdd = pagesToBeAdded[i];
            var pageToAdd = (Page)Activator.CreateInstance(typeToAdd);
            navigationPage.Navigation.InsertPageBefore(pageToAdd, lastPageToAdd);
          }
        }

        navigating = false;
      }

    }



    public void SetNavigationPage(NavigationPage navigationPage)
    {
      this.navigationPage = navigationPage;

      //check when event occurs
      navigationPage.Popped += (s, e) => { if (!navigating) store.Dispatch(new PopAction()); };
      navigationPage.PoppedToRoot += (s, e) => { if (!navigating) store.Dispatch(new PopToRootAction()); };
      //navigationPage.Pushed += (s, e) => store.Dispatch(TODO);
    }


  }



}
