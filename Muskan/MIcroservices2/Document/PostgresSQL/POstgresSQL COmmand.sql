
-- CREATE TABLE Employee (
--     id INT  PRIMARY KEY,
--     first_name VARCHAR(50) NOT NULL,
--     last_name VARCHAR(50) NOT NULL,
--     email VARCHAR(100) NOT NULL UNIQUE,
--     dept VARCHAR(50),
--     salary DECIMAL(10,2) DEFAULT 10000.00,
--     city VARCHAR(100),
--     hire_date DATE NOT NULL DEFAULT CURRENT_DATE
-- );

-- INSERT  INTO employee (id ,first_name,last_name,email,dept,salary,city)
-- Values 
-- ( 1,'Shreya','Kumari','shreya@gmail.com','IT',30000.00,'Patna')
-- INSERT INTO Employee (id, first_name, last_name, email, dept, salary, city, hire_date) 
-- VALUES 
-- (2, 'Priya', 'Verma', 'priya.verma@example.com', 'HR', 11000.50, 'Mumbai', '2024-02-15'),
-- (3, 'Rajesh', 'Gupta', 'rajesh.gupta@example.com', 'Finance', 13000.75, 'Kolkata', '2024-01-20'),
-- (4, 'Sneha', 'Patel', 'sneha.patel@example.com', 'Marketing', 12500.00, 'Bangalore', '2024-03-05'),
-- (5, 'Vikram', 'Reddy', 'vikram.reddy@example.com', 'Sales', 11500.25, 'Hyderabad', '2024-02-28'),
-- (6, 'Anjali', 'Nair', 'anjali.nair@example.com', 'IT', 14000.00, 'Chennai', '2024-03-10'),
-- (7, 'Arjun', 'Mishra', 'arjun.mishra@example.com', 'HR', 10800.00, 'Pune', '2024-02-05'),
-- (8, 'Deepak', 'Yadav', 'deepak.yadav@example.com', 'Finance', 13500.50, 'Ahmedabad', '2024-01-30'),
-- (9, 'Kavita', 'Joshi', 'kavita.joshi@example.com', 'Marketing', 12750.00, 'Jaipur', '2024-02-20'),
-- (10, 'Manoj', 'Singh', 'manoj.singh@example.com', 'Sales', 11900.75, 'Lucknow', '2024-03-12'),
-- (11, 'Amit', 'Sharma', 'amit.sharma@example.com', 'IT', 45000.00, 'Delhi', '2024-01-18'),
-- (12, 'Neha', 'Bansal', 'neha.bansal@example.com', 'HR', 9000.00, 'Chandigarh', '2024-02-25'),
-- (13, 'Rahul', 'Tiwari', 'rahul.tiwari@example.com', 'Finance', 21000.00, 'Pune', '2024-03-02'),
-- (14, 'Pooja', 'Rana', 'pooja.rana@example.com', 'Marketing', 18500.75, 'Bhopal', '2024-01-28'),
-- (15, 'Suresh', 'Kumar', 'suresh.kumar@example.com', 'Sales', 9500.00, 'Guwahati', '2024-02-22'),
-- (16, 'Meena', 'Iyer', 'meena.iyer@example.com', 'IT', 32000.00, 'Kolkata', '2024-03-15'),
-- (17, 'Harish', 'Menon', 'harish.menon@example.com', 'HR', 8800.00, 'Goa', '2024-01-12'),
-- (18, 'Akash', 'Jha', 'akash.jha@example.com', 'Finance', 17500.50, 'Patna', '2024-02-08'),
-- (19, 'Swati', 'Saxena', 'swati.saxena@example.com', 'Marketing', 19800.00, 'Jaipur', '2024-03-20'),
-- (20, 'Vivek', 'Desai', 'vivek.desai@example.com', 'Sales', 26000.00, 'Surat', '2024-02-18'),
-- (21, 'Rekha', 'Bose', 'rekha.bose@example.com', 'IT', 48000.00, 'Mumbai', '2024-01-22'),
-- (22, 'Sahil', 'Kapoor', 'sahil.kapoor@example.com', 'HR', 8400.00, 'Hyderabad', '2024-02-13'),
-- (23, 'Monica', 'Das', 'monica.das@example.com', 'Finance', 24300.00, 'Bangalore', '2024-03-17'),
-- (24, 'Gaurav', 'Nanda', 'gaurav.nanda@example.com', 'Marketing', 27800.00, 'Ahmedabad', '2024-01-26'),
-- (25, 'Ishita', 'Roy', 'ishita.roy@example.com', 'Sales', 11800.00, 'Chennai', '2024-02-27'),
-- (26, 'Yogesh', 'Pillai', 'yogesh.pillai@example.com', 'IT', 50000.00, 'Delhi', '2024-03-08'),
-- (27, 'Anusha', 'Mishra', 'anusha.mishra@example.com', 'HR', 8200.00, 'Nagpur', '2024-01-30'),
-- (28, 'Sameer', 'Khan', 'sameer.khan@example.com', 'Finance', 31500.00, 'Indore', '2024-02-05'),
-- (29, 'Tanya', 'Sen', 'tanya.sen@example.com', 'Marketing', 26800.00, 'Lucknow', '2024-03-11'),
-- (30, 'Ravi', 'Joshi', 'ravi.joshi@example.com', 'Sales', 15800.00, 'Guwahati', '2024-02-14');

