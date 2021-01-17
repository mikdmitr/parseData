using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parseData
{
    public class LayoutVariablesName:ILayoutVariablesName
    {
        public String Name { get; } = "";
        public String FirstName { get; } = "";
        public String SecondName { get; } = "";

        public List<String> NameParts { get; } = new List<String>();

        public LayoutVariablesName(String fullNameString)
        {
            Name = fullNameString.IndexOf(".")>0?fullNameString.Substring(0, fullNameString.Length-1):fullNameString;
            FirstName = Name.Split("-"[0])[0];
            string secondNamePrep=Name.Split("-"[0]).Skip(1).Aggregate("", (current, next) => current + "-" + next, resultSelector => resultSelector);
            SecondName = secondNamePrep.Length>0?secondNamePrep.Substring(1): secondNamePrep;
            NameParts = Name.Split("-"[0]).ToList();
        }
    }
}
