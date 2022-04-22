using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.Core.Domains.Entities;
using AElf.Client.Dto;
using AutoMapper;

namespace AElf.AElfNode.EventHandler.BackgroundJob
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<TransactionResultEto, TransactionWithLogsInfo>();
            CreateMap<TransactionResultDto, TransactionResultEto>().ForMember(dest => dest.MethodName,
                    opts => opts.MapFrom(src => src.Transaction.MethodName))
                .ForMember(dest => dest.FromAddress, opts => opts.MapFrom(src => src.Transaction.From))
                .ForMember(dest => dest.ToAddress, opts => opts.MapFrom(src => src.Transaction.To));
            CreateMap<LogEventDto, LogEventEto>();
            CreateMap<TransactionResultEto, EventContext>();
        }
    }
}