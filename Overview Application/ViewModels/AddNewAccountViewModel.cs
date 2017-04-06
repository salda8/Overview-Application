using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using EntityData;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmValidation;
using QDMS;
using ReactiveUI;


namespace OverviewApp.ViewModels
{
    public class AddNewAccountViewModel : MyValidatableBaseViewModel
    {
        
        private int? port;
        private string ipAddress;
        private decimal? initialBalance;
        
        private string brokerName;
        private string accountNumber;
        private string windowTitle;
        
        private string addNewEditText;
       
        private Account account;
        private readonly Account originalAccount;
        private ReactiveCommand<Unit, Unit> saveCommand;
        private string validationErrorsString;
        private bool? isValid;
        private int strategyId;
        private ReactiveList<Strategy> strategies;
        private ReactiveList<Account> accounts;

        public AddNewAccountViewModel(IMyDbContext context, Account account) : base(context)
        {

            Accounts = new ReactiveList<Account>(context.Account.ToList());
            Strategies = new ReactiveList<Strategy>(context.Strategy.ToList());

            originalAccount = account;



            if (account != null)
            {
                AccountId = originalAccount.ID;
                Account = originalAccount;
                AccountNumber = originalAccount.AccountNumber;
                BrokerName = originalAccount.BrokerName;

                InitialBalance = originalAccount.InitialBalance;
                IpAddress = originalAccount.IpAddress;
                Port = originalAccount.Port;
                StrategyId = originalAccount.StrategyID;
                WindowTitle = "Edit Account";
                AddNewEditText = "Edit";

            }
            else
            {
                
                WindowTitle = "Add Account";
                AddNewEditText = "Add";

            }
            

            ConfigureValidationRules();
            Validator.ResultChanged += OnValidationResultChanged;

        }

        #region Validation
        public string ValidationErrorsString
        {
            get { return validationErrorsString; }
            private set
            {
               
                this.RaiseAndSetIfChanged(ref validationErrorsString,value);
            }
        }

        public bool? IsValid
        {
            get { return isValid; }
            private set { this.RaiseAndSetIfChanged(ref isValid, value); }
        }


        private void ConfigureValidationRules()
        {
            Validator.AddRequiredRule(() => AccountNumber, "Account Name is required");
            Validator.AddRule((string)(nameof(AccountNumber)),
                 () =>
                {
                    bool isAvailable =
                         Context.Account.Any(x => x.AccountNumber == this.AccountNumber);
                
                    return RuleResult.Assert(isAvailable,
                                             $"This account name {AccountNumber} is present. Please choose a different one or edit existing one");
                });

            Validator.AddRequiredRule(() => IpAddress, "IP Address is required.");
            Validator.AddRule((string)(nameof(IpAddress)),
                () =>
                {
                    const string regexPattern =
                        @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}↵(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                    return RuleResult.Assert(Regex.IsMatch(IpAddress, regexPattern),
                        "Ip address must be a valid.");
                });

            Validator.AddRequiredRule(() => Port, "Port is required");

         


        }
        private void OnValidationResultChanged(object sender, ValidationResultChangedEventArgs e)
        {
            if (!IsValid.GetValueOrDefault(true))
            {
                ValidationResult validationResult = Validator.GetResult();

                UpdateValidationSummary(validationResult);
            }
        }
        private void UpdateValidationSummary(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ValidationErrorsString = validationResult.ToString();
        }

        private async void Validate()
        {
            await ValidateAsync();
        }

        private async Task ValidateAsync()
        {
            var result = await Validator.ValidateAllAsync();

            UpdateValidationSummary(result);
        }
        #endregion
        public ReactiveCommand<Unit,Unit> SaveCommand => saveCommand ?? (saveCommand = 
            ReactiveCommand.Create(()=>

                {
                    Validate();

                    if (IsValid.GetValueOrDefault(false))
                    {
                        AddNewAccount();
                    }
                   
                }
        ));
       

        private void AddNewAccount()
        {

            var acc = new Account()
            {
                AccountNumber = this.accountNumber,
                BrokerName = this.brokerName,
              
                InitialBalance = this.initialBalance.GetValueOrDefault(),
                IpAddress = this.ipAddress,
                Port = port ?? 0,
                StrategyID = StrategyId

            };

            if (AccountId==null)
            {
               
                Context.Account.Add(acc);
                
            }
            else
            {
                acc.ID = AccountId.Value;
                Context.Account.Attach(Account);
                Context.Entry(originalAccount).CurrentValues.SetValues(acc);
                
            }
            Context.SaveChanges();
        }

        public int StrategyId
        {
            get { return strategyId; }
            set { this.RaiseAndSetIfChanged(ref strategyId, value); }
        }

        public ReactiveList<Strategy> Strategies
        {
            get { return strategies; }
            set
            {
                this.RaiseAndSetIfChanged(ref strategies, value);
                
            }
        }

        public ReactiveList<Account> Accounts
        {
            get { return accounts; }
            set { this.RaiseAndSetIfChanged(ref accounts, value); }
         }

        public int? AccountId { get; }

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

        public int? Port
        {
            get { return port; }
            set
            {
                this.RaiseAndSetIfChanged(ref port, value);
                Validator.Validate((nameof(Port)));
            }
        }

        public string IpAddress
        {
            get { return ipAddress; }
            set
            {
                this.RaiseAndSetIfChanged(ref ipAddress, value);
                Validator.Validate((nameof(IpAddress)));
            }
        }

        public decimal? InitialBalance
        {
            get { return initialBalance; }
            set { this.RaiseAndSetIfChanged(ref initialBalance, value); }
        }
        
        public string BrokerName
        {
            get { return brokerName; }
            set { this.RaiseAndSetIfChanged(ref brokerName, value); }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
            set
            {
                this.RaiseAndSetIfChanged(ref accountNumber, value);
                Validator.Validate((nameof(AccountNumber)));
            }
        }

        public string AddNewEditText
        {
            get { return addNewEditText; }
            set { this.RaiseAndSetIfChanged(ref addNewEditText, value); }
        }
    }
}
