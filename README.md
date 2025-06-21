# Booking System API


This repository contains a minimal skeleton for the booking system API used in coding tests.
The project is a .NET 8 application with basic entities for users, packages, class schedules, bookings and waitlists.


--- 


The `booking-system/docs/ERD.png` file provides the database schema in 

[dbdiagram.io](https://dbdiagram.io/d/booking-system-661a925a03593b6b61e82214)

 format. You can copy the contents of that file into dbdiagram.io to visualize the relationships.


![Booking System ERD](booking-system/docs/ERD.png)


---

[User1, User2, ..., User10]  --->  [API] ---> [Acquire Lock] ---> [Read/Update Cache] ---> [Check Capacity]
                                                          |
                                                          V
                                  [Book (if available) or Waitlist (if full)] ---> [Release Lock]


  


## Running with Docker Compose

Use `docker-compose up` to start the API and a MySQL instance. The API will connect using the connection string defined in `docker-compose.yml`.

## Using an Environment Connection String

The application reads `ConnectionStrings__DefaultConnection` from the environment. If set, it will connect to that MySQL instance; otherwise an in-memory database is used. Example:

```bash
export ConnectionStrings__DefaultConnection="server=localhost;port=3306;database=booking;user=booking_user;password=booking_pass"
```

## Initialising the Database with Mock Data

A helper script is provided under `scripts/init_db.sh`. It creates the required tables and inserts some mock records. You can run it after the MySQL server is available:

```bash
MYSQL_USER=root MYSQL_PASSWORD=root bash scripts/init_db.sh
```

## Starting Redis with Docker

Redis is also configured in `docker-compose.yml`. Running `docker-compose up` will start a Redis container listening on port `6379`. You can start it manually with the helper script:

```bash
bash scripts/run_redis.sh
```




