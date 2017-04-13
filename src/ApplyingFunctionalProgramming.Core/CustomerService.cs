namespace ApplyingFunctionalProgramming.Core
{
    using System.Data.SqlClient;

    public class CustomerService
    {
        private readonly ICustomerRepository _repo;

        private readonly ILogger _logger;

        private readonly IPaymentGateway _paymentGateway;

        public CustomerService(ICustomerRepository repo, ILogger logger, IPaymentGateway paymentGateway)
        {
            _repo = repo;
            _logger = logger;
            _paymentGateway = paymentGateway;
        }

        // Traditional Approach: happy path
        //public string RefillBalance(int customerId, decimal moneyAmount)
        //{
        //    Customer customer = _repo.GetById(customerId);
        //    customer.Balance += moneyAmount;
        //    _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
        //    _repo.Save(customer);

        //    return "OK";
        //}

        // Traditional Approach: handling exceptions
        //public string RefillBalance(int customerId, decimal moneyAmount)
        //{
        //    if (!IsMoneyAmountValid(moneyAmount))
        //    {
        //        return "Money amount is invalid";
        //    }

        //    Customer customer = _repo.GetById(customerId);
        //    if (customer == null)
        //    {
        //        return "Customer is not found";
        //    }

        //    customer.Balance += moneyAmount;
        //    try
        //    {
        //        _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
        //    }
        //    catch (ChargeFailedException)
        //    {
        //        return "Unable to charge the credit card";
        //    }

        //    try
        //    {
        //        _repo.Save(customer);
        //    }
        //    catch (SqlException)
        //    {
        //        _paymentGateway.RollbackLastTransaction();
        //        return "Unable to connect to the database";
        //    }

        //    return "OK";
        //}

        // Railway oriented programming: https://vimeo.com/113707214
        public string RefillBalance(int customerId, decimal moneyAmount)
        {
            Result<MoneyToCharge> moneyToCharge = MoneyToCharge.Create(moneyAmount);
            Result<Customer> customer = _repo.GetById(customerId).ToResult("Customer is not found");

            return
                Result.Combine(moneyToCharge, customer)
                    .OnSuccess(() => customer.Value.AddBalance(moneyToCharge.Value))
                    .OnSuccess(() => _paymentGateway.ChargePayment(customer.Value.BillingInfo, moneyToCharge.Value))
                    .OnSuccess(
                        () => _repo.Save(customer.Value).OnFailure(() => _paymentGateway.RollbackLastTransaction()))
                    .OnBoth(result => Log(result))
                    .OnBoth(result => result.IsSuccess ? "OK" : result.Error);
        }

        private void Log(Result result)
        {
            this._logger.Log(result.IsFailure ? result.Error : "OK");
        }
    }
}
