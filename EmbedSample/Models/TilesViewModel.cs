using System.Collections.Generic;

namespace paas_demo.Models
{
    public class TilesViewModel
    {
        public List<Tile> Tiles { get; set; }
        public string DashboardName { get; set; }
        public string DashboardId { get; set; }
        public string AccessToken { get; set; }
    }
}