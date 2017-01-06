using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QDMS;

namespace OverviewApp
{
    public class MappingConfiguration
    {
        public static void Register()
        {
            Mapper.Register<ExecutionMessage, QDMS.LiveTrade>()
                   .Member(dest => dest.AccountID, src => src.AccountID)
                   .Member(dest => dest.TradeDirection, src => src.Side)
                   .Member(dest => dest.Position, src => src.Qty)
                   .Member(x => x.InstrumentID, src => src.InstrumentID)
                   .Member(dest => dest.AverageCost, src => src.Price)
                   .Member(dest => dest.UpdateTime, src => src.Time);
                   




            Mapper.Compile();
        }
        private static int GetInstrumentId(string srcDescription)
        {
            return 1;
        }
    }
}
