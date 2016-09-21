using System.Collections.Generic;

namespace paas_demo.Models
{
    //Power BI Dashboards used to deserialize the Get Dashboards response.
    public class Dashboards
    {
        public List<Dashboard> value { get; set; }
    }

    public class Dashboard
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    //Power BI Tiles used to deserialize the Get Tiles response.
    public class Tiles
    {
        public List<Tile> value { get; set; }
    }

    public class Tile
    {
        public string id { get; set; }
        public string title { get; set; }
        public string embedUrl { get; set; }
    }
}
