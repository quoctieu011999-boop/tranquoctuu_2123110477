using System.ComponentModel.DataAnnotations;
namespace ConnectDB.Models;

    public class Reward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointsRequired { get; set; }
        public int Stock { get; set; }
    }

