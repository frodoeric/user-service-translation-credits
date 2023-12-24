# .NET Engineer Challenge

You're starting a new job at Translations INC and you're assigned to do some work on UserService. Apparently a senior developer put together a very initial version of the repository with some nice practices, but then they went on parental leave and for a few weeks the company outsourced the project to the JuniorDev consultancy firm where they might've commited some dubious code. Now Translations INC needs to get the project back on track in-house.

UserService is a REST API for managing user data and "translation credits". Right now it's possible to create new users, get a specific user by ID and get a list of all users. When a new user is created, it is also saved in the company's CRM system which is an external service that's used in the company. The features related to "translation credits" haven't been implemented yet. "Translation credit" refers to a kind of token that a user can spend on translation services from Translations INC. UserService is responsible for tracking how many credits a user has and adding/subtracting them from their balance.

General instructions:
* Create a new git branch from main and work there. Commit your changes as often as possible; the more commits the better. When you're done, create a pull request to merge your branch into main. **Your branch and the pull request are your deliverables**.
* The UserService is an enterprise service which will grow significantly in complexity with the passing of the years. Follow best practices to make the codebase clean, readable and maintainable. Practice SOLID, DRY, DDD, Clean / Hexagonal architecture and make sure to write valuable and robust tests.

Your tasks:
* Review the code in the repository. If you see any code smells, bad practices or code which is not aligned with the intended application architecture please refactor the code while keeping the existing business logic.
    * Focus on the application code and its architecture; don't worry about which DB or ORM we're using, generating DB migrations or specifics about the CRM API - it's just a dummy API that returns 200.
    * If you have any refactoring ideas which will take too long to implement just write them down and add them with some explanation to a "Future work" section in the end of this README.
* Implement these features / fixes:
    * Fix: Emails longer than 100 chars shouldn't be allowed.
    * Feature: User update. Add an endpoint to update a user's basic data (name and email).
    * Feature: Translation credits. Users own translation credits. When a user is created they have 0 credits. For now, credits can be represented by an integer value. Other services need to be able to call UserService endpoints to manage a user's credit or "spend" some credit in the name of a user. Add a few REST endpoints that comply with these requirements:
        * Credits can be added to a user's balance. This will be used for example when a user buys credits.
        * Other services should be able to "spend" an amount of user's credits on their behalf. If the user's balance is insufficient, the action should return an error response. This will be used when a user receives translation services in exchange for credits.
        * It should be possible to subtract credits from a user. This will be used when a user wants to undo a credit purchase and asks for a refund, or when something goes wrong with their payment. It's possible that a user ends up with a negative balance after a subtraction, as they might've spent credits that they got through a payment which eventually got rejected. That's fine.
    * Feature: User tiers. Users are qualified as "Sporadic", "Advanced" or "Special", depending on how many credits they have spent since they were created. Users that have spent 0-99 credits are "Sporadic", 100-999 are "Advanced" and 1000 or higher are "Special". The user tier should be returned together with the user data in the existing GET endpoints.



You can spend as much time as you want on the challenge, but getting to a good solution shouldn't take more than 4-5 hours. Try to keep it simple and focus on what's important.

When you have created your PR and the challenge is ready for review send us an email. Make sure to use your commits and commit messages to help your code reviewer. Break up the task into a series of small, focused, incremental commits. Each commit should focus on a cohesive change or feature. Don't mix mostly unrelated changes within the same commit.

For example:
* Commit 1: Fix email length
* Commit 2: Post email length fix refactoring
* Commit 3: User update feature, added dummy endpoint and green acceptance tests
* Commit 4: User update feature, added all code and acceptance tests. Passing.
* ...

We will also evaluate how you use the PR description as a communication tool. Your PR description should give a general overview of the changes and guide the reviewer through your commits, and the most important lines of code changed (where the reviewer should start).

## How to run the app

You'll need:
* Docker
* Docker-compose
* .NET and Visual Studio or your preferred IDE for .NET.

You can run the application with Docker-compose or with your IDE. In both cases the database will be automatically migrated on first execution.

**Option A)** Just docker-compose:

In the root of the repo run:
```
docker-compose up
```
The application will be running in http://localhost:8001/. Access the API docs in http://localhost:8001/swagger.

**Option B)** Your IDE:

Get a SQL Server DB ready by running:
```
docker-compose up userservice_local_db
```
Then you can run the UserService.API project using your IDE.

### Running tests

For the acceptance tests you need to get a SQL Server DB ready by running:
```
docker-compose up userservice_local_db
```

Then you can run both the unit and acceptance tests using the test runner in your IDE. The DB gets automatically migrated if needed when running the acceptance tests.

## Future Work

### Implementing CQRS (Command Query Responsibility Segregation)

As UserService grows in complexity, implementing CQRS would be beneficial. This pattern involves separating read (query) operations from write (command) operations. Key benefits include:

- **Performance Optimization**: Separate scaling for read and write operations.
- **Complexity Management**: Easier to manage complex business logic by isolating commands from queries.
- **Flexibility in Data Storage**: Different storage mechanisms can be used for read and write sides.
- **Enhanced Scalability**: Independent scaling of reads and writes.
- **Improved Security and Stability**: Separation increases system resilience.

### Integrating Domain Events

Adding Domain Events to the UserService will allow the system to react to significant domain changes more effectively. Benefits include:

- **Decoupling of Components**: Components react to events, reducing direct dependencies.
- **Reactivity and Side-Effect Management**: System becomes more reactive, managing side effects in response to events.
- **Audit Trail and Event Logging**: Automatic creation of an audit trail for significant state changes.
- **Ease of Extending Functionality**: Simplifies adding new features with minimal impact on existing code.
- **Integration with External Systems**: Facilitates communication with external systems or services.
