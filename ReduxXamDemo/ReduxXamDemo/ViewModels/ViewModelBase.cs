﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReduxXamDemo.ViewModels
{
  public class ViewModelBase
  {

    public virtual void OnLoaded()
    {
    }

    public virtual void OnUnloaded()
    {
    }

    #region Notify Property Changed Members
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
