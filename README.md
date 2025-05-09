# PostIt

*(description and project in progress)*

The project uses the default layered architecture, divided into the following levels: 

- **Api** is responsible for interacting with external sources.
- **Application** implements the system's use cases and coordinates the work between **Domain** and **Infrastructure**.
- **Contracts** contains contracts for communication with external sources.
- **Domain** describes business rules, entities, value objects and domain events.
- **Infrastcuture** contains the technical implementation of interfaces with the **Application**, access to the database, email, file systems.
 
