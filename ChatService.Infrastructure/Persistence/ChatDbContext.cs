using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure.Persistence;

public class ChatDbContext : DbContext, IUnitOfWork
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<Message> Messages => Set<Message>();
    public DbSet<UserRelation> UserRelations => Set<UserRelation>();
    public DbSet<ChatReadState> ChatReadStates => Set<ChatReadState>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
      

        builder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(m => m.ChatId).IsRequired();
            entity.Property(m => m.SenderUserId).IsRequired();
            entity.Property(m => m.ReceiverUserId).IsRequired();
            entity.Property(m => m.Content).IsRequired();
            entity.HasIndex(m => m.ChatId); 
            entity.HasIndex(m => new { m.ChatId, m.Id }); 
        });

        builder.Entity<UserRelation>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => new { r.UserAId, r.UserBId }).IsUnique();
        });

        builder.Entity<ChatReadState>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => new { s.ChatId, s.UserId }).IsUnique();
        });

        builder.Entity<Message>()
          .HasQueryFilter(m => !m.IsDeleted);

        base.OnModelCreating(builder);
    }
}
