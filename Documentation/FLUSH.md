

## 🔍 Flush events

Flushing is the process of uploading tracking events to Bloomreach servers.
All tracked events and customer properties are stored in the internal database of the Bloomreach SDK and later flushed based on flushing settings. When an event is successfully sent to Bloomreach API, the object will be deleted from the local database.

> By default, Bloomreach SDK automatically takes care of flushing events to the Bloomreach API. This feature can be turned off by setting the property FlushMode to MANUAL. Please be careful with turning automatic flushing off because if you turn it off, you need to manually call BloomreachSDK.FlushData() to flush the tracked events manually every time there is something to flush.

Bloomreach SDK will only flush data when the device has a stable internet connection. If a connection/server error occurs when flushing the data, it will keep the data stored until it can be flushed later.

You can configure the flushing mode to work differently to suit your needs.

#### 🔧 Flush Modes

 - Period

     * Periodic mode flushes data in your specified interval(60 minutes).
     * Flushing only happens while the app is running.
     * When the app enters the background, all remaining events are flushed.

 - AppClose
     * All events will be flushed once the applications enter the background.

 - Manual
     * Manual flushing mode disables any automatic upload, and it's your responsibility to flush data.

 - Immediate

     * DEFAULT VALUE 
     * Flushes all data immediately as it is received.

It's possible to change the flushing period by setting the property FlushPeriod on the Bloomreach object. The default value is 60 minutes. Due to platform/implementation limitations, the minimum value is 15 minutes.

The Bloomreach SDK Flush service will retry to flush events recorded in the database in case of a failure. If the maximum limit of retries was achieved, the SDK will delete the specific event from the database and not try to send it again. You can configure this value by setting the property maxTries in the Bloomreach Configuration.

#### 💻 Usage
```
 BloomreachSDK.Flush()
```
