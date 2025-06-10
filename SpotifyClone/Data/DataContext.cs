using SpotifyClone.Models;
using Microsoft.EntityFrameworkCore;

namespace SpotifyClone.Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<ArtistDetails> ArtistDetails { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<SongComposers> SongComposers { get; set; }
    public DbSet<UserDetails> UserDetails { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User - UserDetails (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserDetails)
            .WithOne(ud => ud.User)
            .HasForeignKey<UserDetails>(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - Artist (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Artist)
            .WithOne(a => a.User)
            .HasForeignKey<Artist>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Artist - ArtistDetails (One-to-One)
        modelBuilder.Entity<Artist>()
            .HasOne(a => a.Details)
            .WithOne(ad => ad.Artist)
            .HasForeignKey<ArtistDetails>(ad => ad.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Artist - Albums (One-to-Many)
        modelBuilder.Entity<Album>()
            .HasOne(al => al.Artist)
            .WithMany(ar => ar.Albums)
            .HasForeignKey(al => al.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        // Album - Songs (One-to-Many)
        modelBuilder.Entity<Song>()
            .HasOne(s => s.Album)
            .WithMany(al => al.Songs)
            .HasForeignKey(s => s.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        // SongComposers (Many-to-Many between Song and Artist)
        modelBuilder.Entity<SongComposers>()
            .HasOne(sc => sc.Song)
            .WithMany(s => s.SongComposers)
            .HasForeignKey(sc => sc.SongId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SongComposers>()
            .HasOne(sc => sc.Artist)
            .WithMany(a => a.ArtistComposedSongs)
            .HasForeignKey(sc => sc.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        // Song - Playlists (Many-to-Many)
        modelBuilder.Entity<Song>()
            .HasMany(s => s.Playlists)
            .WithMany(p => p.Songs)
            .UsingEntity(j => j.ToTable("SongPlaylists"));

        // Genre relationships - Prevent cascade cycles
        modelBuilder.Entity<Genre>()
            .HasOne(g => g.Artist)
            .WithMany(a => a.Genres)
            .HasForeignKey(g => g.ArtistId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Genre>()
            .HasOne(g => g.Album)
            .WithMany(al => al.Genres)
            .HasForeignKey(g => g.AlbumId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Genre>()
            .HasOne(g => g.Song)
            .WithMany(s => s.Genres)
            .HasForeignKey(g => g.SongId)
            .OnDelete(DeleteBehavior.NoAction);

        // Playlist relationships
        modelBuilder.Entity<Playlist>()
            .HasOne(p => p.UserDetails)
            .WithMany()
            .HasForeignKey(p => p.UserDetailsId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Playlist>()
            .HasOne(p => p.Artist)
            .WithMany(a => a.Playlists)
            .HasForeignKey(p => p.ArtistId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure indexes for better performance
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserDetails>()
            .HasIndex(ud => ud.Username)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=Spotify;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}