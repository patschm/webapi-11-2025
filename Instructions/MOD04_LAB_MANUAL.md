# Module 04: Securing services

# Lab: Using ASP.NET Core Identity with Desktop Client

When securing WebApi Rest services it is best to use OAuth2(.1). 

In Exercise 1 we are going to use a desktop client to use the service and according to the OAuth2 specifications we should use **Authorization Code Grant Flow**. We need an Authentication/Authorization service too. Since that is a complicated and risky endeavor we are going to use Azure Active Directory (App Registration). Other options are Google, Facebook and many more. Whatever your choice, your application needs information about these services, like Client(application)ID and uri's to these services (often provided in metadata endpoints). Many client libraries are available but in this scenario **MSAL** is a good choice

In Exercise 2 we'll do the same thing but now from an SPA client. Since this is a JavaScript client, running in a browser, OAuth2 tells us to use **Implicit Grant Flow**. However with the plans for third party cookies to be removed from browsers, the implicit grant flow is no longer a suitable authentication method. The silent SSO features of the implicit flow do not work without third party cookies, causing applications to break when they attempt to get a new token. Therefore the new recommendation is **Authorization Code Grant Flow**. (like for Desktop Applications). Many client side libraries are available like **MSAL**. But in this case we'll try **oauth2-oidc** (the exercises contain a project using msal if you want to know how that's done)

### Exercise 1: Desktop application with OAuth2 Code grant

#### Task 1: Add ASP.NET Core Identity NuGet

1. Open command prompt window and then browse to **[Drive:]\Mod04\Exercise 1\Starter\ProductReviews.API**.
2. Add package **Microsoft.Identity.Web** to the project. 

#### Task 2: Register ASP.NET Core Identity in the startup file

1.  To configure **ASP.NET Core Identity**, in **Program.cs**  register the following services:
    ```cs
    builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
    ```
    
2. In the request pipeline add the middleware

   - **UseAuthentication()**
   - **UseAuthorization()**

3. ***<u>This registration part is optional. Do this only if you have your own Azure subscription</u>***. 

   - In Azure AD create a new **App Registration**.
   - Select **Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)**
   - Use **Public client/native (Mobile and desktop)** as platform
   - Add **http://localhost** as **Redirect URI**
   - In **Expose an Api** add a scope
     - Scope name: Data.Read
     - Who can consent: Admins and users
     - Admin Consent display name: Read data
     - Admin consent description: Users are allowed to read data from Api

   - In **App roles** create a **Writers Role** role for **Users/groups** and value **Writers** and Description "Can write data".
   - In **Enterprise registrations** find the application registration and assign users to to the writers role you just created.

4. To the **appsettings.json** file, paste the following configuration (if you have your own subscription use the application (client) ID and domain from the app registration from previous exercise):
   ```json
   "AzureAd": {
       "Instance": "https://login.microsoftonline.com/",
       "Domain": "4dotnettraining.onmicrosoft.com",
       "TenantId": "common",
       "ClientId": "79221685-706a-42de-bb1a-b6c095e034a4"
     }
   ```

#### Task 3: Secure the ProductGroupController 
***<u>If you don't use your own subscription ask the instructor to be assigned to the Writers group. He most likely forgot to do that in advance :)</u>***
1. Open **ProductGroupController.cs**

2. Decorate the controller class with the **Authorize** attribute.

3. Decorate both HttpGet actions with

   ```csharp
   RequiredScope("Data.Read")]
   ```

4. Decorate the other actions with

   ```csharp
   [Authorize(Roles = "Writers")]
   ```

5. The service is now secured. Test that you cannot access the product groups. (https://localhost:5001/productgroup)

#### Task 4: Modify the Api proxies to accept access tokens

The Api Proxies are responsible for communicating with the service. They're wrapping the **HttpClient** class which is created in the **CreateHttpClient** method. We need to provide the access token in the request. That's typically done in the **Authorization** header (Bearer).

1. Open the project **ProductReviews.Client.Proxies** in Visual Studio Code

2. In **BaseProxy.cs** find the method **CreateHttpClient()**

3. Right under **_clients.Add(BaseAddress,  client);** add the following code and complete it

   ```csharp
    if (!string.IsNullOrEmpty(AccessToken))
    {
    	// TODO: Make sure the AccessToken is used in requests (client.http. ....)
    }
   ```

4. Save and build the library.

#### Task 5. Use the Msal library in ProductReviews.Client.Wpf

1. Open a command prompt and navigate to **ProductReviews.Client.Wpf** and add the package **Microsoft.Identity.Client** to the project.

2. Open the project **ProductReviews.Client.Wpf** in Visual Studio Code

3. Open the **appsettings.json** file and add the following code (use the values from your own app registration if applicable)

   ```json
   "AzureAd": {
           "Instance": "https://login.microsoftonline.com/",
           "Domain": "4dotnettraining.onmicrosoft.com",
           "TenantId": "common",
           "ClientId": "79221685-706a-42de-bb1a-b6c095e034a4",
           "RedirectUri": "http://localhost"
       }
   ```

4. Open **ApiAccessScopes.cs** and review the content. Change the values with your own scopes if you created your own app registration

5. Open **AzureAdClient.cs**

6. In the  **Initialize()** method add the following code

   ```csharp
    if (_app == null)
    {
        _app = PublicClientApplicationBuilder
        .Create(_aad.ClientId)
        .WithAuthority(_aad.Authority)
        .WithRedirectUri(_aad.RedirectUri)
        .Build();
    }
   ```

7. In **GetAccessTokenForAsync()** add the following code after the **Initialize()** call

   ```c#
    var accounts = await _app!.GetAccountsAsync();
    _account = accounts.FirstOrDefault();
    AuthenticationResult  token;
    try
    {
    	token = await _app.AcquireTokenSilent(scopes, _account).ExecuteAsync();
    }
    catch (MsalUiRequiredException)
    {
    	token = await _app.AcquireTokenInteractive(scopes).ExecuteAsync();
    }
    return token.AccessToken;
   ```

8. In **ProductGroupOverviewViewModel.cs** find the method **LoadProductGroupsAsync** and add the following code

   ```c#
    var scopes = new string[]{ ApiAccessScopes.Read};
               
   try
   {
       var token = await _adClient.GetAccessTokenForAsync(scopes);
       var result = await _proxy.WithBearer(token).GetAsync(Page, count);
       if (result != null)
       {
           _productGroups.Clear();
           result.ForEach(pg=>_productGroups.Add(pg));
       }
   }
   catch(Exception e)
   {
       MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
   }
   ```

9. Test the application	

​	

### Exercise 2: Web Application (SPA) with OAuth2 Code grant

#### Task 1: Add an app Registration

1. *<u>**This registration part is optional. Do this only if you have your own Azure subscription**</u>*

    - In Azure AD create a new **App Registration**.
    - Select **Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)**
    - Use **Single-page application (SPA)** as platform
    - Add **http://localhost/index.html** as **Redirect URI**
    - In **Expose an Api** add a scope
        - Scope name: Data.Read
        - Who can consent: Admins and users
        - Admin Consent display name: Read data
        - Admin consent description: Users are allowed to read data from Api

    - In **App roles** create a **Writers Role** role for **Users/groups** and value **Writers** and Description "Can write data".
    - In **Enterprise registrations** find the application registration and assign users to to the writers role you just created.

