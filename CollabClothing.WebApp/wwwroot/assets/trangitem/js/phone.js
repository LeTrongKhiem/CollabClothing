$(document).ready(function () {
  openPopup(".buy-now", ".popup");
  closePopup(".popup_close", ".popup");
  $(".menu-home-mobile").css("display", "none");
  $("#image-gallery").lightSlider({
    item: 4.5,
    loop: false,
    slideMargin: 5,
    controls: true,
    speed: 600,
    keyPress: true,
    freeMove: true,
    enableDrag: true,
    enableTouch: true,
    currentPagerPosition: "middle",
    thumbItem: 4,
    auto: false,
    responsive: [
      { breakpoint: 768, settings: { item: 3 } },
      { breakpoint: 480, settings: { item: 3 } },
    ],
    onSliderLoad: function (el) {
      el.lightGallery({ download: false, selector: "#image-gallery .lslide" });
    },
  });
  $(".slide_video").owlCarousel({
    loop: false,
    margin: 10,
    autoplay: false,
    nav: true,
    dots: false,
    smartSpeed: 500,
    mouseDrag: true,
    pullDrag: true,
    touchDrag: true,
    responsive: { 0: { items: 3 }, 600: { items: 3 }, 1000: { items: 3 } },
  });
  $(".owl-prev").html('<i class="fa fa-chevron-left" aria-hidden="true"></i>');
  $(".owl-next").html('<i class="fa fa-chevron-right" aria-hidden="true"></i>');
  $("#slider_video_home .iframe").each(function () {
    var ifa = $(this).attr("data-iframe");
    var videoid = getIdVideo(ifa);
    var replacement =
      '<iframe src="https://www.youtube.com/embed/' +
      videoid[1] +
      '" frameborder="0" allowfullscreen webkitallowfullscreen mozallowfullscreen></iframe>';
    $(this).html(replacement);
  });
  $("#slide_video .item").each(function () {
    var ifa = $(this).attr("data-iframe");
    var thum = getThumbYoutube(getUrlYoutube(ifa), "big");
    $(this).append("<img src='" + thum + "' alt=''>");
  });
  $("#slide_video .item").on("click", function () {
    var embed = getIdYoutube($(this).attr("data-iframe"));
    $("#slider_video_home .iframe").html(embed);
  });
  $(".show-lightbox-btn").click(function () {
    $("body").css("overflow-y", "hidden");
    $(".lightbox").css("display", "block");
  });
  $(".close-lightbox-btn").click(function () {
    $(".lightbox").css("display", "none");
    $("body").css("overflow-y", "auto");
  });
  $("#product-detail-viewall").click(function () {
    $(this).css("display", "none");
    $(this).parent().css("position", "relative");
    $(this)
      .parent()
      .children("#product-detail-viewdefault")
      .css("display", "inline-block");
    $(this)
      .parent()
      .parent()
      .children(".product-detail-content")
      .css("height", "auto");
  });
  $("#product-detail-viewdefault").click(function () {
    $(this).css("display", "none");
    $(this).parent().css("position", "absolute");
    $(this)
      .parent()
      .children("#product-detail-viewall")
      .css("display", "inline-block");
    $(this)
      .parent()
      .parent()
      .children(".product-detail-content")
      .animate({ height: "1000px" }, 1000);
  });
  $(".product-detail-viewall").click(function () {
    $(this).css("display", "none");
    $(this).parent().css("position", "relative");
    $(this)
      .parent()
      .children(".product-detail-viewdefault")
      .css("display", "inline-block");
    $(this)
      .parent()
      .parent()
      .children(".product-detail-content")
      .css("height", "auto");
  });
  $(".product-detail-viewdefault").click(function () {
    $(this).css("display", "none");
    $(this).parent().css("position", "absolute");
    $(this)
      .parent()
      .children(".product-detail-viewall")
      .css("display", "inline-block");
    $(this)
      .parent()
      .parent()
      .children(".product-detail-content")
      .animate({ height: "550px" }, 500);
  });
  $(".rating-star").on("click", "i", function () {
    value_star = $(this).data("value");
    $("#rating").val(value_star);
    $(".rating-star").empty();
    var j = 0;
    for (var i = 0; i < value_star; i++) {
      j++;
      $(".rating-star").append(
        '<i class="fa start fa-star" data-value="' +
        j +
        '" aria-hidden="true"></i>'
      );
    }
    for (var i = 0; i < 5 - value_star; i++) {
      j++;
      $(".rating-star").append(
        '<i class="fa start fa-star-o" data-value="' +
        j +
        '" aria-hidden="true"></i>'
      );
    }
  });
  var boxParentFullHeight =
    $(".product-content-right").outerHeight() -
    $(".product-summary .product-summary-price").height();
  var boxParentWidth = $(".product-content-right").width();
  var div_content_right = $(".fix-content-right");
  $(window).scroll(function () {
    var startFix = div_content_right.offset().top;
    var yaBox = div_content_right.outerHeight();
    var endFix = startFix + yaBox - boxParentFullHeight;
    var fromTop = jQuery(document).scrollTop();
    if (fromTop > startFix) {
      if (fromTop > endFix) {
        var stopFix =
          yaBox -
          boxParentFullHeight -
          $(".product-summary .buy-box-v2").height() -
          $(".product-summary .product-summary-price").height();
        $(".product-summary").css({ position: "absolute", top: stopFix });
      } else {
        $(".product-summary").css({
          position: "fixed",
          top: "10px",
          width: boxParentWidth,
        });
      }
    } else {
      $(".product-summary").css({
        position: "relative",
        top: "0",
        width: "100%",
      });
    }
  });
  $("body").on(
    "click",
    ".product-content-box .product-related-box .nav-next",
    function () {
      let div_product_list = $(this)
        .closest(".product-related-box")
        .find(".product-related-list");
      div_product_list.animate(
        {
          scrollTop:
            div_product_list.scrollTop() +
            div_product_list.find(".product-related-item").first().height() +
            5,
        },
        500
      );
    }
  );
  $("body").on(
    "click",
    ".product-content-box .product-related-box .nav-prev",
    function () {
      let div_product_list = $(this)
        .closest(".product-related-box")
        .find(".product-related-list");
      div_product_list.animate(
        {
          scrollTop:
            div_product_list.scrollTop() -
            (div_product_list.find(".product-related-item").first().height() +
              5),
        },
        500
      );
    }
  );
  $("body").on(
    "click",
    ".container > .product-related-box .nav-next",
    function () {
      let div_product_list = $(this)
        .closest(".product-related-box")
        .find(".product-related-list");
      div_product_list.animate(
        {
          scrollLeft:
            div_product_list.scrollLeft() +
            div_product_list.find(".product-related-item").first().width() +
            20,
        },
        500
      );
    }
  );
  $("body").on(
    "click",
    ".container > .product-related-box .nav-prev",
    function () {
      let div_product_list = $(this)
        .closest(".product-related-box")
        .find(".product-related-list");
      div_product_list.animate(
        {
          scrollLeft:
            div_product_list.scrollLeft() -
            (div_product_list.find(".product-related-item").first().width() +
              20),
        },
        500
      );
    }
  );
});
function doChooseVariant(pv) {
  $("body").on(
    "click",
    ".product-content-box .product-price-content .attribute-item",
    function (e) {
      if ($(this).hasClass("disabled")) return;
      if ($(this).hasClass("active")) {
        $(this).removeClass("active");
      } else {
        $(this).parent().find(".attribute-item").removeClass("active");
        $(this).addClass("active");
      }
    }
  );
  $("body").on(
    "click",
    ".product-content-box .product-price-content .attribute-item:not(.warranty-item)",
    function (e) {
      pv.updatePrice();
      pv.updateButtonBuy();
    }
  );
  $("body").on(
    "click",
    ".product-content-box .product-price-content .warranty-item",
    function (e) {
      let div_warranty_box = $(".warranty-default-box");
      let text_warranty_content_default = div_warranty_box.find(
        ".warranty-content-default"
      );
      let text_warranty_content_selected = div_warranty_box.find(
        ".warranty-content-selected"
      );
      let text_warranty_price = $(this)
        .closest(".warranty-list")
        .find(".price-warranty");
      let selected_name_warranty = "";
      if ($(this).hasClass("active")) {
        text_warranty_price
          .show()
          .text(`+${format_price($(this).data("warranty_price"))}`);
        div_warranty_box.find(".warranty-text").show();
        if (text_warranty_content_default.length) {
          selected_name_warranty = ", ";
        }
        selected_name_warranty = selected_name_warranty + $(this).text();
      } else {
        text_warranty_price.attr("style", "display: none !important").text(``);
        if (!text_warranty_content_default.length) {
          div_warranty_box.find(".warranty-text").hide();
        }
      }
      text_warranty_content_selected.text(selected_name_warranty);
      pv.updateButtonBuy();
    }
  );
  function doChooseColor(color_item) {
    if ($(color_item).hasClass("disabled")) return;
    let img = $(
      ".product-content-box .product-slide-image .product_image li"
    ).has('img[data-color="' + $(color_item).data("color_id") + '"]');
    if (img.length) {
      $(
        ".product-content-box .product-slide-image .product_image li"
      ).removeClass("active");
      img.addClass("active");
    }
    if ($(color_item).hasClass("active")) {
      $(color_item)
        .closest(".color-list")
        .find(".color-name-selected")
        .show()
        .text($(color_item).data("title"));
    } else {
      $(color_item)
        .closest(".color-list")
        .find(".color-name-selected")
        .attr("style", "display: none !important");
    }
  }
  $("body").on("click", ".product-content-box .color-item", function (e) {
    doChooseColor(this);
  });
  $("body").on("change", ".product-content-box select#location", function () {
    let val = $(this).val();
    $(".product-content-box select#location-message").val(val);
    $(".store-list ul li").css("display", "none");
    $(".store-list ul").find(`li[data-location=0]`).css("display", "list-item");
    $(".store-list ul")
      .find(`li[data-location=${val}]`)
      .css("display", "list-item");
    pv.updateButtonBuy();
    pv.updatePrice();
  });
  $("body").on(
    "change",
    ".product-content-box select#location-message",
    function () {
      let val = $(this).val();
      $(".product-content-box select#location").val(val);
      $(".store-list ul li").css("display", "none");
      $(".store-list ul")
        .find(`li[data-location=0]`)
        .css("display", "list-item");
      $(".store-list ul")
        .find(`li[data-location=${val}]`)
        .css("display", "list-item");
      pv.updateButtonBuy();
      pv.updatePrice();
    }
  );
  function autoSelectCheapestVariant() {
    try {
      let variants = pv.phone_variants;
      let div_attribute_group = $(".product-price-content .attribute-group");
      if (div_attribute_group.length == 0 || variants.length == 0) return;
      let min_price = variants.reduce(function (min_price, variant) {
        return min_price <= variant.price ? min_price : variant.price;
      }, Infinity);
      let cheapest_variants = variants.filter(function (variant) {
        return variant.price == min_price;
      });
      let div_color_list = div_attribute_group.find(
        ".color-list .attribute-item"
      );
      if (div_color_list.length) {
        $.each(div_color_list, function (index, element) {
          let color_id = Number($(element).data("color_id") ?? 0);
          let check_variant = cheapest_variants.find(function (variant) {
            return variant.color == color_id;
          });
          if (check_variant != undefined) {
            $(element).addClass("active");
            doChooseColor(element);
            return false;
          }
        });
      }
      let div_storage_list = div_attribute_group.find(
        ".storage-list .attribute-item"
      );
      if (div_storage_list.length) {
        $.each(div_storage_list, function (index, element) {
          let storage_id = Number($(element).data("storage_id") ?? 0);
          let check_variant = cheapest_variants.find(function (variant) {
            return variant.storage == storage_id;
          });
          if (check_variant != undefined) {
            $(element).addClass("active");
            return false;
          }
        });
      }
      let div_aspect_list = div_attribute_group.find(
        ".aspect-list .attribute-item"
      );
      if (div_aspect_list.length) {
        $.each(div_aspect_list, function (index, element) {
          let aspect_id = Number($(element).data("aspect_id") ?? 0);
          let check_variant = cheapest_variants.find(function (variant) {
            return variant.aspect == aspect_id;
          });
          if (check_variant != undefined) {
            $(element).addClass("active");
            return false;
          }
        });
      }
      let div_origin_list = div_attribute_group.find(
        ".origin-list .attribute-item"
      );
      if (div_origin_list.length) {
        $.each(div_origin_list, function (index, element) {
          let origin_id = Number($(element).data("origin_id") ?? 0);
          let check_variant = cheapest_variants.find(function (variant) {
            return variant.origin == origin_id;
          });
          if (check_variant != undefined) {
            $(element).addClass("active");
            return false;
          }
        });
      }
    } catch (error) {
      console.error(error);
    }
  }
  pv.updatePrice();
  pv.showButtonAndMessage();
}
function showInfoVariantSync(
  phone_variants,
  extra_prices,
  phone_id,
  instock_status
) {
  class PhoneVariant {
    phone_variants = {};
    extra_prices = {};
    instocks = {};
    phone_variant_ids = {};
    div_product = $(".product-content-box");
    div_captcha = $(".product-captcha");
    div_button = $(".buy-box-v2");
    div_status_box = $(".product-content-box .status-box");
    div_message = $(".product-messsage");
    div_product_store_list = $(".product-store-list");
    div_store_list = $(".product-store-list .store-list");
    button_buy_now = `<a href="javascript::" data-product_type="phone_variants" class="buy buy-now v2-buy">MUA NGAY</a>`;
    button_add_to_cart = `<a href="javascript::" data-product_type="phone_variants" class="buy add-to-cart v2-buy"><div class="icon"></div><span>THÊM VÀO GIỎ HÀNG</span></a>`;
    button_out_stock = `<a href="javascript::"  class="buy-now disabled">Hết hàng</a>`;
    button_register = `<a href="#" class="buy register" data-product_type="phone_variants">ĐẶT HÀNG TRƯỚC</a>`;
    button_chat = `<a href="${$(".mobilecity-messenger").attr("href") ?? "javascript:;"
      }" target="_blank" class="chat">CHAT TƯ VẤN</a>`;
    button_pre_order = `<a href="javascript:;" data-product_id="${phone_id}" data-variant_id="0" data-service_product_id="0" class="pre-order">ĐẶT HÀNG TRƯỚC</a>`;
    message_not_sync = `<p class="message">Phiên bản hiện tại đang tạm hết hàng, quý khách vui lòng liên hệ để được tư vấn hoặc đặt hàng trước hoặc chọn phiên bản khác!</p>`;
    message_stop_business = `<p class="message">Sản phẩm đã ngừng kinh doanh, quý khách vui lòng liên hệ để được tư vấn hoặc tham khảo sản phẩm tương tự!</p>`;
    message_out_stock = `<p class="message">Sản phẩm hiện tại đang tạm hết hàng, quý khách có thể đặt trước sản phẩm hoặc tham khảo sản phẩm tương tự!</p>`;
    message_instock_online = `<p class="message">Còn hàng bán online!</p>`;
    message_out_stock_location = `<p class="message">Sản phẩm đang tạm hết hàng ở khu vực này. Bạn có thể đặt trước sản phẩm này hoặc chọn khu vực khác!</p>`;
    message_out_stock_variant_this_location = `<p class="message">Phiên bản đã chọn ở khu vực này đang tạm hết hàng, quý khách có thể đặt trước sản phẩm hoặc chọn khu vực khác!`;
    message_out_stock_variant_all_location = `<p class="message">Phiên bản đã chọn đang tạm hết hàng, quý khách vui lòng chọn phiên bản khác hoặc đặt trước sản phẩm!`;
    status_message_out_stock = "Hết hàng";
    status_message_in_stock = "Còn hàng";
    color_selected_id = 0;
    aspect_selected_id = 0;
    origin_selected_id = 0;
    storage_selected_id = 0;
    count_attribute_type = 0;
    error = 0;
    constructor(phone_variants, extra_prices, phone_id = 0, instock_status) {
      this.phone_variants = phone_variants;
      this.extra_prices = extra_prices;
      this.phone_id = phone_id;
      this.count_attribute_type = this.div_product.find(
        ".attribute-group .attribute-list"
      ).length;
      this.phone_variant_ids = this.phone_variants
        .map(function (variant) {
          return variant.soft_id;
        })
        .filter(function (value) {
          return value > 0;
        });
    }
    showButtonAndMessage() {
      this.getInstock();
    }

    updateButtonBuy() {
      $(".store-list li .status-stock")
        .removeClass("instock")
        .addClass("outstock")
        .html("(Hết hàng)");
      if (this.error) return;
      if (this.isNotSync()) {
        this.updateStatusNotSync();
        return;
      }
      let status_message_tmp = this.status_message_out_stock;
      if (!this.isInstock()) {
        this.updateStatusOutStock(this.message_out_stock);
      } else {
        let location =
          this.div_product.find(".price_location select#location").val() ?? 1;
        let instock_data = this.getInstockByLocation(location);
        if (instock_data.length == 0) {
          this.updateStatusOutStock(this.message_out_stock_location, true);
          let instock_data_all_location = this.getInstockByLocation(0, true);
          if (instock_data_all_location.length != 0) {
            $(".store-list")
              .find(`li[data-id=0] .status-stock`)
              .removeClass("outstock")
              .addClass("instock")
              .html("(Còn hàng)");
          }
        } else {
          instock_data = this.getInstockByLocation(location, true);
          if (instock_data.length == 0) {
            let instock_data_all_location = this.getInstockByLocation(0, true);
            if (instock_data_all_location.length == 0) {
              this.updateStatusOutStock(
                this.message_out_stock_variant_all_location
              );
            } else {
              $(".store-list")
                .find(`li[data-id=0] .status-stock`)
                .removeClass("outstock")
                .addClass("instock")
                .html("(Còn hàng)");
              this.updateStatusOutStock(
                this.message_out_stock_variant_this_location,
                true
              );
            }
          } else {
            $(".store-list")
              .find(`li[data-id=0] .status-stock`)
              .removeClass("outstock")
              .addClass("instock")
              .html("(Còn hàng)");
            this.div_captcha.hide();
            let stores = this.getStores(instock_data);
            this.showPromotionBox();
            if (stores.length == 0) {
              this.updateStatusOutStock(this.message_instock_online, false);
            } else {
              stores.forEach((store) => {
                $(".store-list")
                  .find(`li[data-id=${store.store_id}] .status-stock`)
                  .removeClass("outstock")
                  .addClass("instock")
                  .html("(Còn hàng)");
              });
            }
            this.div_button.html(this.button_add_to_cart + this.button_buy_now);
            this.div_message.hide();
            status_message_tmp = this.status_message_in_stock;
          }
        }
      }
      this.updateParamButtonBuy();
    }
    hiddenPromotionBox() {
      if ($(window).width() > 768) {
        $(
          ".product-price-box .v2-promotion, .product-price-box .product-info-general, .product-price-box .warranty-default-box, .product-price-box .deal-combo-box"
        ).css({ opacity: 0, visibility: "hidden" });
      } else {
        $(
          ".product-price-box .v2-promotion, .product-price-box .product-info-general, .product-price-box .warranty-default-box, .product-price-box .deal-combo-box"
        ).css({ display: "none" });
      }
    }
    showPromotionBox() {
      if ($(window).width() > 768) {
        $(
          ".product-price-box .v2-promotion, .product-price-box .product-info-general, .product-price-box .warranty-default-box, .product-price-box .deal-combo-box"
        ).css({ opacity: 1, visibility: "visible" });
      } else {
        $(
          ".product-price-box .v2-promotion, .product-price-box .product-info-general, .product-price-box .warranty-default-box, .product-price-box .deal-combo-box"
        ).css({ display: "block" });
      }
    }
    updateStatusOutStock(message, is_show_in_div_message = true) {
      if (this.error) return;
      if (is_show_in_div_message) {
        this.div_message.html(message);
        this.div_message.show();
        this.hiddenPromotionBox();
      } else {
        this.div_message.hide();
      }
      if (isShowCaptchaRegisterProduct()) {
        this.div_captcha.show();
      }
      this.div_button.html(this.button_chat + this.button_register);
    }
    updateStatusNotSync() {
      this.div_button.html(this.button_chat + this.button_pre_order);
      this.div_message.html(this.message_not_sync);
      this.div_message.show();
      this.hiddenPromotionBox();
      let dataVariantId = 0;
      let dataServiceProductId =
        this.div_product.find(".warranty-item.active").data("warranty_id") ?? 0;
      if (this.isSelectedFullAttribute()) {
        let current_variant = this.getCurrentVariant();
        dataVariantId = current_variant.id;
      }
      $(".buy-box-v2 .pre-order")
        .attr("data-variant_id", dataVariantId)
        .attr("data-service_product_id", dataServiceProductId);
    }
    addRequiredAttribute() {
      let attributeGroup = this.div_product.find(".attribute-group");
      if (
        !attributeGroup.hasClass("required") &&
        !pv.isSelectedFullAttribute()
      ) {
        attributeGroup.append(
          `<p class="error-message">Vui lòng chọn phiên bản!</p>`
        );
        attributeGroup.addClass("required");
      }
      if (attributeGroup.hasClass("required")) {
        $("html, body").animate(
          { scrollTop: attributeGroup.offset().top - 50 },
          300
        );
      }
    }
    removeRequiredAttribute() {
      let attributeGroup = this.div_product.find(".attribute-group");
      if (attributeGroup.hasClass("required") && pv.isSelectedFullAttribute()) {
        attributeGroup.find(".error-message").remove();
        attributeGroup.removeClass("required");
      }
    }
    mixInstockDataAndVariant() {
      this.instocks.forEach((element) => {
        let variant_tmp = this.phone_variants.find(function (v) {
          return v.soft_id == element.product_id;
        });
        element["variant"] = variant_tmp;
      }, this);
    }
    getInstockByLocation(location = 0, check_attribute = false) {
      if (check_attribute) this.getCurrentAttribute();
      return this.instocks.filter(function (data) {
        return (
          data.quantity > 0 &&
          (!location || data.location_id == location) &&
          (!check_attribute ||
            ((!this.color_selected_id ||
              data.variant.color == this.color_selected_id) &&
              (!this.origin_selected_id ||
                data.variant.origin == this.origin_selected_id) &&
              (!this.aspect_selected_id ||
                data.variant.aspect == this.aspect_selected_id) &&
              (!this.storage_selected_id ||
                data.variant.storage == this.storage_selected_id)))
        );
      }, this);
    }
    isNotSync() {
      let sync_product = this.getSelectedVariants().find(function (variant) {
        return variant.soft_id > 0;
      });
      return sync_product == undefined;
    }
    isInstock(location = 0, check_attribute = false) {
      if (check_attribute) this.getCurrentAttribute();
      let f = this.instocks.find(function (data) {
        if (
          data.quantity > 0 &&
          (location == 0 || data.location_id == location) &&
          (!check_attribute ||
            ((!this.color_selected_id ||
              data.variant.color == this.color_selected_id) &&
              (!this.origin_selected_id ||
                data.variant.origin == this.origin_selected_id) &&
              (!this.aspect_selected_id ||
                data.variant.aspect == this.aspect_selected_id) &&
              (!this.storage_selected_id ||
                data.variant.storage == this.storage_selected_id)))
        ) {
          return data;
        }
      }, this);
      return typeof f !== "undefined";
    }
    getStores(data) {
      let obj = {};
      let result = [];
      data.forEach(function (v) {
        v["store"].forEach(function (store) {
          if (typeof obj[store.store_id] === "undefined") {
            result.push({ store_id: store.store_id, address: store.address });
            obj[store.store_id] = true;
          }
        });
      });
      return result;
    }
    updatePrice() {
      let div_price = $(
        ".product-content-box .price-and-color .price, .product-content-box .product-order .price, .product-content-box .product-content-right .product-summary-price"
      );
      let min_price = -1;
      let max_price = -1;
      this.getCurrentAttribute();
      this.phone_variants.forEach(function (variant) {
        if (
          (this.color_selected_id && variant.color != this.color_selected_id) ||
          (this.storage_selected_id &&
            variant.storage != this.storage_selected_id) ||
          (this.origin_selected_id &&
            variant.origin != this.origin_selected_id) ||
          (this.aspect_selected_id && variant.aspect != this.aspect_selected_id)
        ) {
          return false;
        }
        if (variant.price > max_price) {
          max_price = variant.price;
        }
        if (min_price == -1 || variant.price < min_price) {
          min_price = variant.price;
        }
      }, this);
      if (max_price < 0) {
        div_price.text("");
        return;
      }
      let location = this.div_product.find("select#location").val();
      let extra_price = this.extra_prices[location] ?? 0;
      min_price += extra_price;
      max_price += extra_price;
      if (min_price == max_price) {
        div_price.text(format_price(min_price));
      } else if (min_price >= 68000000) {
        div_price.text(format_price(min_price));
      } else {
        div_price.text(
          format_price(min_price) + " - " + format_price(max_price)
        );
      }
    }
    getCurrentAttribute() {
      this.color_selected_id = this.div_product
        .find(".color-item.active")
        .data("color_id");
      this.storage_selected_id = this.div_product
        .find(".storage-item.active")
        .data("storage_id");
      this.aspect_selected_id = this.div_product
        .find(".aspect-item.active")
        .data("aspect_id");
      this.origin_selected_id = this.div_product
        .find(".origin-item.active")
        .data("origin_id");
      if (!this.color_selected_id) this.color_selected_id = 0;
      if (!this.storage_selected_id) this.storage_selected_id = 0;
      if (!this.aspect_selected_id) this.aspect_selected_id = 0;
      if (!this.origin_selected_id) this.origin_selected_id = 0;
    }
    updateParamButtonBuy() {
      if (this.error) return;
      let btn = this.div_button.find(".buy");
      if (this.isSelectedFullAttribute()) {
        let current_variant = this.getCurrentVariant();
        btn.addClass("btn-add-cart");
        btn.attr(
          "data-location_id",
          this.div_product.find("select#location").val()
        );
        btn.attr("data-product_id", current_variant.soft_id);
        btn.attr(
          "data-service_product_id",
          this.div_product.find(".warranty-item.active").data("warranty_id") ??
          0
        );
        this.removeRequiredAttribute();
        $(".buy-box-v2 .pre-order")
          .attr("data-variant_id", current_variant.id)
          .attr(
            "data-service_product_id",
            this.div_product
              .find(".warranty-item.active")
              .data("warranty_id") ?? 0
          );
      } else {
        btn.removeClass("btn-add-cart");
      }
    }
    isSelectedFullAttribute() {
      let count_selected_type_attribute = this.div_product.find(
        ".attribute-group .attribute-list .active:not(.warranty-item)"
      ).length;
      return count_selected_type_attribute == this.count_attribute_type;
    }
    getCurrentVariant() {
      if (!this.isSelectedFullAttribute()) return null;
      this.getCurrentAttribute();
      let current_variant = this.phone_variants.find(function (variant) {
        if (
          variant.color == this.color_selected_id &&
          variant.origin == this.origin_selected_id &&
          variant.aspect == this.aspect_selected_id &&
          variant.storage == this.storage_selected_id
        )
          return variant;
      }, this);
      this.updateDealVariant(current_variant);
      return current_variant;
    }
    getSelectedVariants() {
      this.getCurrentAttribute();
      return this.phone_variants.filter(function (variant) {
        return (
          (!this.color_selected_id ||
            variant.color == this.color_selected_id) &&
          (!this.origin_selected_id ||
            variant.origin == this.origin_selected_id) &&
          (!this.aspect_selected_id ||
            variant.aspect == this.aspect_selected_id) &&
          (!this.storage_selected_id ||
            variant.storage == this.storage_selected_id)
        );
      }, this);
    }
    updateAttributeInstock(changColor = true) {
      let classColorItem = "";
      if (changColor) classColorItem = ".color-item";
      this.div_product
        .find(
          `.attribute-item:not(.warranty-item):not(${classColorItem}) svg.icon-tick-bold`
        )
        .addClass("out-stock");
      this.div_product
        .find(
          `.attribute-item:not(.warranty-item):not(${classColorItem}) .product-variation__tick`
        )
        .addClass("out-stock");
      let location = this.div_product.find("select#location").val() ?? 0;
      let tmp_instock_variants = this.instocks;
      tmp_instock_variants.forEach((itemVarriant) => {
        let colorItem = this.div_product.find(
          `.color-item[data-color_id="${itemVarriant.variant.color}"]`
        );
        colorItem.find(".product-variation__tick").removeClass("out-stock");
        colorItem.find("svg.icon-tick-bold").removeClass("out-stock");
        let storageItem = this.div_product.find(
          `.storage-item[data-storage_id="${itemVarriant.variant.storage}"]`
        );
        storageItem.find(".product-variation__tick").removeClass("out-stock");
        storageItem.find("svg.icon-tick-bold").removeClass("out-stock");
        let aspectItem = this.div_product.find(
          `.aspect-item[data-aspect_id="${itemVarriant.variant.aspect}"]`
        );
        aspectItem.find(".product-variation__tick").removeClass("out-stock");
        aspectItem.find("svg.icon-tick-bold").removeClass("out-stock");
        let originItem = this.div_product.find(
          `.origin-item[data-origin_id="${itemVarriant.variant.origin}"]`
        );
        originItem.find(".product-variation__tick").removeClass("out-stock");
        originItem.find("svg.icon-tick-bold").removeClass("out-stock");
      });
    }
    updateDealVariant(current_variant) {
      let location = this.div_product.find("select#location").val();
      let price =
        parseInt(this.extra_prices[location] ?? 0) +
        parseInt(current_variant.price ?? 0);
      let warranty_id =
        this.div_product.find(".warranty-item.active").data("warranty_id") ?? 0;
      let warranty_name =
        this.div_product.find(".warranty-item.active").text() ?? "";
      $('.deal-content .deal-product-main input[name="main_product_id"]').val(
        current_variant.soft_id
      );
      $(
        '.deal-content .deal-product-main input[name="main_service_product_id"]'
      ).val(warranty_id);
      $(
        ".deal-content .deal-product-main .product-current-attribute .product-current-variant"
      ).html(current_variant.variant_text);
      $(
        ".deal-content .deal-product-main .product-current-attribute .product-current-warranty"
      ).html(warranty_name);
      $(".deal-content .deal-product-main .deal-product-price").attr(
        "data-price",
        price
      );
      $(
        ".deal-content .deal-product-main .deal-product-price .deal-price"
      ).html(format_price(price));
    }
  }
  pv = new PhoneVariant(phone_variants, extra_prices, phone_id, instock_status);
  $("body").on("click", ".buy-box-v2 .buy", function (e) {
    e.preventDefault();
    pv.addRequiredAttribute();
  });
  doChooseVariant(pv);
}
function showInfoVariantNotSync(
  phone_variants,
  extra_prices,
  phone_id,
  instock_status
) {
  class PhoneVariant {
    phone_variants = {};
    extra_prices = {};
    div_product = $(".product-content-box");
    div_button = $(".buy-box-v2");
    div_message = $(".product-messsage");
    button_chat = `<a href="${$(".mobilecity-messenger").attr("href") ?? "javascript:;"
      }" target="_blank" class="chat">CHAT TƯ VẤN</a>`;
    button_pre_order = `<a href="javascript:;" data-product_id="${phone_id}" data-variant_id="0" data-service_product_id="0" class="pre-order">ĐẶT HÀNG TRƯỚC</a>`;
    message_not_sync = `<p class="message">Sản phẩm hiện tại đang tạm hết hàng, quý khách vui lòng liên hệ để được tư vấn hoặc tham khảo sản phẩm tương tự!</p>`;
    color_selected_id = 0;
    aspect_selected_id = 0;
    origin_selected_id = 0;
    storage_selected_id = 0;
    count_attribute_type = 0;
    constructor(phone_variants, extra_prices, phone_id, instock_status) {
      var is_stock = 0;
      if (instock_status == 1) {
        is_stock = 1;
      }
      updateIsStockProduct(phone_id, is_stock);
      this.phone_variants = phone_variants;
      this.extra_prices = extra_prices;
      if (instock_status == 5) {
        this.button_pre_order = ``;
      }
      this.count_attribute_type = this.div_product.find(
        ".attribute-group .attribute-list"
      ).length;
    }
    updateButtonBuy() {
      this.updatePrice();
    }
    showButtonAndMessage() {
      this.div_button.html(this.button_chat + this.button_pre_order);
      this.div_message.show();
      $(".attribute-group, .warranty-list").show();
      $(".price-product").show();
    }
    updatePrice() {
      let div_price = $(
        ".product-content-box .price-and-color .price, .product-content-box .product-order .price, .product-content-box .product-content-right .product-summary-price"
      );
      let min_price = -1;
      let max_price = -1;
      this.getCurrentAttribute();
      this.phone_variants.forEach(function (variant) {
        if (
          (this.color_selected_id && variant.color != this.color_selected_id) ||
          (this.storage_selected_id &&
            variant.storage != this.storage_selected_id) ||
          (this.origin_selected_id &&
            variant.origin != this.origin_selected_id) ||
          (this.aspect_selected_id && variant.aspect != this.aspect_selected_id)
        ) {
          return false;
        }
        if (variant.price > max_price) {
          max_price = variant.price;
        }
        if (min_price == -1 || variant.price < min_price) {
          min_price = variant.price;
        }
      }, this);
      if (max_price < 0) {
        div_price.text("");
        return;
      }
      let location = this.div_product.find("select#location").val();
      let extra_price = this.extra_prices[location] ?? 0;
      min_price += extra_price;
      max_price += extra_price;
      if (min_price == max_price) {
        div_price.text(format_price(min_price));
      } else if (min_price >= 68000000) {
        div_price.text(format_price(min_price));
      } else {
        div_price.text(
          format_price(min_price) + " - " + format_price(max_price)
        );
      }
      let dataVariantId = 0;
      let dataServiceProductId =
        this.div_product.find(".warranty-item.active").data("warranty_id") ?? 0;
      if (this.isSelectedFullAttribute()) {
        let current_variant = this.getCurrentVariant();
        dataVariantId = current_variant.id;
      }
      $(".buy-box-v2 .pre-order")
        .attr("data-variant_id", dataVariantId)
        .attr("data-service_product_id", dataServiceProductId);
    }
    getCurrentAttribute() {
      this.color_selected_id = this.div_product
        .find(".color-item.active")
        .data("color_id");
      this.storage_selected_id = this.div_product
        .find(".storage-item.active")
        .data("storage_id");
      this.aspect_selected_id = this.div_product
        .find(".aspect-item.active")
        .data("aspect_id");
      this.origin_selected_id = this.div_product
        .find(".origin-item.active")
        .data("origin_id");
      if (!this.color_selected_id) this.color_selected_id = 0;
      if (!this.storage_selected_id) this.storage_selected_id = 0;
      if (!this.aspect_selected_id) this.aspect_selected_id = 0;
      if (!this.origin_selected_id) this.origin_selected_id = 0;
    }
    getCurrentVariant() {
      if (!this.isSelectedFullAttribute()) return null;
      this.getCurrentAttribute();
      let current_variant = this.phone_variants.find(function (variant) {
        if (
          variant.color == this.color_selected_id &&
          variant.origin == this.origin_selected_id &&
          variant.aspect == this.aspect_selected_id &&
          variant.storage == this.storage_selected_id
        )
          return variant;
      }, this);
      return current_variant;
    }
    isSelectedFullAttribute() {
      let count_selected_type_attribute = this.div_product.find(
        ".attribute-group .attribute-list .active:not(.warranty-item)"
      ).length;
      return count_selected_type_attribute == this.count_attribute_type;
    }
  }
  pv = new PhoneVariant(phone_variants, extra_prices, phone_id, instock_status);
  doChooseVariant(pv);
}
function responsiveRelatedProduct() {
  if ($(window).width() > 768) {
    if ($(".product-related-box .product-related-item").length == 0) return;
    let height_related_phone = $(".phone-related-box").height();
    let height_related_fit = $(".fit-related-box").height();
    let height_related_product = height_related_fit + height_related_phone;
    let height_product_info = $(".product-price-box").height() - 32;
    if (
      $(".product-related-box.phone-related-box .product-related-list")
        .html()
        .trim()
    ) {
      $(".product-related-box.phone-related-box").show();
    }
    if (
      $(".product-related-box.fit-related-box .product-related-list")
        .html()
        .trim()
    ) {
      $(".product-related-box.fit-related-box").show();
    }
    if (height_related_product < height_product_info) return;
    if ($(".fit-related-box .product-related-item").length == 0) {
      $(".phone-related-box .product-related-list").css(
        "max-height",
        height_product_info
      );
      $(".phone-related-box .product-related-nav").show();
    } else if ($(".phone-related-box .product-related-item").length == 0) {
      $(".fit-related-box .product-related-list").css(
        "max-height",
        height_product_info
      );
      $(".fit-related-box .product-related-nav").show();
    } else if (height_related_phone < height_product_info / 2.0) {
      $(".fit-related-box .product-related-list").css(
        "max-height",
        height_product_info - height_related_phone
      );
      $(".fit-related-box .product-related-nav").show();
    } else if (height_related_fit < height_product_info / 2.0) {
      $(".phone-related-box .product-related-list").css(
        "max-height",
        height_product_info - height_related_fit
      );
      $(".phone-related-box .product-related-nav").show();
    } else {
      $(".product-related-box .product-related-list").css(
        "max-height",
        height_product_info / 2.0
      );
      $(".product-related-box .product-related-nav").show();
    }
  } else {
    $(".product-box-mobile").owlCarousel({
      loop: false,
      margin: 10,
      autoplay: false,
      nav: true,
      dots: false,
      smartSpeed: 500,
      mouseDrag: true,
      pullDrag: true,
      touchDrag: true,
      responsive: { 0: { items: 2 }, 600: { items: 3 }, 1000: { items: 3 } },
    });
    $(".owl-prev").html(
      '<i class="fa fa-chevron-left" aria-hidden="true"></i>'
    );
    $(".owl-next").html(
      '<i class="fa fa-chevron-right" aria-hidden="true"></i>'
    );
    if ($(".phone-related-box.product-related-box-mobile .owl-stage").html()) {
      $(".phone-related-box.product-related-box-mobile").show();
    }
    if ($(".fit-related-box.product-related-box-mobile .owl-stage").html()) {
      $(".fit-related-box.product-related-box-mobile").show();
    }
  }
}

