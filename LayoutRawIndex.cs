using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    class LayoutRawIndex:IIndex,  IEquatable<IIndex>
    {
        public  String indexView { get; }
        public ILevel currentLevel { get; }
        public int ordinalInLevel { get; }
        public IIndex Parent { get; }

        public String nextPossibleIndexViewInLevel { get; }


        public LayoutRawIndex(ILevel _currentLevel, int ordinal, IIndex parent=null)
        {
            Parent = parent;
            ordinalInLevel = ordinal;
            currentLevel = _currentLevel;

            if (parent != null)
            {                
                indexView = parent.indexView + "." + ordinal.ToString();
                nextPossibleIndexViewInLevel= parent.indexView + "." + (ordinal+1).ToString();
            }
            else
            {                
                indexView = ordinal.ToString();
                nextPossibleIndexViewInLevel = (ordinal+1).ToString();
            }
            
        }

        

        public bool Equals(IIndex other)
        {
            return other.indexView == indexView;
        }

        public override bool Equals(object other)
        {
            return Equals((IIndex) other);
        }

    }
}
