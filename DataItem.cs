using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public class DataItem : IDataItem
    {
        public String sData { get; } ="";

        public String sExportData 
        {
            get
            {
                string result = sData;
                if (!oLayoutRow.dataTypeIsSingle)
                {
                    result = "";
                    int curSymbolPos = 0;
                    foreach(ItemDataType sItemDataType in oLayoutRow.dataType)
                    {
                        switch  (sItemDataType.dataType)                         
                        {
                            case DataType.implicitDecimalNumeric : 
                                result += "." + sData.Substring(curSymbolPos, sItemDataType.dataLength);
                                curSymbolPos += sItemDataType.dataLength;
                                break;
                            default:
                                result += sData.Substring(curSymbolPos, sItemDataType.dataLength);
                                curSymbolPos += sItemDataType.dataLength;
                                break;
                        }
                    }            
                }
                return result;

            }        
        }

        public ILayoutRow oLayoutRow { get; }

        public bool dataIsValid { get { return oLayoutRow.dataLength == sData.Length; } } 

        public DataItem(ILayoutRow _LayoutRow, String dataString)
        {
            sData = dataString;
            oLayoutRow = _LayoutRow;
        }
    }
}
