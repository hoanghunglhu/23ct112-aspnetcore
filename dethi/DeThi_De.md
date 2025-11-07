# ĐỀ THI GIỮA KỲ - MÔN LẬP TRÌNH BACK-END ASP.NET CORE
## Cấp độ: Dễ

**Hình thức:** Báo cáo slide và demo sản phẩm.
**Thời gian:** 2 tuần.
**Điểm tối đa:** 5 điểm (Bonus để đạt 8 điểm).

---

### YÊU CẦU CƠ BẢN (Tối đa 5 điểm)

Xây dựng một ứng dụng ASP.NET Core Web API để quản lý danh sách các **Sản phẩm (Product)**.

**1. Tạo Project:**
   - Tạo một project ASP.NET Core Web API mới.

**2. Tạo Model:**
   - Tạo một class `Product` với các thuộc tính sau:
     - `Id` (int): Mã sản phẩm (tự động tăng).
     - `Name` (string): Tên sản phẩm.
     - `Price` (double): Giá sản phẩm.
     - `Description` (string): Mô tả ngắn về sản phẩm.

**3. Lưu trữ dữ liệu:**
   - Sử dụng một danh sách tĩnh (`List<Product>`) trong một class service để lưu trữ dữ liệu. **Không cần kết nối cơ sở dữ liệu.**

**4. Xây dựng API Endpoints:**
   - Implement đầy đủ các chức năng CRUD (Create, Read, Update, Delete) thông qua các API endpoint:
     - `GET /api/products`: Lấy về danh sách tất cả sản phẩm.
     - `GET /api/products/{id}`: Lấy về thông tin một sản phẩm theo `Id`.
     - `POST /api/products`: Thêm một sản phẩm mới.
     - `PUT /api/products/{id}`: Cập nhật thông tin một sản phẩm đã có.
     - `DELETE /api/products/{id}`: Xóa một sản phẩm.

**5. Kiểm thử:**
   - Sử dụng Swagger UI (có sẵn trong project) hoặc Postman để kiểm tra và demo các chức năng của API.

---

### YÊU CẦU BONUS (Tối đa 3 điểm - Tổng điểm tối đa 8)

**1. Data Validation (1.5 điểm):**
   - Áp dụng các `Data Annotations` cho model `Product`:
     - `Name`: Bắt buộc phải có (`Required`) và có độ dài tối đa 100 ký tự (`StringLength`).
     - `Price`: Phải là một giá trị lớn hơn 0 (`Range`).
   - API phải trả về lỗi `400 Bad Request` kèm thông báo rõ ràng nếu dữ liệu đầu vào không hợp lệ.

**2. Chức năng tìm kiếm (1.5 điểm):**
   - Implement một endpoint mới:
     - `GET /api/products/search?name={keyword}`: Tìm kiếm và trả về danh sách các sản phẩm có tên chứa `keyword` (không phân biệt chữ hoa, chữ thường).

---

### YÊU CẦU BÁO CÁO

- Chuẩn bị một file slide (PowerPoint, Google Slides, ...) trình bày các nội dung sau:
  1. Giới thiệu tổng quan về project.
  2. Cấu trúc project và giải thích vai trò của các thành phần (Controller, Model, Service).
  3. Trình bày cách implement các chức năng CRUD.
  4. Demo trực tiếp các API đã xây dựng.
  5. (Nếu có) Trình bày các chức năng bonus đã thực hiện.
