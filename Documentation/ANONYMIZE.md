
## 🕵 Anonymize

Anonymize is a feature that allows you to switch users. A typical use-case is user login/logout.

Anonymize will delete all stored information and reset the current customer. The new customer will be generated, install and session start events tracked. In addition, push notification token from the old user will be wiped and tracked for the new user to make sure the device won't get duplicate push notifications.

#### 💻 Usage

``` csharp
 ExponeaSDK.Anonymize();
```

### Project settings switch
SDK also allows you to switch to a different project, keeping the benefits described above. The new user will have the same events as if the app was installed on a new device.

#### 💻 Usage

``` csharp
 ExponeaSDK.Anonymize(
    new Project(
        "project-token",
        "your-auth-token",
        "https://api.exponea.com"
    )
);
```