# Xamarin : Take or Select Image from Gallery using Media Plugin (Android & iOS)

### Steps to install required dependencies:

#### Step A:

Install the below nuget packages,

1. Xamarin.Essentials
2. Xam.Plugin.Media


#### Step B (Android Required Setup):

In MainActivity.cs file (Android Project),

1. Add a reference,
```
using Xamarin.Essentials;
```
2. On OnCreate(Bundle savedInstanceState) method add,
```
...
Xamarin.Essentials.Platform.Init(this, savedInstanceState);
...
```
3. To handle runtime permissions on Android, Xamarin.Essentials must receive any OnRequestPermissionsResult. Hence add the following method aswell,
```
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
{
    Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}
```
The media plugin uses the Xamarin.Essentials, so 
<a href="https://docs.microsoft.com/en-us/xamarin/essentials/get-started">
here
<a/>
the complete guid to use Xamarin.Essentials.

4. Add a new folder called xml into your Resources folder and add a new XML file called file_paths.xml. Make sure that this XML file has a Build Action of: AndroidResource. Then add the below code in it.
```
<?xml version="1.0" encoding="utf-8"?>
<paths xmlns:android="http://schemas.android.com/apk/res/android">
    <external-files-path name="my_images" path="Pictures" />
    <external-files-path name="my_movies" path="Movies" />
</paths>
```

#### Step C (iOS Required Setup):

In Info.plist (iOD Project),
```
<key>NSCameraUsageDescription</key>
<string>This app needs access to the camera to take photos.</string>
<key>NSPhotoLibraryUsageDescription</key>
<string>This app needs access to photos.</string>
<key>NSMicrophoneUsageDescription</key>
<string>This app needs access to microphone.</string>
<key>NSPhotoLibraryAddUsageDescription</key>
<string>This app needs access to the photo gallery.</string>
```

#### Step D (Take image from Camera or Select image from Gallery):

##### To Take Photo from Camera:
###### xaml:
```
<Button Text="Take Photo"
        Clicked="TakePhotoBtnClicked">
</Button>

<Image x:Name="image"></Image>
<Label>Path:</Label>
<Label x:Name="imagelocation"></Label>
```
###### xaml.cs
```
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
```

##### To Take Photo from Gallery:
###### xaml:
```
<Button Text="Gallery"
        Clicked="SellectFromGalleryBtnClicked">
</Button>

<Image x:Name="image"></Image>
<Label>Path:</Label>
<Label x:Name="imagelocation"></Label>
```
###### xaml.cs
```
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
```

Referred: <a href="https://github.com/jamesmontemagno/MediaPlugin">MediaPlugin<a/> by James Montemagno.
