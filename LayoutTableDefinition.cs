using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parseData
{
    public class LayoutTableDefinition:ILayoutTableDefinition
    {
        public List<ILayoutRow> columns { get; } = new List<ILayoutRow>();
        
        public String Name { get; }

        public LayoutTableDefinition(ILayoutRow initialRaw, List<ILayoutRow> layoutRows)
        {
            Name = initialRaw.recordType==LayoutRowType.record?initialRaw.varName.Name:initialRaw.varName.SecondName;
            isMulti= initialRaw.recordType == LayoutRowType.redefines;
            ILayoutRow occursLayoutRow = layoutRows.Where(a => (a.varName.FirstName == initialRaw.varName.SecondName)&&(a.recordType==LayoutRowType.occurs)).FirstOrDefault();
            possibleOccurs = isMulti ? occursLayoutRow.iOccuresTimes : 0;
            multiPartVolume = isMulti ? layoutRows.Where(a => a.varName.Name == initialRaw.redefinedVarName.Name).FirstOrDefault().dataLength : 0;
            fillerSize= isMulti ? layoutRows.Where(a => a.Index.indexView == occursLayoutRow.Index.nextPossibleIndexViewInLevel).FirstOrDefault().dataLength : 0;
        }

        public void AddColumn(ILayoutRow layoutRow)
        {
            columns.Add(layoutRow);
        }

        public int possibleOccurs { get; }
        public int fillerSize { get; }
        public int multiPartVolume { get; }
        public bool isMulti { get; }

        public bool multiPartVolumeCheck { get => columns.Where(a => a.isGroupPart).Select(a=>a.dataLength).Aggregate( (prev, next) => prev + next)*possibleOccurs + fillerSize == multiPartVolume; }
        public int commonRawPartSize { get=> columns.Where(a => a.isCommonPart).Select(a => a.dataLength).Aggregate((prev, next) => prev + next); }
        public int groupRawPartSize { get => isMulti?columns.Where(a => a.isGroupPart).Select(a => a.dataLength).Aggregate((prev, next) => prev + next):0; }
        public int fullRawPartSize { get => commonRawPartSize + groupRawPartSize; }
        public int maxPossibleVolumeWithoutFiller { get => fullRawPartSize * possibleOccurs; }
    }
}
