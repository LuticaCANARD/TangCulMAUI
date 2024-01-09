using Newtonsoft.Json.Linq;
using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI;

public partial class PersonList : ContentPage
{

	public PersonList()
	{
		InitializeComponent();
		LoadPersonList();

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
                Schema.PersonStatus status_Dis = Schema.PersonStatus.Alive;
                JToken alive_ = personObject["st_die"];
                if (alive_ != null)
                    status_Dis = (int)alive_ == 2 ? Schema.PersonStatus.Alive :
                                 (int)alive_ == 1 ? Schema.PersonStatus.Sick : Schema.PersonStatus.Dead;

                Schema.Person loadedperson = new Schema.Person(
                    (string)(personObject["name"] ?? "error"),
                    ((int)(personObject["age"] ?? 0)),
                    personObject["trait"].ToObject<string[]>(),
                    status_Dis,
                    (string)personObject["trait"]
                    );
                AppData.Instance.PersonData.Add(loadedperson);
            }
        }
        catch (Exception ex)
        {

        }
	}
}