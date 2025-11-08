# ĐỀ THI GIỮA KỲ - MÔN LẬP TRÌNH BACK-END ASP.NET CORE
## Cấp độ: Vừa

**Hình thức:** Báo cáo slide và demo sản phẩm.
**Thời gian:** 2 tuần.
**Điểm tối đa:** 7 điểm (Bonus để đạt 9 điểm).

---

### YÊU CẦU CƠ BẢN (Tối đa 7 điểm)

Xây dựng một ứng dụng ASP.NET Core Web API để quản lý **Sinh viên (Student)** và **Lớp học (Class)**, sử dụng Entity Framework Core để kết nối với cơ sở dữ liệu.

**1. Tạo Project và Cài đặt:**
   - Tạo một project ASP.NET Core Web API mới.
   - Cài đặt các gói NuGet cần thiết:
     - `Microsoft.EntityFrameworkCore.SqlServer` (hoặc `...Sqlite`).
     - `Microsoft.EntityFrameworkCore.Tools`.

**2. Tạo Models:**
   - Tạo 2 class model:
     - `Class`:
       - `Id` (int): Mã lớp.
       - `Name` (string): Tên lớp (ví dụ: "23CT112", "22CT211").
       - `Students` (ICollection<Student>): Danh sách sinh viên thuộc lớp này.
     - `Student`:
       - `Id` (int): Mã sinh viên.
       - `Name` (string): Tên sinh viên.
       - `DateOfBirth` (DateTime): Ngày sinh.
       - `ClassId` (int): Khóa ngoại tham chiếu đến `Class`.
       - `Class` (Class): Đối tượng `Class` mà sinh viên thuộc về.

**3. Cấu hình Database:**
   - Tạo `AppDbContext` kế thừa từ `DbContext`.
   - Khai báo các `DbSet` cho `Student` và `Class`.
   - Cấu hình chuỗi kết nối trong `appsettings.json`.
   - Sử dụng `EF Core Migrations` để tạo cơ sở dữ liệu và các bảng tương ứng.

**4. Xây dựng API Endpoints:**
   - **Quản lý Lớp học:**
     - `GET /api/classes`: Lấy danh sách tất cả các lớp.
     - `POST /api/classes`: Tạo một lớp học mới.
   - **Quản lý Sinh viên:**
     - `POST /api/students`: Thêm một sinh viên mới vào một lớp đã có.
     - `GET /api/students`: Lấy danh sách tất cả sinh viên.
     - `GET /api/classes/{classId}/students`: Lấy danh sách tất cả sinh viên thuộc một lớp cụ thể.
     - `PUT /api/students/{id}`: Cập nhật thông tin sinh viên (không cho phép đổi lớp).

**5. Kiểm thử:**
   - Sử dụng Swagger UI hoặc Postman để kiểm tra và demo các chức năng của API.

---

### YÊU CẦU BONUS (Tối đa 2 điểm - Tổng điểm tối đa 9)

**1. Sử dụng DTO (Data Transfer Object) (1 điểm):**
   - Tạo các DTO (ví dụ: `StudentDto`, `CreateStudentDto`, `ClassDto`) để tách biệt model của API với model của Entity.
   - Áp dụng AutoMapper (hoặc map thủ công) để chuyển đổi giữa Entity và DTO trong các controller. Điều này giúp che giấu cấu trúc database và tránh lỗi "object cycle".

**2. Phân trang (Pagination) (1 điểm):**
   - Implement chức năng phân trang cho endpoint lấy danh sách sinh viên:
     - `GET /api/students?pageNumber=1&pageSize=10`: Trả về danh sách sinh viên theo số trang (`pageNumber`) và kích thước trang (`pageSize`).
   - Response nên bao gồm thông tin về tổng số trang, tổng số mục, trang hiện tại.

---

### YÊU CẦU BÁO CÁO

- Chuẩn bị một file slide (PowerPoint, Google Slides, ...) trình bày các nội dung sau:
  1. Giới thiệu tổng quan về project.
  2. Cấu trúc project, giải thích về Models, DbContext, Controllers.
  3. Trình bày cách thiết lập Entity Framework Core và Migrations.
  4. Demo trực tiếp các API đã xây dựng, cho thấy sự tương tác giữa các đối tượng.
  5. (Nếu có) Trình bày về DTO và Pagination đã implement.