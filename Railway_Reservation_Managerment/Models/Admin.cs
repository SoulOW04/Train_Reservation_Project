namespace Railway_Reservation_Managerment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        public int Admin_Id { get; set; }

        [StringLength(10)]
        public string AdmUserName { get; set; }

        [StringLength(10)]
        public string AdmPassword { get; set; }
    }
}
