using InternProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternProject.Persistence.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.TeamLead)
                .WithMany()
                .HasForeignKey(x => x.TeamLeadId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

