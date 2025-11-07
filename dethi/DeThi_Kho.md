# ĐỀ THI GIỮA KỲ - MÔN LẬP TRÌNH BACK-END ASP.NET CORE
## Cấp độ: Khó

**Hình thức:** Báo cáo slide và demo sản phẩm.
**Thời gian:** 2 tuần.
**Điểm tối đa:** 9 điểm (Bonus để đạt 10 điểm).

---

### YÊU CẦU CƠ BẢN (Tối đa 9 điểm)

Xây dựng một ứng dụng ASP.NET Core Web API cho một blog đơn giản, bao gồm các chức năng **xác thực người dùng (Authentication)**, **phân quyền (Authorization)** và quản lý các thực thể có quan hệ nhiều-nhiều.

**1. Tạo Project và Cài đặt:**
   - Tạo một project ASP.NET Core Web API.
   - Cài đặt các gói NuGet cần thiết:
     - `Microsoft.EntityFrameworkCore.SqlServer` (hoặc `...Sqlite`).
     - `Microsoft.EntityFrameworkCore.Tools`.
     - `Microsoft.AspNetCore.Authentication.JwtBearer`.
     - `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.

**2. Thiết lập Models và Database:**
   - Sử dụng `IdentityUser` của ASP.NET Core Identity để quản lý người dùng.
   - Tạo các model sau:
     - `Post`:
       - `Id`, `Title`, `Content`, `PublishedDate`.
       - `AuthorId` (string): Khóa ngoại đến `IdentityUser`.
       - `Author` (IdentityUser): Người tạo bài viết.
       - `PostTags` (ICollection<PostTag>): Quan hệ nhiều-nhiều với `Tag`.
     - `Tag`:
       - `Id`, `Name`.
       - `PostTags` (ICollection<PostTag>): Quan hệ nhiều-nhiều với `Post`.
     - `PostTag` (Bảng trung gian):
       - `PostId`, `TagId`.
   - Cấu hình `AppDbContext` kế thừa từ `IdentityDbContext<IdentityUser>`.
   - Thiết lập quan hệ nhiều-nhiều giữa `Post` và `Tag` bằng Fluent API.
   - Sử dụng `EF Core Migrations` để tạo schema database.

**3. Xác thực và Phân quyền (Authentication & Authorization):**
   - **Endpoint xác thực:**
     - `POST /api/auth/register`: Đăng ký một tài khoản người dùng mới.
     - `POST /api/auth/login`: Đăng nhập và trả về một JWT (JSON Web Token).
   - **Bảo vệ Endpoints:**
     - Áp dụng `[Authorize]` attribute cho các endpoint quản lý bài viết.
     - Chỉ có người dùng đã đăng nhập mới có thể tạo bài viết.
   - **Phân quyền dựa trên vai trò và người sở hữu:**
     - Bất kỳ ai (kể cả chưa đăng nhập) cũng có thể xem danh sách bài viết và chi tiết bài viết.
     - Chỉ **tác giả** của bài viết mới có quyền **cập nhật** hoặc **xóa** bài viết đó.

**4. Xây dựng API Endpoints:**
   - **Quản lý Bài viết (Post):**
     - `GET /api/posts`: Lấy danh sách tất cả bài viết (chỉ cần thông tin cơ bản như Id, Title, AuthorName).
     - `GET /api/posts/{id}`: Lấy chi tiết một bài viết (bao gồm cả danh sách các `Tag` của nó).
     - `POST /api/posts`: Tạo một bài viết mới (yêu cầu xác thực). Khi tạo, có thể đính kèm một danh sách các `Tag` đã có hoặc tạo `Tag` mới.
     - `PUT /api/posts/{id}`: Cập nhật bài viết (chỉ tác giả mới có quyền).
     - `DELETE /api/posts/{id}`: Xóa bài viết (chỉ tác giả mới có quyền).
   - **Quản lý Tag:**
     - `GET /api/tags`: Lấy danh sách tất cả các `Tag`.
     - `GET /api/posts/by-tag/{tagName}`: Lấy danh sách các bài viết được gắn `Tag` tương ứng.

---

### YÊU CẦU BONUS (Tối đa 1 điểm - Tổng điểm tối đa 10)

**Triển khai vai trò "Admin" (1 điểm):**
   - Thêm vai trò (Role) "Admin" và "User" vào hệ thống Identity.
   - Implement logic để một người dùng có vai trò "Admin" có thể **xóa bất kỳ bài viết nào**, không phân biệt tác giả.
   - Tạo một endpoint `[Authorize(Roles = "Admin")]` để gán vai trò cho người dùng.

---

### YÊU CẦU BÁO CÁO

- Chuẩn bị một file slide (PowerPoint, Google Slides, ...) trình bày các nội dung sau:
  1. Giới thiệu tổng quan và mục tiêu của project.
  2. Sơ đồ cơ sở dữ liệu và giải thích các mối quan hệ (đặc biệt là quan hệ nhiều-nhiều).
  3. Trình bày luồng hoạt động của JWT Authentication (Register -> Login -> Access Protected Resource).
  4. Giải thích cách implement logic phân quyền (chủ sở hữu và vai trò Admin).
  5. Demo trực tiếp các API, bao gồm cả các trường hợp thành công và thất bại (ví dụ: user A cố gắng xóa bài của user B).
  6. (Nếu có) Trình bày chức năng bonus đã thực hiện.
