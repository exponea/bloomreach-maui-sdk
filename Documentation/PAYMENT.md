## 🔍 Payments

Bloomreach SDK has a convenience method `Track` to help you track information about a payment for product/service within the application.
```
void Track(Payment);
```
To support multiple platforms and use-cases, SDK defines Map of values that contains basic information about the purchase.
#### 💻 Usage

```csharp
BloomreachSDK.Track(new Payment(
    12.34,
    "EUR",
    "CardHolder",
    "abcd1234",
    "Best product",
    "INV_12345"
));
```
