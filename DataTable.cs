using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parseData
{
    public class DataTable : IDataTable
    {
        public List<IDataRow> rows { get; } = new List<IDataRow>();

        public void AddRaw(IDataRow dataRow)
        {
            rows.Add(dataRow);
        }

        public List<String> columnName
        {
            get
            {
                return rows.FirstOrDefault().DataItems.Select(a => a.oLayoutRow.varName.SecondName).ToList();
            }
        }

        public String columnNameExport { get=>columnName.Aggregate((cur,next)=>cur+";"+next); }

        public List<String> dataExport
        {
            get
            {
                return rows.Select(a => a.DataForExport).ToList();
            }
        }

        public int possibleOccurs { get;  }

        public int FillerSize { get;  }

        public DataTable (ILayoutTableDefinition _tableDef)
        {
            tableDef = _tableDef;
            possibleOccurs = _tableDef.possibleOccurs;
            FillerSize = _tableDef.fillerSize;
            Name = _tableDef.Name;
            maxVolume = _tableDef.multiPartVolume+ _tableDef.commonRawPartSize*_tableDef.possibleOccurs;
            commonRawPartSize = _tableDef.commonRawPartSize;
            groupRawPartSize = _tableDef.groupRawPartSize;
            isMulti = _tableDef.isMulti;


        }

        public int NumberOfRaws { get=>rows.Count; } 
        public int actualSize { get => rows.Count*rows.FirstOrDefault().size; }

        public ILayoutTableDefinition tableDef { get; }

        public String Name { get; }

        public int maxVolume { get; }

        public int commonRawPartSize { get; }

        public int groupRawPartSize { get; }

        public bool isMulti { get; }


    }
}
