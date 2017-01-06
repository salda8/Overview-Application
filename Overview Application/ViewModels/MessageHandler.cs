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
        private readonly IMyDbContext context;
        private Func<LiveTrade, bool> accountIdAndSymbolForOrdersEquealFunc;
        public MessageHandler(IMyDbContext context)
        {
            this.context = context;
        }
        #region OpenOrder Handling
        /// <summary>
        ///     Handles the order status.
        /// </summary>
        public List<OpenOrder> UpdateOpenOrders()
        {
            List<OrderStatusMessage> orderSatusMessage = context.OrderStatusMessages.Where(x=>x.Status.ToUpper() == "CANCELLED" || x.Status.ToUpper() == "FILLED").ToList();
            List<OpenOrder> openOrders = context.OpenOrders.ToList();
            
            foreach (OpenOrder openOrder in openOrders)
            {
                if (orderSatusMessage.Any(x=>x.PermanentId==openOrder.PermanentId))
                {
                    context.OpenOrders.Remove(openOrder);
                }
                else
                {
                    HandleOpenOrder(openOrder, openOrders);
                }
            }

            context.OrderStatusMessages.RemoveRange(orderSatusMessage);
            context.SaveChanges();

            return openOrders;

        }
        
        /// <summary>
        ///     Handles the open order.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="openOrders"></param>
        private void HandleOpenOrder(OpenOrder message, ICollection<OpenOrder> openOrders)
        {
            if (message.Status == "Cancelled")
            {
                openOrders.Remove(message);
                context.OpenOrders.Remove(message);
            }
            else if (message.Status == "Filled")
            {
                openOrders.Remove(message);
                context.OpenOrders.Remove(message);
            }
        }

        #endregion

        #region LiveTradeHandling

            //PREREQ logic: One trade, one account, one strategy
            public List<LiveTrade> UpdateLiveTrades(List<LiveTrade> liveTradesList)
        {
            Int32 latestId = 0;
            var executionMessages = context.ExecutionMessages.Where(x => x.ID > latestId).ToList();
            var liveTrades = new List<LiveTrade>();

            foreach (var message in executionMessages)
            {
                accountIdAndSymbolForOrdersEquealFunc = x => x.AccountID == message.AccountID &&
                                                             x.Instrument.SymbolForOrders ==
                                                             message.Instrument.SymbolForOrders;
                
                LiveTrade liveTrade = new LiveTrade();
                var item = liveTrades.LastOrDefault(accountIdAndSymbolForOrdersEquealFunc);
                if (item == null)
                {
                    message.Map(liveTrade);
                    liveTrades.Add(liveTrade);
                    context.LiveTrades.Add(liveTrade);
                }
                else if (ItIsSameSideTrade(message, liveTradesList))
                {

                    var newFillPrice = (item.AveragePrice * item.Quantity +
                                        message.Price * message.Quantity) / (item.Quantity + message.Quantity);
                    var newQuantity = item.Quantity + message.Quantity;
                    message.Map(liveTrade);
                    liveTrade.Quantity = newQuantity;
                    liveTrade.AveragePrice = newFillPrice;

                    AddToListRemoveAndAddToDatabase(liveTrades, liveTrade, message);
                }
                else //different side trade
                {
                    if (message.Quantity > item.Quantity)
                    {
                        var newFillPrice = (item.AveragePrice * item.Quantity -
                                            message.Price * message.Quantity) /
                                           (item.Quantity - message.Quantity);
                        var newQuantity = message.Quantity - item.Quantity;
                        message.Map(liveTrade);
                        liveTrade.AveragePrice = newFillPrice;
                        liveTrade.Quantity = newQuantity;

                        AddToListRemoveAndAddToDatabase(liveTrades, liveTrade, message);
                    }
                    else
                    {
                        context.LiveTrades.RemoveRange(
                            context.LiveTrades.Where(x => x.AccountID == message.AccountID));
                    }

                    liveTrades.Remove(item);

                }


            }
            return liveTrades;
        }
        private void AddToListRemoveAndAddToDatabase(List<LiveTrade> liveTrades, LiveTrade liveTrade,
                                                     ExecutionMessage message)
        {
            liveTrades.Add(liveTrade);
            context.LiveTrades.RemoveRange(context.LiveTrades.Where(x => x.AccountID == message.AccountID));
                //InstrumentID==InstrumentID account which is trading more than one symbol.
            context.LiveTrades.Add(liveTrade);
        }

        /// <summary>
        ///     Check if the same side trade exist.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private static bool ItIsSameSideTrade(ExecutionMessage message, List<LiveTrade> liveTrades)
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
                 liveTrades.Any(accountIdAndSymbolForOrdersEquealFunc);
        }
        #endregion
        public static TradeDirection ConvertFromString(string side) => side == "SLD" ? TradeDirection.Long : TradeDirection.Short;
        public List<TradeHistory> UpdateTradeHistory(int latestHistoryTrade)
        {
            return (from s1 in context.ExecutionMessages
                                                  join s2 in context.CommissionMessages
                                                  on s1.ExecutionId equals s2.ExecutionId
                                                  where s2.ID > latestHistoryTrade
                                                  select new TradeHistory
                                                  {

                                                      ID = s2.ID,
                                                      AccountID = s1.AccountID,
                                                      ExecId = s1.ExecutionId,
                                                      ExecTime = s1.Time,
                                                      Side = ConvertFromString(s1.Side),
                                                      Quantity = s1.Quantity,
                                                      InstrumentID = s1.InstrumentID,
                                                      Price = s1.Price,
                                                      Commission = s2.Commission,
                                                      RealizedPnL = s2.RealizedPnL
                                                  }).ToList();

            

            
        }
    }
}