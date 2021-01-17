using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface ILayoutVariablesName
    {
        String Name { get; }
        String FirstName { get; }
        String SecondName { get; }

        List<String> NameParts { get; }
    }
}
