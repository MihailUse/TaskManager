USE DataTestC;


INSERT INTO Role (name, description) VALUES 
('Owner', 'A person who created the project'),
('Administrator', 'A person who can add/remove members, edit entries, set the priority of tasks'),
('Tester', 'A person who can change the status of tasks and create new tasks'),
('Developer', 'A person who develops software');


INSERT INTO Status (name) VALUES 
('To Do'),
('In Progress'),
('To Test'),
('Testing'),
('Done');