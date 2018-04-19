using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReduxXamDemo.Utils
{
  public class ListItemClickedBehavior : Behavior<ListView>
  {
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create("Command", typeof(Command), typeof(ListItemClickedBehavior), null);

    public Command Command
    {
      get { return (Command)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    public ListView AssociatedObject { get; private set; }

    protected override void OnAttachedTo(ListView listView)
    {
      base.OnAttachedTo(listView);
      AssociatedObject = listView;
      listView.ItemSelected += ListView_ItemSelected;
      listView.BindingContextChanged += ListView_BindingContextChanged;
    }

    protected override void OnDetachingFrom(ListView listView)
    {
      base.OnDetachingFrom(listView);
      listView.ItemSelected -= ListView_ItemSelected;
      listView.BindingContextChanged -= ListView_BindingContextChanged;
      AssociatedObject = null;
    }

    private bool isDeselecting = false;
    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      if (isDeselecting)
      {
        isDeselecting = false;
        return;
      }

      if (Command == null) return;

      if (Command.CanExecute(e.SelectedItem))
      {
        Command.Execute(e.SelectedItem);
      }

      //automatically deselect
      isDeselecting = true;
      ((ListView)sender).SelectedItem = null;
    }

    private void ListView_BindingContextChanged(object sender, EventArgs e)
    {
      base.OnBindingContextChanged();
      BindingContext = AssociatedObject.BindingContext;
    }


  }
}
