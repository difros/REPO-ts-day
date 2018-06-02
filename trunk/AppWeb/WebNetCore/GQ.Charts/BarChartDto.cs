using System.Collections.Generic;

namespace GQ.Charts
{
    public class BarChartDto
    {
        public string key { get; set; }
        public object y { get; set; }
        public string type { get; set; }
        public int yAxis { get; set; }
        public bool? area { get; set; }
        public int? strokeWidth { get; set; }
        public string classed { get; set; }
        public List<BarchartItemDto> values { get; set; } = new List<BarchartItemDto>();
        public List<RegionesItemDto> regiones { get; set; } = new List<RegionesItemDto>();
    }

    public class RegionesItemDto
    {
        public object x1 { get; set; }
        public object x2 { get; set; }
        public string label { get; set; }
        public string color { get; set; }
        public string IsError { get; set; } = "0";
    }

    public class BarchartItemDto
    {
        public BarchartItemDto()
        { }

        public BarchartItemDto(string label, object y)
        {
            this.label = label;
            this.y = y;
        }

        public BarchartItemDto(string label, object y, string x)
        {
            this.label = label;
            this.y = y;
            this.x = x;
        }

        public object x { get; set; }
        public object y { get; set; }
        public string label { get; set; }
    }
}
