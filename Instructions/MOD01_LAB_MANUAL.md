
# Module 1: Creating and Consuming ASP.NET Core Web APIs 

# Lab: Creating an ASP.NET Core Web API 

#### Scenario

In this lab, you will create and use ASP.NET Core Web APIs.

#### Objectives

After completing this lab, you will be able to:

- Create a Web API controller to expose APIs.
- Invoke the API through a browser.
- Use **HttpClient**, create **ConsoleApplication**, and then connect to the server by using **HttpClient**.


### Exercise 1: Create an ASP.NET Core Web API project

#### Task 1: Create an ASP.NET Core Web Api project

1. Open Command Prompt, and then browse to **[Drive:]\Allfiles\Mod01\LabFiles\Starter**.
2. Create an **ASP.NET Core Web API** project using the command line tools. Use **ProductReviews.API** as name.
   ```cmd
   dotnet new webapi -n ProductReviews.API --use-controllers
   ```
3. Remove the files **WeatherForecast.cs** and **WeatherForecastController.cs**
4. Open the project in Visual Studio Code
5. Add references to the following projects:
   - **ProductReviews.Repositories.EntityFramework**
6. Add the following nuget packages
   - **Microsoft.EntityFrameworkCore.SqlServer**
7. Create the **ProductGroupController** in the **Controllers** folder.
8. Examine the interface **IProductGroupRepository** and **IRepository** in project **ProductReviews.Interfaces**
8. Create a private field for **IProductGroupRepository** and initialize it through the constructor

#### Task 2: Add action methods to the controller for GET, POST, and PUT

1. Add a new **GET** action with the *page* and *count* parameters, and then return the List of **ProductGroup**.

   Http uri should look like this: **/productgroup?page=1&count=10**

2. Add a new **GET** action with the *id* parameter, and then return the **ProductGroup** entity.

   Http uri should look like this: **/productgroup/1**

3. Add a new **POST** action with the *ProductGroup* parameter, and then return the **ProductGroup** entity.

   Http uri should look like this: **/productgroup**

   ProductGroup will be in the body

4. Add a new **PUT** action with the *id* and *ProductGroup* parameters, and then return the **ProductGroup** entity.

   Http uri should look like this: **/productgroup/1**

   ProductGroup will be in the body

   Add a new **DELETE** action with the *id* and *ProductGroup* parameters, and then return the **ProductGroup** entity.

   Http uri should look like this: **/productgroup/1**

#### Task 3: Configure the Services

1. In **Program.cs**, register and configure the **ProductReviewContext** for SqlServer database
2. Register the Entity Framework DatabaseContext.
   ```csharp
   builder.Services.AddDbContext<ProductReviewsContext>(opt=>{
      opt.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Mod1DB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=true");
   });
   ```
3. Register the **ProductGroupRepository** as a service.

   ```csharp
   builder.Services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
   ```

4. Build the application

### Exercise 2: Use the API from a Browser

#### Task 1: Use a browser to access the GET action

1. Run the **ProductReviews.API** project. Note the ports used.

2. Open a browser,  and navigate to the following URL:
   ```url
    https://localhost:[port]/swagger
   ```
   >**Note**: If there is an error in the Console after running the application, run the following command: **dotnet dev-certs https --trust**.
   
3. Test a few api commands using Swagger.
   
4. Close all open windows.

### Exercise 3: Create a Client

#### Task 1: Create a console project and add a reference to System.Net.Http

1. To change the directory to the Starter project, run the following command:
   ```bash
    cd [Drive:]\Allfiles\Mod01\LabFiles\Starter
   ```
   
2. Create a new console application called **ProductReviews.ConsoleClient**.

3. Add reference to **ProductReviews.DAL.EntityFramework** (It would be better to have your entities in a separate assembly)

4. Start the WebApi project **ProductReviews.API**

#### Task 2: Use HttpClient to Call the GET, POST and PUT Actions of the Controller

1. Open the **ProductReviews.ConsoleClient** project in Visual Studio Code.
2. Create a method **ReadProductGroupAsync** and read all productgroups from the service
2. Run the **ProductReviews.ConsoleClient** project.
3. Create a method **AddProductGroupAsync** and upload a new **ProductGroup**.
3. Run the **ProductReviews.ConsoleClient** project.
4. Create a method **UpdateProductGroupAsync** and modify an existing **ProductGroup**.
5. Run the **ProductReviews.ConsoleClient** project.
7. Close all open windows.

