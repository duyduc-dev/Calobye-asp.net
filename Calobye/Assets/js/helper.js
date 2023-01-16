import target from './targetHTML.js';

const helper = {
  showMenuHeader(isShow = true) {
    const { iconNavMenu, item, wrapItem, borderNav, categoryActive } = target.NavHeaderActive;
    let hasShow = isShow;

    if (iconNavMenu.hasClass('active') && isShow) {
      iconNavMenu.addClass('!bg-rosy_pink');
      item.addClass('!bg-white');
      wrapItem.addClass('!rotate-[-180deg]');
      borderNav.addClass('!h-1.5');
      categoryActive.addClass('!translate-y-0 !opacity-100');
      hasShow = true;
    } else {
      iconNavMenu.removeClass('!bg-rosy_pink');
      item.removeClass('!bg-white');
      wrapItem.removeClass('!rotate-[-180deg]');
      borderNav.removeClass('!h-1.5');
      categoryActive.removeClass('!translate-y-0 !opacity-100');
      hasShow = false;
    }

    return hasShow;
  },

  showCategoriesHeaderWhenClickArrow(isShow = true) {
    const { wrapIconArrowNav, iconArrowDown, iconArrowUp, popupShowFullCategories } = target.NavHeaderArrow;
    let hasShow = isShow;

    if (wrapIconArrowNav?.hasClass('active') && isShow) {
      iconArrowDown.hide(200);
      iconArrowUp.show(100);
      popupShowFullCategories.addClass('!translate-y-0 !opacity-100');
      hasShow = true;
    } else {
      iconArrowDown.show(200);
      iconArrowUp.hide(100);
      popupShowFullCategories.removeClass('!translate-y-0 !opacity-100');
      hasShow = false;
    }

    return hasShow;
  },

  searchProductOnMobile(searchKey, callback) {
    $.get(`/api/search-product/${searchKey || ''}`, (data, status) => {
      if (status === 'success') {
        const result = JSON.parse(data?.data) || [];
        callback(result)
      }
    })

	}

};

export default helper;
