import utils from '../utils.js';

const  params = new Proxy(new URLSearchParams(window.location.search), {
  get: (searchParams, prop) => searchParams.get(prop),
})

const Products = {
  Pagination() {
    
    $('#pagination-product').pagination({
      dataSource: (done) => {
        if (type === 'search') {
          $.get(`/api/product-category/${slug}`, (data, status) => {
            if (status === 'success') {
              done(JSON.parse(data?.data) || [])
            }
          })
        } else {
          $.get(`api/search-product/${params.q || ''}`, (data, status) => {
            if (status === 'success') {
              done(JSON.parse(data?.data) || [])
            }
          })
        }
       
			},
      pageSize: 6,
      showPrevious: true,
      showNext: true,
      autoHidePrevious: true,
      autoHideNext: true,
      callback: function (data, pagination) {
        // template method of yourself
        let html = data?.map(({ title, desc, price, thumbnail, slug = '' }) => templateProductPagination(title, desc, price, thumbnail, slug));
        $('#pagination-container').html(html);
      },
    });
  },

  init() {
    this.Pagination();
  },
};

Products.init();

function templateProductPagination(title = '', desc = '', price = 0, thumbnail = '', slug = '') {
  return `
<a href="/product/${slug}"
  class="block relative text-center md:h-[570px] md:border md:border-light_gray_2 hover:border-rosy_pink cursor-pointer"
>
  <div>
    <img
      src="/Assets/images/product/${thumbnail || '1.jpg'}"
      alt=""
      class="rounded-xl md:rounded-none object-contain w-full max-h-[350px] md:min-h-[350px]"
    />
  </div>
  <div>
    <div class="my-7.5 text-sm md:text-base md:px-3">
      <h4 class="text-haft_text hidden md:block truncate">${title || ''}</h4>
      <h5 class="text-grey_text_88 truncate">${desc || ''}</h5>
      <div class="flex flex-col md:flex-row items-start md:items-center justify-center w-full">
        <div class="mt-4 md:mt-2 font-bold text-lg md:text-2xl">${utils.formatMoney(
          price
        )} <span class="text-judge_grey text-[14px]">VNĐ</span></div>
      </div>
    </div>
  </div>
</a>`;
}

// console.log(templateProductPagination('hello', 'duy duc', 120000000));
