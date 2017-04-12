using Common;
using DataAccess;
using MvvmValidation;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.EntityModels;
using Common.Interfaces;

namespace OverviewApp.ViewModels
{
    public class AddNewAccountViewModel :ValidateableBaseViewModel
    {
        private int? port = 4001;
        private string ipAddress = "127.0.0.1";
        private decimal? initialBalance = 100000;

        private string brokerName = "Interactive Brokers";
        private string accountNumber;
        private string windowTitle;

        private string addNewEditText;

        private Account account;
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

            Account originalAccount = account;

            if (account != null)
            {
                AccountIDVisibility = true;
                AccountID = originalAccount.ID;
                Account = originalAccount;
                AccountNumber = originalAccount.AccountNumber;
                BrokerName = originalAccount.BrokerName;

                InitialBalance = originalAccount.InitialBalance;
                IpAddress = originalAccount.IpAddress;
                Port = originalAccount.Port;
                StrategyId = originalAccount.StrategyID;
                WindowTitle = "Edit Account";
                AddNewEditText = "Edit";
                IsAccountNumberEditable = false;
            }
            else
            {
                WindowTitle = "Add Account";
                AddNewEditText = "Add";
                IsAccountNumberEditable = true;
            }

            ConfigureValidationRules();
            Validator.ResultChanged += OnValidationResultChanged;
        }

        public bool AccountIDVisibility { get; set; } = false;

        public bool IsAccountNumberEditable { get; set; }

        #region Validation

        public string ValidationErrorsString
        {
            get { return validationErrorsString; }
            private set
            {
                this.RaiseAndSetIfChanged(ref validationErrorsString, value);
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
            Validator.AddRule(nameof(AccountNumber),
                 () =>
                 {
                     if (!IsAccountNumberEditable) return RuleResult.Valid();
                     bool isAvailable =
                          Context.Account.Any(x => x.AccountNumber == AccountNumber);

                     return RuleResult.Assert(!isAvailable,
                                              $"This account name {AccountNumber} is present. Please choose a different one or edit existing one");
                 });

            Validator.AddRequiredRule(() => IpAddress, "IP Address is required.");
            Validator.AddRule((string)(nameof(IpAddress)),
                () =>
                {
                    const string regexPattern =
                        @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
                    return RuleResult.Assert(Regex.IsMatch(IpAddress, regexPattern),
                        "Ip address must be a valid.");
                });

            Validator.AddRequiredRule(() => Port, "Port is required");
            Validator.AddRequiredRule(() => StrategyId, "Strategy is required");
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

        private void Validate() => UpdateValidationSummary(Validator.ValidateAllAsync().Result);

        #endregion Validation

        public ReactiveCommand<Unit, Unit> SaveCommand => saveCommand ?? (saveCommand =
            ReactiveCommand.Create(() =>

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
                AccountNumber = accountNumber,
                BrokerName = brokerName,
                InitialBalance = initialBalance.GetValueOrDefault(),
                IpAddress = ipAddress,
                Port = port ?? 0,
                StrategyID = StrategyId
            };

            if (AccountID == null)
            {
                //Context.Account.Add(acc);
                Context.AccountSummary.Add(new AccountSummary()
                {
                    Account=acc,
                    CashBalance = 0,
                    DayTradesRemaining = 0,
                    EquityWithLoanValue = 0,
                    InitMarginReq = 0,
                    MaintMarginReq = 0,
                    NetLiquidation = 0,
                    UnrealizedPnL = 0
                });
            }
            else
            {
                var accountEntity = Context.Account.Find(AccountID);
                Context.UpdateEntryValues(accountEntity, acc);
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

        public int? AccountID { get; }

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