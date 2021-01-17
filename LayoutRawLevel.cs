using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public class LayoutRawLevel : ILevel,  IEquatable<ILevel> //IEquatable<LayoutRawLevel>,
    {
        public String levelView { get; } = "";
        public int ordinalNumber { get; set; } = 0;
        public bool isRoot { get { return ordinalNumber == 0; }  }

        public LayoutRawLevel(String lv)
        {
            levelView = lv;
            //ordinalNumber = ord;
        }

        //public bool Equals(LayoutRawLevel other)
        //{
        //    return other.levelView == levelView;
        //}

        public bool Equals(ILevel other)
        {
            return other.levelView == levelView;
        }

        public override bool Equals(object other) => Equals(other as ILevel);


    }
}
