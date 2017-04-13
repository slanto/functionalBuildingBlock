namespace ApplyingFunctionalProgramming.Core
{
    public interface IPaymentGateway
    {
        void RollbackLastTransaction();
        Result ChargePayment(string billingInfo, MoneyToCharge moneyToCharge);
    }
}
