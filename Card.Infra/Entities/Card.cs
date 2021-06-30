using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cards.Infra.Entities
{
    [Table("Card")]
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        public int CostumerId { get; set; }
        public long CardNumber { get; set; }
        public DateTime TokenRegistrationDate { get; set; }
    }
}
