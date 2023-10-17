namespace Railway_Reservation_Managerment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fare_Detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fare_Detail()
        {
            Train_Booking = new HashSet<Train_Booking>();
        }

        [Key]
        public int Fare_Id { get; set; }

        [StringLength(50)]
        public string From_City { get; set; }

        [StringLength(50)]
        public string To_City { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Depature_Date { get; set; }

        public TimeSpan? Depature_Time { get; set; }

        public int? Train_Id { get; set; }

        [StringLength(50)]
        public string Class { get; set; }

        public int? Price { get; set; }

        public virtual Train_Details Train_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Train_Booking> Train_Booking { get; set; }
    }
}
