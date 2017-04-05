using ExpressMapper;
using QDMS;

namespace OverviewApp
{
    public class MappingConfiguration
    {
        public static void Register()
        {
            Mapper.Register<ExecutionMessage, LiveTrade>()
                   .Member(dest => dest.AccountID, src => src.AccountID)
                   .Member(dest => dest.TradeDirection, src => src.Side)
                   .Member(dest => dest.Quantity, src => src.Quantity)
                   .Member(x => x.InstrumentID, src => src.InstrumentID)
                   .Member(dest => dest.AveragePrice, src => src.Price)
                   .Member(dest => dest.UpdateTime, src => src.Time);

            Mapper.Compile();
        }

        private static int GetInstrumentId(string srcDescription)
        {
            return 1;
        }
    }
}