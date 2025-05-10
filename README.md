# PostIt

*(description and project in progress)*

### Description

[>] The project is a blogging platform. Each user can share their impressions, ideas, etc. by creating posts and writing comments. 

### Architecture

The project uses the default layered architecture, divided into the following levels: 

- **Api** is responsible for interacting with external sources.
- **Application** implements the system's use cases and coordinates the work between **Domain** and **Infrastructure**.
- **Contracts** contains contracts for communication with external sources.
- **Domain** describes business rules, entities, value objects and domain events.
- **Infrastcuture** contains the technical implementation of interfaces with the **Application**, access to the database, email, file systems.

### Technical implementation

[>] **PostgresSQL** is used as the main database. **JWT Bearer** with access and refresh tokens is used for authentication. Refresh tokens are stored in the **Redis** database and can be revoked as needed. Password hashing is performed via **BCrypt**. The **MinIO** file system is also used to store user avatars.
