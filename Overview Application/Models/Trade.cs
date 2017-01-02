using System;
using System.Threading;
using IBApi;

namespace OverviewApp.Models
{
    public class Trade
    {
        /// <summary>
        ///     Trades the specified contract.
        /// </summary>
        /// <param name="cntract">The contract.</param>
        /// <param name="i">The i.</param>
        /// <param name="wrapper"></param>
        public static void PlaceMarketTrade(string cntract, double i, IbClient wrapper)
        {
            //var contract = Context.GetContract(cntract);
            //if (i >= 1)
            //{
            //    MakeMktTrade(contract, "BUY", "MKT", Convert.ToInt32(i), wrapper);
            //}
            //else if (i <= -1)
            //{
            //    MakeMktTrade(contract, "SELL", "MKT", Convert.ToInt32(i), wrapper);
            //}
        }

        /// <summary>
        ///     Makes the MKT trade.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="wrapper">The wrapper.</param>
        private static void MakeMktTrade(Contract contract, string direction, string type, int quantity,
            IbClient wrapper)
        {
            var order = new Order
            {
                Action = direction,
                OrderType = type,
                Account = wrapper.AccountNumber,
                TotalQuantity = Math.Abs(quantity),
                OrderId = wrapper.NextOrderId++,
                Tif = "GTC"
            };

            wrapper.ClientSocket.placeOrder(order.OrderId, contract, order);
            //Thread.Sleep(5000);
        }

        /// <summary>
        ///     Places the limit trade.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        /// <param name="wrapper">The wrapper.</param>
        /// <param name="type">The type.</param>
        /// <param name="ocagroup">The ocagroup.</param>
        public static void PlaceLimitTrade(Contract contract, double quantity, decimal price, IbClient wrapper,
            string type, string ocagroup)
        {
            if (quantity >= 1)
            {
                MakeLmtTrade(contract, "BUY", type, Convert.ToInt32(quantity), price, wrapper, ocagroup);
            }
            else if (quantity <= -1)
            {
                MakeLmtTrade(contract, "SELL", type, Convert.ToInt32(quantity), price, wrapper, ocagroup);
            }
        }

        /// <summary>
        ///     Makes the LMT trade.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <param name="toInt32">To int32.</param>
        /// <param name="price">The price.</param>
        /// <param name="wrapper">The wrapper.</param>
        /// <param name="ocagroup">The ocagroup.</param>
        private static void MakeLmtTrade(Contract contract, string direction, string type, int toInt32, decimal price,
            IbClient wrapper, string ocagroup)
        {
            if (type == "LMT")
            {
                var order = new Order
                {
                    Action = direction,
                    OrderType = type,
                    LmtPrice = (double) price,
                    Account = wrapper.AccountNumber,
                    TotalQuantity = Math.Abs(toInt32),
                    OrderId = wrapper.NextOrderId++,
                    OcaType = 1,
                    OcaGroup = ocagroup,
                    Tif = "GTC"
                };

                wrapper.ClientSocket.placeOrder(order.OrderId, contract, order);
                Thread.Sleep(1);
            }
            else
            {
                var order = new Order
                {
                    Action = direction,
                    OrderType = type,
                    AuxPrice = (double) price,
                    //LmtPrice = (double)price,

                    Account = wrapper.AccountNumber,
                    TotalQuantity = Math.Abs(toInt32),
                    OrderId = wrapper.NextOrderId++,
                    OcaType = 1,
                    OcaGroup = ocagroup,
                    Tif = "GTC"
                };
                wrapper.ClientSocket.placeOrder(order.OrderId, contract, order);
            }

            //wrapper.ClientSocket.placeOrder(order.OrderId, contract, order);
        }
    }
}