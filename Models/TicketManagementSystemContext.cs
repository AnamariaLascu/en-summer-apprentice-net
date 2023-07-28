using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TMS.Api.Models;

public partial class TicketManagementSystemContext : DbContext
{
    public TicketManagementSystemContext()
    {
    }

    public TicketManagementSystemContext(DbContextOptions<TicketManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<TicketCategory> TicketCategories { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-CL1V1GF\\SQLEXPRESS;Initial Catalog=ticket_management_system;Integrated Security=True;TrustServerCertificate=True;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__B611CB9D4D74E328");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ_Customer_email").IsUnique();

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customerID");
            entity.Property(e => e.CustomerName)
                .HasColumnType("text")
                .HasColumnName("customerName");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("email");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__2DC7BD69CC8B8800");

            entity.ToTable("Event");

            entity.Property(e => e.EventId)
                .ValueGeneratedNever()
                .HasColumnName("eventID");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.EventDescription)
                .HasColumnType("text")
                .HasColumnName("eventDescription");
            entity.Property(e => e.EventName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("eventName");
            entity.Property(e => e.EventTypeId).HasColumnName("eventTypeID");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.VenueId).HasColumnName("venueID");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .HasConstraintName("FK_Event_EventType");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .HasForeignKey(d => d.VenueId)
                .HasConstraintName("FK_Event_Venue");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("PK__EventTyp__04ACC49DCA01580C");

            entity.ToTable("EventType");

            entity.HasIndex(e => e.EventTypeName, "UQ__EventTyp__F1C27EB1F0F02A88").IsUnique();

            entity.Property(e => e.EventTypeId)
                .ValueGeneratedNever()
                .HasColumnName("eventTypeID");
            entity.Property(e => e.EventTypeName)
                .HasMaxLength(500)
                .HasColumnName("eventTypeName");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809337D75BF7171");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("orderID");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.NumberOfTickets).HasColumnName("numberOfTickets");
            entity.Property(e => e.OrderedAt)
                .HasColumnType("datetime")
                .HasColumnName("orderedAt");
            entity.Property(e => e.TicketCategoryId).HasColumnName("ticketCategoryID");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("numeric(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customer");

            entity.HasOne(d => d.TicketCategory).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TicketCategoryId)
                .HasConstraintName("FK_Orders_TicketCategory");
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(e => e.TicketCategoryId).HasName("PK__TicketCa__56F2E67A40E62870");

            entity.ToTable("TicketCategory");

            entity.Property(e => e.TicketCategoryId)
                .ValueGeneratedNever()
                .HasColumnName("ticketCategoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EventId).HasColumnName("eventID");
            entity.Property(e => e.Price)
                .HasColumnType("numeric(10, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketCategories)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_TicketCategory_Event");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.VenueId).HasName("PK__Venue__4DDFB71F4FD9320A");

            entity.ToTable("Venue");

            entity.Property(e => e.VenueId)
                .ValueGeneratedNever()
                .HasColumnName("venueID");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Location)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Type)
                .HasColumnType("text")
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
