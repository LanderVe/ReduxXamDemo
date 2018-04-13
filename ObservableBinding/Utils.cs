using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Forms;

namespace ObservableBinding
{
  public static class Utils
  {
    public static IObservable<TProperty> Observe<TProperty>(this BindableObject bindableObject, BindableProperty bindableProperty)
    {
      var propertyName = bindableProperty.PropertyName;

      return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                      handler => handler.Invoke,
                      h => bindableObject.PropertyChanged += h,
                      h => bindableObject.PropertyChanged -= h)
                  .Where(e => e.EventArgs.PropertyName == propertyName)
                  .Select(e => (TProperty)bindableObject.GetValue(bindableProperty));
    }


  }
}
