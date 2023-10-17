namespace Railway_Reservation_Managerment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Train_Details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Train_Details()
        {
            Fare_Detail = new HashSet<Fare_Detail>();
        }

        [Key]
        public int Train_Id { get; set; }

        [StringLength(50)]
        public string Train_name { get; set; }

        public int? No_of_compartment { get; set; }

        public int? Seats_Available { get; set; }

        [StringLength(50)]
        public string Train_Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fare_Detail> Fare_Detail { get; set; }
    }
}
