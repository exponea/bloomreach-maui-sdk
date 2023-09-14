using Bloomreach;

namespace BloomreachTests;

public class BloomreachSdkMock : BloomreachSDK
{
    public BloomreachSdkMock(MethodInvokeCollector methodInvokeCollector)
    {
        Channel = new MethodChannelConsumer(methodInvokeCollector);
    }
}