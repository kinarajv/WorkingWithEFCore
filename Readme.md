### Modern Database
There is two modern database right now
- **RDBMS (Relational Database Management System)** : SQL Server, PostgreSQL, MySQL, and SQLite
		1970, 
		Using Structured Query Language for query. 
		Using tabular structure or column and row. Data deduplication. 
		Expensive to scale.
		Save structured data.
		Using ACID, Atomicity, Consistency, Isolation and Durability for data consistency and integrity.
- **NoSQL (or Not Only SQL)** is Apache Cassandra, MongoDB, Redis, and Cosmos DB
		2000s. 
		Using its own language for query.
		Easier to scale or change its structured. No need for schema and don't have relation between table.
		Save unstructured data. 
		Not using ACID for faster performance and flexibility.

Decision for using RDBMS or NoSQL is depend on the type of data, scalability(vertical or horizontal), data consistency or integrity, performance, and query language.

### Entity Framework
ORM (Object Relational Mapping) is used to associate **columns in table** with **properties in classes**.
The truly version that support cross-platform is called EF Core.

#### Workflow
###### Database First 
Database already exist, build a model to matches its structure and features.
###### Code First 
No database exist, build model and use EF to generate the DB.

---
#### Create a Console App for EF Core (Database First)
First sample database by Microsoft in early 1990s, that we will create it later.
![Northwind Diagram](https://github.com/kinarajv/WorkingWithEFCore/blob/master/Northwind.png?raw=true "Diagram Northwind")

> [!note]
> 1. Install SQLite3 and set the path
> 2. Create Database (if the database not created yet)
> 3. Make a class that represent connection to the database, you can use a class that inherit from `DbContext`
> 4. Map a table by creating a class that have multiple properties and set the relation to another classs
> 5. After create a table classes or `entity`, declare it on the database class or the `DbContext` by using `DbSet<>`
###### Install SQLite
SQLite is small, cross-platform, easy to manage or deploy, and common RDBMS that used on mobile apps.
1. You can download it via `sqlite.org/download.html` and choose `Precompiled Binaies Section` and pick the bundle of command line tools.
2. Extract it to root of C:\\SQLite
3. Windows Settings > Edit the System Environment Variables > Environment Variables > System Variables > Select Path > enter C:\\SQlite

###### Create the Database from SQL
1. Put the `sql script` inside the project folder
2. Run the Windows Terminal, run `sqlite3 Northwind.db -init Northwind4SQLite.sql`
3. Wait until it shows `Northwind.db` 
4. Check data inside it using SQLite Studio
	1. Download http://sqlitestudio.pl
	2. Add database

###### Create Northwind Database context class
1. Create a public class that represent the database
2. Inherit it from `DbContext`
3. Override a method name `OnConfiguring` , its use to set the database connection string
```cs
using Microsoft.EntityFrameworkCore; // DbContext, DbContextOptionsBuilder 
namespace DatabaseNorthwind; // this manages the connection to the database 
public class Northwind : DbContext 
{ 
	protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder) 
	{ 
		string path = Path.Combine( Environment.CurrentDirectory, "Northwind.db"); 
		string connection = $"Filename={path}"; 
		optionsBuilder.UseSqlite(connection); 
	} 
}
```
4. In `Main program`
```cs
using DatabaseNorthwind;
Northwind db = new Northwind();
Console.WriteLine($"Provider: {db.Database.ProviderName}");
```

#### Defining EF Core Models
There is three way to define EF Models or entity. 
You can use
- Conventions
- Annotation Attributes
- Fluent API
##### Use Convention
1. Name of the class will be plurals on the database property. 
	Class `Product` will be present table `Products`
2. Name of the property that use `Id` will be the primary key
	Class `Product` have property `ProductId`, it will be the primary key
3. String type is `nvarchar` in database
##### Use Annotations
`[Require]` is for property that must have a value, the value is not null
`[StringLength(50)]` for declare the string up to 50 characters in length
`[Column(TypeName = "money", Name = "UnitPrice")]` is for declare the column type and column name that used in the table
##### Use Fluent API
```cs
modelBuilder.Entity<Product>() 
			.Property(product => product.ProductName) 
			.IsRequired() 
			.HasMaxLength(40);
modelBuilder.Entity<Product>() 
			.Property(p => p.Cost) 
			.HasColumnName("UnitPrice") 
			.HasColumnType("money");
modelBuilder.Entity<Category>() 
			.ToTable("Categories") 
			.HasKey(c => c.CategoryId);
// Relationship 
modelBuilder.Entity<Product>() 
			.HasOne(p => p.Category) 
			.WithMany(c => c.Products) 
			.HasForeignKey(p => p.CategoryId);
```
It used for declare in the `Product` class, there will be `ProductName` property that value must be not null, but the maximum length of string is 40 character.
For code below, the `Product` class will be have data at least one row that can use `HasData` method. It called `data seeding`
```cs
modelBuilder.Entity<Product>() 
			.HasData(
				new Product 
				{ 
					ProductId = 1, 
					ProductName = "Chai", 
					UnitPrice = 8.99M 
				});
```

---
### Scaffolding
Process by using `dotnet ef tools` to create class by use reverse engineering.

Requirement :
1. Install `dotnet-ef tools` 
	```bash
	dotnet tool list --global
	dotnet tool install --global dotnet-ef
	```
2. Add `Microsoft.EntityFrameworkCore.Design` to the project by using NuGet
3. Follow this command
```bash
dotnet ef dbcontext scaffold "Filename=Northwind.db" Microsoft.EntityFrameworkCore.Sqlite --table Categories --table Products --output-dir AutoGenModels --namespace WorkingWithEFCore.AutoGen --data-annotations --context Northwind
```
`dotnet ef` is the tool
`dbcontext scaffold` is the command
`FileName=NorthWind.db` is connection string
`MS.EFCore.Sqlite` is the provider
`--table` is which table will be generated
`output-dir` is the output directory
`namespace` the namespace declared
`--data-anotations`