﻿using ReduxXamDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReduxXamDemo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectSizeView : ViewPage<SelectSizeViewModel>
	{
		public SelectSizeView()
		{
			InitializeComponent ();
		}
	}
}