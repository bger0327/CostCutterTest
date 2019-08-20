# Senior C# Test

## Task Definition

A basic solution is provided in this repository, with some core NuGet Packages already installed:

- MySQL.Data
- Dapper
- Xunit for unit tests

If you would prefer to use any alternative packages, please feel free to do so.

Structure your code as you wish. Use as many projects as you wish.

Some configuration is already provided, e.g. database connection string to the test database & database class to provide a MySqlConnection. This will instantiate a connection to a MySQL 8 cloud instance. This database will be running continuously. Note that the username / password provided only grants `SELECT` privileges. This should be sufficient to complete the exercise. Please study the database using a database client of your choice.

Using the database provided, create a desktop application which does the following:

### Browse Orders

- Allow the user to browse orders placed at each branch, with search functionality.
- Given a selected order, display relevant information to the order, include details of the equipment, trim, engine and model for each vehicle.

### Update Orders

Although you cannot update records into the database, allow the user to edit existing orders, based on the data in the database. 
Add data validation as deemed required. (E.g. Every order's sale price must be greater than 0.)

Given the information provided, feel free to add any extra features such as filters.

If you have time, please feel free to add some unit and/or integration tests to cover your code. A basic test harness for both unit and integration testing is set up.

When you have completed your task, please submit your code either as a ZIP archive or provide a link to your checked in source code in your personal Github repository or similar service.
