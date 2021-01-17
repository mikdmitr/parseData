using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface IDataItem
    {
        String sData { get; } 
        String sExportData { get; }
        ILayoutRow oLayoutRow { get; }
        bool dataIsValid { get; }
    }
}
