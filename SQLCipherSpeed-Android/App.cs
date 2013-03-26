
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SQLCipherSpeed
{
	[Application] 
	public class App : Application { 

		public App() : base()
		{ 
		} 

		public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{ 
		} 
		
		public TrialRunner Runner {get; set;}
	} 
}

