namespace Railway_Reservation_Managerment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Passenger")]
    public partial class Passenger
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Passenger()
        {
            Train_Booking = new HashSet<Train_Booking>();
        }

        [Key]
        public int Login_Id { get; set; }

        [StringLength(50)]
        public string Login_Name { get; set; }

        [StringLength(100)]
        public string Login_Password { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string ResetPasswordCode { get; set; }

        [StringLength(100)]
        public string PurchaseCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Train_Booking> Train_Booking { get; set; }
    }
}
