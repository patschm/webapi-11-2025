
# Module 2: Extending ASP.NET Core HTTP services

# Lab: Customizing the ASP.NET Core Pipeline

#### Scenario

In this lab, you will customize the ASP.NET Core Pipeline.

#### Objectives

After completing this lab, you will be able to:

- Add inversion of control by using Dependency Injection to the project.
- Create a cache mechanism and action filters.
- Add middleware to inform the client through header response.
  

### Exercise 1: Use Dependency Injection to Get a Repository Object

In large applications you might end up with registering hundreds of Repositories for the Dependency Injector. A pattern to overcome this the Unit of Work pattern (https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)

#### Task 1: Create an interface for the repository 

1.  Open a **Command Prompt** window, and then browse to **[Drive]\Allfiles\Mod02\LabFiles\Starter\ProductReviews.Interfaces**.
2.  Open the project in Microsoft Visual Studio Code.
3.  In **ProductReviews.Interfaces**, add a new interface, and then name it **IUnitOfWork.cs**.
4.  To **IUnitOfWork.cs**, add the following method signatures:
    ```cs
    IProductGroupRepository ProductGroupRepository { get; }
    IProductRepository ProductRepository { get; }
    IBrandRepository BrandRepository { get; }
    IReviewRepository ReviewRepository { get; }
    Task SaveAsync();
    ```

#### Task 2: Implement the interface on the repository

1. In the **ProductReviews.Repositories.EntityFramework** project, create a **UnitOfWork** class
2. Create a private field ProductReviewsContext and initialize it through a constructor.
3. Implement the **IUnitOfWork** interface.

#### Task 3: Register the UnitOfWork object in the ASP.NET Core Dependency Injection mechanism

1. In the **ProductReviews.API** project, locate **Program.cs**.
2. In the **ConfigureServices** method, register **UnitOfWork**.
3. Remove the Dependencies for the repositories

#### Task 4: Change the controller’s constructors to request an injected repository

1. Modify all the controllers to use the **IUnitOfWork** interface
2. Test the **ProductReviews.API** service.



### Exercise 2: Create a Cache Filter

#### Task 1: Create an action filter for cache headers

1. In the **ProductReviews.API** project, create an **Filters** folder.

2. Create a new **CacheAttribute** class, and make sure that it is derived from **ActionFilterAttribute**.

3. Add the following fields:
      ```cs
    private string _headerMessage { get; set; }
    private TimeSpan _durationTime;
    private const int _defaultDuration = 60;
    private Dictionary<string,(DateTime, IActionResult)> _cache = new Dictionary<string, (DateTime,IActionResult)>();
    ```
    
4. To initiate the **_durationTime** (in seconds) field, add a constructor with the optional **int** parameter. Use **_defaultDuration** as default value

5. To initiate the **_headerMessage** field, add a constructor with the **string** parameter. 

6. To check the cache validation, add a new **CacheValid** method with the **FilterContext** parameter and return **bool**. Add the following body

      ```c#
      StringValues xCacheHeader = context.HttpContext.Request.Headers[_headerMessage!];
      if (xCacheHeader == "false" || xCacheHeader.Count == 0)
      {
      	if (_cache.TryGetValue(context.HttpContext.Request.Path, out (DateTime, IActionResult) cacheValue))
          {
              if (DateTime.Now - cacheValue.Item1 < _durationTime)
              {
                  return true;
              }
      	}
      }
      return false;
      ```

7. To override the **OnActionExecuting** method, enter the following code:
   ```cs
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (CacheValid(context))
        {
            context.Result = _cache[context.HttpContext.Request.Path].Item2;
            return;
        }
        base.OnActionExecuting(context);
    }
   ```
   
8.  To override the **OnResultExecuted** method, enter the following method:
    ```cs
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        if(!CacheValid(context))
             _cache[context.HttpContext.Request.Path] = (DateTime.Now,context.Result);
        base.OnResultExecuted(context);
    }
    ```

#### Task 2: Add the cache filter to several actions

1. In **ProductGroupController**, above the **Get** methods, add the **Cache("X-No-Cache")** attribute.
   
#### Task 3: Test cacheable and non-cacheable actions from a browser

1. Run the **ProductReviews.API** service.

2. Open Windows Powershell.

3. To get the productgroups from the server, run the following command:
    ```bash
    $result = Invoke-WebRequest https://localhost:[port]/productgroup -UseBasicparsing
    ```
    >**Note:** If you get the **The underlying connection was closed: An unexpected error occurred on a send** error message, run the  **[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12** command, and then redo the step.
    
4. To display the http result, run the following command:
    ```bash
    $result
    ```
    
5. To display only the http content, run the following command:
    ```bash
    $result.Content
    ```
    
6. Set a breakpoint in **ProductGroupController** **Get** method and execute the following command a few times. The breakpoint should hit only once a minute.
   ```bash
   Invoke-WebRequest https://localhost:[port]/productgroup -UseBasicparsing
   ```



### Exercise 3: Create a Debugging Middleware

#### Task 1: Create a middleware class to calculate the execution time

1. Under **ProductReviews.API** folder, create a new **Middleware** folder.
2. Create a new **ExecutionTimeMiddleware** class.
3. Add a private field **_next** of type **RequestDelegate** and initialize through coonstructor.
4. Add a new  **InvokeAsync** method with the **HttpContext** parameter and a return **Task** entity.

#### Task 2: Write server and debug information to response headers

1. Inside the method add:

    ```csharp
    context.Response.Headers.Add("X-Server-Name", Environment.MachineName);
    context.Response.Headers.Add("X-OS-Version", Environment.OSVersion.VersionString);
    ```

2. Under **X-OS-Version**, add the **Request Execution Time** header:
    ```cs
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    
    context.Response.OnStarting(state => 
    {
        var httpContext = (HttpContext)state;
        stopwatch.Stop();
        httpContext.Response?.Headers?.Append("X-Request-Execution-Time", stopwatch.ElapsedMilliseconds.ToString());
        return Task.CompletedTask;
    }, context);
    ```

3. To continue to the next middleware, under **X-Request-ExecutionTime** enter the following code:
   ```cs
    await _next();
   ```

#### Task 3: Create the IApplicationBuilder helper class

1. Create a static extension method **UseExecutionTime** for **IApplicationBuilder** with the **IApplicationBuilder** parameter and a return **IApplicationBuilder** entity. 

#### Task 4: Register the middleware to the ASP.NET Core pipeline

1. In **Program.cs**, under **ProductReviews.API**, locate the **Configure** method.
2. Above **app.UseHttpsRedirection**, enter **UseExecutionTime**.
   
#### Task 5: Test the new middleware from a browser

1. Run the **ProductReviews.API** service.

2. Open Windows Powershell.

3. To get the productgroups from the server, run the following command:

   ```bash
   $result = Invoke-WebRequest https://localhost:[port]/productgroups -UseBasicparsing
   ```

4. To display the http headers, run the following command:

   ```bash
   $result.Headers
   ```

5. Verify that you received **x-os-version**, **x-server-name**, and **x-request-execution-time** from the server.

6. Close all open windows.