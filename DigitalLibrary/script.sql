USE [master]
GO
/****** Object:  Database [IslamicUloom]    Script Date: 19/05/2017 12:49:47 PM ******/
CREATE DATABASE [IslamicUloom]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IslamicUloom', FILENAME = N'E:\User DataBase Directory\IslamicUloom.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'IslamicUloom_log', FILENAME = N'E:\User DataBase Directory\IslamicUloom_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [IslamicUloom] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IslamicUloom].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IslamicUloom] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IslamicUloom] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IslamicUloom] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IslamicUloom] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IslamicUloom] SET ARITHABORT OFF 
GO
ALTER DATABASE [IslamicUloom] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IslamicUloom] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IslamicUloom] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IslamicUloom] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IslamicUloom] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IslamicUloom] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IslamicUloom] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IslamicUloom] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IslamicUloom] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IslamicUloom] SET  DISABLE_BROKER 
GO
ALTER DATABASE [IslamicUloom] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IslamicUloom] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IslamicUloom] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IslamicUloom] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IslamicUloom] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IslamicUloom] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IslamicUloom] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IslamicUloom] SET RECOVERY FULL 
GO
ALTER DATABASE [IslamicUloom] SET  MULTI_USER 
GO
ALTER DATABASE [IslamicUloom] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IslamicUloom] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IslamicUloom] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IslamicUloom] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [IslamicUloom] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'IslamicUloom', N'ON'
GO
USE [IslamicUloom]
GO
/****** Object:  Table [dbo].[Abwaabs]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Abwaabs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[BaabNo] [int] NULL,
	[NoOfPages] [int] NULL,
	[BookId] [int] NULL,
 CONSTRAINT [PK_Abwaabs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Authors]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Desigination] [nvarchar](50) NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BookMarks]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookMarks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[PageId] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_BookMarks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Books]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[AuthorId] [int] NULL,
	[PublisherId] [int] NULL,
	[PublishingYear] [date] NULL,
	[EditionNumber] [int] NULL,
	[BookCompleted] [bit] NULL,
	[NoOfPages] [int] NULL,
	[NoOfAbwaabs] [int] NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EditedBook]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EditedBook](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NULL,
	[UserId] [int] NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_EditedBook] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Pages]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BaabId] [int] NULL,
	[PageDetails] [nvarchar](max) NULL,
	[PageNumberDisplay] [nvarchar](50) NULL,
	[PageTag] [nvarchar](50) NULL,
	[BookId] [int] NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Publishers]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publishers](
	[Id] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 19/05/2017 12:49:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Password] [nvarchar](150) NULL,
	[EmailAddress] [varchar](150) NULL,
	[Role] [nvarchar](150) NULL,
	[UserPhone] [nvarchar](150) NULL,
	[Location] [nvarchar](150) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [IslamicUloom] SET  READ_WRITE 
GO
