## 🚀 Fetching Data

Bloomreach Maui SDK has some methods to retrieve your data from the Bloomreach web application.

#### Get customer recommendation

Get items recommended for a customer.

#### 💻 Usage

``` csharp
var recommendations = await Bloomreach.BloomreachSDK.FetchRecommendation(
    new CustomerRecommendationOptions(
        id: recommendationId.Text,
        fillWithRandom: true)
);
```

#### Consent Categories

Fetch the list of your existing consent categories.

#### 💻 Usage

``` csharp
var consents = await Bloomreach.BloomreachSDK.FetchConsents();
```