using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysInternshipManagement.Models.Widget
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