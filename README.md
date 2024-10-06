# Rateify

## Setup

You must have docker installed on your computer.

For start the project, open your favorite cli, go to project root folder and type this;

#### docker compose up -d

## Usage

Rating service swagger url
http://localhost:5205/swagger/index.html

Notification service swagger url
http://localhost:5206/swagger/index.html

Seq url
http://localhost:8081/#/events

## Example Requests

### Rating Service

```
curl -X 'GET' \
  'http://localhost:5205/api/providers/2/average-rating' \
  -H 'accept: text/plain'
```

```
curl -X 'POST' \
  'http://localhost:5205/api/ratings' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "point": 5,
  "providerId": 1,
  "customerId": 1
}'

```

### Notification Service

```
curl -X 'GET' \
  'http://localhost:5206/api/notifications' \
  -H 'accept: text/plain'
```

## Notes

The system contains two microservices. The rating service generates a message after create rating and the notification service listens to this message. At the same time, there is a consumer in the rating service project that listens to this message. 

By listening to the message on the notification service side, it is understood that a new rating has been created and it is recorded in Redis. When the endpoint in this service is called, all records in the redis are returned from response. When the call ends, the records in the redis are deleted so records does not return from the endpoint again. 

On the Rating service side, when this message is listened, the average score of the Provider is calculated. I chose an asynchronous method for the health of the system. I aimed to prevent future loads and race conditions. At the same time, I applied denormalization on the database side to handle the computational load easily.


## Possible Improvements

Provider filter can be added to the endpoint on the notification service side. This is a business improvement but i wasn't sure to add. Also, normally there should not be a project called Contracts, but I added this for demo purposes. In a real world scenario, microservices should be completely seperate from each other. Also we should add ci/cd. 

## Libraries & Technologies

- .Net 8
- PostgreSQL
- RabbitMq
- MassTransit
- Redis
- Docker
- EF 8
- Swagger
- Serilog
- Seq
- Testcontainers
- Fixture for Tests
- XUnit
- Moq

