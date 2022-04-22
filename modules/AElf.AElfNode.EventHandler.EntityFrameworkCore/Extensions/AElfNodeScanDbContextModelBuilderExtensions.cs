using System;
using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.AElfNode.EventHandler.EntityFrameworkCore.Constants;
using AElf.AElfNode.EventHandler.EntityFrameworkCore.Options;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AElf.AElfNode.EventHandler.EntityFrameworkCore.Extensions
{
    public static class AElfNodeScanDbContextModelBuilderExtensions
    {
        public static void ConfigureAElfLibTransactionManagement(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] Action<AElfNodeScanDbContextModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AElfNodeScanDbContextModelBuilderConfigurationOptions(
                AElfNodeScanDbProperties.DbTablePrefix,
                AElfNodeScanDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<TransactionWithLogsInfo>(b =>
            {
                b.ToTable(options.TablePrefix + "TransactionWithLogsInfo", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.TransactionId).HasMaxLength(AElfNodeProcessorConsts.MaxTransactionIdLength)
                    .IsRequired();
                b.Property(x => x.BlockNumber).IsRequired();
                b.Property(x => x.BlockTime).HasMaxLength(AElfNodeProcessorConsts.MaxDatetimeLength).IsRequired();
                b.Property(x => x.BlockHash).HasMaxLength(AElfNodeProcessorConsts.MaxBlockHashLength).IsRequired();
                b.Property(x => x.ChainId).IsRequired();
                b.Property(x => x.SaveTicks).IsRequired();
                b.HasKey(x => x.Id);
                b.HasIndex(x => new {x.SaveTicks});
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<SaveData>(b =>
            {
                b.ToTable(options.TablePrefix + "SaveData", options.Schema);
                b.ConfigureByConvention();
                b.Property(x => x.Data).HasMaxLength(AElfNodeProcessorConsts.MaxDataValueLength)
                    .IsRequired();
                b.HasKey(x => x.Id);
                b.Property(x => x.Key).HasMaxLength(AElfNodeProcessorConsts.MaxDataKeyLength).IsRequired();
                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<AElfNodeDbContext>();
        }
    }
}