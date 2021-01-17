using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface IIndex
    {
        String indexView { get; }
        ILevel currentLevel { get; }
        int ordinalInLevel { get; }
        IIndex Parent { get;  }
        String nextPossibleIndexViewInLevel { get; }

    }
}
