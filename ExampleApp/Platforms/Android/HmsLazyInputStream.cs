using Android.Content;
using Android.Util;
using Huawei.Agconnect.Config;

namespace ExampleApp;

public class HmsLazyInputStream : LazyInputStream
{
    public HmsLazyInputStream(Context context) : base(context)
    {
    }

    public override Stream Get(Context context)
    {
        Stream jsonStream;
        try
        {
            jsonStream = context.Assets?.Open("agconnect-services.json");
        }
        catch (Exception e)
        {
            Log.Error(e.ToString(), $"Can't open agconnect file:" + e.Message);
            jsonStream = null;
        }
        Console.WriteLine("HMS-BR agconnect found: " + (jsonStream != null));
        return jsonStream;
    }
}