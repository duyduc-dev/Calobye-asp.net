const targetHTML = {
  NavHeaderActive: {
    iconNavMenu: $('[data-header-nav-menu="nav-menu"]'),
    wrapItem: $('[data-header-nav-menu="nav-menu"] #wrap-item'),
    item: $('[data-header-nav-menu="nav-menu"] .item'),
    borderNav: $('#nav-border-active'),
    categoryActive: $('#category-active'),
    iconCloseNav: $('#close-nav-menu'),
  },

  // handle on screen < 1024px
  NavHeaderArrow: {
    wrapIconArrowNav: $('[data-header-nav-arrow="nav-arrow"]'),
    iconArrowDown: $('[data-header-nav-arrow="nav-arrow"] #arrow-down'),
    iconArrowUp: $('[data-header-nav-arrow="nav-arrow"] #arrow-up'),
    popupShowFullCategories: $('[data-header-show-all-category=""]'),
  },

  cart: {
    amountCart: $('#amount-cart'),
    amountCartMobile: $('#amount-cart-mobile'),
  },

  SearchHeaderMobile: {
    iconSearch: $('[onclick-search-mobile]'),
    iconSearchActive: $('#icon-search-mobile-active'),
    iconSpinLoading: $('#loading-searching'),
    wrapSearchMobile: $('#search-mobile'),
    iconCloseSearchMobile: $('[onclick-close-search-mobile]'),
    showResultSearch: $('#show-result-search'),
    inputSearchOnMobile: $('[input-search-mobile]'),
  },

  sidebar: {
    btnCloseSidebar: $('[onclick-close-sidebar]'),
    overlay: $('#overlay-sidebar'),
    sidebarInner: $('#sidebar'),
    btnShowSidebar: $('[onclick-show-sidebar]'),
  },

  distributionPartnerHomePage: {
    btnSeeMore: $('#btn-see-distribution'),
    containItem: $('#distribution-partner'),
  },
};
export default targetHTML;
