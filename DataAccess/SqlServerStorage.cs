//using System;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Data.SqlClient;
//using System.Timers;
//using DataStructures;
//using IBApi;

//#pragma warning disable 67

//namespace DataAccess
//{
//    public class SqlServerStorage : IDataService
//    {
//        #region Fields

//        /// <summary>
//        ///     Periodically updates the Connected property.
//        /// </summary>
//        private readonly Timer connectionStatusUpdateTimer;

//        #endregion

//        #region

//        //private Logger _logger = LogManager.GetCurrentClassLogger();

//        public SqlServerStorage()
//        {
//            Name = "Local Storage";
//            connectionStatusUpdateTimer = new Timer(1000);
//            connectionStatusUpdateTimer.Elapsed += _connectionStatusUpdateTimer_Elapsed;
//            connectionStatusUpdateTimer.Start();
//        }

//        #endregion

//        #region Properties

//        /// <summary>
//        ///     Whether the connection to the data source is up or not.
//        /// </summary>
//        public bool Connected { get; set; }

//        /// <summary>
//        ///     The name of the data source.
//        /// </summary>
//        public string Name { get; private set; }

//        #endregion

//        #region

//        /// <summary>
//        ///     Gets the portfolio summary.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<PortfolioSummary> GetPortfolioSummary()
//        {
//            var list = new ObservableCollection<PortfolioSummary>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM PortfolioSummary";
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new PortfolioSummary
//                            {
//                                Account = reader.GetString(1),
//                                StrategyName = reader.GetString(2),
//                                NetLiquidation = reader.GetDouble(3),
//                                LivePosition = reader.GetBoolean(4),
//                                OpenPnl = reader.GetDouble(5),
//                                StartEquity = reader.GetDouble(6),
//                                StartDate = reader.GetDateTime(7),
//                                Profit = reader.GetDouble(8),
//                                ProfitPercent = reader.GetDouble(9),
//                                DaysRunning = reader.GetInt32(10),
//                                DailyPercent = reader.GetDouble(11),
//                                GatewayPort = reader.GetInt32(12)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the portfolio summary.
//        /// </summary>
//        /// <param name="acc">The acc.</param>
//        /// <returns></returns>
//        public ObservableCollection<PortfolioSummary> GetPortfolioSummary(string acc)
//        {
//            var list = new ObservableCollection<PortfolioSummary>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM PortfolioSummary WHERE Account=?acc";
//                    cmd.Parameters.AddWithValue("?acc", acc);
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new PortfolioSummary
//                            {
//                                Account = reader.GetString(1),
//                                StrategyName = reader.GetString(2),
//                                NetLiquidation = reader.GetDouble(3),
//                                LivePosition = reader.GetBoolean(4),
//                                OpenPnl = reader.GetDouble(5),
//                                StartEquity = reader.GetDouble(6),
//                                StartDate = reader.GetDateTime(7),
//                                Profit = reader.GetDouble(8),
//                                ProfitPercent = reader.GetDouble(9),
//                                DaysRunning = reader.GetInt32(10),
//                                DailyPercent = reader.GetDouble(11),
//                                GatewayPort = reader.GetInt32(12)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the live trades.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<LiveTrade> GetLiveTrades()
//        {
//            var list = new ObservableCollection<LiveTrade>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM Portfolio";
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new LiveTrade
//                            {
//                                Symbol = reader.GetString(1),
//                                Position = reader.GetDouble(2),
//                                MarketPrice = reader.GetFloat(3),
//                                MarketValue = reader.GetFloat(4),
//                                AverageCost = reader.GetFloat(5),
//                                UnrealizedPnL = reader.GetFloat(6),
//                                RealizedPnl = reader.GetFloat(7),
//                                Account = reader.GetString(8),
//                                UpdateTime = reader.GetDateTime(9)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the live trades.
//        /// </summary>
//        /// <param name="acc">The acc.</param>
//        /// <returns></returns>
//        public ObservableCollection<LiveTrade> GetLiveTrades(string acc)
//        {
//            var list = new ObservableCollection<LiveTrade>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM Portfolio WHERE Account=?acc";
//                    cmd.Parameters.AddWithValue("?acc", acc);
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new LiveTrade
//                            {
//                                Symbol = reader.GetString(1),
//                                Position = reader.GetDouble(2),
//                                MarketPrice = reader.GetFloat(3),
//                                MarketValue = reader.GetFloat(4),
//                                AverageCost = reader.GetFloat(5),
//                                UnrealizedPnL = reader.GetFloat(6),
//                                RealizedPnl = reader.GetFloat(7),
//                                Account = reader.GetString(8),
//                                UpdateTime = reader.GetDateTime(9),
//                                Port = reader.GetInt16(12)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the open orders.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<OpenOrder> GetOpenOrders()
//        {
//            var list = new ObservableCollection<OpenOrder>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM openorders LIMIT 5";
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new OpenOrder
//                            {
//                                PermId = reader.GetInt32(1),
//                                Symbol = reader.GetString(2),
//                                Position = reader.GetDouble(5),
//                                Status = reader.GetString(3),
//                                LimitPrice = reader.GetFloat(4),
//                                Account = reader.GetString(6),
//                                Type = reader.GetString(9),
//                                UpdateTime = reader.GetDateTime(8)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the open orders.
//        /// </summary>
//        /// <param name="acc">The acc.</param>
//        /// <returns></returns>
//        public ObservableCollection<OpenOrder> GetOpenOrders(string acc)
//        {
//            var list = new ObservableCollection<OpenOrder>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM openorders WHERE Account=?acc";
//                    cmd.Parameters.AddWithValue("?acc", acc);

