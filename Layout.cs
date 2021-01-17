using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parseData
{
    public class Layout
    {
        //public static ILevelSameView levelCompare = new ILevelSameView();

        public List<ILevel> Levels { get; } = new List<ILevel>();
        public List<IIndex> Indexes { get; } = new List<IIndex>();
        public List<ILayoutRow> LayoutRows { get; } = new List<ILayoutRow>();
        

        public void AddLevel(ILevel level)
        {
            if (!LevelAlreadyExists(level))
            {
                level.ordinalNumber = Levels.Count;
                Levels.Add(level);
            }              
        }

        public void AddLayoutRaw(ILayoutRow raw)
        {
            raw.parent = raw.recordType == LayoutRowType.record ? null : LayoutRows.Where(a => a.Index == raw.Index.Parent).FirstOrDefault();
            LayoutRows.Add(raw);
        }

        public Boolean LevelAlreadyExists(ILevel level) 
        {
            return Levels.IndexOf(level) >= 0 ;
        }

        public void AddRawIndex(IIndex index)
        {
            if (!IndexAlreadyExists(index))                      
                Indexes.Add(index);            
        }

        public Boolean IndexAlreadyExists(IIndex index)
        {
            return Indexes.IndexOf(index) >= 0 ;
        }

        public List<ILayoutTableDefinition> TableDefinitions 
        {
            get
            {
                List<ILayoutTableDefinition> result = new List<ILayoutTableDefinition>();

                if (isSingleTableLayout)
                {
                    ILayoutTableDefinition tableDef = new LayoutTableDefinition(LayoutRows.Where(a=>a.recordType==LayoutRowType.record).FirstOrDefault(), LayoutRows);
                    
                    foreach (ILayoutRow column in LayoutRows.Where(a => a.recordType == LayoutRowType.singleColumn))
                        tableDef.AddColumn(column);
                    result.Add(tableDef);
                }
                else
                {
                    foreach(ILayoutRow tableStart in LayoutRows.Where(a => a.recordType == LayoutRowType.redefines))
                    {
                        ILayoutTableDefinition tableDef = new LayoutTableDefinition(tableStart, LayoutRows);

                        foreach (ILayoutRow column in LayoutRows.Where(a => ((a.recordType == LayoutRowType.singleColumn)&&(a.Level==tableStart.Level)&&(a.varName.Name!= tableStart.redefinedVarName.Name))||((a.recordType == LayoutRowType.singleColumn) && (a.varName.FirstName == tableStart.varName.SecondName))))
                            tableDef.AddColumn(column);
                        result.Add(tableDef);
                    }

                }

                return result;
            } 
        }

        public bool isSingleTableLayout 
        {
            get
            {
                return NumberOfTables==1;
            }
        }

        public int NumberOfTables
        {
            get => LayoutRows.Where(a => a.recordType == LayoutRowType.redefines).Any() ? LayoutRows.Where(a => a.recordType == LayoutRowType.redefines).Count() : 1;
        }

    }


    //class ILevelSameView : EqualityComparer<ILevel>
    //{
    //    public override bool Equals(ILevel b1, ILevel b2)
    //    {
    //        if (b1 == null && b2 == null)
    //            return true;
    //        else if (b1 == null || b2 == null)
    //            return false;

    //        return (b1.levelView == b2.levelView);
    //    }

    //    public override int GetHashCode(ILevel bx)
    //    {
    //        int hCode = (bx.ordinalNumber+3) ^ (bx.ordinalNumber + 4) ^ (bx.ordinalNumber + 5);
    //        return hCode.GetHashCode();
    //    }
    //}

}
