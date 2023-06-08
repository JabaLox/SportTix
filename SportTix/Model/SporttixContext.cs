using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SportTix.Model;

public partial class SporttixContext : DbContext
{
    public static SporttixContext Context { get; } = new SporttixContext();
    public SporttixContext()
    {
    }

    public SporttixContext(DbContextOptions<SporttixContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Typeticket> Typetickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userticket> Usertickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseMySql("server=localhost;user=root;database=sporttix;password=kolyan28", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.IdMatches).HasName("PRIMARY");

            entity.ToTable("matches");

            entity.HasIndex(e => e.IdTeamGuest, "Team2_idx");

            entity.Property(e => e.IdMatches)
                .ValueGeneratedNever()
                .HasColumnName("idMatches");
            entity.Property(e => e.DateMatch).HasColumnType("datetime");
            entity.Property(e => e.IdTeamGuest).HasColumnName("idTeamGuest");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Не начался'")
                .HasColumnType("enum('Не начался','Идёт','Окончен')");

            entity.HasOne(d => d.IdTeamGuestNavigation).WithMany(p => p.Matches)
                .HasForeignKey(d => d.IdTeamGuest)
                .HasConstraintName("Team2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("idRole");
            entity.Property(e => e.NameRole).HasColumnType("text");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.IdTeams).HasName("PRIMARY");

            entity.ToTable("teams");

            entity.Property(e => e.IdTeams)
                .ValueGeneratedNever()
                .HasColumnName("idTeams");
            entity.Property(e => e.City).HasMaxLength(45);
            entity.Property(e => e.Logotype).HasMaxLength(100);
            entity.Property(e => e.NameTeam).HasMaxLength(45);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("PRIMARY");

            entity.ToTable("ticket");

            entity.HasIndex(e => e.IdMatches, "match_idx");

            entity.HasIndex(e => e.IdTypeTicket, "type_idx");

            entity.Property(e => e.IdTicket)
                .ValueGeneratedNever()
                .HasColumnName("idTicket");
            entity.Property(e => e.IdMatches).HasColumnName("idMatches");
            entity.Property(e => e.IdTypeTicket).HasColumnName("idTypeTicket");
            entity.Property(e => e.Sector).HasMaxLength(45);

            entity.HasOne(d => d.IdMatchesNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdMatches)
                .HasConstraintName("match");

            entity.HasOne(d => d.IdTypeTicketNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdTypeTicket)
                .HasConstraintName("TypeTicket1");
        });

        modelBuilder.Entity<Typeticket>(entity =>
        {
            entity.HasKey(e => e.IdTypeTicket).HasName("PRIMARY");

            entity.ToTable("typeticket");

            entity.Property(e => e.IdTypeTicket)
                .ValueGeneratedNever()
                .HasColumnName("idTypeTicket");
            entity.Property(e => e.NameTypeTicket).HasMaxLength(45);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "login_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Role, "role_idx");

            entity.Property(e => e.IdUsers)
                .ValueGeneratedNever()
                .HasColumnName("idUsers");
            entity.Property(e => e.DateBirth).HasColumnType("datetime");
            entity.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("password");
            entity.Property(e => e.Patronomyc).HasColumnType("text");
            entity.Property(e => e.Photo).HasMaxLength(45);
            entity.Property(e => e.SurName).HasColumnType("text");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("role");
        });

        modelBuilder.Entity<Userticket>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdTicket })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("userticket");

            entity.HasIndex(e => e.IdTicket, "ticket_idx");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.DateBuy).HasColumnType("datetime");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.Usertickets)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ticket");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Usertickets)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
