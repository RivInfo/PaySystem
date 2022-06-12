using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        Order order = new Order(12, 200);

        Console.WriteLine(new PaymentSystem1().GetPayingLink(order));
        Console.WriteLine(new PaymentSystem2().GetPayingLink(order));
        Console.WriteLine(new PaymentSystem3().GetPayingLink(order));

        //Выведите платёжные ссылки для трёх разных систем платежа: 
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}
    }
}

public class Order
{
    public readonly int Id;
    public readonly int Amount;

    public Order(int id, int amount) => (Id, Amount) = (id, amount);
}

public interface IPaymentSystem
{
    public string GetPayingLink(Order order);
}

public class PaymentSystem1 : IPaymentSystem
{
    private readonly string _url = "pay.system1.ru/order?amount=12000RUB&hash=";

    public string GetPayingLink(Order order)
    {
        MD5 md5 = MD5.Create();

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(order.Id.ToString());
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        string outHesh = System.Text.Encoding.ASCII.GetString(hashBytes);

            return _url + outHesh;
    }
}

public class PaymentSystem2 : IPaymentSystem
{
    private readonly string _url = "order.system2.ru/pay?hash=";

    public string GetPayingLink(Order order)
    {
        MD5 md5 = MD5.Create();

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes((order.Amount + order.Id).ToString());
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        string outHesh = System.Text.Encoding.ASCII.GetString(hashBytes);

            return _url + outHesh;
    }
}

public class PaymentSystem3 : IPaymentSystem
{
    private readonly string _url = "system3.com/pay?amount=12000&curency=RUB&hash=";

    private readonly string _secretKey = "ewq";

    public string GetPayingLink(Order order)
    {
        SHA1 md5 = SHA1.Create();

        byte[] inputBytes = System.Text.Encoding.ASCII
            .GetBytes((order.Amount + order.Id).ToString() + _secretKey);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        string outHesh = System.Text.Encoding.ASCII.GetString(hashBytes);

        return _url + outHesh;
    }
}