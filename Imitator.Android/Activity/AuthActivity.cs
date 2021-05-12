using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Imitator.Android.Activity
{
    [Activity(Label = "AuthActivity")]
    public class AuthActivity : AppCompatActivity
    {
        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        private Button btn_auth_form;

        private int MY_PERMISSIONS_REQUEST_CAMERA = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {

            }
            catch (Exception ex)
            {

            }
            // Create your application here
        }
    }
}