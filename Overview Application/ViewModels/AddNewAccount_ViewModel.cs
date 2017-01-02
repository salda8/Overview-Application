using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using QDMS;
using ReactiveUI;


namespace OverviewApp.ViewModels
{
    public class AddNewAccountViewModel : ReactiveObject
    {
        private int port;
        private string ipAddress;
        private decimal initialBalance;
        private decimal currentBalance;
        private string brokerName;
        private string accountNumber;
        private string windowTitle;
        private ReactiveCommand<Unit, Unit> addNewAccountCommand;

        public AddNewAccountViewModel(Account account = null) 
        {
            if (account==null)
            {
                WindowTitle = "Edit Account";
            }
            else
            {
                WindowTitle = "Add new Account";
            }

           
        }

        public ReactiveCommand<Unit,Unit> AddNewAccountCommand
            => addNewAccountCommand ?? (addNewAccountCommand = ReactiveCommand.Create(AddNewAccount));
        


        private void AddNewAccount()
        {
            var account = new Account()
            {
                AccountNumber = this.AccountNumber,
                BrokerName = this.BrokerName,
                CurrentBalance = this.CurrentBalance,
                InitialBalance = this.InitialBalance,
                IpAddress = this.IpAddress,
                Port = this.Port


            };
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set { this.RaiseAndSetIfChanged(ref windowTitle, value); }
        }

        public int Port
        {
            get { return port; }
            set { this.RaiseAndSetIfChanged(ref port, value); }
        }

        public string IpAddress
        {
            get { return ipAddress; }
            set { this.RaiseAndSetIfChanged(ref ipAddress, value); }
        }

        public decimal InitialBalance
        {
            get { return initialBalance; }
            set {this.RaiseAndSetIfChanged(ref initialBalance, value); }
        }

        public decimal CurrentBalance
        {
            get { return currentBalance; }
            set { this.RaiseAndSetIfChanged(ref currentBalance, value); }
        }

        public string BrokerName
        {
            get { return brokerName; }
            set { this.RaiseAndSetIfChanged(ref brokerName, value); }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
            set { this.RaiseAndSetIfChanged(ref accountNumber, value); }
        }
    }
}
