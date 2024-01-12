using Newtonsoft.Json.Linq;
using TangCulMAUI.DataGrid;
using TangCulMAUI.Schema;
using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI;

public partial class PersonList : ContentPage
{
    public List<Person> SelectedPersonData = [];
    public List<Person> Source_ = AppData.Instance.PersonData;

    public PersonList()
	{
        LoadPersonList();
        AppData.Instance.PersonData.Add(
            new Person("nem", 12, ["aa"], PersonStatus.Alive, "bb")
         );
        InitializeComponent();
        BindingContext = new PersonData();
    }


	public static void LoadPersonList()
	{
        try
        {
            AppData.Instance.PersonData.Clear();
            string seriallizedData = File.ReadAllText(AppData.Instance.SavePath);
            JArray personOrigionData = JArray.Parse(seriallizedData);
            foreach (JObject personObject in personOrigionData.Cast<JObject>())
            {
                PersonStatus status_Dis = PersonStatus.Alive;
                JToken alive_ = personObject["state_die"];
                if (alive_ != null)
                    status_Dis = (int) alive_ == 2 ? PersonStatus.Alive :
                                 (int) alive_ == 1 ? PersonStatus.Sick : PersonStatus.Dead;

                Person loadedperson = new(
                    (string)(personObject["name"] ?? "error"),
                    (int)(personObject["age"] ?? 0),
                    personObject["trait"].ToObject<string[]>(),
                    status_Dis,
                    (string)personObject["trait"]
                );
                AppData.Instance.PersonData.Add(loadedperson);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
	}

    public static void SetPersonSetting()
    {
        try
        {
            string seriallizedData = File.ReadAllText(AppData.Instance.SettingPath);
            JObject origin = JObject.Parse(seriallizedData);
            AppData.Instance.setting = new PersonSetting(origin);

        } 
        catch (Exception ex)
        {
            // 에러나면 알려주기.
            Console.WriteLine(ex.ToString());
        }
    }
}