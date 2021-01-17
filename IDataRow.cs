using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface IDataRow
    {
        List<IDataItem> DataItems { get; }
        void AddDataItem(IDataItem dataItem);
        String DataForExport { get; }
        int columnCount { get; }
        int size { get; }
    }
}
