import targetHTML from '../targetHTML.js';

$(document).ready(() => {
  class HomePage {
    AppSwiper = () => {
      const optionsSwiper = {
        loop: true,
        autoplay: {
          delay: 3000,
          pauseOnMouseEnter: false,
          disableOnInteraction: false,
        },
      };

      // todo - slider
      new Swiper('#swiper', {
        ...optionsSwiper,
        clickable: true,
        // Optional parameters
        direction: 'horizontal',
        effect: 'fade',
        lazy: true,
        // Navigation arrows
        navigation: {
          nextEl: '.swiper-btn-next',
          prevEl: '.swiper-btn-prev',
        },
        pagination: {
          el: '.swiper-pagination',
          type: 'fraction',
          renderFraction: (current, total) => /*html*/ `
          <div class="flex items-center justify-center">
            <div class="bg-unknown_2 rounded-[50px] px-8">
              <span class="font-bold pr-1 text-[20px] ${current}"></span>/
              <span class="${total}"></span>
            </div>
          </div>`,
        },
      });

      // todo - slide product
      new Swiper('#swiper-product', {
        ...optionsSwiper,
        slidesPerView: 2,
        // slidesPerGroup: 2,
        breakpoints: {
          640: {
            slidesPerView: 3,
          },

          1024: {
            slidesPerView: 4,
          },
        },

        navigation: {
          nextEl: '.swiperNext-product',
          prevEl: '.swiperPrev-product',
        },
      });

      // todo - distribution partner
      new Swiper('#swiper-distribution-partner', {
        ...optionsSwiper,
        slidesPerView: 3,
        // slidesPerGroup: 2,
        breakpoints: {
          1024: {
            slidesPerView: 4,
          },
        },

        navigation: {
          nextEl: '.swiperNext-product',
          prevEl: '.swiperPrev-product',
        },
      });
    };

    ToggleShowDistributionPartnerOnMobile() {
      const initList = [
        {
          link: '#',
          image: 'heros-fitness-yoga.jpg',
        },
        {
          link: '#',
          image: 'vantri-golf-club.png',
        },
        {
          link: '#',
          image: 'tiki.png',
        },
      ];
      const list = [
        {
          link: '#',
          image: 'shopee.png',
        },
        {
          link: '#',
          image: 'lazada.png',
        },
        {
          link: '#',
          image: 'king-fitness.png',
        },
        {
          link: '#',
          image: 'ium-global.png',
        },
        {
          link: '#',
          image: 'ecofit.png',
        },
      ];

      const { btnSeeMore, containItem } = targetHTML.distributionPartnerHomePage;

      const renderItem = (list) =>
        list.map(
          ({ link, image }) => `
      <a href="${link}">
      <div class="rounded-full overflow-hidden border-2">
        <img src="/Assets/images/distribution-partner/${image}" class="object-contain" alt="" />
      </div>
    </a>
      `
        );

      let isShowMore = false;

      btnSeeMore.on('click', (e) => {
        const thisTarget = $(e.target);
        let o = '';
        if (isShowMore) {
          thisTarget.html('+ Xem hết');
          o = renderItem(initList).join('');
          containItem.html(o);
          isShowMore = !isShowMore;
        } else {
          thisTarget.html('+ Xem ít hơn');
          o = renderItem(initList.concat(list)).join('');
          containItem.html(o);
          isShowMore = !isShowMore;
        }
      });
    }

    Start() {
      this.AppSwiper();
      this.ToggleShowDistributionPartnerOnMobile();
    }
  }

  new HomePage().Start();
});
