CREATE DATABASE QlyBongDa

CREATE TABLE Account
(
	IdAccount int  primary key,
	Username varchar(50) not null,
	Password varchar (50) not null,
	Type int
)


CREATE TABLE Employee
(
	IdEmployee int  primary key,
	Name nvarchar(50),
	Gender nvarchar(5),
	Phonenumber varchar(11),
	Address nvarchar(30),
	DateOfBirth date,
	Position nvarchar(20),
	Startingdate date,
	IdAccount int,
	ImageFile text,
	IsDeleted int,
)
CREATE TABLE FieldInfo
(
	IdFieldInfo int primary key,
	IdField int,
	StartingTime datetime,
	EndingTime datetime,
	Status int,
	PhoneNumber varchar(11),
	CustomerName nvarchar(50),
	Note text,
	Discount bigint,
	Price bigint
	--status: 1 là sân đã đặt, 2 là sân đã checkin( đang đá), 3 là sân đá xong đợi thanh toán
)
CREATE TABLE FootballField
(
	IdField int  primary key,
	Name varchar(50),
	Type int,
	Status int,
	Note text,
	IsDeleted int
	--isdeleted: 1 là đã xóa, 0 là bth
)
CREATE TABLE Goods
(
	IdGoods int primary key,
	Name nvarchar(50),
	Unit nvarchar(20),
	UnitPrice bigint,
	ImageFile text,
	Quantity int,
	IsDeleted int
)
CREATE TABLE Salary
(	
	IdEmployee int ,
	NumOfShift int,
	NumOfFault int,
	TotalSalary bigint,
	SalaryMonth date,
	IdSalaryRecord int,
	primary key(IdEmployee,IdSalaryRecord)
)
CREATE TABLE SalaryRecord
(
	IdSalaryRecord int  primary key,
	SalaryRecordDate datetime,
	Total bigint,
	IdAccount int
)
CREATE TABLE SalarySetting
(
	SalaryBase bigint,
	MoneyPerShift bigint,
	MoneyPerFault bigint,
	TypeEmployee nvarchar(20) primary key,
	StandardWorkDays int
)
CREATE TABLE StockReceipt
(
	IdStockReceipt int  primary key,
	IdAccount int,
	DateTimeStockReceipt datetime,
	Total bigint
)
CREATE TABLE StockReceiptInfo
(
	IdStockReceipt int,
	IdGoods int,
	Quantity int,
	ImportPrice bigint,
	PRIMARY KEY (IdStockReceipt,IdGoods)
)
CREATE TABLE TimeFrame
(
	Id int  primary key,
	StartTime varchar(10) not null,
	EndTime varchar(10) not null,
	FieldType int,
	Price bigint,
)

ALTER TABLE Salary
ADD CONSTRAINT FK_idSalaryRecord FOREIGN KEY(IdSalaryRecord) REFERENCES SalaryRecord(IdSalaryRecord)

ALTER TABLE FieldInfo
ADD CONSTRAINT FK_FieldInfo_IdField FOREIGN KEY(IdField) REFERENCES FootballField(IdField)


ALTER TABLE StockReceipt 
ADD CONSTRAINT FK_StockReceipt_IdAccount FOREIGN KEY(IdAccount) REFERENCES Account(IdAccount)
ALTER TABLE StockReceiptInfo
ADD CONSTRAINT FK_StockReceiptInfo_IdStockReceipt FOREIGN KEY(IdStockReceipt) REFERENCES StockReceipt(IdStockReceipt)
ALTER TABLE StockReceiptInfo
ADD CONSTRAINT FK_StockReceiptInfo_IdGoodsReceipt FOREIGN KEY(IdGoods) REFERENCES Goods(IdGoods)
ALTER TABLE Employee
ADD CONSTRAINT FK_Employee_IdAccount FOREIGN KEY(IdAccount) REFERENCES Account(IdAccount)
ALTER TABLE Salary
ADD CONSTRAINT FK_Salary_idEmployee FOREIGN KEY (IdEmployee) REFERENCES Employee(IdEmployee) 
ALTER TABLE SalaryRecord
ADD CONSTRAINT FK_SalaryRecord_idAccount FOREIGN KEY (IdAccount) REFERENCES Account(IdAccount)


INSERT INTO Account VALUES
(1, 'admin', '202CB962AC59075B964B07152D234B70', 0)
SELECT * FROM Account



