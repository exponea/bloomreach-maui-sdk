namespace Bloomreach;

public class Payment
{
    public Payment(double value, string currency, string? system = null, string? productId = null, string? productTitle = null, string? receipt = null)
    {
        Value = value;
        Currency = currency;
        System = system;
        ProductId = productId;
        ProductTitle = productTitle;
        Receipt = receipt;
    }

    public double Value { get; set; }
    public string Currency { get; set; }
    public string? System { get; set; }
    public string? ProductId { get; set; }
    public string? ProductTitle { get; set; }
    public string? Receipt { get; set; }
}