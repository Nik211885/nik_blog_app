## Infrastructure

You can create migrations file when change data to database

After run command you need create ef tool 


```csharp
 dotnet tool install --global dotnet-ef
```


If you want to have multiple db context in ef command you should 
specific db context you want to create and move migration to database with
argument in ef tool is --context


Create migrations file

```csharp
dotnet ef migrations add [migrations message] --project Infrastructure --startup-project WebApi --output-dir Data/Migrations
```

Move all migration with ddl to create database 

```csharp
  dotnet ef database update --project Infrastructure --startup-project WebApi
```

If you want to remove migrations remove stack migration history

```csharp
    dotnet ef migrations remove --project Infrastructure --startup-project WebApi
```