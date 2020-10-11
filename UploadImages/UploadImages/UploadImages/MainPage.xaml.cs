using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UploadImages
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void TakePhotoBtnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            //Check whether camera is available or take photo is supported on the device

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample", //Directory of the image
                Name = "sample.jpg", //Name of the image
                PhotoSize = PhotoSize.Medium, //Photo size
                AllowCropping = true, //Allow cropping (supports only on iOS)
            });

            if (file == null)
                return;

            imagelocation.Text = file.Path;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }


        private async void SellectFromGalleryBtnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            //Check whether camera is available or take photo is supported on the device

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Avaialble.", "OK");
                return;
            } 

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            imagelocation.Text = file.Path;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }
    }
}
