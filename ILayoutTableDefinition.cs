using System;
using System.Collections.Generic;
using System.Text;

namespace parseData
{
    public interface ILayoutTableDefinition
    {
        List<ILayoutRow> columns { get; }
        String Name { get; }
        void AddColumn(ILayoutRow layoutRow);
        int possibleOccurs { get; }
        int fillerSize { get; }
        int multiPartVolume { get; }
        bool isMulti { get; }

        bool multiPartVolumeCheck { get ; }
        int commonRawPartSize { get ; }
        int groupRawPartSize { get; }
        int fullRawPartSize { get; }

        int maxPossibleVolumeWithoutFiller { get; }
    }
}
