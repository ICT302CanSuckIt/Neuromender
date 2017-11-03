ALTER TABLE users
ADD EnabledEAlerts tinyint(4);
ALTER TABLE users
ADD CHECK ((EnabledEAlerts = 0) OR (EnabledEAlerts = 1));
ALTER TABLE users
Alter EnabledEAlerts SET DEFAULT 0;