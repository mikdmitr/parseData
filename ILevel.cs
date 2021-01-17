using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface ILevel
    {
        String levelView { get; } 
        int ordinalNumber { get; set; }
        bool isRoot { get; }
    }
}
