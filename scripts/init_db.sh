set -e

MYSQL_HOST=${MYSQL_HOST:-localhost}
MYSQL_PORT=${MYSQL_PORT:-3306}
MYSQL_USER=${MYSQL_USER:-root}
MYSQL_PASSWORD=${MYSQL_PASSWORD:-root}
MYSQL_DATABASE=${MYSQL_DATABASE:-booking}

mysql -h "$MYSQL_HOST" -P "$MYSQL_PORT" -u "$MYSQL_USER" -p"$MYSQL_PASSWORD" <<SQL
CREATE DATABASE IF NOT EXISTS \`$MYSQL_DATABASE\`;
USE \`$MYSQL_DATABASE\`;

CREATE TABLE IF NOT EXISTS users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  email VARCHAR(255) NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  email_verified BOOLEAN DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS packages (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  country VARCHAR(255) NOT NULL,
  credits INT NOT NULL,
  price DECIMAL(10,2) NOT NULL,
  expire_at DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS class_schedules (
  id INT AUTO_INCREMENT PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  country VARCHAR(255) NOT NULL,
  required_credits INT NOT NULL,
  start_time DATETIME NOT NULL,
  capacity INT NOT NULL
);

CREATE TABLE IF NOT EXISTS user_packages (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  package_id INT NOT NULL,
  remaining_credits INT NOT NULL,
  purchased_at DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(id),
  FOREIGN KEY (package_id) REFERENCES packages(id)
);

CREATE TABLE IF NOT EXISTS bookings (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  class_schedule_id INT NOT NULL,
  canceled BOOLEAN DEFAULT FALSE,
  booked_at DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(id),
  FOREIGN KEY (class_schedule_id) REFERENCES class_schedules(id)
);

CREATE TABLE IF NOT EXISTS waitlists (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  class_schedule_id INT NOT NULL,
  added_at DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(id),
  FOREIGN KEY (class_schedule_id) REFERENCES class_schedules(id)
);

INSERT INTO users (email, password_hash, email_verified) VALUES
('alice@example.com', 'hash1', TRUE),
('bob@example.com', 'hash2', TRUE);

INSERT INTO packages (name, country, credits, price, expire_at) VALUES
('Starter', 'US', 10, 100.00, DATE_ADD(NOW(), INTERVAL 1 YEAR)),
('Premium', 'US', 50, 400.00, DATE_ADD(NOW(), INTERVAL 1 YEAR));

INSERT INTO class_schedules (title, country, required_credits, start_time, capacity) VALUES
('Yoga', 'US', 1, DATE_ADD(NOW(), INTERVAL 1 DAY), 20),
('Pilates', 'US', 1, DATE_ADD(NOW(), INTERVAL 2 DAY), 15);

INSERT INTO user_packages (user_id, package_id, remaining_credits, purchased_at) VALUES
(1, 1, 10, NOW()),
(2, 2, 50, NOW());

INSERT INTO bookings (user_id, class_schedule_id, canceled, booked_at) VALUES
(1, 1, FALSE, NOW()),
(2, 2, FALSE, NOW());

INSERT INTO waitlists (user_id, class_schedule_id, added_at) VALUES
(1, 2, NOW());
SQL