# Module 3: Hosting Services On-Premises and in Containers

### Exercise 1: Creating a Web App IIS

#### Task 1: Prepare IIS

1. Download [.NET Core Hosting Bundle Installer](https://dotnet.microsoft.com/permalink/dotnetcore-current-windows-runtime-bundle-installer)
2. Restart the server or execute `net stop was /y` followed by `net start w3svc` in an **administrator** command shell.
3. On the IIS server, create a folder to contain the app's published folders and files.
4. Start IIS Manager  **(C:\Windows\System32\inetsrv\InetMgr.exe**)
5. In IIS Manager, open the server's node in the **Connections** panel. Right-click the **Sites** folder. Select **Add Website** from the contextual menu.
6. Provide a **Site name** and set the **Physical path** to the app's deployment folder that you created.
7. Provide the **Binding** configuration, set port to 8888 and create the website by selecting **OK**.
8. Add another  **https Binding** on port 8889. Use an SSL certificate (You might need to create one. Root node/Server Certificates)
9. In **Application Pools** open the pool that has your **Site name**.
10. Set **.NET CLR version** to **No Managed Code**
11. In Advanced Settings set **Identity** to your own user name and password (not good practise but it prevents messing around in SqlServer which need grant access to the Identity account)

#### Task 2: Deploy ProductReviews.API

1. Open a command prompt and navigate to **[Drive:]\AllFiles\Mod03\Starter\Exercise 1\ProductReviews.API**.

2. Publish the site using

   ```bash
    dotnet publish --configuration Release -o <path_to_folder_you_created_in_step_3>
   ```

3. Test the deployment **ProductReviews.API** (http://localhost:8888/productgroup)

#### Task 3: Configure an environment variable and the database connection string

1. Open a command prompt and navigate to **[Drive:]\AllFiles\Mod03\Starter\Exercise 1\ProductReviews.API**.
2. Open the project Visual Studio Code.
3. In **Program.cs** find the connection string. Copy the value and replace it for a string variable **connectionString**
3. Assign the environment entry **ASPNETCORE_DATABASE** to **connectionString**
4. In IIS Manager, Application Pools, stop  the pool from your web site
5. In Sites/<your_Site> open the configuration Editor.
6. Select: **system.webServer/aspNetCore** 
7. In **environmentVariables** add the entry **ASPNETCORE_DATABASE** with your connectionstring
8. Close window and **apply** changes
9. Now start the application pool again and test your web site.Exercise 2: Deploying an ASP.NET Core Web API to the Web App

### Exercise 2: Publishing the ASP.NET Core Web Service on Kubernetes

Before you start make sure Docker is running (Docker Desktop)
Once running enable Kubernetes (Settings->Kunerneted->Enable)

#### Task 1: Prepare Kubernetes

1. Enable Kubernetes on Docker Desktop (Settings->Kunerneted->Enable) 
2. Check on the command prompt if kubectl is installed. If not add it to the PATH vatiables (C:\Program Files\Docker\Docker\Resources\bin\kubectl.exe)
3. Check if the docker-desktop context is available
   ```cmd
   kubectl config get-contexts
   ```
4. Set the docker-desktop context
   ```cmd
   kubectl config use-context docker-desktop
   ```
5. Test the nodes
   ```cmd
   kubectl get nodes
   ```

#### Task 2: Deploy Sql Server in kubernetes
**Warning** If we want to run sqlserver in a container the container storage is gone after the container get destroyed.
Normally you'll create an persistent volume to store the sql server data but on Docker Desktop that's too difficult and beyond the scope.
In Azure Kubernetes the steps to create a persistent volume would look something like this:
```yaml
apiVersion: storage.k8s.io/v1
kind: StorageClass
metadata:
   name: azure-disk
provisioner: kubernetes.io/azure-disk
parameters:
  storageaccounttype: Standard_LRS
  kind: Managed
```
And
```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
    name: mssql-data
spec:
  accessModes:
  - ReadWriteOnce # the volume can be mounted as read-write by a single node
#  - ReadOnlyMany  # the volume can be mounted read-only by many nodes
#  - ReadWriteMany # the volume can be mounted as read-write by many nodes
  storageClassName: managed-csi # managed-csi-premium for premium
  resources:
    requests:
      storage: 5Gi
```
   1. Set the SqlServer sa password in kubernetes secrets
      ```cmd
      kubectl create secret generic mssql --from-literal=MSSQL_SA_PASSWORD="Test_1234567"
      ```
2. Install sqlserver container create the following yaml-file
   ```yaml
   apiVersion: apps/v1
   kind: Deployment
   metadata:
   name: mssql-deployment
   spec:
   replicas: 1
   selector:
      matchLabels:
         app: mssql
   template:
      metadata:
         labels:
         app: mssql
      spec:
         terminationGracePeriodSeconds: 30
         hostname: mssqlinst
         securityContext:
         fsGroup: 10001
         containers:
         - name: mssql
         image: mcr.microsoft.com/mssql/server:2022-latest
         resources:
            requests:
               memory: "2G"
               cpu: "1"
            limits:
               memory: "2G"
               cpu: "1"
         ports:
         - containerPort: 1433
         env:
         - name: MSSQL_PID
            value: "Developer"
         - name: ACCEPT_EULA
            value: "Y"
         - name: MSSQL_SA_PASSWORD
            valueFrom:
               secretKeyRef:
               name: mssql
               key: MSSQL_SA_PASSWORD
   ```
3. Upload the yaml file to Kubernetes
   ```cmd
   kubectl apply -f <your_yaml_file>
   ```
4. Check if the container is running
   ```cmd
   kubectl get pods
   ```
   or more verbose
   ```cmd
   kubectl describe pods
   ```
5. To make SqlServer publicly accessible we'll add a load balancer. Create the following yaml file
   ```yaml
   apiVersion: v1
   kind: Service
   metadata:
   name: mssql-deployment
   spec:
   selector:
      app: mssql
   ports:
      - protocol: TCP
         port: 4444
         targetPort: 1433
   type: LoadBalancer
   ```
6. Upload the yaml file to Kubernetes
   ```cmd
   kubectl apply -f <your_yaml_file>
   ```
7. Check the services and note the public ip-address
   ```cmd
   kubectl get services
   ```
8. Try to connect to SqlServer with SqlServer Managament studio
   - Server name: tcp:localhost, 4444
   - Authentication: Sql Server Authentication
   - Login: sa
   - Password: Test_1234567
   - Under options select checkbox Trust server certificate

#### Task 3: Test the connection
1. Open the command prompt and go to the following directory:

   ```bash
   cd [Drive:]\Allfiles\Mod03\Labfiles\Starter\Exercise 2\ProductReviews.DatabaseConsole
   ```

2. Open project in Visual Studio Code

3. Check the connection string if it is correct.

   ```sql
   Server=tcp:localhost,4444;Database=Mod1DB;User Id=sa;Password=Test_1234567;MultipleActiveResultSets=True;TrustServerCertificate=true
   ```
4. Test **ProductReviews.DatabaseConsole** if the connection string works.
5. Check in SqlServer Management Studio if the database and tables is created.

#### Task 4: Use a Docker container to build a self-contained ASP.NET Core web service

1. Open the command prompt and go to the following directory:

   ```bash
   cd [Drive:]\Allfiles\Mod04\Labfiles]Starter\Exercise 2
  
2. Navigate to the **ProductReviews.API** project, add a new **Dockerfile** file. \
In the **Dockerfile** file, first create a runtime image

   ```dockerfile
   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
   WORKDIR /app
   EXPOSE 80

   ENV ASPNETCORE_URLS=http://+:80
   ENV ASPNETCORE_DATABASE="Server=tcp:host.docker.internal,4444;Database=Mod1DB;User Id=sa;Password=Test_1234567;MultipleActiveResultSets=True;TrustServerCertificate=true"
   ```
   > host.docker.internal is a special url in docker which refers to the ip-address of the host machine. Only useful in development scenarios.

3. Next create a build image and copy the files from your machine into the build image.

   ```dockerfile
   FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   ARG configuration=Release
   WORKDIR /src
   COPY . .
   WORKDIR /src/ProductReviews.API
   RUN dotnet restore "ProductReviews.API.csproj"
   RUN dotnet build "ProductReviews.API.csproj" -c $configuration -o /app/build
   ```

4. Publish the code in the build image

   ```dockerfile
   FROM build AS publish
   ARG configuration=Release
   RUN dotnet publish "ProductReviews.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false
   ```

5. Finally copy the publish output to the runtime environment

   ```dockerfile
   FROM base AS final
   WORKDIR /app
   COPY --from=publish /app/publish .
   ENTRYPOINT ["dotnet", "ProductReviews.API.dll"]
   ```

6. On the command prompt navigate to

   ```bash
   cd [Drive:]\Allfiles\Mod03\Labfiles]Starter\Exercise 2
   ```

7. Execute the command: (don't forget the . in the end)

    ```bash
    docker build -t productreviews -f .\ProductReviews.API\Dockerfile .
    ```

8. Run the container

    ```bash
    docker run -p 8080:80 productreviews
    ```

9. Open a web browser and navigate to http://localhost:8080/productgroup

#### Task 5: Deploy the api project to kubernetes.

1. The LoadBalancer services enables public access to SqlServer which is not desireable. When the ProductReviews.API project runs in kubernetes as well we can use internal communication. ClusterIP services makes internal communication possible.\
Create a yaml file for configuring the ClusterIP service and add the following instructions
   ```yaml
   apiVersion: v1
   kind: Service
   metadata:
      name: mssql-ep
   spec:
      type: ClusterIP
      selector:
         app: mssql
      ports:
         - protocol: TCP
           port: 1433
           targetPort: 1433
   ```
2. Upload the service to kubernetes
   ```cmd
   kubectl apply -f <your_yaml_file>
   ```
3. Modify the connectionstring [Drive:]\Allfiles\Mod03\Labfiles]Starter\Exercise 2\ProductReviews.API\Dockerfile to
   ```yaml
   ENV ASPNETCORE_DATABASE="Server=mssql-ep;Database=Mod1DB;User Id=sa;Password=Test_1234567;MultipleActiveResultSets=True;TrustServerCertificate=true"
   ```
   >Note the mssql-ep as the server name. It's the name of the ClusterIP service.
4. Rebuild the image
   ```cmd
   docker build -t productreviews -f .\ProductReviews.API\Dockerfile .
   ```
5. Create a yaml file for your productreviews container and Loadbalancer for public access.
   ```yaml
   apiVersion: apps/v1
   kind: Deployment
   metadata:
      name: productreviews-deployment
   spec:
      replicas: 1
      selector:
         matchLabels:
            app: productreviews
      template:
         metadata:
            labels:
              app: productreviews
      spec:
         containers:
         - name: productreviews-api
           image: productreviews
           imagePullPolicy: Never
   ---
   apiVersion: v1
   kind: Service
   metadata:
      name: productsreviews-lb
   spec:
      type: LoadBalancer
      selector:
         app: productreviews
      ports:
      - name: http
      port: 8888
      targetPort: 80
   ```
   > imagePullPolicy: Never will disable pulling from a remote server (hub) effectively using the image on localmachine

6. Open the browser and navigate to
   ```cmd
   http://localhost:8888/productgroup
   ```
#### Task 6: (Optional) Enable https
All run on http but in live scenarios you want to enable https. For that use ingress. Ingress is a kind of route table that redirects incoming requests to an endpoint in kubernetes. To execute the ingress instructions you'll need and ingress controller.
1. Deploy an ingress controller
   ```shell
   kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.9.0/deploy/static/provider/cloud/deploy.yaml
   ```
2. Create a yaml file for the ingress table
   ```yaml
   apiVersion: networking.k8s.io/v1
   kind: Ingress
   metadata:
   name: ingress-api
   annotations:
      nginx.ingress.kubernetes.io/rewrite-target: /
   spec:
   ingressClassName: nginx
   rules:
   - http:
         paths:
         - path: /
         pathType: Prefix
         backend:
            service:
               name: productsreviews-lb
               port:
               number: 80
   ```
3. Deploy the ingress yaml
   ```cmd
   kubectl apply -f <your_ingress_yaml>
   ```
4. Test the service
   ```cmd
   https://localhost/productgroup
   ```