-- Select * from Employee;

-- Where Clause 
-- SELECT * FROM employee WHERE dept = 'IT';
-- SELECT * FROM employee WHERE dept = 'IT' and city = 'Patna';
-- SELECT * FROM employee WHERE salary >= 16000.00;

--Distinct 
-- SELECT Distinct dept , salary  From employee  ;
-- SELECT COUNT(DISTINCT dept) FROM employee;

--order by 
-- SELECT * FROM employee ORDER BY salary DESC;
-- SELECT * FROM employee ORDER BY salary;
-- SELECT * FROM employee ORDER BY first_name DESC;

-- Limit
-- SELECT * FROM employee LIMIT 5;
-- The OFFSET clause is used to specify where to start selecting the records to return.
-- SELECT * FROM employee LIMIT 20 OFFSET 5;

-- MIn
-- SELECT MIN(salary) FROM employee;




--  String Function 

--  Concate 
-- select concat(first_name , ' ' ,last_name) from employee
-- select * , concat(first_name , ' ' ,last_name) AS Full_name from employee;
--  Concate_WS

-- select  concat_ws(' ',  first_name , last_name ,salary) AS Full_name from employee;
--  SUBSTR
-- select substr('Hello World' , 1, 5)
--  Replace 
-- select replace('Hello World' , 'Hello', 'Hey')
-- select replace('dept' , 'IT', 'Tech') from employee
--  Reverse 
-- select reverse(first_name) from employee
--  LEFT
-- select left('helo world' , 4 )
--  RIGHT
-- select right('helo world' , 4 )
--  LENGTH
-- select Length('Hello')
-- select length(first_name) from employee
-- select length(dept) from employee
--  UPPER
-- select upper(first_name) from employee
--  LOWER
-- select lower(first_name) from employee
--  TRIM
-- select TRIM('   heloworld   ')
--  LTRIM
--  RTRIM
--  POSITION
-- select POSITION('OM' in  'Thomas')
-- select POSITION('om' in  'Thomas')
--  STRING_AGG

-- Task 1 
-- 1:Shreya:Kumari:IT
-- select concat_ws(':' , id ,first_name , last_name,dept)
--  from employee limit 1
-- 1:Shreya Kumari:IT:30000.00
-- select concat_ws(':' , id ,concat_ws(' ' , first_name , last_name),dept ,salary)
--  from employee limit 1

-- Task 2 
-- different type of department 
-- select distinct dept from employee
-- display max min salry 
-- select min(salary) from employee;
-- select max(salary) from employee;

--top 3 record
-- select * from employee limit 3;

-- select * from employee where  first_name LIke 'A%'

-- show recor where length of the last_name is dfour character
-- select * from employee where LENGTH(last_name) = 4;

-- find total number of emplouee
-- select count(id ) from employee
-- SELECT DISTINCT dept, COUNT(id) 
-- FROM employee 
-- GROUP BY dept;

-- SELECT SUM(salary) AS total_salary FROM employee where dept='IT';
-- SELECT distinct dept ,  SUM(salary) AS total_salary FROM employee Group by dept;

--Alter quey (Operation on tbale structure)
-- CREATE TABLE person (
--     id SERIAL PRIMARY KEY,
--     name VARCHAR(100) NOT NULL,
--     email VARCHAR(50) NOT NULL UNIQUE
-- );

-- insert into person ( id ,name ,email) 
-- values 
-- (1 , 'Sam Peter' , 'sam@gmail.com'),
-- (2 , 'John Doe' , 'john@gmail.com'),
-- (3 , 'Harry Poter' , 'harry@gmail.com')
-- Add a column
-- ALTER TABLE person 
-- ADD COLUMN age INT DEFAULT 0;
-- ?remove column 
-- ALTER TABLE person 
-- Drop column age;
-- Update column 
-- ALTER TABLE person 
-- Rename COLUMN age to userage;
-- Change data type
-- ALTER TABLE person 
-- ALTER COLUMN name
-- SET DATA TYPE VARCHAR(200)
-- set default 
--  ALTER TABLE person 
-- ALTER COLUMN name
-- SET DEFAULT 'Unknown'

--  ALTER TABLE person 
-- ALTER COLUMN name
-- DROP DEFAULT ;

-- select * from person ;
-- insert into person ( id ,email) 
-- values 
-- (4 , 'jammy@gmail.com')


