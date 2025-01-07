# Youre-for-Unity-Desktop

> The YOURE.ID Sign In Component Unity Package provides a simple and convenient way for Unity developers to integrate YOURE sign-in functionality into their applications. With this package, users can quickly and easily sign in to YOURE and access their accounts without leaving the Unity environment.

### Supported Platforms: 
Windows

## Installation via Unity Package Manager

Installing a Unity Package via Git URL
You can install a Unity package via Git URL using the Package Manager. Here are the steps to follow:
1. Open your Unity project.
2. Open the Package Manager window by selecting Window > Package Manager from the Unity Editor menu.
3. Click the "+" button at the top left corner of the Package Manager window and select "Add package from git URL".
4. In the text field that appears, enter the Git URL: https://github.com/Youre-Group/Youre-for-Unity-Desktop.git
5. Click the Add button to begin the installation process.
6. Once the installation is complete, the package will be available in your project and you can start using it.

## Usage

```c#
public class SimpleAuthenticate : MonoBehaviour
{
    private void Start()
    {
        // YOURE Games will provide you with client id, endpoint url
        // The deeplink scheme has to be coordinated with YOURE.
        Youre.Init("ENTER YOUR CLIENT ID","https://ENTER YOUR ENDPOINT URL","ENTER_YOUR_DEEPLINK_SCHEME");
    
        Youre.Auth.SignInFailed += () =>
        {
            Debug.Log("SignIn failed");
        };

        Youre.Auth.SignInSucceeded += user =>
        {
            Debug.Log("Received YOURE.ID from callback: "+user.Id);
            Debug.Log("Received YOURE AccessToken from callback: "+user.AccessToken);
            Debug.Log("Received YOURE User Name from callback: "+user.UserName);
            Debug.Log("Received YOURE User Email from callback: "+user.Email);
        };
        
        AutoSignIn();
    }
    
    private async void AutoSignIn()
    {
      if(Youre.Auth.WasSignedIn())
      {
         YoureUser user = await Youre.Auth.GetActiveUser();
         if(user == null)
         {
            await Youre.Auth.SignIn();
         }
      }
    }
}
```

## Methods

### Youre.Auth.WasSignedIn()
```c#
// Will return TRUE if Youre.Id user was signed-in
bool wasSignedIn = Youre.Auth.WasSignedIn();
```


### Youre.Auth.GetActiveUser()
```c#
// Will return the YoureUser if user was signed-in and session is still valid
YoureUser user = await Youre.Auth.GetActiveUser();
```

### Youre.Auth.SignOut() 
```c#
bool isSignedOut = await Youre.Auth.SignOut();
```

## ISSUES
Due to compatibility we removed this Newtonsoft.Json.dll from package. Please add this to project manually if you have compiler issues.
- Newtonsoft.Json.dll

### License

Copyright © 2025, YOURE Games, The MIT License (MIT)
