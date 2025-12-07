(function ($) {
    "use strict";

    // Khởi tạo ScrollReveal với các tùy chọn chung
    const sr = ScrollReveal({
        origin: 'top',
        distance: '60px',
        duration: 2500,
        delay: 400,
        reset: false // Điều này sẽ làm cho hiệu ứng tái diễn khi cuộn lên/xuống
    });

    // Áp dụng hiệu ứng cho các phần tử
    sr.reveal('.top-info', { origin: 'left' });
    sr.reveal('.top-link', { origin: 'right' });
    sr.reveal('.fa-user', { delay: 700});
    sr.reveal('.footer', { origin: 'bottom' });
    sr.reveal(`h4`, { delay: 200 });
    sr.reveal(`h2`, { delay: 300 });
    sr.reveal(`h1`, { delay: 400 });
    sr.reveal(`p`, { delay: 500 });
    sr.reveal(`.fe-box`, { origin: 'top' });
    sr.reveal(`.pro`, { opacity: 0.2 })
    sr.reveal(`.banner-box`, { origin: 'left' });
    sr.reveal(`.banner-box2`, { origin: 'right' });


    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner(0);
 


    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow');
            } else {
                $('.fixed-top').removeClass('shadow');
            }
        } else {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow').css('top', -55);
            } else {
                $('.fixed-top').removeClass('shadow').css('top', 0);
            }
        } 
    });
    
    
   // Back to top button
   $(window).scroll(function () {
    if ($(this).scrollTop() > 300) {
        $('.back-to-top').fadeIn('slow');
    } else {
        $('.back-to-top').fadeOut('slow');
    }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Testimonial carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 2000,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav : true,
        navText : [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:1
            },
            992:{
                items:2
            },
            1200:{
                items:2
            }
        }
    });


    // vegetable carousel
    $(".vegetable-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav : true,
        navText : [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            },
            1200:{
                items:4
            }
        }
    });


    // Modal Video
    $(document).ready(function () {
        var $videoSrc;
        $('.btn-play').click(function () {
            $videoSrc = $(this).data("src");
        });
        console.log($videoSrc);

        $('#videoModal').on('shown.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc + "?autoplay=1&amp;modestbranding=1&amp;showinfo=0");
        })

        $('#videoModal').on('hide.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc);
        })
    });



    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });


    $(document).ready(function () {
        const suggestionsContainer = $("#suggestionsBox");
        const searchBox = $("#searchBox");
        let debounceTimer;

        // === Hiển thị lịch sử khi focus và input rỗng ===
        searchBox.on("focusin", function () {
            if ($(this).val().trim().length === 0) {
                // Gọi API lấy lịch sử
                $.get("/Shop/History")
                    .done(function (data) {
                        suggestionsContainer.empty();

                        if (data.length === 0) {
                            suggestionsContainer.hide();
                            return;
                        }

                        data.forEach(item => {
                            const historyItem = `
                            <div class="list-group-item list-group-item-action history-item d-flex justify-content-between align-items-center" data-id="${item.id}">
                                <div class="d-flex align-items-center">
                                    <i class="fa fa-history me-2 text-muted"></i>
                                    <span class="keyword-text">${item.keyword}</span>
                                </div>
                                <button class="btn btn-sm btn-link text-danger btn-delete-history" style="text-decoration: none;">✕</button>
                            </div>`;

                            suggestionsContainer.append(historyItem);
                        });

                        suggestionsContainer.show();
                    })
                    .fail(function (xhr) {
                        console.error("History error:", xhr.status, xhr.statusText);
                    });
            }
        });

        // === Gợi ý sản phẩm khi có ký tự nhập ===
        searchBox.on("input", function () {
            const query = $(this).val().trim();

            clearTimeout(debounceTimer);

            if (query.length === 0) {
                suggestionsContainer.empty().hide();
                return;
            }

            debounceTimer = setTimeout(() => {
                $.get(`/Shop/SuggestSearch?query=${query}`)
                    .done(function (data) {
                        suggestionsContainer.empty();

                        if (data.length === 0) {
                            const noResult = `
                            <div class="list-group-item list-group-item-action">
                                <div class="d-flex align-items-center">
                                    <p>Không tìm thấy sản phẩm nào!</p>
                                </div>
                            </div>`;
                            suggestionsContainer.append(noResult).show();
                            return;
                        }

                        data.forEach(item => {
                            const suggestionItem = `
                                <div class="list-group-item list-group-item-action suggestion-item" data-id="${item.maHh}" data-name="${item.tenHh}">
                                    <div class="d-flex align-items-center">
                                        <img src="/Hinh/HangHoa/${item.hinh}" alt="${item.tenHh}" width="50" height="50" class="me-3"/>
                                        <div>
                                            <h6 class="mb-0">${item.tenHh}</h6>
                                            <small class="text-muted">${item.donGia.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</small>
                                        </div>
                                    </div>
                                </div>
                            `;
                            suggestionsContainer.append(suggestionItem);
                        });

                        suggestionsContainer.show();
                    })
                    .fail(function (xhr) {
                        console.error("Suggest error:", xhr.status, xhr.statusText);
                        suggestionsContainer.empty().hide();
                    });
            }, 300);
        });
       
        // Click vào mục lịch sử để chọn
        suggestionsContainer.on("click", ".history-item", function (e) {
            // Nếu là nút X thì chỉ xoá, không làm gì khác
            if ($(e.target).hasClass("btn-delete-history")) {
                e.preventDefault();      // Ngăn hành vi mặc định (submit form)
                e.stopPropagation();     // Ngăn sự kiện lan ra ngoài .history-item

                const historyItem = $(this).closest(".history-item");
                const id = historyItem.data("id");

                $.ajax({
                    url: "/Shop/DeleteHistory",
                    method: "POST",
                    data: { id },
                    success: function () {
                        historyItem.remove(); // Xoá khỏi giao diện
                    },
                    error: function () {
                        alert("Không thể xoá lịch sử tìm kiếm.");
                    }
                });

                return; // Dừng lại, không chọn keyword
            }

            // Nếu không phải click nút X thì xử lý chọn keyword
            const selectedText = $(this).find(".keyword-text").text();
            searchBox.val(selectedText);
            suggestionsContainer.empty().hide();
      
        });

        // Click vào mục gợi ý để chọn
        // Click vào sản phẩm gợi ý để đưa tên lên ô input
        suggestionsContainer.on("click", ".suggestion-item", function () {
            const selectedProductName = $(this).data("name"); // Lấy tên từ data-name
            searchBox.val(selectedProductName); // Gán vào ô input

            suggestionsContainer.empty().hide(); // Ẩn danh sách gợi ý
        });



        $(document).on("mousedown", function (event) {
            // Nếu click không phải vào input hoặc vào danh sách gợi ý
            if (!$(event.target).closest('#searchBox, #suggestionsBox').length) {
                $("#suggestionsBox").empty().hide();
            }
        });
    });

    


})(jQuery);

