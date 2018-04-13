using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ReduxLib
{
  public class Store<TApplicationState>
  {
    private Subject<object> dispatcher;
    private IConnectableObservable<TApplicationState> storeStateStream;
    private object dispatchLock = new object();

    public Store(IReducer<TApplicationState> reducer, TApplicationState initialState = default(TApplicationState))
    {
      dispatcher = new Subject<object>();

      storeStateStream = GetStateObservable(reducer, initialState);
      storeStateStream.Connect();

      //for initialState, might be set in reducer
      Dispatch<object>(null);
    }

    public void Dispatch<TAction>(TAction action)
    {
      lock (dispatchLock)
      {
        dispatcher.OnNext(action);
      }
    }

    private IConnectableObservable<TApplicationState> GetStateObservable(IReducer<TApplicationState> reducer, TApplicationState initialState)
    {
      return dispatcher
      .Scan(initialState, (state, action) => reducer.Reduce(state, action))
      .Replay(1);
    }

    public IObservable<TSlice> Select<TSlice>(Func<TApplicationState, TSlice> selector)
    {
      return storeStateStream.Select(state => selector(state)).DistinctUntilChanged();
    }

    //public IObservable<TApplicationState> GetState()
    //{
    //  return storeStateStream.DistinctUntilChanged();
    //}

  }
}
