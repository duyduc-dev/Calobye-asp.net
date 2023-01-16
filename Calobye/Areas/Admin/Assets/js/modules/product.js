

(function () {
	'use strict'

	const product = {
		changToSlug() {
			const inputTitle = $('input[name="title"]')
			const inputSlug = $('input[name="slug"]')

			inputTitle.on('input', (e) => {
				var value = $(e.currentTarget).val()
				inputSlug.val((slugify(value.toLowerCase())))
			})
		},

		previewImg() {
			const inputImgPreview = $("input[preview-image]");
			const img = $("img[show-preview]");
			const clearPreview = $('[clear-preview]');

			inputImgPreview?.on('change', evt => {
				const [file] = inputImgPreview[0].files
				if (file) {
					img?.attr('src', URL.createObjectURL(file))
				}
			});

			const defaultAvatar = clearPreview.data('default-avatar');

			clearPreview.on('click', () => {
				img?.attr('src', defaultAvatar || "/Assets/images/placeholder-img.jpg")
				inputImgPreview.val(null);
			})
		},

		init() {
			this.changToSlug();
			this.previewImg();
		}
};

product.init();

})()