# PostIt

*(description and project in progress)*

### Description

[>] The project is a blogging platform. Each user can share their impressions, ideas, etc. by creating posts and writing comments. 

### Architecture

The project uses the default layered clean architecture, divided into the following levels: 

- **Application** implements the system's use cases and coordinates the work between **Domain** and **Infrastructure**.
- **Common** contains common components for all layers.
- **Contracts** contains contracts for communication with external sources.
- **Domain** describes business rules, entities and value object.
- **Hosts** is responsible for interacting with external sources.
- **Infrastructure** contains the technical implementation of interfaces with the **Application**, access to the database, email, file systems.
- **Migrations** Contains scripts that describe the structures of the database schema.

### Technical implementation

[>] **PostgreSQL** is used as the primary database. **JWT Bearer** with access and refresh tokens is used for authentication. Refresh tokens are stored in the **Redis** database and can be revoked as needed. Password hashing is implemented using **BCrypt**. The **MinIO** file system is utilized for storing user avatars. Background tasks are managed with **Hangfire**. Logging is handled via **Serilog** and **OpenSearch**. **Papercut** is used to simulate the email service.

### Setup

Clone this repository:
```
git clone https://github.com/1args/PostIt
```
For the necessary conditions, you need to have [Docker](https://www.docker.com/) installed:
```
cd localhost
docker compose up -d
```
The API is then accessed via http://localhost:5217/. 

