using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieTicketsBookings.Models
{

    public partial class Prn222FinalProjectContext : DbContext
    {
        public Prn222FinalProjectContext()
        {
        }

        public Prn222FinalProjectContext(DbContextOptions<Prn222FinalProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<BookingItem> BookingItems { get; set; }

        public virtual DbSet<BookingSeatsDetail> BookingSeatsDetails { get; set; }

        public virtual DbSet<Cinema> Cinemas { get; set; }

        public virtual DbSet<Discount> Discounts { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        public virtual DbSet<FoodAndDrink> FoodAndDrinks { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<MovieCategory> MovieCategories { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<Revenue> Revenues { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<Row> Rows { get; set; }

        public virtual DbSet<Seat> Seats { get; set; }

        public virtual DbSet<Showtime> Showtimes { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
            //    optionsBuilder.UseSqlServer(ConnectionString);
            //}
        }
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("Server=LAPTOP-KU6LEG9B\\SQLEXPRESS;User Id=sa;Password=123456;Database=PRN222_FinalProject;TrustServerCertificate=true;MultipleActiveResultSets=true;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC27E252D881");

                entity.ToTable("Booking");

                entity.HasIndex(e => e.TicketCode, "UQ__Booking__598CF7A3BE591D55").IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.BookingDate).HasColumnType("datetime");
                entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.TicketCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Showtime).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ShowtimeId)
                    .HasConstraintName("FK__Booking__Showtim__5DCAEF64");

                entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Booking__UserID__5EBF139D");
            });

            modelBuilder.Entity<BookingItem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__BookingI__3214EC27C4099CF5");

                entity.ToTable("BookingItem");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.BookingId).HasColumnName("BookingID");
                entity.Property(e => e.FoodAndDrinksId).HasColumnName("FoodAndDrinksID");

                entity.HasOne(d => d.Booking).WithMany(p => p.BookingItems)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__BookingIt__Booki__6A30C649");

                entity.HasOne(d => d.FoodAndDrinks).WithMany(p => p.BookingItems)
                    .HasForeignKey(d => d.FoodAndDrinksId)
                    .HasConstraintName("FK__BookingIt__FoodA__693CA210");
            });

            modelBuilder.Entity<BookingSeatsDetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__BookingS__3214EC275E46427B");

                entity.ToTable("BookingSeatsDetail");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.BookingId).HasColumnName("BookingID");
                entity.Property(e => e.SeatId).HasColumnName("SeatID");

                entity.HasOne(d => d.Booking).WithMany(p => p.BookingSeatsDetails)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__BookingSe__Booki__66603565");

                entity.HasOne(d => d.Seat).WithMany(p => p.BookingSeatsDetails)
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("FK__BookingSe__SeatI__656C112C");
            });

            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Cinema__3214EC279E6C07E4");

                entity.ToTable("Cinema");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC272DB76551");

                entity.ToTable("Discount");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Code).HasMaxLength(50);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC277B9B054A");

                entity.ToTable("Feedback");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Comments).HasMaxLength(1000);
                entity.Property(e => e.CreateAt).HasColumnType("datetime");
                entity.Property(e => e.MovieId).HasColumnName("MovieID");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Movie).WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK__Feedback__MovieI__4222D4EF");

                entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Feedback__UserID__412EB0B6");
            });

            modelBuilder.Entity<FoodAndDrink>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__FoodAndD__3214EC2703F6DBF0");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Image).IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Movies__3214EC2777ADD849");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Actor).HasMaxLength(255);
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.Director).HasMaxLength(255);
                entity.Property(e => e.Poster).IsUnicode(false);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.TrailerUrl)
                    .HasMaxLength(255)
                    .HasColumnName("TrailerURL");

                entity.HasOne(d => d.Category).WithMany(p => p.Movies)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Movies__Category__3B75D760");
            });

            modelBuilder.Entity<MovieCategory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MovieCat__3214EC27ADC403B2");

                entity.ToTable("MovieCategory");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__News__3214EC27F9361EE5");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreateAt).HasColumnType("datetime");
                entity.Property(e => e.Image).HasMaxLength(255);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.News)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK__News__CreateBy__44FF419A");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC27DDB19457");

                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.BookingId).HasColumnName("BookingID");
                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Payment__Booking__619B8048");

                entity.HasOne(d => d.Discount).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK__Payment__Discoun__628FA481");
            });

            modelBuilder.Entity<Revenue>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Revenue__3214EC2735426181");

                entity.ToTable("Revenue");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CinemaId).HasColumnName("CinemaID");

                entity.HasOne(d => d.Cinema).WithMany(p => p.Revenues)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK__Revenue__CinemaI__5441852A");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Role__3214EC2701722917");

                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Room__3214EC27F051F437");

                entity.ToTable("Room");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CinemaId).HasColumnName("CinemaID");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Cinema).WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK__Room__CinemaID__4BAC3F29");
            });

            modelBuilder.Entity<Row>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Rows__3214EC275AD3B496");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.RoomId).HasColumnName("RoomID");
                entity.Property(e => e.RowName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Type).HasMaxLength(255);

                entity.HasOne(d => d.Room).WithMany(p => p.Rows)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__Rows__RoomID__4E88ABD4");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Seat__3214EC2792AA445E");

                entity.ToTable("Seat");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.RowId).HasColumnName("RowID");
                entity.Property(e => e.SeatName)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasMaxLength(255);

                entity.HasOne(d => d.Row).WithMany(p => p.Seats)
                    .HasForeignKey(d => d.RowId)
                    .HasConstraintName("FK__Seat__RowID__5165187F");
            });

            modelBuilder.Entity<Showtime>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Showtime__3214EC276CCD0851");

                entity.ToTable("Showtime");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.MovieId).HasColumnName("MovieID");
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK__Showtime__MovieI__59063A47");

                entity.HasOne(d => d.Room).WithMany(p => p.Showtimes)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__Showtime__RoomID__59FA5E80");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__User__3214EC276FCBF7AF");

                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__RoleID__3E52440B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}