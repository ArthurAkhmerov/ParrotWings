using System.Data.Entity;
using PW.Persistence.DTO;

namespace PW.Persistence
{
	internal class DataContext : DbContext
	{
		public DataContext() : base("pw") { }
		public DataContext(string connectionString) : base(connectionString) { }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//default settings
			modelBuilder.Properties()
				.Where(p => p.PropertyType == typeof(string))
				.Configure(x => x.IsRequired());

			modelBuilder.Properties()
				.Where(p => p.Name == "Id")
				.Configure(x => x.IsKey());

			modelBuilder.Types().Configure(t => t.ToTable(t.ClrType.Name.Replace("PDTO", ""), "PW"));

			var userPdto = modelBuilder.Entity<UserPDTO>();
			userPdto.Property(c => c.CreatedAt).HasColumnType("datetime2").HasPrecision(7);

			var transferPdto = modelBuilder.Entity<TransferPDTO>();
			transferPdto.Property(c => c.CreatedAt).HasColumnType("datetime2").HasPrecision(7);
			transferPdto.HasRequired(t => t.UserFrom).WithMany(u => u.TransfersFrom).HasForeignKey(t => t.UserFromId).WillCascadeOnDelete(false);
			transferPdto.HasRequired(t => t.UserTo).WithMany(u => u.TransfersTo).HasForeignKey(t => t.UserToId).WillCascadeOnDelete(false);


			//unreadMessagesPdto.HasKey(x => new { x.MessageId, x.UserId });
			//unreadMessagesPdto.HasRequired(um => um.User).WithMany(u => u.UnreadMessages).HasForeignKey(um => um.UserId).WillCascadeOnDelete(false);
			//unreadMessagesPdto.HasRequired(um => um.Message).WithMany(m => m.UnreadMessages).HasForeignKey(um => um.MessageId).WillCascadeOnDelete(false);

			//var attachmentPdto = modelBuilder.Entity<AttachmentPDTO>();
			//attachmentPdto.HasOptional(x => x.Message).WithMany(m => m.Attachments).HasForeignKey(x => x.MessageId).WillCascadeOnDelete(false);

			//modelBuilder.Entity<MobileDevice>()
			//	.HasKey(x => new { x.UserId, x.DeviceToken })
			//	.Property(x => x.DeviceToken).HasColumnType("nvarchar").HasMaxLength(1024);

			var sessionPdto = modelBuilder.Entity<SessionPDTO>();
			sessionPdto.Property(c => c.CreatedAt).HasColumnType("datetime2").HasPrecision(7);
			sessionPdto.Property(c => c.LastUsage).HasColumnType("datetime2").HasPrecision(7);
		}
	}
}