--  ALTER TABLE person 
-- ADD COLUMN
--  mob VARCHAR(15)
--  CHeck (Length(mob) >= 10);
-- select * from person;
--  insert into person ( id ,name ,email, userage, mob) 
-- values 
-- (4 , 'Ram','ram@gmail.com' ,25,  1235)
--  insert into person ( id ,name ,email, userage, mob) 
-- values 
-- (5 , 'Sohan','Sohan@gmail.com' ,25,  12345467895)

-- CAse expression 
-- select first_name ,salary ,
-- CASE 
--   When salary >=12000 THen 'High salary' 
--   Else ' LOW'
--   END AS salary_category from employee;
--   select *from employee;
-- Select first_name ,salary ,
-- case 
--   when salary > 0 then ROund(salary*.10)
--   end As bonus 
--   from employee;

-- select 
-- CASE 
--   When salary >=20000 THen 'High' 
--   When salary <=20000 and salary > 10000 Then 'Mid'
--   Else ' Low'
--   END AS salary_category ,COUNT(id) 
--   from employee
--   GROUP BY salary_category;

-- select avg(salary) from employee;

-- Types of relationship 
-- One to One 

-- One to Many 

-- Many to Many

-- foreign Key 

-- Create table customer (
-- customer_id serial primary key ,
-- customer_name varchar (100) not null
-- );

-- Create table orders (
-- order_id serial primary key ,
-- order_date date not null,
-- price numeric not null,
-- customer_id integer not null,
-- Foreign key (customer_id) references
-- customer (customer_id)
-- );
-- INSERT INTO customer (customer_name) 
-- VALUES 
-- ('Amit Sharma'),
-- ('Neha Bansal'),
-- ('Rahul Tiwari'),
-- ('Pooja Rana'),
-- ('Suresh Kumar'),
-- ('Meena Iyer'),
-- ('Harish Menon'),
-- ('Akash Jha'),
-- ('Swati Saxena'),
-- ('Vivek Desai');

-- INSERT INTO orders (order_date, price, customer_id) 
-- VALUES 
-- ('2024-03-01', 160.50, 1),
-- ('2024-03-05', 550.00, 2),
-- ('2024-03-10', 9950.75, 3),
-- ('2024-03-15', 2200.00, 4),
-- ('2024-03-20', 1500.25, 5),
-- ('2024-03-22', 1780.40, 6),
-- ('2024-03-25', 2100.00, 7),
-- ('2024-03-28', 890.00, 8),
-- ('2024-03-30', 1330.75, 9),
-- ('2024-04-01', 2500.50, 10);

-- Joins
-- Cross joins
-- Every row from table is connected to row from another
-- SELECT * from customer Cross join orders;
-- Inner Joins
-- Return onl;y the rows where there is a much between the specified columns in both the left (or first ) and right (or second) tables
-- SELECT * from customer c 
-- Inner join
-- orders o
-- On c.customer_id = o.customer_id;
-- SELECT c.customer_name  ,COUNT (o.order_id) , sum(o.price) from customer c 
-- Inner join
-- orders o
-- On c.customer_id = o.customer_id
-- group by c.customer_name;
-- Left Joins
-- Return all rows from the left (or first) table and the matching rows from the right (or second) table
-- SELECT * from customer c 
-- Left join
-- orders o
-- On c.customer_id = o.customer_id

-- Right Joins

-- SELECT * from customer c 
-- Right join
-- orders o
-- On c.customer_id = o.customer_id

-- Many to Many
-- students
-- courses 
-- student_courses
-- create table students (
-- student_id serial primary key ,
-- name varchar(100) not null
-- )
-- insert into students(name)
-- values 
-- ('Raju'),(
-- 'Shyam'),('Sohan'),('Nancy'),('Sita'),('Rama')

-- create table courses (
-- course_id serial primary key ,
-- name varchar(100) not null,
-- fee numeric not null 
-- )
-- insert into courses(name , fee)
-- values 
-- ('Physics' , 1200),
-- ('Chemistry' , 1500),
-- ('Mathematics',1800 )

-- create table enrollment (
-- enrollment_id serial primary key ,
-- course_id  int not null,
-- student_id int  not null,
-- enrollment_date DATe Not null ,
-- Foreign key (student_id) references students (student_id),
-- Foreign key (course_id) references courses (course_id)
-- )

-- insert into enrollment(student_id , course_id ,enrollment_date )
-- values 
-- (2 , 3 ,'2024-03-02'),
-- (3 , 2 ,'2024-03-01'),
-- (1,3 ,'2024-05-10'),
-- (2 , 2 ,'2024-03-01'  ),
-- (3 , 1,'2024-01-11')

-- select s.name , c.name ,c.fee ,e.enrollment_date from enrollment e
-- join students s on   e.student_id = s.student_id 
-- join courses c on   e.course_id = c.course_id 
