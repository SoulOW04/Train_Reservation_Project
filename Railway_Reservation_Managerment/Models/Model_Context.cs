namespace Railway_Reservation_Managerment.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model_Context : DbContext
    {
        public Model_Context()
            : base("name=Model_Context")
        {
        }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Fare_Detail> Fare_Detail { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<Train_Booking> Train_Booking { get; set; }
        public virtual DbSet<Train_Details> Train_Details { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
