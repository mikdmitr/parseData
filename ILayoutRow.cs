using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public enum LayoutRowType
    {
        record=1,
        singleColumn=2,       
        group=3,
        redefines=4,
        occurs=5,  //subtableStart
        filler =6,
        comment=7,
        empty=8

    }

    public enum DataType
    {
        none=0,
        numeric=1,
        alphabetic=2,
        alphaNumeric=3,
        implicitDecimal=4,
        sign=5,
        assumedDecimal=6,
        signedNumeric=7,
        implicitDecimalNumeric=8

    }

    public struct ItemDataType
    {
        public DataType dataType { get; }
        public int dataLength { get; }
        public string defaultValue { get; } 

        public ItemDataType(DataType _dataType, int _dataLength=0, String _defaultValue="")
        {
            dataType = _dataType;
            dataLength = _dataLength;
            defaultValue = _defaultValue;
        }

    }

    public interface ILayoutRow
    {
        ILevel Level { get; }
        int iRowNumber { get; }
        IIndex Index { get; }
        ILayoutVariablesName varName { get; }
        String sPreComments { get; set; }
        String sOrigin { get; }
        ILayoutVariablesName redefinedVarName { get; }
        int iOccuresTimes { get; }
        ILayoutRow parent { get; set; }
        LayoutRowType recordType { get; }
        List<ItemDataType> dataType { get; }
        int dataLength { get; }
        bool dataTypeIsSingle { get; }
        bool isCommonPart { get; }
        bool isGroupPart { get; }
    }
}
