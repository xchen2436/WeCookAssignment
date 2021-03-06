USE [master]
GO
/****** Object:  Database [SchoolDB]    Script Date: 2022-07-12 10:11:30 PM ******/
CREATE DATABASE [SchoolDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SchoolDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER2017\MSSQL\DATA\SchoolDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SchoolDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER2017\MSSQL\DATA\SchoolDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [SchoolDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SchoolDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SchoolDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SchoolDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SchoolDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SchoolDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SchoolDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [SchoolDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SchoolDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SchoolDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SchoolDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SchoolDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SchoolDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SchoolDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SchoolDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SchoolDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SchoolDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SchoolDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SchoolDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SchoolDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SchoolDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SchoolDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SchoolDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SchoolDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SchoolDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SchoolDB] SET  MULTI_USER 
GO
ALTER DATABASE [SchoolDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SchoolDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SchoolDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SchoolDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SchoolDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SchoolDB] SET QUERY_STORE = OFF
GO
USE [SchoolDB]
GO
/****** Object:  Table [dbo].[Marks]    Script Date: 2022-07-12 10:11:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marks](
	[StudentId] [int] NOT NULL,
	[Subject] [nvarchar](50) NULL,
	[Marks] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 2022-07-12 10:11:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Class] [nvarchar](50) NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Marks] ([StudentId], [Subject], [Marks]) VALUES (1, N'PHP', 98)
INSERT [dbo].[Marks] ([StudentId], [Subject], [Marks]) VALUES (5, N'C#', 70)
GO
INSERT [dbo].[Students] ([StudentId], [Name], [Username], [Password], [Class]) VALUES (1, N'Simon', N'simon01', N'123', N'PHP')
INSERT [dbo].[Students] ([StudentId], [Name], [Username], [Password], [Class]) VALUES (2, N'Bill', N'qwer123', N'123', N'Java')
INSERT [dbo].[Students] ([StudentId], [Name], [Username], [Password], [Class]) VALUES (3, N'Joe', N'Joe0505', N'123', N'Java')
INSERT [dbo].[Students] ([StudentId], [Name], [Username], [Password], [Class]) VALUES (4, N'Xiao', N'Xiaoxx', N'abcdefg', N'C#')
GO
/****** Object:  StoredProcedure [dbo].[SP_Mark]    Script Date: 2022-07-12 10:11:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Mark]
	@StudentId int,
	@Subject nvarchar(50),
	@Marks nvarchar(50),
	@OperationType int
AS
BEGIN TRAN
	IF(@OperationType = 1)
	BEGIN
	IF(@StudentId = 0)
		BEGIN 
			ROLLBACK
				RAISERROR ('Can not find this student !!!',16,1);
			RETURN
		END
	INSERT INTO [Marks](StudentId, [Subject], Marks) VALUES (@StudentId,@Subject,@Marks)
	END

	ELSE IF (@OperationType = 2)
	BEGIN 
		IF(@StudentId = 0)
		BEGIN 
			ROLLBACK
				RAISERROR ('Can not find this student !!!',16,1);
			RETURN
		END
		UPDATE [Marks] SET [Subject] = @Subject, Marks = @Marks WHERE StudentId = @StudentId
	END

	ELSE IF (@OperationType = 3)
	BEGIN 
		IF(@StudentId = 0)
		BEGIN 
			ROLLBACK
				RAISERROR ('Can not find this student !!!',16,1);
			RETURN
		END
		DELETE FROM [Marks] WHERE StudentId = @StudentId
	END
COMMIT TRAN
GO
/****** Object:  StoredProcedure [dbo].[SP_Student]    Script Date: 2022-07-12 10:11:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Student]
	@StudentId int,
	@Name nvarchar(50),
	@Username nvarchar(50),
	@Password nvarchar(50),
	@Class nvarchar(50), 
	@OperationType int
AS
BEGIN TRAN
	IF(@OperationType = 1) --Insert
	BEGIN
		IF EXISTS(SELECT * FROM [Students] WHERE Username=@Username)
	BEGIN
		ROLLBACK
			RAISERROR ('This username already exist !!!',16,1);
		RETURN
	END
	SET @StudentId= (SELECT COUNT(*) FROM [Students]) + 1
	INSERT INTO [Students] (StudentId, [Name], Username, [Password], Class) VALUES (@StudentId,@Name,@Username,@Password,@Class)
	SELECT * FROM [Students] WHERE StudentId = @StudentId
	END

	ELSE IF (@OperationType = 2)
	BEGIN 
		IF(@StudentId = 0)
		BEGIN 
			ROLLBACK
				RAISERROR ('Can not find this student !!!',16,1);
			RETURN
		END
		UPDATE [Students] SET Password = @Password, Class = @Class WHERE StudentId = @StudentId
		SELECT * FROM [Students] WHERE StudentId = @StudentId
	END

	ELSE IF (@OperationType = 3)
	BEGIN 
		IF(@StudentId = 0)
		BEGIN 
			ROLLBACK
				RAISERROR ('Can not find this student !!!',16,1);
			RETURN
		END
		DELETE FROM [Students] WHERE StudentId = @StudentId
	END
COMMIT TRAN
GO
USE [master]
GO
ALTER DATABASE [SchoolDB] SET  READ_WRITE 
GO