//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new OpenOrder
//                            {
//                                PermId = reader.GetInt32(1),
//                                Symbol = reader.GetString(2),
//                                Position = reader.GetDouble(5),
//                                Status = reader.GetString(3),
//                                LimitPrice = reader.GetFloat(4),
//                                Account = reader.GetString(6),
//                                UpdateTime = reader.GetDateTime(7)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the equity.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<Equity> GetEquity(int id = 0)
//        {
//            var list = new ObservableCollection<Equity>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM equity WHERE idEquity>?id";
//                    cmd.Parameters.AddWithValue("?id", id);
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new Equity
//                            {
//                                Account = reader.GetString(3),
//                                UpdateTime = reader.GetDateTime(2),
//                                Value = reader.GetDouble(1),
//                                Id = reader.GetInt16(0)
//                            });
//                            //+ " |Profit(%):" + reader.GetString(9) + " |Avg.DailyProfit(%):" +
//                            //reader.GetString(11) + " | Start Equity:" + reader.GetString(6) + " |StartDate:" +
//                            //reader.GetString(7) + " |Days Running:" + reader.GetString(10));
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the bars.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<Candlestick> GetBars(int id = 0)
//        {
//            var data = new ObservableCollection<Candlestick>();
//            using (var con = Db.OpenConnection())
//            {
//                var cmd = con.CreateCommand();
//                cmd.CommandText =
//                    "SELECT close,high,low,open, bartime, symbol, dbtime, timeframe,volume, idMinutebars FROM minutebars WHERE idMinuteBars>?id";
//                //FORMAT(close,6),FORMAT(high,6),FORMAT(low,6),FORMAT(close,6),
//                //cmd.Parameters.AddWithValue("?tickerid", tickerid);
//                //cmd.Parameters.AddWithValue("?limit", limit);
//                //cmd.Parameters.AddWithValue("?tf", tf);
//                cmd.Parameters.AddWithValue("?id", id);
//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        data.Add(new Candlestick
//                        {
//                            Id = reader.GetInt32(9),
//                            Open = reader.GetDouble(3),
//                            High = reader.GetDouble(1),
//                            Low = reader.GetDouble(2),
//                            Close = reader.GetDouble(0),
//                            BarTime = reader.GetDateTime(4),
//                            Symbol = reader.GetString(5),
//                            Interval = reader.GetInt32(7),
//                            Volume = reader.GetInt32(8),
//                            DbTime = reader.GetDateTime(6)
//                        });
//                    }
//                }
//            }
//            return data;
//        }

//        /// <summary>
//        ///     Gets the matlabvalues.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<Matlabvalue> GetMatlabvalues()
//        {
//            var list = new ObservableCollection<Matlabvalue>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM matlabvalues";

//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new Matlabvalue
//                            {
//                                Time = reader.GetDateTime(2),
//                                Symbol = reader.GetString(1),
//                                Value = reader.GetDouble(3)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the trade history.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<TradeHistory> GetTradeHistory(int id = 0)
//        {
//            var list = new ObservableCollection<TradeHistory>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM historicaltrades WHERE idHistoricalTrade>?id";
//                    cmd.Parameters.AddWithValue("?id", id);

//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new TradeHistory
//                            {
//                                ExecId = reader.GetString(10),
//                                Symbol = reader.GetString(2),
//                                Position = reader.GetDouble(3),
//                                Price = reader.GetFloat(5),
//                                Account = reader.GetString(1),
//                                DbTime = reader.GetDateTime(9),
//                                ExecTime = reader.GetDateTime(8),
//                                Commission = reader.GetDouble(7),
//                                RealizedPnL = reader.GetFloat(6),
//                                Side = reader.GetString(4),
//                                Id = reader.GetInt16(0)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the trade history.
//        /// </summary>
//        /// <param name="acc">The acc.</param>
//        /// <returns></returns>
//        public ObservableCollection<TradeHistory> GetTradeHistory(string acc)
//        {
//            var list = new ObservableCollection<TradeHistory>();
//            using (var con = Db.OpenConnection())
//            {
//                using (var cmd = con.CreateCommand())
//                {
//                    cmd.CommandText =
//                        "SELECT * FROM historicaltrades WHERE Account=?acc";
//                    cmd.Parameters.AddWithValue("?acc", acc);

