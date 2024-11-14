document.addEventListener("DOMContentLoaded", () => {
    const sidebarLoginButton = document.getElementById("sidebar-login-btn"); // Nút login trong sidebar
    const headerLoginButton = document.getElementById("header-login-btn"); // Nút login trong header
    const loginModal = document.getElementById("login-modal");
    const loginForm = document.getElementById("login-form");
    const mainContent = document.querySelector(".main-content");

    // Mở modal đăng nhập khi click vào nút login trong sidebar
    sidebarLoginButton.addEventListener("click", () => {
        loginModal.style.display = "flex";
    });

    // Mở modal đăng nhập khi click vào nút login trong header
    headerLoginButton.addEventListener("click", () => {
        loginModal.style.display = "flex";
    });

    // Xử lý khi người dùng submit form đăng nhập
    loginForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;

        const loginData = {
            username: username,
            password: password,
        };

        try {
            const response = await fetch("http://localhost:5000/api/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(loginData),
            });

            if (response.ok) {
                const data = await response.json();
                const token = data.Token;

                // Lưu token vào localStorage để sử dụng sau
                localStorage.setItem("jwtToken", token);

                // Ẩn modal đăng nhập
                loginModal.style.display = "none";

                // Hiển thị nội dung chính sau khi đăng nhập thành công
                mainContent.style.display = "block";
            } else {
                alert("Tên đăng nhập hoặc mật khẩu không đúng");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Có lỗi xảy ra, vui lòng thử lại sau.");
        }
    });

    // Ẩn modal khi người dùng bấm ra ngoài
    window.addEventListener("click", (e) => {
        if (e.target === loginModal) {
            loginModal.style.display = "none";
        }
    });
});