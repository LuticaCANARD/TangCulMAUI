using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();
	}
    private async void SetPath(object sender, EventArgs e)
    {
        var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".json" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/json" } },
                    { DevicePlatform.macOS, new[] { "json"} }, // UTType values
                });
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select Setting.json",
            FileTypes = customFileType

        });
        ;
        if(result!=null && result.FileName.EndsWith(".json"))
        {
            AppData.Instance.SettingPath = result.FullPath;
        }

    }
}