$(() => {
	const ProductDetail = {

		HandleChangeAmountProduct() {
			const btnDecrement = $('#btn-minus-amount');
			const btnIncrement = $('#btn-plus-amount');
			const showAmountProduct = $('#amount-product');
			const InputAmountProduct = $('.input-amount-product');


			btnDecrement.on('click', () => {
				const amount = parseInt(showAmountProduct.text());
				showAmountProduct.text(amount - 1 <= 0 ? 1 : amount - 1);
				InputAmountProduct.val(showAmountProduct.text());
			})

			btnIncrement.on('click', () => {
				const amount = parseInt(showAmountProduct.text());
				showAmountProduct.text(amount + 1);
				InputAmountProduct.val(showAmountProduct.text());

			})

		},

		HandleClickBuyNow() {
			const btnBuyNow = $('.btn-buy-now');
			const formBuyNow = $('#form-buy-now');

			btnBuyNow.click(() => {
				formBuyNow.submit();
			});
		},

		init() {
			this.HandleChangeAmountProduct();
			this.HandleClickBuyNow();
		}
	}

	ProductDetail.init();
})