2. Open command prompt window and then browse to **[Drive:]\Mod04\Exercise 2\Starter\ProductReviews.API**.

3. Install angular

    ```npm
    npm i -g @angular/cli@17.3.3
    ```

4. Add the following entry in **appsettings.json**:

    ```json
     "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "Domain": "4dotnettraining.onmicrosoft.com",
        "TenantId": "common",
        "ClientId": "2d2bae6b-b650-4a24-85b8-51b3248d2b77"
      }
    ```

    (Change Domain and ClientId to the values of your registration if you use your own subscription)

5. To configure **ASP.NET Core Identity**, in **Program.cs**, register the following services:

    ```cs
    builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
    ```

#### Task2: Enable CORS

CORS (Cross-Origin Resource Sharing) is important when you want to use Web Api resources from JavaScript in the browser (XmlHttpRequest). For security reasons, browsers won't allow these request unless certain headers are available.

1. In Program.cs, configure cors service.

   ```csharp
   builder.Services.AddCors(options=>{
       options.AddPolicy("Allow_All", builder=>{
           builder.AllowAnyOrigin();
           builder.AllowAnyHeader();
           builder.AllowAnyMethod();
       });
   });
   ```

2. In the request pipeline add the CORS middleware

   ```
   app.UseCors("Allow_All");
   ```

3. Start **ProductReviews.API**

#### Task 3: Use angular-oauth2-oidc to add OAuth2 to a SPA.

1. Open a command prompt and navigate to **ProductReviews.Client.Angular.Oidc**.

2. Restore the npm packages (npm install)

3. add the following package to the project:

   ```bash
   npm install angular-oauth2-oidc@17.0.2	
   ```

4. Open the project in Visual Studio Code

#### Task 4: Set the App Registration information

1. Open the file **app.config.ts**

2. Check and modify (if you use your own Azure subscription) the following info

   1. tenantId. Modify it to your tenantId if you use your own Azure Subscription
   2. clientId. Modify it to your clientId if you use your own Azure Subscription
   3. RedirectUri. Should refer to **index.html**
   4. All the **scopes** arrays. 
   5. Also note responseType (code grant) and scope

3. Open the file **app.module.ts** (in src/app)

4. import oidc

   ```typescript
   import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
   ```

5. In **NgModule** add the **OAuthModule.forRoot()** in the **imports** array. (we're not going to use the **HttpInterceptor** here which is more convenient)

6. Also check the **providers** array where we determine where to store AccessTokens (localStorage in this case)

7. Review **main.component.ts** where the **oidc** initialization happens. Here we raise the login popup (**configureFlow()**)

#### Task 5: Make a secure call to the Web Api

1. Open file **product-group-list.component.ts**

2. In the **constructor** inject the **OAuthService**

    ```typescript
    private authSvc: OAuthService
    ```

3. In the **loadProductGroups** method retrieve an access token for the api
    ```typescript
    const token = this.authSvc.getAccessToken();
    const options = {
        headers:{
            "Authorization": `Bearer ${token}`
        }
    };
    ```

4. Open file **home.component.ts** and locate the **login** method.

5. Initialize the login

    ```typescript
     const token = this.authSvc.getAccessToken();
     if (token != null)
     {
     	this.router.navigate(["productgrouplist"]);
     }
     else
     {
     	this.authSvc.initCodeFlow();
     }  
    ```

6. Save all and run the application (ng serve --open.   You might need to add *C:\\ProgramData\npm* to the Environment PATH variables: ```setx PATH "%PATH%;C:\ProgramData\npm```)

#### Task 6: Optional. If time permits. Make ProductGroup Details work

1. When you edit a productgroup, you'll notice that no data is loaded. Try to solve that.

