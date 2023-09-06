using Exponea;

namespace ExponeaTests;

public class ExponeaSdkMock : ExponeaSDK
{
    public ExponeaSdkMock(MethodInvokeCollector methodInvokeCollector)
    {
        Channel = new MethodChannelConsumer(methodInvokeCollector);
    }
}