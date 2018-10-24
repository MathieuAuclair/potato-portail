namespace SysInternshipManagement.Views.Shared.Widget.Model
{
    public class FlatUiInputButton
    {
        public string InputName { get; set; }
        public string Name { get; set; }

        public FlatUiInputButton(string inputName)
        {
            InputName = inputName;
        }
        public FlatUiInputButton(string inputName, string name)
        {
            InputName = inputName;
            Name = name;
        }
    }
}