using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace ReduxXamDemo.Utils
{
  static class ObservablesExtensions
  {
    /// <summary>
    /// subscribes to an observable and adds the subscription to a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="onNext"></param>
    /// <param name="subscriptions"></param>
    /// <returns></returns>
    public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, List<IDisposable> subscriptions)
    {
      var sub = observable.Subscribe(onNext);
      subscriptions.Add(sub);
      return sub;
    }

    /// <summary>
    /// subscribes to an observable and adds the subscription to a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="subject"></param>
    /// <param name="subscriptions"></param>
    /// <returns></returns>
    public static IDisposable Subscribe<T>(this IObservable<T> observable, Subject<T> subject, List<IDisposable> subscriptions)
    {
      var sub = observable.Subscribe(subject);
      subscriptions.Add(sub);
      return sub;
    }

  }
}
