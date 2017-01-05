using EntityData;
using ExpressMapper.Extensions;
using QDMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverviewApp.ViewModels
{
    internal class MessageHandler
    {
        private IMyDBContext Context;
        public MessageHandler(IMyDbContext context)
        {
            Context = context;
        }

        //PREREQ logic: One trade, one account, one strategy
        public List<LiveTrade> UpdateLiveTrades(List<LiveTrade> liveTradesList)
        {
            Int32 latestId = 0;
            var executionMessages = Context.ExecutionMessages.Where(x => x.ID > latestId).ToList();
            var liveTrades = new List<LiveTrade>();

            foreach (var message in executionMessages)
            {
                LiveTrade liveTrade= new LiveTrade();
                if (ItIsFirstTradeOnAccount(message, liveTradesList))
                {
                   
                    message.Map(liveTrade);
                    liveTrades.Add(liveTrade);
                    Context.LiveTrades.Add(liveTrade)
                    
                    
                }
                else if (ItIsSameSideTrade(message,liveTradesList))
                {
                    var item =
                        liveTrades.LastOrDefault(
                            x => x.AccountID == message.AccountID && x.InstrumentID== message.InstrumentID);
                    var newFillPrice = (item.AverageCost * item.Position + message.Price * message.Qty) / (item.Position + message.Qty);
                    var newQty = item.Position + message.Qty;
                    message.Map(liveTrade);
                    liveTrade.Position = newQty;
                    liveTrade.AverageCost = newFillPrice;

                    liveTrades.Add(liveTrade);
                    Context.LiveTrades.RemoveRange(Context.LiveTrades.Where(x => x.AccountID == message.AccountID));//InstrumentID==InstrumentID account which is trading more than one symbol.
                    Context.LiveTrades.Add(liveTrade);
                    
                   
                }
                else //different side trade
                {
                    var item =
                        liveTrades.LastOrDefault(
                            x => x.AccountID == message.AccountID && x.InstrumentID == message.InstrumentID);

                    if (message.Qty > item.Position)
                    {
                        var newFillPrice = (item.AverageCost * item.Position - message.Price * message.Qty) / (item.Position - message.Qty);
                        var newQty = message.Qty - item.Position;
                        message.Map(liveTrade);
                        liveTrade.AverageCost = newFillPrice;
                        liveTrade.Position = newQty;
                        
                        liveTrades.Add(liveTrade);
                        Context.LiveTrades.RemoveRange(Context.LiveTrades.Where(x => x.AccountID == message.AccountID));
                        Context.LiveTrades.Add(liveTrade);
                    }
                    else
                    {
                        Context.LiveTrades.RemoveRange(Context.LiveTrades.Where(x => x.AccountID == message.AccountID));
                    }

                    liveTrades.Remove(item);

                }

               
            }
            return liveTrades;
        }

        /// <summary>
        ///     Check if the same side trade exist.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private bool ItIsSameSideTrade(ExecutionMessage message, List<LiveTrade> liveTrades)
        {
            return liveTrades.Any(
                 x =>
                     x.TradeDirection == ConvertFromString(message.Side) &&
                     x.Instrument.SymbolForOrders == message.Instrument.SymbolForOrders);

        }

        /// <summary>
        ///     Check if on same account and same contract exists any trade.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private bool ItIsFirstTradeOnAccount(ExecutionMessage message, List<LiveTrade> liveTrades)
        {
            return
                 liveTrades.Any(x=>x.AccountID== message.AccountID &&
                     x.Instrument.SymbolForOrders == message.Instrument.SymbolForOrders);
        }

        public static TradeDirection ConvertFromString(string side) => side == "SLD" ? TradeDirection.Long : TradeDirection.Short;
        public List<TradeHistory> UpdateTradeHistory(int latestHistoryTrade)
        {
            return (from s1 in Context.ExecutionMessages
                                                  join s2 in Context.CommissionMessages
                                                  on s1.ExecutionId equals s2.ExecutionId
                                                  where s2.ID > latestHistoryTrade
                                                  select new TradeHistory
                                                  {

                                                      ID = s2.ID,
                                                      AccountID = s1.AccountID,
                                                      ExecId = s1.ExecutionId,
                                                      ExecTime = s1.Time,
                                                      Side = ConvertFromString(s1.Side),
                                                      Position = s1.Qty,
                                                      InstrumentID = s1.InstrumentID,
                                                      Price = s1.Price,
                                                      Commission = s2.Commission,
                                                      RealizedPnL = s2.RealizedPnL
                                                  }).ToList();

            

            
        }
    }
}