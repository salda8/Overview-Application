/* Copyright (C) 2013 Interactive Brokers LLC. All rights reserved.  This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBApi;

namespace OverviewApp.Models
{
    public class IbClient : EWrapper
    {
        #region Fields

        public static List<IbClient> WrapperList = new List<IbClient>();
        public static bool Firstime = true;
        public static double LastPrice;
        public static DateTimeOffset Time;
        private readonly TaskFactory tf = new TaskFactory();

        #endregion

        #region

        public IbClient()
        {
            ClientSocket = new EClientSocket(this, new EReaderMonitorSignal());
        }

        #endregion

        #region Properties

        public EClientSocket ClientSocket { get; set; }
        public int NextOrderId { get; set; }
        public string AccountNumber { get; set; }
        public double Equity { get; set; }

        #endregion

        #region

        public virtual void error(Exception e)
        {
            throw e;
        }

        public virtual void error(string str)
        {
            //Console.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff")+ "Error: " + str + "\n");
        }

        public virtual void error(int id, int errorCode, string errorMsg)
        {
        }

        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close,
            long volume, double wap, int count)
        {
        }

        public virtual void historicalData(int reqId, string date, double open, double high, double low, double close,
            int volume, int count, double wap, bool hasGaps)
        {
        }

        public virtual void connectionClosed()
        {
        }

        public virtual void currentTime(long time)
        {
        }

        public virtual void tickPrice(int tickerId, int field, double price, int canAutoExecute)
        {
        }

        public virtual void tickSize(int tickerId, int field, int size)
        {
            //Console.WriteLine("Tick Size. Ticker Id:" + tickerId + ", Field: " + field + ", Size: " + size+"\n");
        }

        public virtual void tickString(int tickerId, int tickType, string value)
        {
            /*Console.WriteLine("Tick string. Ticker Id:" + tickerId + ", Type: " + tickType + ", Value: " + UnixTimeStampToDateTime(Convert.ToDouble(value)));
            Console.Write(DateTime.Now.ToString("hh:mm:ss.fff").ToString("mm:ss.fff"));
            Console.WriteLine("***************************"+"\n");*/
        }

        public virtual void tickGeneric(int tickerId, int field, double value)
        {
        }

        public virtual void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints,
            double impliedFuture, int holdDays, string futureExpiry, double dividendImpact, double dividendsToExpiry)
        {
        }

        public virtual void tickSnapshotEnd(int tickerId)
        {
        }

        public virtual void nextValidId(int orderId)
        {
            NextOrderId = orderId;
        }

        public virtual void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
        }

        public virtual void managedAccounts(string accountsList)
        {
        }

        public virtual void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta,
            double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
        }

        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
        }

        public virtual void accountSummaryEnd(int reqId)
        {
            Console.WriteLine("AccountSummaryEnd. Req Id: " + reqId + "\n");
        }

        public virtual void updateAccountValue(string key, string value, string currency, string accountName)
        {
        }

        public virtual void UpdatePortfolio(Contract contract, int position, double marketPrice, double marketValue,
            double averageCost, double unrealisedPnl, double realisedPnl, string accountName)
        {
        }

        public virtual void updateAccountTime(string timestamp)
        {
            //Console.WriteLine("UpdateAccountTime. Time: " + timestamp + "\n");
        }

        public virtual void accountDownloadEnd(string account)
        {
        }

        public virtual void Position(string account, Contract contract, int pos, double avgCost)
        {
        }

        public virtual void OrderStatus(int orderId, string status, int filled, int remaining, double avgFillPrice,
            int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
        }

        public virtual void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
        }

        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
        }

        public virtual void commissionReport(CommissionReport commissionReport)
        {
        }

        public virtual void openOrderEnd()
        {
        }

        public virtual void contractDetails(int reqId, ContractDetails contractDetails)
        {
        }

        public virtual void contractDetailsEnd(int reqId)
        {
            Console.WriteLine("ContractDetailsEnd. " + reqId + "\n");
        }

        public virtual void execDetailsEnd(int reqId)
        {
            Console.WriteLine("ExecDetailsEnd. " + reqId + "\n");
        }

        public virtual void fundamentalData(int reqId, string data)
        {
            Console.WriteLine("FundamentalData. " + reqId + "" + data + "\n");
        }

        public virtual void marketDataType(int reqId, int marketDataType)
        {
            Console.WriteLine("MarketDataType. " + reqId + ", Type: " + marketDataType + "\n");
        }

        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            Console.WriteLine("UpdateMarketDepth. " + tickerId + " - Position: " + position + ", Operation: " +
                              operation + ", Side: " + side + ", Price: " + price + ", Size" + size + "\n");
        }

        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side,
            double price, int size)
        {
            Console.WriteLine("UpdateMarketDepthL2. " + tickerId + " - Position: " + position + ", Operation: " +
                              operation + ", Side: " + side + ", Price: " + price + ", Size" + size + "\n");
        }

        public virtual void updateNewsBulletin(int msgId, int msgType, string message, string origExchange)
        {
            Console.WriteLine("News Bulletins. " + msgId + " - Type: " + msgType + ", Message: " + message +
                              ", Exchange of Origin: " + origExchange + "\n");
        }

        public virtual void positionEnd()
        {
            Console.WriteLine("PositionEnd \n");
        }

        public virtual void scannerParameters(string xml)
        {
            Console.WriteLine("ScannerParameters. " + xml + "\n");
        }

        public virtual void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance,
            string benchmark, string projection, string legsStr)
        {
            Console.WriteLine("ScannerData. " + reqId + " - Rank: " + rank + ", Symbol: " +
                              contractDetails.Summary.Symbol + ", SecType: " + contractDetails.Summary.SecType +
                              ", Currency: " + contractDetails.Summary.Currency
                              + ", Distance: " + distance + ", Benchmark: " + benchmark + ", Projection: " + projection +
                              ", Legs String: " + legsStr + "\n");
        }

        public virtual void scannerDataEnd(int reqId)
        {
            Console.WriteLine("ScannerDataEnd. " + reqId + "\n");
        }

        public virtual void receiveFA(int faDataType, string faXmlData)
        {
            Console.WriteLine("Receing FA: " + faDataType + " - " + faXmlData + "\n");
        }

        public virtual void bondContractDetails(int requestId, ContractDetails contractDetails)
        {
            Console.WriteLine("Bond. Symbol " + contractDetails.Summary.Symbol + ", " + contractDetails.Summary);
        }

        public virtual void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            Console.WriteLine("Historical data end - " + reqId + " from " + startDate + " to " + endDate + " TIME:" +
                              DateTime.UtcNow.ToString("ss.ffffff"));
        }

        public virtual void verifyMessageAPI(string apiData)
        {
            Console.WriteLine("verifyMessageAPI: " + apiData);
        }

        public virtual void verifyCompleted(bool isSuccessful, string errorText)
        {
            Console.WriteLine("verifyCompleted. IsSuccessfule: " + isSuccessful + " - Error: " + errorText);
        }

        public virtual void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            Console.WriteLine("verifyAndAuthMessageAPI: " + apiData + " " + xyzChallenge);
        }

        public virtual void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            Console.WriteLine("verifyAndAuthCompleted. IsSuccessful: " + isSuccessful + " - Error: " + errorText);
        }

        public virtual void displayGroupList(int reqId, string groups)
        {
            Console.WriteLine("DisplayGroupList. Request: " + reqId + ", Groups" + groups);
        }

        public virtual void displayGroupUpdated(int reqId, string contractInfo)
        {
            Console.WriteLine("displayGroupUpdated. Request: " + reqId + ", ContractInfo: " + contractInfo);
        }

        #endregion

        public static string UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var userTime = TimeZoneInfo.ConvertTimeFromUtc(dtDateTime, easternZone);
            return userTime.ToString("hh:mm:ss.fff");
        }
        public void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue,
                                    double averageCost, double unrealisedPnl, double realisedPnl,
                                    string accountName)
        {
        }
        public void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice,
                                int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
        }
        public void position(string account, Contract contract, double pos, double avgCost)
        {
        }
        public void connectAck()
        {
        }
        public void positionMulti(int requestId, string account, string modelCode, Contract contract, double pos,
                                  double avgCost)
        {
        }
        public void positionMultiEnd(int requestId)
        {
        }
        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value,
                                       string currency)
        {
        }
        public void accountUpdateMultiEnd(int requestId)
        {
        }
        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId,
                                                      string tradingClass, string multiplier, HashSet<string> expirations,
                                                      HashSet<double> strikes)
        {
        }
        public void securityDefinitionOptionParameterEnd(int reqId)
        {
        }
        public void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
        }
    }

    public class Position
    {
        #region Properties

        public string Account { get; set; }
        public Contract Contract { get; set; }
        public int Quantity { get; set; }
        public double AverageCost { get; set; }

        #endregion
    }
}