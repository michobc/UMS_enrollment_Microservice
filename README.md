## Introduction

Optimize the University Management System already created.

## Microservices
Create a microservice that handles students class enrollments
### Steps:
    • Create new .Net Project that handles students class enrollment
    • Create independent database for the newly created microservice
    • Using RabbitMQ, allow an admin to add a new course using the main API, while automatically
applying the necessary data changes within the microservice database.

- Note :  the Infrastructure should be publishing not Consuming messages, in this case we are consuming => we put the service in the application layer

// create migration:

command : dotnet ef migrations add InitialCreate

// apply migration:

command : dotnet ef database update

// apply references:
dotnet add reference ../LabSession5.Infrastructure/LabSession5.Infrastructure.csproj

## Multi-Tenancy
Allow the same system to be used by multiple branches of the university. Users from one branch should not be able
to see the data from a different branch. (Use Schema-Based multi tenancy)
### Steps:
    • Think about the problem at hand.
    • Find a solution for that problem.
    • Implement it!

### Extra
Allow Administrators to have full access to their own branch data, but only read access to other branches' data.

## QUESTIONS:
- migration in persistence layer directly

# Michel BOU CHAHINE
## Inmind.ai