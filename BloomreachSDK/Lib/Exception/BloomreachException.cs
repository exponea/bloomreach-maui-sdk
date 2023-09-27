namespace Bloomreach;

public class BloomreachException: Exception
{
    private BloomreachException(string message): base(message) { }

    public static BloomreachException Common(string message)
    {
        return new BloomreachException(message);
    }
}