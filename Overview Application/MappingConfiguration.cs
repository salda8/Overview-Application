using Common;
using ExpressMapper;
using OverviewApp.TradingEntitiesPl;

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

            Mapper.Register<OpenOrder, OpenOrderPl>()
                .Member(dest => dest.AccountNumber, src => src.Account.AccountNumber)
                .Member(dest => dest.Symbol, src => src.Instrument.Symbol);

            Mapper.Register<LiveTrade, LiveTradePl>()
                .Member(dest => dest.AccountNumber, src => src.Account.AccountNumber).Member(dest => dest.Symbol, src => src.Instrument.Symbol);
            Mapper.Register<TradeHistory, TradeHistoryPl>()
                .Member(dest => dest.AccountNumber, src => src.Account.AccountNumber).Member(dest => dest.Symbol, src => src.Instrument.Symbol);
            Mapper.Register<PortfolioSummary, PortfolioSummaryPl>()
                .Member(dest => dest.AccountNumber, src => src.Account.AccountNumber);
            Mapper.Register<AccountSummary, AccountSummaryPl>()
                .Member(dest => dest.AccountNumber, src => src.Account.AccountNumber);
            Mapper.Register<Equity, EquityPl>()
                .Member(dest => dest.AccountNumber, src => src.Account.AccountNumber);

            // Mapper.RegisterCustom<Account, string>(src => src.AccountNumber);

            Mapper.Compile();
        }

        private static int GetInstrumentId(string srcDescription)
        {
            return 1;
        }
    }
}