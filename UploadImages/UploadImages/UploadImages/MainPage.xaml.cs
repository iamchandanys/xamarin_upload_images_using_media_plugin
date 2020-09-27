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
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Avaialble.", "OK");
                return;
            }
            else
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var cameraAndStorageRequest = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });

                    cameraStatus = cameraAndStorageRequest[Permission.Camera];
                    storageStatus = cameraAndStorageRequest[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    files.Clear();
                    
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        AllowCropping = true,
                        PhotoSize = PhotoSize.Medium,
                        Directory = "Sample",
                        Name = "sample.jpg"
                    });

                    if (file == null)
                        return;

                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });

                    //files.Add(file);

                    //await DisplayAlert("File Location", "Pic Added Successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");

                    CrossPermissions.Current.OpenAppSettings();
                }
            }
        }
    }
}
