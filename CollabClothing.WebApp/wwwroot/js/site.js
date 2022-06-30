// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var SiteController = function () {
	this.initialize = function () {
		loadCartData();
		CartAddSite();
		removeProductCart();
		selectSize();
		//ButtonLoadMore();
	}
	function removeProductCart() {
		$('body').on('click', '.header__cart-remove', function (e) {
			e.preventDefault();
			const id = $(this).data('id');
			const quantity = 0;
			$.ajax({
				type: "POST",
				url: '/Cart/UpdateCart',
				data: {
					id: id,
					quantity: quantity
				},
				success: function (response) {
					const count = response.reduce((item, object) => {
						return item + object.quanitty;
					}, 0);
					loadCartData();//load lai cart update price
					$('#count-cart-quantity').text(count);
				},
				error: function (err) {
					console.log(err)
				},
			})
		});
	}
	function loadCartData() {
		$.ajax({
			type: "GET",
			url: '/Cart/GetCart',
			success: function (response) {
				if (response.length === 0) {
					$('#no-product-in-cart').css('display', 'block')
				}
				const count = response.reduce((accumulator, object) => {
					return accumulator + object.quantity
				}, 0);
				$('#count-cart-quantity').text(count);
				console.log(count)
				var html = '';
				$.each(response, (e, item) => {
				const selectSize = $('#select-size').val();

					html += "<a style=\"text-decoration:none;\" href=\"/Product/Detail/" + item.productId + "\">"
						+ "<li class=\"header__cart-item\">"
						+ "<img src=\"" + $('#hiBaseAddress').val() + item.image + "\" class=\"header__cart-img\">"
						+ "<div class=\"header__cart-item-info\">"
						+ "<div class=\"header__cart-item-head\">"
						+ "<h5 class=\"header__cart-item-name\">" + item.name + "</h5>"
						+ "<div class=\"header__cart-item-price-wrap\">"
						+ " <span class=\"header__cart-price\">" + item.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) + "</span>"
						+ "<span class=\"header__cart-mutiply\">x</span>"
						+ "<span class=\"header__cart-qnt\">" + item.quantity + "</span>"
						+ "</div>"
						+ "</div>"
						+ "<div class=\"header__cart-item-body\">"
						+ "<span class=\"header__cart-item-desciption\">Phân loại " + item.type + "</span>"
						+ "<a class=\"header__cart-remove\" data-id=\"" + item.productId + "\">Xóa</a>"
						+ "</div>"
						+ "</div>"
						+ "</li>"
						+ "</a>"
				})
				$('#list_product_cart_item').html(html);
				console.log(response);
			},
		});
	}
	function ButtonLoadMore() {
		$('body').on('click', '.container-btn-extend', function (e) {
			e.preventDefault();
			const id = $(this).data('id');
			$.ajax({
				//type: "POST",
				url: '/danh-muc/load/',
				success: function (response) {
					var html = '';
					$.each(response, (e, item) => {
						html += `<div class=" productCount col l-2-4 l-3-m m-4 c-6">
							<div class="container-product__item">
								<div class="container-product__item-heading">
									<div class="container-product__item-img">
										<img src="${$('#hiBaseAddress').val() + item.image}" />
									</div>

									<div class="container-product-guarantee">
										<a href='#' class="container-product__item-link">
											<div class="container-product-guarantee__heading">
												<img src="https://mobilecity.vn/public/assets/img/icon-mobilecity-care.png"
													alt="Guarantee"
													class="container-product-guarantee__heading-img">
													<h3 class="container-product-guarantee__heading-text">
														CollabClothing
													</h3>
											 </div>

												<ul class="container-product-guarantee__list">
													<li class="container-product-guarantee__item">
														Hỗ trợ đổi trả trong
														vòng 7 ngày
													</li>
													<li class="container-product-guarantee__item">
														Cam kết hàng chính
														hãng
													</li>
													<li class="container-product-guarantee__item">
														Tặng Voncher cho các
														lần mua sau
													</li>
													<li class="container-product-guarantee__item">
														Hoàn tiền nếu phát
														hiện hàng giả
													</li>
												</ul>
									</a>
											<a href='#' class="container-product-guarantee__btn">Bảo hành vàng</a>
										</div>
									</div>
									<div class="container-product__item-wrap">
										<div class="container-product__item-info">
											<a asp-controller="Product" asp-action="Detail" asp-route-id="@item.Id" class="container-product__item-name">
												@item.ProductName
											</a>
											<i class="container-product__item-sale-icon fas fa-gift"></i>
										</div>
										@{
									// define format VND
									var format = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

								}
										<div class="container-product__item-buy">
											<span class="container-product__item-price">
												@*@Model.ParseToVND(@item.PriceCurrent)*@

												<div class="cta-btn">
													<span class="old-price">@String.Format(format, "{0:c0}", @item.PriceOld )</span>
													<span class="new-price">@String.Format(format, "{0:c0}", @item.PriceCurrent )</span>
												</div>
											</span>
											<a href='#' class="container-product__item-btn" data-id="@item.Id"><localize>MUA</localize></a>
										</div>
									</div>
									<ul class="container-product__item-gifts-list">
										<li class="container-product__item-gift">
											1. Cam kết:
											<span class="container-product__item-gift--highlight">
												Hàng chính hãng
											</span>
											khi mua BHV
										</li>
										<li class="container-product__item-gift">
											2. Voncher:
											<span class="container-product__item-gift--highlight">
												Lên tới 100k
											</span>
										</li>
									</ul>
									<ul class="container-product-marker__list">
										<li class="container-product-marker__item container-product-marker__item--new">
											Mới
										</li>
										<li class="container-product-marker__item container-product-marker__item--hot">
											Hot
										</li>
									</ul>
								</div>
							</div>`
						$('#content').html(html);
						console.log('asdasd');
					});
				},
				data: {
					id: id
				},
			});
		})
	}
	function CartAddSite() {
		$('body').on('click', '.container-product__item-btn', function (e) {
			e.preventDefault();
			const id = $(this).data('id');
			const selectSize = $('#select-size').val();
			$.ajax({
				type: "POST",
				url: '/Cart/AddToCart/',
				success: function (response) {
					const count = response.reduce((accumulator, object) => {
						return accumulator + object.quantity
					}, 0);
					$('#count-cart-quantity').text(count);
					console.log(count);
					loadCartData();
					console.log(selectSize);
				},
				data: {
					id: id
				},
			});
		})
	}

	function selectSize() {
		$(document).ready(function () {
			$("#select-size").change(function () {
				var select = $('#select-size option:selected').val();
				var selectSizeId = $(this).find(':selected').data('id');
				console.log(selectSizeId);
			$('#selected-sizeid').html(select);
			})
		});
	}
}
