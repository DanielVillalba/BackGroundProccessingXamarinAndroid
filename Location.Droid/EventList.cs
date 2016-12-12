using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
namespace Location.Droid
{
    [Activity(Label = "@string/movementHistory")]
    public class EventList : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Create your application here
            var trackChanges = Intent.Extras.GetStringArrayList("trackChanges") ?? new string[0];
            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, trackChanges);
        }
    }
}