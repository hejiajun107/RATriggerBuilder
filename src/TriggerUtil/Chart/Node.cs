using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil.Chart
{
    public class ChartVM
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        
        public List<Category> Categories { get; set; } = new List<Category>();
        
        public List<Link> Links { get; set; } = new List<Link>();

    }

    public class Node
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int SymbolSize { get; set; } = 10;

        public string Value { get; set; }

        public int Category { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
    }

    public class Link
    {
        public string Source { get; set; }
        public string Target { get; set; }
    }
}
