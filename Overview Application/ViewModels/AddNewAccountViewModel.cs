using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using DataStructures;
using EntityData;
using GalaSoft.MvvmLight.CommandWpf;
using QDMS;
using ReactiveUI;


namespace OverviewApp.ViewModels
{
    public class AddNewAccountViewModel : MyBaseViewModel
    {
        
        private int port;
        private string ipAddress;
        private decimal initialBalance;
        private decimal currentBalance;
        private string brokerName;
        private string accountNumber;
        private string windowTitle;
        
        private string addNewEditText;
       
        private Account account;
        private readonly bool addingNew;
        private readonly Account originalAccount;
        private ReactiveCommand<Unit, Unit> saveCommand;

        public AddNewAccountViewModel(IMyDbContext context, ILogger logger,Account account = null) : base(context, logger)
        {
             
            originalAccount = account;

            if (account != null)
            {
                Account = account;
                WindowTitle = "Edit Account";
                AddNewEditText = "Save Changes";
                AccountNumber = originalAccount.AccountNumber;
                BrokerName = originalAccount.BrokerName;
                CurrentBalance = originalAccount.CurrentBalance;
                InitialBalance = originalAccount.InitialBalance;
                IpAddress = originalAccount.IpAddress;
                Port = originalAccount.Port;

            }
            else
            {
                WindowTitle = "Add new Account";
                AddNewEditText = "Add New";
                addingNew = true;

            }
           
        }

        public ReactiveCommand<Unit,Unit> SaveCommand => saveCommand ?? (saveCommand = ReactiveCommand.Create(AddNewAccount));
       

        private void AddNewAccount()
        {

            var acc = new Account()
            {
                AccountNumber = this.accountNumber,
                BrokerName = this.brokerName,
                CurrentBalance = this.currentBalance,
                InitialBalance = this.initialBalance,
                IpAddress = this.ipAddress,
                Port = this.port

            };
            if (addingNew)
            {
               
                Context.Accounts.Add(acc);
                
            }
            else
            {
                Context.Accounts.Attach(Account);
               
                Context.Entry(originalAccount).CurrentValues.SetValues(acc);
                
            }
            Context.SaveChanges();
        }

        public Account Account
        {
            get { return account; }
            set { this.RaiseAndSetIfChanged(ref account, value); }
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
            set { this.RaiseAndSetIfChanged(ref initialBalance, value); }
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

        public string AddNewEditText
        {
            get { return addNewEditText; }
            set { this.RaiseAndSetIfChanged(ref addNewEditText, value); }
        }
    }
}
