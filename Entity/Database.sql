
USE master;
GO

IF NOT EXISTS (
    SELECT [name] FROM sys.databases WHERE [name] = N'LearnAspNetCore'
)
CREATE DATABASE LearnAspNetCore;
GO

USE LearnAspNetCore;
GO
IF OBJECT_ID('[dbo].[users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[users];
GO

CREATE TABLE [dbo].[users]
(
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [name] NVARCHAR(255) NOT NULL,
    [gender] BIT NOT NULL DEFAULT 1,
    [birthday] DATE,
    [email] VARCHAR(255),
    [phone] VARCHAR(20),
    [address] NVARCHAR(500)
);
GO
IF OBJECT_ID('[dbo].[news]', 'U') IS NOT NULL
    DROP TABLE [dbo].[news];
GO

CREATE TABLE [dbo].[news]
(
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [title] NVARCHAR(255) NOT NULL,
    [content] NVARCHAR(255) NOT NULL,
    [author] NVARCHAR(255) NOT NULL
);
GO
IF OBJECT_ID('[dbo].[user_tokens]', 'U') IS NOT NULL
    DROP TABLE [dbo].[user_tokens];
GO

CREATE TABLE [dbo].[user_tokens]
(
    [Id] INT PRIMARY KEY,
    [user_id] INT NOT NULL,
    [token] VARCHAR(255) NOT NULL,
    CONSTRAINT FK_USER_TOKEN_USER_ID FOREIGN KEY ([user_id]) REFERENCES [users]([id])
);
GO
INSERT INTO [dbo].[news] (title, content, author)
VALUES 
(N'Tin tức công nghệ 2025', N'Công nghệ AI đang phát triển mạnh mẽ với các mô hình ngôn ngữ thế hệ mới.', N'Nguyễn Minh Tâm'),
(N'Apple ra mắt iPhone 17', N'iPhone 17 được trang bị chip A21 Bionic và màn hình ProMotion 165Hz.', N'Lê Hoàng Anh'),
(N'Không khí lạnh tràn về miền Bắc', N'Nhiệt độ Hà Nội giảm còn 18 độ, dự báo sẽ rét đậm trong vài ngày tới.', N'Phạm Thu Trang'),
(N'World Cup 2026 chuẩn bị khởi tranh', N'Giải đấu sẽ được tổ chức tại ba quốc gia: Mỹ, Canada và Mexico.', N'Trần Quang Huy'),
(N'Ra mắt game Sinh tồn Thế giới mở', N'Tựa game mới của hãng Việt Game Studio thu hút hàng triệu lượt tải.', N'Ngô Đức Dũng'),
(N'Tesla giới thiệu xe điện tự lái hoàn toàn', N'Elon Musk cho biết đây là bước tiến lớn trong công nghệ lái tự động.', N'Lê Hải Long'),
(N'Trường học ứng dụng AI trong giảng dạy', N'Nhiều trường đại học ở Việt Nam bắt đầu triển khai công cụ AI hỗ trợ sinh viên.', N'Hoàng Phương Thảo'),
(N'Khởi nghiệp xanh thu hút đầu tư', N'Nhiều startup môi trường gọi vốn thành công nhờ ý tưởng tái chế thông minh.', N'Đỗ Thành Công'),
(N'YouTube thử nghiệm chế độ không quảng cáo miễn phí', N'Người dùng sẽ được xem video liên tục mà không cần trả phí.', N'Nguyễn Bảo Nam'),
(N'Phát hiện hành tinh mới có khả năng sống được', N'Các nhà khoa học NASA công bố phát hiện hành tinh có khí quyển tương tự Trái Đất.', N'Vũ Thị Hạnh');
GO
