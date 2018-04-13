﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObservableBinding
{
  [ContentProperty("Path")]
  public class BindExtension : IMarkupExtension
  {
    private IDisposable listenSubscription;
    private IDisposable emitSubscription;
    private BindableObject bindingTarget;
    private BindableProperty bindingProperty;

    public string Path { get; set; }
    public BindingMode Mode { get; set; }

    public BindExtension() { }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
      var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

      if (valueProvider != null)
      {
        bindingTarget = valueProvider.TargetObject as BindableObject;
        bindingProperty = valueProvider.TargetProperty as BindableProperty;

        bindingTarget.BindingContextChanged += BindingContextSource_BindingContextChanged;
        var bindingContext = bindingTarget?.BindingContext;

        SetupBinding(bindingContext);
      }

      return bindingProperty.DefaultValue;
    }

    private void BindingContextSource_BindingContextChanged(object sender, EventArgs e)
    {
      RemoveBinding();

      var newBindingContext = (bindingTarget?.BindingContext);
      if (newBindingContext != null)
      {
        SetupBinding(newBindingContext);
      }
    }

    private void SetupBinding(object source)
    {
      //sanity check
      if (source == null) return;

      var boundObject = GetObjectFromPath(source, Path);
      if (boundObject == null) return;

      //set default BindingMode 
      if (Mode == BindingMode.Default) Mode = bindingProperty.DefaultBindingMode;

      //bind to Observable and update property
      if (Mode == BindingMode.OneWay || Mode == BindingMode.TwoWay)
      {
        SetupListenerBinding(boundObject);
      }

      //send property values to Observer
      if (Mode == BindingMode.OneWayToSource || Mode == BindingMode.TwoWay)
      {
        SetupEmitBinding(boundObject);
      }

    }

    private void RemoveBinding()
    {
      //stop listening to observable
      listenSubscription?.Dispose();
      emitSubscription?.Dispose();
    }

    #region Listen
    private void SetupListenerBinding(object observable)
    {
      //IObservable<T> --> typeof(T)
      var observableGenericType = observable.GetType().GetTypeInfo()
        .ImplementedInterfaces
        .Single(type => type.IsConstructedGenericType && type.GetGenericTypeDefinition() == (typeof(IObservable<>)))
        .GenericTypeArguments[0];

      //add subscription
      MethodInfo method = typeof(BindExtension).GetTypeInfo().DeclaredMethods
        .Where(mi => mi.Name == nameof(SubscribePropertyForObservable))
        .Single();

      MethodInfo generic = method.MakeGenericMethod(observableGenericType);
      generic.Invoke(this, new object[] { observable, bindingTarget, bindingProperty });
    }

    private void SubscribePropertyForObservable<TProperty>(IObservable<TProperty> observable, BindableObject d, BindableProperty property)
    {
      if (observable == null) return;

      //automatic ToString
      if (property.ReturnType == typeof(string) && typeof(TProperty) != typeof(string))
      {
        listenSubscription = observable.Select(val => val.ToString()).ObserveOn(SynchronizationContext.Current).Subscribe(val => d.SetValue(property, val));
      }
      //any other case
      else
      {
        listenSubscription = observable.ObserveOn(SynchronizationContext.Current).Subscribe(val => d.SetValue(property, val));
      }

    }
    #endregion

    #region Emit
    private void SetupEmitBinding(object observer)
    {
      //add subscription
      MethodInfo method = typeof(BindExtension).GetTypeInfo().DeclaredMethods
        .Where(mi => mi.Name == nameof(SubScribeObserverForProperty))
        .Single();

      MethodInfo generic = method.MakeGenericMethod(bindingProperty.ReturnType);
      generic.Invoke(this, new object[] { observer, bindingTarget, bindingProperty });
    }

    private void SubScribeObserverForProperty<TProperty>(IObserver<TProperty> observer, BindableObject d, BindableProperty propertyToMonitor)
    {
      if (propertyToMonitor.DeclaringType.GetTypeInfo().IsAssignableFrom(d.GetType().GetTypeInfo()) && observer != null)
      {
        emitSubscription = d.Observe<TProperty>(propertyToMonitor)
                  .Subscribe(observer);
      }
    }
    #endregion

    #region Helper
    /// <summary>
    /// If the Binding looks something like this
    /// {o:Bind Parent.Child.SubChild}, we need to retrieve SubChild
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="path"></param>
    /// <returns>The value returned by the path, null if any properties in the chain is null</returns>
    private object GetObjectFromPath(object bindingContext, string path)
    {
      var properties = path.Split('.');
      var current = bindingContext;

      foreach (var prop in properties)
      {
        if (current == null) break;
        current = current.GetType().GetRuntimeProperty(prop).GetValue(current);
      }

      return current;
    }

    #endregion

  }
}
