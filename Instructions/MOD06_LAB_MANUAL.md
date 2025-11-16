
# Module 6: SignalR

# Lab: Adding SignalR to your application

#### Scenario

In this lab, you will use SignalR to send new or modified reviews to a moderation page where moderators can accept or reject reviews.

#### Objectives

After you complete this lab, you will be able to:
-	Write SignalR Hubs and use them in ASPNET Core WebApi project
-	Write client side logic to listen to incoming reviews

### Exercise 1: Write a Hub and send data.

#### Task 1: Create a Hub

1. Open the command prompt, and then navigate to **[Drive:]\AllFiles\Mod06\Starter\ProductReviews.API** folder.
2. Open the project with Microsoft Visual Studio Code.
2. In the Hubs Folder create a class **ReviewHub** that derives from **Hub (Microsoft.AspNetCore.SignalR)**

#### Task 2: Register the SignalR service.

 1. Open **Program.cs** and add **SignalR** to the services

#### Task 3: Add SignalR middleware

1. Open **Program.cs** and  Map the **ReviewHub** to endpoint **/reviewmonitor**

#### Task 4: Notify clients for new or modified reviews

1. Open **ReviewController.cs** and inject **IHubContext\<ReviewHub>**
2. In the **Post** method send all clients a notification with first arguments
   - A string **ReviewAdded**
   - The Review object **review**
3. In the **Post** method send all clients a notification with first arguments
   - A string **ReviewModified**
   - The Review object **review**
4. Build the **ProductReview.Api** project and run it.

#### Task 4: Test if the WebApi project works

1. Start the **ProductReview.Api** project and navigate to **https://localhost:5001/swagger**

2. Go to the **Review** section and open the **Post** group;

3. Post a review with the following body

   ```json
   {
      "author": "pierre",
      "email": "pierre@baker.com",
      "text": "This product is fairly good",
      "score": 4,
      "productId":1
   }
   ```

4. Keep the application running.

   

### Exercise 2: Add SignalR Client Side Logic

#### Task 1: Install Client side packages

1. Open the command prompt, and then navigate to **[Drive:]\AllFiles\Mod06\Starter\ProductReviews.Moderation** folder.

2. Execute the following command to install LibMan

   ```bash
   dotnet tool install -g Microsoft.Web.LibraryManager.Cli
   ```

3. Execute the following command to install the SignalR client libraries

   ```bash
   libman install @microsoft/signalr@latest -p unpkg -d wwwroot/lib/microsoft/signalr --files dist/browser/signalr.js --files dist/browser/signalr.min.js
   ```

#### Task 2: Connect with the SignalR endpoint and receive notifications.

1. Open file **wwwroot\js\index.js**

2. Under the **$reviews** variable create a **const connection** and create the **SignalR**  connection to **http://localhost:5001/reviewmonitor**

   ```javascript
   const baseurl = "https://localhost:5001";
   const connection = new signalR.HubConnectionBuilder().withUrl(`${baseurl}/reviewmonitor`).build();
   ```
3. Start the connection

    ```javascript
    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });
    ```

3. Create an event handler for **ReviewAdded**. The handler should call the function **reviewItem** and add the result to the **$reviews** variable.

   ```javascript
   connectionon("ReviewAdded", function (review) {
       var rev = reviewItem(review, "added")
       $reviews.append(rev);
   });
   ```

4. Create an event handler for **ReviewModified**. The handler should call the function **reviewItem** and add the result to the **$reviews** variable.

   ```javascript
   connectionon("ReviewModified", function (review) {
       var rev = reviewItem(review, "modified")
       $reviews.append(rev);
   });
   ```

6. Make sure **index.js** is added in **Index.cshtml (Views\Home)**

#### Task 3: Test the application


1. Make sure **ProductReview.Api** is still running

2. Start **ProductReview.Monitor**

3. In a new browser instance navigate to **https://localhost:5001/swagger**

4. Go to the **Review** section and open the **Post** group;

5. Post a review with the following body

   ```json
   {
      "author": "pierre",
      "email": "pierre@baker.com",
      "text": "This product is fairly good",
      "score": 4,
      "productId":1
   }
   ```

6. Check in  **ProductReview.Monitor** if the review shows up.