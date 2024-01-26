using TangCulMAUI.Schema;

namespace TangCulMAUI;

public partial class PersonEdit : ContentPage
{
	Person _editor;
    public PersonEdit(Person p)
	{
        _editor = p;
        InitializeComponent();

	}
}