const phones = (() => {
    const banners = [
        {
            img: "./assets/img/banner1.jpg",
            link: "#",
        },
        {
            img: "./assets/img/banner2.jpg",
            link: "#",
        },
        {
            img: "./assets/img/banner3.jpg",
            link: "#",
        },
    ];

    let currentBanner = 0;
    const prevBtn = document.querySelector(".phone-banner__btn-prev");
    const nextBtn = document.querySelector(".phone-banner__btn-next");
    let firstBanner;
    let bannerPaginationBtns;
    let setTimeChangeBanner;

    return {
        render() {
            const percent = 100 / banners.length;
            const bannerPaginations = [];
            const htmlBanner = banners
                .map((banner, index) => {
                    let first = "";
                    if (index === 0) {
                        first = "banner--first";
                        bannerPaginations.push(
                            `<div data-set="${percent * index
                            }%" class="phone-banner-pagination__item active"></div>`
                        );
                    } else {
                        bannerPaginations.push(
                            `<div data-set="-${percent * index
                            }%" class="phone-banner-pagination__item"></div>`
                        );
                    }
                    return `
                    <a href=${banner.link
                        } class="banner__link ${first}" style="width: ${percent}%">
                        <img src=${banner.img} alt="Banner ${index + 1
                        }" class="banner__img">
                    </a>
                `;
                }, "")
                .join("");

            document.querySelector(".phone-banner__list").innerHTML = htmlBanner;
            document.querySelector(".phone-banner__list").style.width = `${banners.length * 100
                }%`;
            document.querySelector(".phone-banner-pagination").innerHTML =
                bannerPaginations.join("");
        },
        handleBanner() {
            const _this = this;
            firstBanner = document.querySelector(".banner--first");
            bannerPaginationBtns = document.querySelectorAll(
                ".phone-banner-pagination__item"
            );

            prevBtn.onclick = () => {
                this.prevBanner();
                this.autoChangeBanner(true);
            };

            nextBtn.onclick = () => {
                this.nextBanner();
                this.autoChangeBanner(true);
            };

            bannerPaginationBtns.forEach(function (btn, index) {
                btn.onclick = () => {
                    currentBanner = index;
                    firstBanner.style.marginLeft =
                        bannerPaginationBtns[currentBanner].dataset.set;
                    document
                        .querySelector(".phone-banner-pagination__item.active")
                        .classList.remove("active");
                    bannerPaginationBtns[currentBanner].classList.add("active");
                    _this.autoChangeBanner(true);
                };
            });
        },
        prevBanner() {
            currentBanner -= 1;
            if (currentBanner < 0) {
                currentBanner = banners.length - 1;
            }
            firstBanner.style.marginLeft =
                bannerPaginationBtns[currentBanner].dataset.set;
            document
                .querySelector(".phone-banner-pagination__item.active")
                .classList.remove("active");
            bannerPaginationBtns[currentBanner].classList.add("active");
        },
        nextBanner() {
            currentBanner += 1;
            if (currentBanner > banners.length - 1) {
                currentBanner = 0;
            }
            firstBanner.style.marginLeft =
                bannerPaginationBtns[currentBanner].dataset.set;
            document
                .querySelector(".phone-banner-pagination__item.active")
                .classList.remove("active");
            bannerPaginationBtns[currentBanner].classList.add("active");
        },
        autoChangeBanner(isClick) {
            const _this = this;
            if (isClick) {
                clearInterval(setTimeChangeBanner);
                setTimeChangeBanner = setInterval(function () {
                    _this.nextBanner();
                }, 3000);
            } else {
                setTimeChangeBanner = setInterval(function () {
                    _this.nextBanner();
                }, 3000);
            }
        },
        init() {
            this.render();
            this.handleBanner();
            this.autoChangeBanner();
        },
    };
})();

phones.init();
