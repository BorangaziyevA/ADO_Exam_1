namespace ExamWork_1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableExchangeRate")]
    public class TableExchangeRate
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string title { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime pubDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public double description { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int quant { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string index { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string change { get; set; }
    }
}
