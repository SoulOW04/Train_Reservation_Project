namespace Railway_Reservation_Managerment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Train_Booking
    {
        [Key]
        public int TrainBooking_Id { get; set; }

        public int? Login_Id { get; set; }

        public int? Fare_Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        public virtual Fare_Detail Fare_Detail { get; set; }

        public virtual Passenger Passenger { get; set; }
    }
}
