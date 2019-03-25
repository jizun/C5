using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziri.MDL
{
    public class ListOrderField
    {
        public string Name { get; set; }
        public OrderByMode Mode { get; set; }
    }

    public enum OrderByMode
    {
        None = 1,
        Desc = 2,
        Asc = 3
    }

    public class ListFilterField
    {
        public string Name { get; set; }
        public FilterCmpareMode CmpareMode { get; set; }
        public List<string> Value { get; set; }
    }

    public enum FilterCmpareMode
    {
        Equal,
        Like,
    }
}
