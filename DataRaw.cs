using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parseData
{
    public class DataRaw:IDataRow
    {
        public List<IDataItem> DataItems { get; } = new List<IDataItem>();

        public void AddDataItem(IDataItem dataItem)
        {
            DataItems.Add(dataItem);
        }

        public String DataForExport
        {
            get
            {
                return DataItems.Select(a=>a.sExportData).Aggregate((cur, next) => cur + ";" + next);
            }
        }

        public int columnCount { get=>DataItems.Count; }

        public int size { get=>DataItems.Select(a=>a.oLayoutRow.dataLength).Aggregate((cur,next)=>cur+next); }
    }
}
