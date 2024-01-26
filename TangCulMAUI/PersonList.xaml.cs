using Newtonsoft.Json.Linq;
using TangCulMAUI.DataGrid;
using TangCulMAUI.Schema;
using TangCulMAUI.Schema.InternalData;
namespace TangCulMAUI;

public partial class PersonList : ContentPage
{
    public List<Person> SelectedPersonData = [];
    public List<Person> Source_ = AppData.Instance.PersonData;
    PersonDataView data;
    public PersonList()
    {
        LoadPersonList();
        AppData.Instance.PersonData.Add(
            new Person(1,"nem", 12, ["aa"], PersonStatus.Alive, "bb")
         );
        InitializeComponent();
        data = new PersonDataView
        {
            People = AppData.Instance.PersonData
        };
        BindingContext = data;



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
                    (int)(personObject["id"] ?? 0),
                    (string)(personObject["name"] ?? "error"),
                    (int)(personObject["age"] ?? 0),
                    personObject["trait"].ToObject<string[]>(),
                    status_Dis,
                    (string)personObject["agent"]
                );
                AppData.Instance.PersonData.Add(loadedperson);
            }
        }
        catch (Exception ex) { 
            Console.WriteLine(ex.ToString());
        }
	}
    private void EditPerson(object sender, EventArgs e)
    {
        // 아마 이번주 목요일 끝낼듯 함. 
        // 새 창을 띄워 올린 다음, 그 창에서 편집한 인물을 
        AppData.Instance.PersonData.Add(
             new Person(2, "nem2", 12, ["aa"], PersonStatus.Alive, "bb")
        );

        data.RefreshCommand.Execute(null);
        //data.SelectedPerson.Status;
    }
}