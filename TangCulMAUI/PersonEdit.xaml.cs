using TangCulMAUI.DataGrid;
using TangCulMAUI.Schema;

namespace TangCulMAUI;

public partial class PersonEdit : ContentPage
{   
	Person _editor;
    PersonDataView _view;

    public PersonEdit(Person p, PersonDataView view)
	{
        _editor = p;
        _view = view;
        InitializeComponent();
        ChangedName.Text = p.Name;


    }
}