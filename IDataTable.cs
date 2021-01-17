using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface IDataTable
    {
        //List<ILayoutRow> layout { get; }
        List<IDataRow> rows { get; }
        void AddRaw(IDataRow dataRow);
        List<String> columnName { get; }
        List<String> dataExport { get; }
        int possibleOccurs { get;  }
        int FillerSize { get;  }
        int NumberOfRaws { get; }
        int actualSize { get; }
        int maxVolume { get; }
        ILayoutTableDefinition tableDef { get; }
        String Name { get; }
        int commonRawPartSize { get; }
        int groupRawPartSize { get; }
        bool isMulti { get; }
        String columnNameExport { get; }

    }
}
