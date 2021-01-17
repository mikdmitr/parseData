using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parseData
{
    public class LayoutRaw:ILayoutRow
    {
        public ILevel Level { get; } = null;
        public int iRowNumber { get; }
        public IIndex Index { get; } = null;
        public ILayoutVariablesName varName { get; } = null;        
        public String sPreComments { get; set; } = "";
        public String sOrigin { get; }
        public int iOccuresTimes { get; } = 0;
        public ILayoutVariablesName redefinedVarName { get; } = null;
        public ILayoutRow parent { get; set; } = null;
        public LayoutRowType recordType { get; } = LayoutRowType.comment;
        public List<ItemDataType> dataType { get; } = new List<ItemDataType>();

        public LayoutRaw(String sStr, int iRn, ILevel _Level, IIndex _Index)
        {
            sOrigin = sStr.Trim();
            iRowNumber = iRn;
                        
            if (sOrigin.IndexOf("*") == 0)
            {
                //dataType.Add(new ItemDataType(DataType.none));                
            }
            else if (sOrigin.Length==0)
            {
                recordType = LayoutRowType.empty;
                //dataType.Add(new ItemDataType(DataType.none));                
            }
            else
            {
                string[] words = sOrigin.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Level = _Level; 
                Index = _Index;

                if (words.Skip(1).FirstOrDefault().IndexOf(".") >= 0)  //RECORD OR GROUP
                {                    
                    varName = new LayoutVariablesName(words.Skip(1).FirstOrDefault().Substring(0, words.Skip(1).FirstOrDefault().Length - 1));
                    
                    if (varName.SecondName.IndexOf("RECORD") >= 0)
                        recordType = LayoutRowType.record;
                    else
                        recordType = LayoutRowType.group;
                }
                else if (Array.IndexOf(words, "REDEFINES") > 0)
                {
                    recordType = LayoutRowType.redefines;
                    varName = new LayoutVariablesName(words.Skip(1).FirstOrDefault());
                    redefinedVarName = new LayoutVariablesName(words.Where(a => (a.IndexOf(".") > 0)).FirstOrDefault());
                }
                else if (Array.IndexOf(words, "REDEFINES") > 0)
                {
                    recordType = LayoutRowType.redefines;
                    varName = new LayoutVariablesName(words.Skip(1).FirstOrDefault());
                    redefinedVarName = new LayoutVariablesName(words.Where(a => (a.IndexOf(".") > 0)).FirstOrDefault());
                }
                else if (Array.IndexOf(words, "OCCURS") > 0)
                {
                    recordType = LayoutRowType.occurs;
                    varName = new LayoutVariablesName(words.Skip(1).FirstOrDefault());
                    iOccuresTimes = int.Parse(words[Array.IndexOf(words, "OCCURS") + 1]);
                }
                else if (Array.IndexOf(words, "FILLER") > 0)
                {
                    recordType = LayoutRowType.filler;
                    varName = new LayoutVariablesName(words.Skip(1).FirstOrDefault());

                    String dataTypeString = words[Array.IndexOf(words, "PIC") + 1];

                    dataType = stringToListOfItemsDataType(dataTypeString);
                }
                else if ((Array.IndexOf(words, "FILLER") == -1)&& (Array.IndexOf(words, "PIC") >0))
                {
                    recordType = LayoutRowType.singleColumn;
                    varName = new LayoutVariablesName(words.Skip(1).FirstOrDefault());

                    String dataTypeString = words[Array.IndexOf(words, "PIC") + 1];

                    String defValue = "";
                    if(Array.IndexOf(words, "VALUE") > 0)
                    {
                        String valPrep = words[Array.IndexOf(words, "VALUE") + 1];
                        defValue = valPrep.IndexOf(".")>0? valPrep.Substring(0, valPrep.Length-1):valPrep;
                        defValue = defValue.Trim("'"[0]);
                    }
                                            
                    dataType = stringToListOfItemsDataType(dataTypeString,defValue);
                }
            }
        }

        private List<ItemDataType> stringToListOfItemsDataType(String picStr, String defVal="")
        {
            List<ItemDataType> result = new List<ItemDataType>();

            String pStr = picStr.IndexOf(".") == (picStr.Length - 1) ? picStr.Substring(0, picStr.Length - 1) : picStr;

            string[] words = pStr.Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(String word in words)
            {
                string[] typeAndLength=word.Split(new char[] { '(' });

                DataType dT=DataType.none;
                switch (typeAndLength[0])
                {
                    case "X":
                        dT = DataType.alphaNumeric;
                        break;
                    case "9":
                        dT = DataType.numeric;
                        break;
                    case "S9":
                        dT = DataType.signedNumeric;
                        break;
                    case "V9":
                        dT = DataType.implicitDecimalNumeric;
                        break;
                }

                ItemDataType itemDataType = new ItemDataType(dT,int.Parse(typeAndLength[1]), defVal);
                result.Add(itemDataType);
            }


            return result;
        }

        public int dataLength
        {
            get 
            {
                if (dataType.Count() == 0)
                {
                    return 0;
                }
                else 
                {
                    return dataType.Select(a => a.dataLength).Aggregate(0, (current, next) => current + next, resultSelector => resultSelector);                
                }
            
            }
        }       
        
        public bool dataTypeIsSingle
        {
            get 
            {
                return dataType.Count > 1 ? false : true;
            }
            
        }


        public bool isCommonPart { get=>this.parent.recordType==LayoutRowType.record; }
        public bool isGroupPart { get => this.parent.recordType == LayoutRowType.occurs; }

    }
}
