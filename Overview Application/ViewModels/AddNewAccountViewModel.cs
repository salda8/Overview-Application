using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using DataStructures;
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
        private decimal? currentBalance;
        private string brokerName;
        private string accountNumber;
        private string windowTitle;
        
        private string addNewEditText;
       
        private Account account;
        private readonly bool addingNew;
        private readonly Account originalAccount;
        private ReactiveCommand<Unit, Unit> saveCommand;
        private string validationErrorsString;
        private bool? isValid;

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
                         Context.Accounts.Any(x => x.AccountNumber == this.AccountNumber);
                
                    return RuleResult.Assert(isAvailable,
                                             $"This account name {AccountNumber} is present. Please choose a different one or edit existing one");
                });

            Validator.AddRequiredRule(() => IpAddress, "IP Adress is required.");
            Validator.AddRule((string)(nameof(IpAddress)),
                () =>
                {
                    const string regexPattern =
                        @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}↵(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                    return RuleResult.Assert(Regex.IsMatch(IpAddress, regexPattern),
                        "Ip adress must be a valid.");
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
                CurrentBalance = this.currentBalance.GetValueOrDefault(),
                InitialBalance = this.initialBalance.GetValueOrDefault(),
                IpAddress = this.ipAddress,
                Port = this.port.Value

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

        public decimal? CurrentBalance
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
