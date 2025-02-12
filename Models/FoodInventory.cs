using System.ComponentModel.DataAnnotations;

namespace SocietyHopeOrg.Models
{
    public class FoodInventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int CannedFoods { get; set; } = 0;
        public int Beans { get; set; } = 0;
        public int PastaOrRice { get; set; } = 0;
        public int SugarFlourSaltCondiments { get; set; } = 0;
        public int BabyProducts { get; set; } = 0;
    }

}
