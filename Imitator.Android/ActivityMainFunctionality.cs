using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;

namespace Imitator.Android
{
    public class ActivityMainFunctionality : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.PageMainFunctionality);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.NavigationFormMainFunctionality);

            navigation.NavigationItemSelected += (sender, e) =>
            {
                FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                switch (e.Item.ItemId)
                {
                    case Resource.Id.tasks:
                        MapActivity content2 = new MapActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();
                        break;
                    case Resource.Id.boxes:
                        MainBoxStatusActivity content = new MainBoxStatusActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                        break;
                    case Resource.Id.c_status:
                        ManageBoxActivity content3 = new ManageBoxActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content3).AddToBackStack(null).Commit();
                        break;
                    case Resource.Id.alarms:
                        AlarmsActivity content4 = new AlarmsActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content4).AddToBackStack(null).Commit();
                        break;

                    case Resource.Id.exit_driver:
                        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                        alert.SetTitle("Внимание!");
                        alert.SetMessage("Вы действительно хотите выйти ?");
                        alert.SetPositiveButton("Да", (senderAlert, args) =>
                        {
                            Leaveprofile();
                        });
                        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        {
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                        break;
                }
            };


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_driver_layout);

        }

        //public interface IBackButtonListener
        //{
        //    void OnBackPressed();
        //}
        //public override void OnBackPressed()
        //{
        //    if (StaticUser.IsUserOrMapActivity == true)
        //    {
        //        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
        //        alert.SetTitle("Внимание!");
        //        alert.SetMessage("Вы действительно хотите выйти со своего профиля ?");
        //        alert.SetPositiveButton("Да", (senderAlert, args) =>
        //        {
        //            Leaveprofile();
        //        });
        //        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
        //        {
        //        });
        //        Dialog dialog = alert.Create();
        //        dialog.Show();
        //    }
        //    else
        //    {
        //        base.OnBackPressed();
        //        //var currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.framelayout);
        //        //var listener = currentFragment as IBackButtonListener;
        //        //if (listener != null)
        //        //{
        //        //    listener.OnBackPressed();
        //        //    return;
        //        //}
        //    }
        //}
    }
}