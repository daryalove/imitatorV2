using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Imitator.Android.Activity.MainFunctionality;
using Imitator.Android.Activity.PersonalData;
using AlertDialog = Android.App.AlertDialog;
using Toast = Android.Widget.Toast;
using ToastLength = Android.Widget.ToastLength;

namespace Imitator.Android.Activity
{
    [Activity(Label = "ActivityMainFunctionality")]
    public class ActivityMainFunctionality : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.PageMainFunctionality);

                Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "Имитатор";

                BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.NavigationFormMainFunctionality);

                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                ActivitySensorsData ContentSensorsData = new ActivitySensorsData();
                transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentSensorsData).AddToBackStack(null).Commit();

                navigation.NavigationItemSelected += (sender, e) =>
                {
                    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.navigation_sensors_data:
                            ActivitySensorsData ContentSensorsData = new ActivitySensorsData();
                            transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentSensorsData).AddToBackStack(null).Commit();
                            break;
                        case Resource.Id.navigation_map:
                            ActivityMap ContentMap = new ActivityMap();
                            transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentMap).AddToBackStack(null).Commit();
                            break;
                        case Resource.Id.navigation_container_state:
                            ActivityContainerState ContentContainerState = new ActivityContainerState();
                            transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentContainerState).AddToBackStack(null).Commit();
                            break;
                        case Resource.Id.navigation_photographic_recordin:
                            ActivityPhotographicRecording ContentPhotographicRecording = new ActivityPhotographicRecording();
                            transaction.Replace(Resource.Id.framelayoutFormMainFunctionality, ContentPhotographicRecording).AddToBackStack(null).Commit();
                            break;
                        //case Resource.Id.navigation_PersonalData:
                        //    break;
                        //case Resource.Id.navigation_Exit:
                        //    break;
                            //case Resource.Id.exit_driver:
                            //    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                            //    alert.SetTitle("Внимание!");
                            //    alert.SetMessage("Вы действительно хотите выйти ?");
                            //    alert.SetPositiveButton("Да", (senderAlert, args) =>
                            //    {
                            //        Leaveprofile();
                            //    });
                            //    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                            //    {
                            //    });
                            //    Dialog dialog = alert.Create();
                            //    dialog.Show();
                            //    break;
                    }
                };
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }          
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.navigation_appBar_menu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_appBar_PersonalData:
                    {
                        Intent PersonalData = new Intent(this, typeof(ActivityPersonalData));
                        StartActivity(PersonalData);
                        this.Finish();
                        return true;
                    }
                case Resource.Id.navigation_appBar_Exit:
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Внимание!");
                        alert.SetMessage("Вы действительно хотите выйти со своего профиля ?");
                        alert.SetPositiveButton("Да", (senderAlert, args) =>
                        {
                            try
                            {
                                Intent Auth = new Intent(this, typeof(MainActivity));
                                StartActivity(Auth);
                                this.Finish();
                            }
                            catch (System.Exception ex)
                            {
                                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                            }
                        });
                        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        {
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                        return true;
                    }
            }

            return base.OnOptionsItemSelected(item);
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