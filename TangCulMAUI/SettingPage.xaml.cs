using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TangCulMAUI.DataGrid;
using TangCulMAUI.Schema.InternalData;
using Newtonsoft.Json.Linq;
using TangCulMAUI.Schema;
namespace TangCulMAUI;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();
	}
    private async void SetPersonDataPath(object sender, EventArgs e)
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
            PickerTitle = "Select Data.json",
            FileTypes = customFileType

        });
        if(result!=null && result.FileName.EndsWith(".json"))
        {
            AppData.Instance.SavePath = result.FullPath;
            //PersonList.LoadPersonList();
        }

    }
    public static void SavePersonList(object sender, EventArgs e)
    {
        JArray personList = [];
        try
        {
            foreach (Person person in AppData.Instance.PersonData)
            {
                personList.Add(person.SavePersonToJsonObject());
            }
            string db = personList.ToString();
            File.WriteAllText(AppData.Instance.SavePath, db);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    private async void SetSettingDataPath(object sender, EventArgs e)
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
        if (result != null && result.FileName.EndsWith(".json"))
        {
            AppData.Instance.SettingPath = result.FullPath;
            PersonList.SetPersonSetting();
        }
    }

    private void CallPopup(object sender, EventArgs e)
    {
        Window lao = new(new TraitEditer());
        Application.Current?.OpenWindow(lao);
    }

}