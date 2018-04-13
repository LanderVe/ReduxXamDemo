namespace ReduxLib
{
  public interface IReducer<TState>
  {
    TState Reduce(TState state = default(TState), object action = default(object));
  }
}
