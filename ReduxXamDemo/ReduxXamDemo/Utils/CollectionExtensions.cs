using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ReduxXamDemo.Utils
{
  static class CollectionExtensions
  {
    public static ImmutableList<T> MoveItemAtIndexToFront<T>(this ImmutableList<T> list, T item)
    {
      var builder = list.ToBuilder();
      builder.Remove(item);
      builder.Insert(0, item);
      return builder.ToImmutable();
    }

    public static ImmutableSortedDictionary<K,V> Update<K,V>(this ImmutableSortedDictionary<K,V> dict, K key, V value)
    {
      var builder = dict.ToBuilder();
      builder.Remove(key);
      builder.Add(key, value);
      return builder.ToImmutable();
    }

    //static ImmutableList<T> MoveItemAtIndexToFront<T>(this ImmutableList<T> list, T item)
    //{
    //  list = list.Remove(item);
    //  return list.Insert(0, item);
    //}

  }
}
