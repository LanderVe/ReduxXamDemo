﻿using ReduxLib;
using ReduxXamDemo.State.Shape;
using ReduxXamDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    private readonly Store<ApplicationState> store;

    public NavigationService(Store<ApplicationState> store)
    {
      this.store = store;

      ConfigurePageMappings();

      store.Select(state => state.Router).Subscribe(OnNewRouterState);
    }

    private void ConfigurePageMappings()
    {
      string nspace = nameof(ReduxXamDemo.Views);

      var q = from t in Assembly.GetExecutingAssembly().GetTypes()
              where t.IsClass && t.Namespace == nspace
              select t;

      pageMappings = q.ToDictionary(pageType => pageType.BaseType.GenericTypeArguments[0].Name);
    }

    private void OnNewRouterState(RouterState routerState)
    {
      var current = navigationPage.Navigation.NavigationStack.Select(page => page.GetType()).ToList();
      var desired = routerState.Stack.Select(el => pageMappings[el.ViewModelName]).ToList();

      //if match do nothing
      if (current.SequenceEqual(desired)) return;


      var longestLastIndex = Math.Max(current.Count, desired.Count) - 1;

      var pagesToBeRemoved = new List<Page>();
      var pagesToBeAdded = new List<Type>();
      var pagesToBeReplaced = new Dictionary<int, Type>(); //by index

      FindDifference();
      ApplyDifference();

      void FindDifference()
      {
        for (int i = 0; i < longestLastIndex; i++)
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

      void ApplyDifference()
      {
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
        for (int i = 0; i < pagesToBeRemoved.Count - 1; i++)
        {
          var pageToRemove = pagesToBeRemoved[i];
          navigationPage.Navigation.RemovePage(pageToRemove);
        }
        //pop last
        navigationPage.Navigation.PopAsync();

        //add
        //navigate to last page
        var lastTypeToAdd = pagesToBeAdded[pagesToBeAdded.Count - 1];
        var lastPageToAdd = (Page)Activator.CreateInstance(lastTypeToAdd);
        navigationPage.Navigation.PushAsync(lastPageToAdd);

        //insert before
        for (int i = 0; i < pagesToBeAdded.Count - 1; i++)
        {
          var typeToAdd = pagesToBeAdded[i];
          var pageToAdd = (Page)Activator.CreateInstance(typeToAdd);
          navigationPage.Navigation.InsertPageBefore(pageToAdd, lastPageToAdd);
        }

      }


      //void NavigateLast()
      //{
      //  //check last
      //  var lastCurrentPageType = current.ElementAtOrDefault(longestLastIndex);
      //  var lastDesiredPageType = desired.ElementAtOrDefault(longestLastIndex);

      //  //if same return
      //  if (lastCurrentPageType == lastDesiredPageType) return;

      //  //navigate forward
      //  else if (lastCurrentPageType == null)
      //  {
      //    var lastDesiredPage = (Page)Activator.CreateInstance(lastDesiredPageType);
      //    navigationPage.Navigation.PushAsync(lastDesiredPage);
      //  }

      //  //navigate backwards
      //  else if (lastDesiredPageType == null)
      //  {
      //    navigationPage.Navigation.PopAsync();
      //  }

      //  //replace last page
      //  else
      //  {
      //    var lastCurrentPage = navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 1];
      //    var lastDesiredPage = (Page)Activator.CreateInstance(lastDesiredPageType);

      //    navigationPage.Navigation.PushAsync(lastDesiredPage); //push new
      //    navigationPage.Navigation.RemovePage(lastCurrentPage); //remove old
      //  }
      //}

    }



    public void SetNavigationPage(NavigationPage navigationPage)
    {
      this.navigationPage = navigationPage;

      //check when event occurs
      //navigationPage.Popped += (s,e) => store.Dispatch(TODO);
      //navigationPage.PoppedToRoot += (s,e) => store.Dispatch(TODO);
      //navigationPage.Pushed += (s,e) => store.Dispatch(TODO);
    }


  }



}