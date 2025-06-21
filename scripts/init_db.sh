#!/usr/bin/env bash

# Simple database initialization script for local development/testing.
# Requires the MySQL client to be installed and a server accessible using the
# provided environment variables.

set -euo pipefail

HOST="${MYSQL_HOST:-localhost}"
PORT="${MYSQL_PORT:-3306}"
USER="${MYSQL_USER:-root}"
PASSWORD="${MYSQL_PASSWORD:-root}"
DB="${MYSQL_DATABASE:-booking}"

mysql -h "$HOST" -P "$PORT" -u "$USER" -p"$PASSWORD" <<SQL
CREATE DATABASE IF NOT EXISTS \`$DB\`;
USE \`$DB\`;

-- users table
CREATE TABLE IF NOT EXISTS users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  profile_picture VARCHAR(255) NULL,
  user_name VARCHAR(255) NULL,
  email VARCHAR(255) NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  email_verified BOOLEAN DEFAULT FALSE
);

-- packages table
CREATE TABLE IF NOT EXISTS packages (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  country VARCHAR(255) NOT NULL,
  credits INT NOT NULL,
  price DECIMAL(10,2) NOT NULL,
  expire_at DATETIME NOT NULL
);

-- class schedules
CREATE TABLE IF NOT EXISTS class_schedules (
  id INT AUTO_INCREMENT PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  country VARCHAR(255) NOT NULL,
  required_credits INT NOT NULL,
  start_time DATETIME NOT NULL,
  capacity INT NOT NULL
);

-- user_packages table
CREATE TABLE IF NOT EXISTS user_packages (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  package_id INT NOT NULL,
  remaining_credits INT NOT NULL,
  purchased_at DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(id),
  FOREIGN KEY (package_id) REFERENCES packages(id)
);

-- bookings table
CREATE TABLE IF NOT EXISTS bookings (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  class_schedule_id INT NOT NULL,
  canceled BOOLEAN DEFAULT FALSE,
  booked_at DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(id),
  FOREIGN KEY (class_schedule_id) REFERENCES class_schedules(id)
);

-- waitlists table
CREATE TABLE IF NOT EXISTS waitlists (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  class_schedule_id INT NOT NULL,
  added_at DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(id),
  FOREIGN KEY (class_schedule_id) REFERENCES class_schedules(id)
);

-- sample data
INSERT INTO users (email, password_hash, email_verified, user_name)
VALUES
  ('alice@example.com', 'pass', TRUE, 'Alice'),
  ('bob@example.com', 'pass', FALSE, 'Bob');

INSERT INTO packages (name, country, credits, price, expire_at)
VALUES
  ('Starter', 'US', 5, 9.99, DATE_ADD(NOW(), INTERVAL 30 DAY)),
  ('Pro', 'US', 10, 19.99, DATE_ADD(NOW(), INTERVAL 60 DAY));

INSERT INTO class_schedules (title, country, required_credits, start_time, capacity)
VALUES
  ('Yoga Basics', 'US', 1, DATE_ADD(NOW(), INTERVAL 1 DAY), 20),
  ('Advanced Pilates', 'US', 2, DATE_ADD(NOW(), INTERVAL 2 DAY), 15);

INSERT INTO user_packages (user_id, package_id, remaining_credits, purchased_at)
VALUES
  (1, 1, 5, NOW());

INSERT INTO bookings (user_id, class_schedule_id, canceled, booked_at)
VALUES
  (1, 1, FALSE, NOW());

INSERT INTO waitlists (user_id, class_schedule_id, added_at)
VALUES
  (1, 2, NOW());
SQL