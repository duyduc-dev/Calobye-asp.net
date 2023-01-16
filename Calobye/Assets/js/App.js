('use strict');
import utils from './utils.js';
import target from './targetHTML.js';
import helper from './helper.js';

$(document).ready(() => {
  // App run
  class App {
    HandleNavMenuHeader() {
      const { iconNavMenu, iconCloseNav } = target.NavHeaderActive;
      const { wrapIconArrowNav } = target.NavHeaderArrow;

      // Click icon menu
      iconNavMenu?.on('click', () => {
        iconNavMenu?.toggleClass('active');
        helper.showMenuHeader();
      });

      // Click icon close menu
      iconCloseNav?.click(() => {
        iconNavMenu.removeClass('active');
        helper.showMenuHeader(false);
      });

      // Handle click icon arrow nav
      wrapIconArrowNav?.on('click', () => {
        wrapIconArrowNav?.toggleClass('active');
        helper.showCategoriesHeaderWhenClickArrow();
      });
    }

    async HandleSearchHeaderMobile() {
      // => ==> <== >= <= >> << == === != !== =/= __ && ||
      const {
        iconSearch,
        iconCloseSearchMobile,
        wrapSearchMobile,
        showResultSearch,
        inputSearchOnMobile,
        iconSearchActive,
        iconSpinLoading,
      } = target.SearchHeaderMobile;

      iconSearch?.on('click', async () => {
        wrapSearchMobile.addClass('!translate-y-0 !z-[100] !opacity-100 !pointer-events-auto');
        inputSearchOnMobile.focus();
        search("");
    });

      iconCloseSearchMobile?.on('click', () => {
        showResultSearch.html('');
        wrapSearchMobile.removeClass('!translate-y-0 !z-[100] !opacity-100 !pointer-events-auto');
      });

      inputSearchOnMobile.on('input', (e) => {
        const valueInput = $(e.target).val();
        search(valueInput);
      });


      const search = utils.debounce(async (valueInput, isPending) => {
        if (isPending) {
          iconSearchActive.hide(100);
          iconSpinLoading.show(100);
        } else {
          iconSearchActive.show(100);
          iconSpinLoading.hide(100);

          helper.searchProductOnMobile(valueInput, (data) => {
            showResultSearch.html(
              data.map(
                ({ title, slug, thumbnail, price, desc }) => /*html*/ `
              <a href="/product/${slug}" class="flex items-center my-2 ">
                <div class="w-4/12">
                  <img src="/Assets/images/product/${thumbnail}" alt="" />
                </div>
                <div class="w-8/12 flex flex-col justify-end pl-6 pr-4 text-black_olive">
                  <span class="text-[13px] font-bold">${title}</span>
                  <span class="text-xs py-1">${desc}</span>
                  <div class="flex items-end justify-end">
                    <span class="text-lg text-judge_grey font-bold leading-none">${utils.formatMoney(price)}</span>
                  </div>
                </div>
              </a>`
              )
            );

          })
        }
      });
    }

    HandleSidebarActive() {
      const { btnCloseSidebar, overlay, sidebarInner, btnShowSidebar } = target.sidebar;
      btnCloseSidebar?.on('click', () => {
        overlay.removeClass('!z-[100] !opacity-100 !pointer-events-auto');
        sidebarInner.removeClass('!translate-x-0');
      });

      btnShowSidebar?.on('click', () => {
        overlay.addClass('!z-[100] !opacity-100 !pointer-events-auto');
        sidebarInner.addClass('!translate-x-0');
      });
    }

    HandleClickOnTop() {
      const btnClickOnTop = $('#clickOnTop');
      btnClickOnTop.hide();
      $(window).scroll((e) => {
        if ($(window).scrollTop() >= 500) {
          btnClickOnTop.show(100);
        } else {
          btnClickOnTop.hide(100);
        }
      });

      btnClickOnTop.click(() => {
        $(window).scrollTop(0);
      });
    }

    TippyJs() {
      tippy('.btn-user', {
        interactive: true,
        placement: 'bottom-end',
        popperOptions: {
          strategy: 'fixed',
        },
        trigger: 'click',
        hideOnClick: true,
        animation: true,
        zIndex: 99999999,

        onShow(instance) {
          $(instance.popper).show(300);
        },
        onHide(instance) {
          $(instance.popper).hide(300);
        },
        render(instance) {
          // The recommended structure is to use the popper as an outer wrapper
          // element, with an inner `box` element
          const popper = document.createElement('div');
          const box = document.createElement('div');
          const boxInner = document.createElement('div');

          popper.appendChild(box);
          box.appendChild(boxInner);

          boxInner.className = 'tippy-user';
          boxInner.innerHTML = $('#tippy-inner-box-user').html();

          // Return an object with two properties:
          // - `popper` (the root popper element)
          // - `onUpdate` callback whenever .setProps() or .setContent() is called
          return {
            popper,
            onUpdate: (prevProps, nextProps) => {
              // DOM diffing
              if (prevProps.content !== nextProps.content) {
                boxInner.innerHTML = nextProps.content;
              }
            }, // optional
          };
        },
      });
    }

    FormatMoney() {
      const ele = $('[data-format-money]');

      ele.each((item, e) => {
        const money = $(e).data('format-money');
        $(e).text(utils.formatMoney(money))
			})
      
    }

    HandleShowPasswordOnClick() {
      const btn = $("#show-password");
      const input = $('input[type="password"]')
      let isShowPassword = false;

      btn.on('click', () => {
        btn.html(isShowPassword ? '<i class="fa-solid fa-eye-slash"></i>' : '<i class="fa-solid fa-eye"></i>');
        input.attr("type", isShowPassword ? 'password' : 'text')

        isShowPassword = !isShowPassword;
      });

		}

    Start() {
      this.HandleClickOnTop();
      this.HandleNavMenuHeader();
      this.HandleSidebarActive();
      this.HandleSearchHeaderMobile();
      this.TippyJs();
      this.HandleShowPasswordOnClick();
      this.FormatMoney();
    }
  }

  const app = new App();
  app.Start();
});

