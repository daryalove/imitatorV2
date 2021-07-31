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

namespace Imitator.Android.Activity
{
    public static class StaticDataClass
    {
        public static string UserLogin { get; set; }
        public static string UserPassword { get; set; }
    }
}