namespace ApplyingFunctionalProgramming.Core
{
    public interface ICustomerRepository
    {
        Maybe<Customer> GetById(int id);

        Result Save(Customer customer);
    }
}
