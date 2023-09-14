

## 🔍 Tracking
Exponea SDK allows you to track events that occur while using the app and add properties of your customer. When SDK is first initialized, we generate a cookie for the customer that will be used for all the tracking. You can retrieve that cookie using `ExponeaSDK.GetCustomerCookie()`.

> If you need to reset the tracking and start fresh with a new user, you can use [Anonymize](./ANONYMIZE.md) functionality.

## 🔍 Tracking Events
> Some events are tracked automatically. We track installation event once for every customer, and when `AutomaticSessionTracking` is enabled in [ExponeaConfiguration](./CONFIG.md) we automatically track session events.

You can define any event types for each of your projects based on your business model or current goals. If you have a product e-commerce website, your essential customer journey will probably/most likely be:

* Visiting your App
* Searching for a specific product
* Product page
* Adding product to the cart
* Going through the ordering process
* Payment

So the possible events for tracking will be: `search`, `product view`, `add a product to cart`, `checkout`, `purchase`. Remember that you can define any event names you wish. However, our recommendation is to make them self-descriptive and human-understandable.


#### 💻 Usage

``` csharp
ExponeaSDK.TrackEvent(new Event("page_view") { ["thisIsAStringProperty"] = "thisIsAStringValue" }); 
```

## 🔍 Default Properties

It’s possible to set values in the [ExponeaConfiguration](../Documentation/CONFIG.md) to be sent in every tracking event. Once Exponea is configured, you can also change default properties calling `SetDefaultProperties`. Notice that those values will be overwritten if the tracking event has properties with the same key name. 

#### 💻 Usage 

``` csharp
ExponeaSDK.SetDefaultProperties(new Dictionary<string, object>()
{
    { "thisIsADefaultStringProperty", "This is a default string value" },
    { "thisIsADefaultIntProperty", 1},
    { "thisIsADefaultDoubleProperty", 12.53623}

});
```

## 🔍 Tracking Customer Properties

#### Identify Customer

Save or update your customer data in the Exponea APP through this method.


#### 💻 Usage 

``` csharp
ExponeaSDK.IdentifyCustomer(new Customer("donald@exponea.com") { ["name"] = "John" });
```
As result, the customer ID and properties are registered in your Exponea APP and you should see them in list `Data & Assets > Customers`
> Tracking event for `identifyCustomer` contains also default properties by default. If you want to disallow it, please set `AllowDefaultCustomerProperties` to FALSE. See docs in [Config](CONFIG.md) page
> If you define a default properties (see Default Properties), they are sent along with customer properties.
> In that case, please ensure that you allowed API to update a customer properties or you allowed to create a new properties in your Exponea APP (Project settings -> Access Management -> API -> Group permissions and check -> Customer properties)

### 🧳 Storing the events

When events are tracked and for any reason can not be sent to the server immediately (no connection, server maintenance, sending error), they stay persisted in a local database. In case of reasons like no connection or server downtime, they are never deleted, and SDK is trying to send them until the request is successful. In case send is unsuccessful for another reason, SDK counts sending tries, and if a number of tries exceed the maximum, the event is deleted. The maximum of tries is set to 10 by default, but you can set this value by setting the `MaxTries` field on the `Configuration` object when configuring the SDK.

## Manual session tracking
If you decide to opt out of our `AutomaticSessionTracking`, you can track sessions manually with `ExponeaSDK.TrackSessionStart()` and `ExponeaSDK.TrackSessionEnd()`.