//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            list.Add(new TradeHistory
//                            {
//                                ExecId = reader.GetString(10),
//                                Symbol = reader.GetString(2),
//                                Position = reader.GetDouble(3),
//                                Price = reader.GetFloat(5),
//                                Account = reader.GetString(1),
//                                DbTime = reader.GetDateTime(9),
//                                ExecTime = reader.GetDateTime(8),
//                                Commission = reader.GetDouble(7),
//                                RealizedPnL = reader.GetFloat(6),
//                                Side = reader.GetString(4)
//                            });
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        ///     Gets the strategy.
//        /// </summary>
//        /// <returns></returns>
//        public ObservableCollection<Strategy> GetStrategy()
//        {
//            var list = new ObservableCollection<Strategy>();
//            using (var con = Db.OpenConnection())
//            {
//                var cmd = con.CreateCommand();
//                cmd.CommandText =
//                    "SELECT * FROM Strategy";

//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        list.Add(new Strategy
//                        {
//                            StrategyName = reader.GetString(1),
//                            Calmari = reader.GetDouble(2),
//                            BacktestProfit = reader.GetDouble(3),
//                            BacktestDrawDown = reader.GetDouble(4),
//                            BacktestPeriod = reader.GetDouble(5),
//                            Symbols = reader.GetString(6),
//                            Filepath = reader.GetString(8),
//                            DailyProfit = reader.GetDouble(9),
//                            DaysRunning = reader.GetDouble(10),
//                            Profit = reader.GetDouble(11),
//                            OpenPnL = reader.GetDouble(12)
//                        });
//                        //+ " |Profit(%):" + reader.GetString(9) + " |Avg.DailyProfit(%):" +
//                        //reader.GetString(11) + " | Start Equity:" + reader.GetString(6) + " |StartDate:" +
//                        //reader.GetString(7) + " |Days Running:" + reader.GetString(10));
//                    }
//                }
//            }
//            return list;
//        }

//        public void ReqGlobalCancel(string acc)
//        {
//        }

//        #endregion

//        private void _connectionStatusUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
//        {
//            using (
//                var connection = DbUtils.CreateSqlServerConnection("qdmsdata"))

//            //useWindowsAuthentication: Settings.Default.SqlServerUseWindowsAuthentication))
//            {
//                try
//                {
//                    connection.Open();
//                    Connected = connection.State == ConnectionState.Open;
//                }
//                catch
//                {
//                    Connected = false;
//                }
//            }
//        }

//        /// <summary>
//        ///     Connect to the data source.
//        /// </summary>
//        public void Connect()
//        {
//        }

//        /// <summary>
//        ///     Disconnect from the data source.
//        /// </summary>
//        public void Disconnect()
//        {
//        }

//        private bool TryConnect(out SqlConnection connection)
//        {
//            connection = DbUtils.CreateSqlServerConnection("qdmsdata",
//                useWindowsAuthentication: true);//Settings.Default.SqlServerUseWindowsAuthentication);
//            try
//            {
//                connection.Open();
//            }
//            catch (Exception ex)
//            {
//                //Log(LogLevel.Error, "Local storage: DB connection failed with error: " + ex.Message);
//                return false;
//            }

//            return connection.State == ConnectionState.Open;
//        }

//        public static Contract GetContract(string cntract)
//        {
//            var contract = new Contract();
//            using (var con = Db.OpenConnection())
//            {
//                var cmd = con.CreateCommand();
//                cmd.CommandText =
//                    "SELECT * FROM contract WHERE symbol = ?symbol AND Rollover_Date>?date ORDER BY contract_id ASC LIMIT 1";
//                cmd.Parameters.AddWithValue("?symbol", cntract);
//                cmd.Parameters.AddWithValue("?date", DateTime.UtcNow.Date);

//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var ticker = reader.GetString(6);
//                        var productcode = ticker[1].ToString() + ticker[0] + ticker[2] + ticker[3];

//                        contract.Symbol = reader.GetString(1);
//                        contract.SecType = "FUT";
//                        contract.Currency = "USD";
//                        contract.Exchange = reader.GetString(4);
//                        contract.LocalSymbol = productcode;
//                    }
//                }
//            }
//            return contract;
//        }
//    }
//}

//#pragma warning restore 67