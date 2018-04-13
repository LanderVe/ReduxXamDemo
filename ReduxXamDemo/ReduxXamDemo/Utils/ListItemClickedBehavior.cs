using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReduxXamDemo.Utils
{
  public class ListItemClickedBehavior : Behavior<ListView>
  {
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create("Command", typeof(bool), typeof(ListItemClickedBehavior), false);

    public Command Command
    {
      get { return (Command)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    protected override void OnAttachedTo(ListView listView)
    {
      listView.ItemSelected += ListView_ItemSelected;
      base.OnAttachedTo(listView);
    }
    protected override void OnDetachingFrom(ListView listView)
    {
      listView.ItemSelected -= ListView_ItemSelected;
      base.OnDetachingFrom(listView);
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      Command.Execute(e.SelectedItem);

      //automatically deselect
      ((ListView)sender).SelectedItem = null;
    }


  }
}
