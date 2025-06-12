using SpotifyClone.Models;
using Microsoft.EntityFrameworkCore;

namespace SpotifyClone.Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ArtistDetails> ArtistDetails { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<SongComposers> SongComposers { get; set; }
    public DbSet<UserDetails> UserDetails { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure cascade delete behavior to prevent cycles
        
        // SongComposers relationships - prevent cascade cycles
        modelBuilder.Entity<SongComposers>()
            .HasOne(sc => sc.Artist)
            .WithMany(a => a.ArtistComposedSongs)
            .HasForeignKey(sc => sc.ArtistId)
            .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict
            
        modelBuilder.Entity<SongComposers>()
            .HasOne(sc => sc.Song)
            .WithMany(s => s.SongComposers)
            .HasForeignKey(sc => sc.SongId)
            .OnDelete(DeleteBehavior.Cascade); // Keep cascade for Song -> SongComposers

        // Genre relationships - prevent multiple cascade paths
        modelBuilder.Entity<Genre>()
            .HasOne(g => g.Artist)
            .WithMany(a => a.Genres)
            .HasForeignKey(g => g.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Genre>()
            .HasOne(g => g.Album)
            .WithMany(a => a.Genres)
            .HasForeignKey(g => g.AlbumId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Genre>()
            .HasOne(g => g.Song)
            .WithMany(s => s.Genres)
            .HasForeignKey(g => g.SongId)
            .OnDelete(DeleteBehavior.Restrict);

        // Playlist relationships
        modelBuilder.Entity<Playlist>()
            .HasOne(p => p.UserDetails)
            .WithMany(ud => ud.Playlists)
            .HasForeignKey(p => p.UserDetailsId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Playlist>()
            .HasOne(p => p.Artist)
            .WithMany(a => a.Playlists)
            .HasForeignKey(p => p.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure many-to-many relationship for Song-Playlist
        modelBuilder.Entity<Song>()
            .HasMany(s => s.Playlists)
            .WithMany(p => p.Songs)
            .UsingEntity(j => j.ToTable("SongPlaylists"));

        // Configure many-to-many relationship for Artist-Song (featuring artists)
        modelBuilder.Entity<Artist>()
            .HasMany(a => a.FeaturingSongs)
            .WithMany(s => s.FeaturingArtist)
            .UsingEntity<Dictionary<string, object>>(
                "ArtistSong",
                j => j.HasOne<Song>().WithMany().HasForeignKey("FeaturingSongsId").OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Artist>().WithMany().HasForeignKey("FeaturingArtistId").OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("FeaturingArtistId", "FeaturingSongsId");
                    j.ToTable("ArtistSong");
                });

        // User-Artist relationship (one-to-one)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Artist)
            .WithOne(a => a.User)
            .HasForeignKey<Artist>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User-UserDetails relationship (one-to-one)
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserDetails)
            .WithOne(ud => ud.User)
            .HasForeignKey<UserDetails>(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Artist-ArtistDetails relationship (one-to-one)
        modelBuilder.Entity<Artist>()
            .HasOne(a => a.Details)
            .WithOne(ad => ad.Artist)
            .HasForeignKey<ArtistDetails>(ad => ad.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Artist-Album relationship
        modelBuilder.Entity<Album>()
            .HasOne(a => a.Artist)
            .WithMany(ar => ar.Albums)
            .HasForeignKey(a => a.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Album-Song relationship
        modelBuilder.Entity<Song>()
            .HasOne(s => s.Album)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=Spotify;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}