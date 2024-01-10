using Newtonsoft.Json.Linq;
using TangCulMAUI.DataGrid;
using TangCulMAUI.Schema;
using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI;

public partial class PersonList : ContentPage
{


    public List<Person> SelectedPersonData = new List<Person>();
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


	public void LoadPersonList()
	{
        try
        {
            AppData.Instance.PersonData.Clear();
            string seriallizedData = File.ReadAllText(AppData.Instance.SettingPath);
            JArray personOrigionData = JArray.Parse(seriallizedData);
            foreach (JObject personObject in personOrigionData.Cast<JObject>())
            {
                PersonStatus status_Dis = PersonStatus.Alive;
                JToken alive_ = personObject["st_die"];
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
}