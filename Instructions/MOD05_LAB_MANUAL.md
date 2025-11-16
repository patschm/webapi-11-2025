
# Module 5: Diagnostics and Monitoring

# Lab: Monitoring ASP.NET Core with ETW

#### Scenario

In this lab, you will use Event Tracing for Windows (ETW) on Windows to monitor exception and garbage collection (GC) events in a Microsoft ASP.NET Core application.

#### Objectives

After you complete this lab, you will be able to:
-	Collect and analyze ETW events with PerfView for an ASP.NET Core application on Windows.
-	Add Logging to your ASP.NET Core application

### Exercise 1: Collect and view ETW events

#### Task 1: Run the ASP.NET Core application

1. Open the command prompt, and then navigate to **[Drive:]\AllFiles\Mod05\Exercise 1\Problem1\PageMaker** folder.
2. Run the project.
2. Open a browser and navigate to https://localhost:7051/csv/index
2. In **Choose File**, select file **[Drive:]\\AllFiles\Mod05\Exercise 1\Orders.csv**
2. Click Upload and wait. It takes a while and we wonder why.

#### Task 2: Record .NET ETW events in PerfView

 1. Open **[Drive:]\\AllFiles\Mod05\PerfView.exe**, and then collect all the events.
 1. Repeat the call to https://localhost:7051/csv/index and once completed, stop collecting.

#### Task 3: View CPU Stack details and call stacks in PerfView

1. In **PerfView**, after a few seconds a **zip** file should appear with the ETW data. 
2. Open **CPU Stacks** and select the process **PageMaker** and select the **By Name** tab.
3. One name should stand out. Open it (at some point there should appear something with **TableServicve++\<GenerateTableAsyn\>**). 
4. Right click **TableServicve++\<GenerateTableAsyn\>** and select **Goto Source**
5. You should find that a lot of time is being spent on concatinating a string.

#### Task 4: Examine the Managed Heap

1. Return to the main window in **PerfView** and open **Memory Group**

2. Inspect the GCStats and note that a lot of GC's happen for Gen 2 (generally a bad thing). Let's find out why.

3. Open **GC Heap Alloc Ignore Free** and note that **LargeObject** (objects > 85k) is responsible for almost all the samples.

4. Open the **LargeObject** node and find out who's responsible

   > It appears that excessive string concatenation generates at some point objects that exceed the 85k threshold over and over again causing the GC to clean the memory for Gen 2 which is VERY costly. In the program you can solve it by using **StringBuilder** which is done in **TableServiceOptimal**.

5. **Optional**; Fix it and test again

### Exercise 2: Add Logging to your application

#### Task 1: Add the Logging to the web service project

1. Open the command prompt, and then navigate to **[Drive:]\AllFiles\Mod05\Exercise 2\Starter\ProductReviews.API** folder.

2. Open the project in Visual Studio Code.

3. In **Program.cs**, clear all existing log providers and add providers for **Console** and **Debug**

   > Console is added by default so normally you won't have to change a thing.

4. In **appsettings.json**, configure the providers to show the following logging information

   - Console
     - Default: Error
     - Microsoft.EntityFrameworkCore.Database.Command: Information

5. On the command prompt start the application and in a browser navigate to https://localhost:7016/swagger and execute a few commands. Watch the command windows.

   > **Beware**: You might think the application is building but it is actually running

6. Stop the application and add the following to the Console configuration

   - Microsoft.AspNetCore: Information

7. Again start the application and execute a few commands in swagger. Not the output in the command prompt.

   > Now both logging information from EntityFramwork and AspNetCore should show up

8. Disable output for AspNetCore (Microsoft.AspNetCore: None)

#### Task 2: Add logging to the BrandController

1. Open **BrandController.cs** and inject the **ILogger\<BrandController\>**.

2. In action **Get(int page = 1, int count = 10)** log the following text as information

   ```csharp
   $"Start {nameof(BrandController)}/{nameof(Get)}?page={page}&count={count}"
   ```

3. In action **Get(int id)** log the following text as information

   ```csharp
   $"Start {nameof(BrandController)}/{nameof(Get)}/{id}"
   ```

4. In action **Post([FromBody]Brand brand)** log the following text as information

   ```csharp
   $"Start {nameof(BrandController)}/{nameof(Post)}"
   ```

   Also display **JsonConvert.SerializeObject(brand)** as **Trace** information

5. In action **Put(int id, [FromBody]Brand brand)** log the following text as information

   ```csharp
   $"Start {nameof(BrandController)}/{nameof(Put)}/{id}"
   ```

   Also display **JsonConvert.SerializeObject(brand)** as **Trace** information

3. In action **Delete(int id)** log the following text as information

   ```cs
   $"Start {nameof(BrandController)}/{nameof(Delete)}/{id}"
   ```


7. On the command prompt start the application and in a browser navigate to https://localhost:7016/swagger and execute a few commands for the Brands. Watch the command windows.

   > None of your logging should show

8. In **appsettings.json** add to the Console logging section the following source

   ```csharp
   "ProductReviews.Api": "Information"
   ```

9. Run the application again and **Post** the following **Brand** data

   ```json
   {
     "name": "Test Brand"
   }
   ```

   > The Trace data shouldn't show up in the console

10. Now change the switch **ProductReviews.Api** to **Trace** and repeat step 9.
